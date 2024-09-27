namespace ntlm.Damien
{
    public static class Extensions
    {

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
