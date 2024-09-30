﻿namespace ntlm.Damien
{
    using Octokit;

    public static class Extensions
    {

        /// <summary>
        /// If the client matches one of the teams.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="teams"></param>
        /// <returns></returns>
        public static bool HasTeam(this Client client, IEnumerable<Team> teams)
            => teams.Any(t => t.Name == client.DevTeam || t.Name == client.AdminTeam);

        /// <summary>
        /// If the team exists.
        /// </summary>
        /// <param name="teams"></param>
        /// <param name="team"></param>
        /// <returns></returns>
        public static bool HasTeam(this IEnumerable<Team> teams, string team)
            => teams.Any(t => t.Name == team);

        /// <summary>
        /// If the client or team is ntlm.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool IsNtlm(this string name)
            => name.IsClient("ntlm");


        /// <summary>
        /// Returns the list of reporistories matching the given clients.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static IEnumerable<string> OfClients(this IEnumerable<string> repositories, IEnumerable<string> clients)
            => repositories.Where(x => clients.Any(c => x.IsClient(c)));

        /// <summary>
        /// If the client or team corresponds to the given client.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool IsClient(this string name, string client)
        {
            if (name.Contains("."))
                return name.Split('.')[0] == client;
            else if (name.Contains("-"))
                return name.Split('-')[0] == client;
            else
                return name == client;
        }

        /// <summary>
        /// The client.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetClient(this string name)
        {
            if (name.Contains("."))
                return name.Split('.')[0];
            else if (name.Contains("-"))
                return name.Split('-')[0];
            else
                return name;
        }


        /// <summary>
        /// If the role matches.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private static bool IsRole(this string name, string role)
        {
            string[] tab = [];
            if (name.Contains("."))
                tab = name.Split('.');
            else if (name.Contains("-"))
                tab = name.Split("-");
            return tab.Length > 1 && tab[1].ToLower() == role.ToLower();
        }

        /// <summary>
        /// If the role matches.
        /// </summary>
        /// <param name="team"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        private static bool IsRole(this Team team, string role) => team.Name.IsRole(role);

        /// <summary>
        /// If the team is a dev team.
        /// </summary>
        /// <param name="team"></param>
        /// <returns></returns>
        public static bool IsDev(this Team team) => team.IsRole("dev");

        /// <summary>
        /// If the team is a dev team.
        /// </summary>
        /// <param name="team"></param>
        /// <returns></returns>
        public static bool IsAdmin(this Team team) => team.IsRole("admin");

        /// <summary>
        /// If the team is an ntlm team.
        /// </summary>
        /// <param name="team"></param>
        /// <returns></returns>
        public static bool IsNtlm(this Team team) => team.Name.IsNtlm();

        /// <summary>
        /// If the repository is an ntlm repository.
        /// </summary>
        /// <param name="repo"></param>
        /// <returns></returns>
        public static bool IsNtlm(this Repository repo) => repo.Name.IsNtlm();

        /// <summary>
        /// If the client is an ntlm repository.
        /// </summary>
        /// <param name="repo"></param>
        /// <returns></returns>
        public static bool IsNtlm(this Client client) => client.Name.IsNtlm();

        /// <summary>
        /// Returns the list of reporistories from a distant txt file.
        /// </summary>
        /// <param name="fileUrl"></param>
        /// <returns></returns>
        public static string[] GetRepositoryListFromFile(this string fileUrl, string? token)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // Ajouter le token dans les en-têtes de la requête HTTP
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                    // Télécharge le contenu du fichier de manière synchrone
                    string fileContent = client.GetStringAsync(fileUrl).GetAwaiter().GetResult();

                    // Divise le contenu en un tableau de chaînes par ligne
                    string[] repositories = fileContent.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                    return repositories;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erreur lors de la récupération du fichier : {ex.Message}");
                    return new string[0]; // Retourne un tableau vide en cas d'erreur
                }
            }
        }

    }
}
