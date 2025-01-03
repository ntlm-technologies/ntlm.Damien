﻿namespace ntlm.Damien
{

    using LibGit2Sharp;
    using LibGit2Sharp.Handlers;
    using Octokit;
    using System;
    using System.Collections.Generic;
    using System.Net.Http.Headers;
    using System.Net.WebSockets;
    using System.Text.Json;
    using System.Text;
    using L = LibGit2Sharp;
    using O = Octokit;

    /// <summary>
    /// Represents a github service to clone repositories to local drive.
    /// </summary>
    public class GithubService(string basePath, string? token) : BaseService
    {

        #region Init

        /// <summary>
        /// The possible folders in a solution where the settings can be stored
        /// </summary>
        public static readonly string[] SettingsFolders = [
            "setting", 
            "settings",
            "configuration",
            "configurations",
            "config"
            ];

        public GithubService(string basePath) : this(basePath, null)
        {
        }

        public GithubService() : this(Directory.GetCurrentDirectory())
        {
        }

        /// <summary>
        /// Branches.
        /// 
        /// Take ["dev", "dev2", "master"] 
        /// 
        /// Will try to checkout dev if exists. I not, dev2 if exists, etc.
        /// </summary>
        public string[] Branches { get; set; } = [];


        /// <summary>
        /// Github personal access token.
        /// </summary>
        public string? Token { get; set; } = token;

        /// <summary>
        /// Settings.
        /// </summary>
        public Settings Settings { get; } = new Settings();

        /// <summary>
        /// Local directory.
        /// </summary>
        public string BasePath { get; set; } = basePath;



        #endregion




        #region Clone

        /// <summary>
        /// Clones repositories to local directory.
        /// </summary>
        public async Task CloneAsync(CancellationToken ct)
        {
            await CloneAsync(
                (await GetRepositoriesAsync())
                .Select(x => x.Name)
                .ToArray()
                , ct);

        }

        /// <summary>
        /// Clones repositories to local directory analysing settings.
        /// </summary>
        public void Clone() =>
            CloneAsync(CancellationToken.None).GetAwaiter().GetResult();

        /// <summary>
        /// Clones a repository to local directory.
        /// </summary>
        /// <param name="urls">Urls of Github directories.</param>
        public void Clone(string repo)
        {
            var tUrl = TransformUrl(repo);
            try
            {
                string repoName = GetRepositoryNameFromUrl(tUrl);
                if (repoName.Equals(Settings.DamienRepository, StringComparison.CurrentCultureIgnoreCase))
                    return;
                string repoPath = repoName.GetRepositoryPath(BasePath);

                // Cloning
                Log($"Clonage de {repoName}...");

                if (Directory.Exists(repoPath))
                {
                    Log($"Le dépôt {repoName} existe déjà.");
                    FetchAndUpdateRepository(repoPath);
                }
                else
                {
                    var cloneOptions = new CloneOptions();
                    cloneOptions.FetchOptions.CredentialsProvider = GetCredentialsHandler();

                    L.Repository.Clone(tUrl, repoPath, cloneOptions);
                    Log($"Cloné avec succès : {repoName}");
                }
                Checkout(repoPath);

            }
            catch (Exception ex)
            {
                Warn($"Erreur lors du clonage de {tUrl}: {ex.Message}");
                if (ex.InnerException != null)
                    Warn($"Détails : {ex.InnerException.Message}");
            }

        }


        /// <summary>
        /// Clones the repositories of the specified clients.
        /// </summary>
        /// <param name="urls">Urls of Github directories.</param>

        public async Task CloneClientsAsync(string[] clients, CancellationToken ct)
            => await CloneAsync(
                (await GetRepositoriesAsync())
                .Select(x => x.Name)
                .OfClients(clients)
                .ToArray()
                , ct);

        /// <summary>
        /// Clones a list repositories to local directory.
        /// </summary>
        /// <param name="repos">Urls of Github directories.</param>
        public async Task CloneAsync(string[] repos, CancellationToken ct)
        {
            Warnings.Clear();

            OnProgressChanged(this, 0);

            int i = 0;


            await Task.Run(() => {

                try
                {
                    foreach (var repo in repos)
                    {
                        ct.ThrowIfCancellationRequested();

                        Clone(repo);

                        i++;
                        OnProgressChanged(this, (i * 100) / repos.Length);
                    }
                }
                catch (OperationCanceledException)
                {
                    Log("Clonage annulé par l'utilisateur.");
                }
                catch (Exception ex)
                {
                    Log($"Erreur lors du clonage : {ex.Message}");
                }


            }, ct);

          
            Log("Terminé.");
        }

        /// <summary>
        /// Clones a list repositories to local directory.
        /// </summary>
        /// <param name="urls">Urls of Github directories.</param>
        public void Clone(params string[] urls) =>
            CloneAsync(urls, CancellationToken.None).GetAwaiter().GetResult();

        /// <summary>
        /// Checks out a repo to the most preferred branch.
        /// </summary>
        /// <param name="repoPath"></param>
        public void Checkout(string repoPath)
        {
            // Branching
            if (Branches != null)
            {
                var repo = new L.Repository(repoPath);

                // Si des fichiers sont modifiés, effectuer un stash
                if (repo.RetrieveStatus().IsDirty)
                {
                    Log("Des modifications non validées détectées, création d'un stash...");
                    repo.Stashes.Add(repo.Config.BuildSignature(DateTimeOffset.Now), "Sauvegarde temporaire");
                }

                foreach (var branchName in Branches.Where(x => !string.IsNullOrWhiteSpace(x)))
                {
                    // Vérifier si la branche existe localement
                    var branch = repo.Branches[branchName];

                    if (branch == null)
                    {
                        // Si la branche n'existe pas localement, vérifiez si elle existe sur le dépôt distant
                        var remoteBranch = repo
                            .Branches
                            .FirstOrDefault(x => x.FriendlyName == $"origin/{branchName}");
                        if (remoteBranch != null)
                        {
                            // Créer la branche locale à partir de la branche distante
                            branch = repo.CreateBranch(branchName, remoteBranch.Tip);
                            repo.Branches.Update(branch, b => b.TrackedBranch = remoteBranch.CanonicalName);
                            Log($"Branche '{branchName}' créée localement à partir de la branche distante.");
                        }
                    }

                    if (branch != null)
                    {
                        // Effectuer le checkout sur la branche
                        Commands.Checkout(repo, branch);
                        Log($"Branche trouvée et activée : {branch.FriendlyName}");
                        return; // Quitter dès que le checkout est réussi
                    }
                }

                //Log("Aucune des branches spécifiées n'a été trouvée.");

            }
        }

        /// <summary>
        /// If true fetches repositories already cloned.
        /// </summary>
        public bool Fetch { get; set; }

        /// <summary>
        /// Fetches an updates a repository.
        /// </summary>
        /// <param name="repoPath"></param>
        public void FetchAndUpdateRepository(string repoPath)
        {
            if (!Fetch) return;

            // Vérifiez si le dossier contient un dépôt Git
            if (!L.Repository.IsValid(repoPath))
            {
                Console.WriteLine("Le dossier spécifié n'est pas un dépôt Git.");
                return;
            }

            var repo = new L.Repository(repoPath);

            // Récupérer les dernières modifications depuis le dépôt distant
            var remote = repo.Network.Remotes["origin"];

            var fetchOptions = new FetchOptions
            {
                CredentialsProvider = GetCredentialsHandler()
            };

            Log("Récupération des mises à jour depuis l'origine...");
            Commands.Fetch(repo, remote.Name, remote.FetchRefSpecs.Select(x => x.Specification), fetchOptions, null);

            Log("Récupération terminée.");

            // Effectuer un pull pour intégrer les changements (si vous voulez directement pull)
            var signature = new L.Signature("Your Name", "youremail@example.com", DateTimeOffset.Now);
            var mergeResult = Commands.Pull(repo, signature, new PullOptions
            {
                FetchOptions = fetchOptions
            });

            if (mergeResult.Status == MergeStatus.Conflicts)
            {
                Log("Conflits détectés lors du pull.");
            }
            else if (mergeResult.Status == MergeStatus.UpToDate)
            {
                Log("Le dépôt est déjà à jour.");
            }
            else
            {
                Log("Mise à jour effectuée avec succès.");
            }

        }

        /// <summary>
        /// Returns a credentila handler.
        /// </summary>
        /// <returns></returns>
        public virtual CredentialsHandler GetCredentialsHandler() =>
            (_url, _user, _cred) =>
                new UsernamePasswordCredentials
                {
                    Username = Token,
                    Password = string.Empty
                };

        /// <summary>
        /// Transforms the url.
        /// </summary>
        /// <param name="repo"></param>
        /// <returns></returns>
        public virtual string TransformUrl(string repo) =>
            $"{Settings.GithubUrl}{Settings.Organization}/{repo}";
        
        /// <summary>
        /// Returns the name of a repository from its url.
        /// </summary>
        /// <param name="repoUrl"></param>
        /// <returns></returns>
        static string GetRepositoryNameFromUrl(string repoUrl)
            => repoUrl[(repoUrl.LastIndexOf('/') + 1)..];


        #endregion

        #region Teams

        private Team[]? teams;
        private GitHubClient? gitHubClient;

        /// <summary>
        /// The teams of the organization.
        /// </summary>
        public async Task<Team[]> GetTeamsAsync()
        {

            if (teams == null)
            {
                try
                {
                    var client = GetGitHubClient();
                    teams = [.. (await client.Organization.Team.GetAll(Settings.Organization))];
                }
                catch (Exception ex)
                {
                    Warn($"Erreur lors de l'importation des équipes : {ex.Message}");
                    throw;
                }
            }
            return teams;
        }

        /// <summary>
        /// Returns a Github client.
        /// </summary>
        /// <returns></returns>
        public GitHubClient GetGitHubClient()
        {
            gitHubClient ??= new GitHubClient(new O.ProductHeaderValue(Settings.DamienRepository))
            {
                Credentials = new O.Credentials(Token)
            };
            return gitHubClient;
        }

        private Team[]? userTeams;

        /// <summary>
        /// Returns the teams of a user.
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public async Task<Team[]> GetUserTeamsAsync(string userName)
        {
            if (userTeams == null)
            {
                var list = new List<Team>();

                var client = GetGitHubClient();

                try
                {
                    // Récupérer toutes les équipes de l'organisation
                    var organizationTeams = await client.Organization.Team.GetAll(Settings.Organization);

                    // Utilisation de HttpClient pour effectuer des requêtes API manuelles
                    using var httpClient = new HttpClient();
                    httpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue(Settings.Organization, "1.0"));
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

                    foreach (var team in organizationTeams)
                    {
                        // Construire l'URL pour vérifier l'appartenance de l'utilisateur à une équipe
                        var requestUrl = $"https://api.github.com/teams/{team.Id}/memberships/{userName}";

                        // Envoyer la requête GET pour vérifier l'appartenance de l'utilisateur
                        var response = await httpClient.GetAsync(requestUrl);

                        // Vérifier si la réponse indique que l'utilisateur est membre de l'équipe
                        if (response.IsSuccessStatusCode)
                        {
                            list.Add(team);
                        }
                    }

                    // Afficher les équipes
                    Log($"L'utilisateur {userName} appartient aux équipes suivantes dans l'organisation {Settings.Organization} :");
                    foreach (var team in list)
                    {
                        Log($"- {team.Name}");
                    }
                }
                catch (Exception ex)
                {
                    Warn($"Erreur : {ex.Message}");
                }

                userTeams = [.. list];
            }


            return userTeams;
        }


        /// <summary>
        /// Returns the teams of a user from its token.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<Team[]> GetUserTeamsAsync()
        {
            if (userTeams == null && Token != null)
            {
                var list = new List<Team>();

                var client = GetGitHubClient();

                var userLogin = (await GetUser())?.Login;

                try
                {
                    // Récupération des équipes de l'organisation
                    var teams = await client.Organization.Team.GetAll(Settings.Organization);

                    // Filtrer les équipes auxquelles appartient l'utilisateur
                    foreach (var team in teams)
                    {
                        var allMembers = await client.Organization.Team.GetAllMembers(team.Id);

                        // Vérification si l'utilisateur appartient à l'équipe
                        if (allMembers.Any(member => member.Login.Equals(userLogin, StringComparison.OrdinalIgnoreCase)))
                        {
                            list.Add(team);
                        }
                    }

                    Log($"Vous appartenez {(teams.Count > 1 ? "aux équipes" : "à l'équipe")} suivante{(teams.Count > 1 ? "s" : "")}:");
                    foreach (var team in list)
                        Log($" - {team.Name}");
                }
                catch (Exception ex)
                {
                    Warn($"Erreur : {ex.Message}");
                }

                userTeams = [.. list];
            }
            else
            {
                userTeams = [];
            }


            return userTeams;
        }

        private User? user;

        /// <summary>
        /// Returns the user name.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<User?> GetUser()
        {
            if (user == null)
            {
                var client = GetGitHubClient();

                try
                {
                    // Récupération de l'utilisateur courant basé sur le token
                    user = await client.User.Current();

                    Log($"Bienvenue {user.Login} !");
                }
                catch (Exception ex)
                {
                    Warn($"Erreur : {ex.Message}");
                }

            }
            return user;
        }


        private readonly Dictionary<string, string[]> teamRepositories = [];

        /// <summary>
        /// Returns the repositories of a team.
        /// </summary>
        /// <param name="teamName"></param>
        /// <returns></returns>
        public async Task<string[]> GetTeamRepositoriesAsync(string teamName)
        {
            if (!teamRepositories.TryGetValue(teamName, out string[]? value))
            {
                var team = await GetTeam(teamName);
                string[] repos = [];
                if (team != null)
                {
                    var client = GetGitHubClient();
                    try
                    {
                        repos = (await client.Organization.Team.GetAllRepositories(team.Id)).Select(x => x.Name).ToArray();
                    }
                    catch (Exception ex)
                    {
                        Warn($"Erreur : {ex.Message}");
                    }

                }
                teamRepositories[teamName] = repos;
                return repos;
            }
            else
            {
                return value;
            }
        }

        /// <summary>
        /// Returns a team.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<Team?> GetTeam(string name)
            => (await GetTeamsAsync()).FirstOrDefault(x => x.Name == name);

        /// <summary>
        /// Adds a repository to a team.
        /// </summary>
        /// <param name="teamName"></param>
        /// <param name="permission"></param>
        /// <param name="repositories"></param>
        /// <returns></returns>
        public async Task AddRepositoriesToTeamAsync(
            string teamName,
            string permission,
            params string[] repositories)
        {
            var teams = await GetTeamsAsync();
            if (!teams.HasTeam(teamName))
                return;

            // Configuration du client GitHub avec le PAT
            //var client = GetGitHubClient();

            try
            {
                var team = await GetTeam(teamName);

                if (team == null)
                {
                    Warn($"Équipe '{teamName}' introuvable.");
                    return;
                }

                Log($"Ajout des dépôts à l'équipe : {team.Name}");

                // Pour chaque repository, ajouter l'équipe avec les permissions spécifiées
                foreach (var repoName in repositories)
                {
                    try
                    {
                        Log($"Ajout du dépôt : {repoName} à l'équipe {team.Name} avec la permission '{permission}'.");

                        // Créer l'objet RepositoryPermissionRequest avec les permissions spécifiées

                        // Ajouter le repository à l'équipe avec les permissions spécifiées
                        using var httpClient = new HttpClient();
                        httpClient.DefaultRequestHeaders.Add("User-Agent", "PostmanRuntime/7.32.3");
                        httpClient.DefaultRequestHeaders.Add("Accept", "application/vnd.github+json");
                        httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {Token}");
                        httpClient.DefaultRequestHeaders.Add("X-GitHub-Api-Version", "2022-11-28");

                        var url = $"https://api.github.com/orgs/{Settings.Organization}/teams/{team.Slug}/repos/{Settings.Organization}/{repoName}";

                        var requestBody = new { permission }; 
                        var content = new StringContent(
                            JsonSerializer.Serialize(requestBody),
                            Encoding.UTF8,
                            "application/json"
                        );

                        var response = await httpClient.PutAsync(url, content);


                        Log($"Dépôt {repoName} ajouté avec succès.");
                    }
                    catch (Exception ex)
                    {
                        Warn($"Erreur lors de l'ajout du dépôt {repoName} : {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Warn($"Erreur : {ex.Message}");
            }
        }


        /// <summary>
        /// Removes repositories from a team.
        /// </summary>
        /// <param name="teamName"></param>
        /// <param name="repositories"></param>
        /// <returns></returns>
        public async Task RemoveRepositoriesFromTeamAsync(
            string teamName,
            params string[] repositories)
        {
            var client = GetGitHubClient();
            var teamRepos = await GetTeamRepositoriesAsync(teamName);

            try
            {
                var team = await GetTeam(teamName);

                if (team == null)
                    return;

                // Pour chaque repository, ajouter l'équipe avec les permissions spécifiées
                foreach (var repoName in repositories.Where(x => teamRepos.Contains(x)))
                {
                    try
                    {
                        Console.WriteLine($"Retrait du dépôt : {repoName} à l'équipe {team.Name}.");

                        // Ajouter le repository à l'équipe avec les permissions spécifiées
                        await client.Organization.Team.RemoveRepository(
                            team.Id,
                            Settings.Organization,
                            repoName
                        );

                        Console.WriteLine($"Dépôt {repoName} retiré avec succès.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Erreur lors du retrait du dépôt {repoName} : {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Warn($"Erreur : {ex.Message}");
            }
        }

        private O.Repository[]? repositories;

        /// <summary>
        /// Returns all the repositories.
        /// </summary>
        /// <param name="teamName"></param>
        /// <param name="repositories"></param>
        /// <returns></returns>
        public async Task<O.Repository[]> GetRepositoriesAsync()
        {
            if (repositories == null)
            {
                var client = GetGitHubClient();

                try
                {
                    repositories = [.. (await client.Repository.GetAllForOrg(Settings.Organization))];

                }
                catch (Exception ex)
                {
                    Warn($"Erreur : {ex.Message}");
                    repositories = [];
                }
            }
            return repositories;
        }

        ///// <summary>
        ///// Aplies permissions to a team for specific repositories.
        ///// </summary>
        ///// <param name="team"></param>
        ///// <param name="getPermission"></param>
        ///// <param name="repositories"></param>
        ///// <returns></returns>
        //public async Task ApplyPermissionsAsync(
        //    string team,
        //    Func<string, TeamPermissionLegacy?> getPermission,
        //    params string[] repositories
        //    )
        //{

        //    Warnings.Clear();

        //    OnProgressChanged(this, 0);
            
        //    int i = 0;
            
        //    foreach (var repo in repositories)
        //    {
        //        var permission = getPermission(repo);

        //        if (permission != null)
        //            await AddRepositoriesToTeamAsync(team, permission.Value, repo);
        //        else
        //            await RemoveRepositoriesFromTeamAsync(team, repo);
        //        i++;
        //        OnProgressChanged(this, (i * 100) / repositories.Length);

        //    }
        //}

        private Client[]? clients;

        /// <summary>
        /// Returns the owner from the clients.
        /// </summary>
        /// <returns></returns>
        public async Task<Client> GetOwner()
            => (await GetClientsAsync()).FirstOrDefault(x => x.IsOwner(Settings)) ?? new Client(Settings.Owner);

        /// <summary>
        /// Returns the client of ntlm-technologies from the name of a repositories.
        /// </summary>
        /// <returns></returns>
        public async Task<Client[]> GetClientsAsync()
        {
            clients ??= (await GetRepositoriesAsync())
                .Select(x =>
                {
                    if (x.Name.Contains('.'))
                        return x.Name.Split('.')[0];
                    else if (x.Name.Contains('-'))
                        return x.Name.Split('-')[0];
                    else
                        return null;
                })
                .Select(x => x?.ToLower())
                .GroupBy(x => x)
                .Select(x => x.Key)
                .OfType<string>()
                .Select(x => new Client(x)
                {
                    ExtraRepositories = GetClientExtraRepositories(x)
                })
                .ToArray();
            return clients;
        }

        /// <summary>
        /// Applies all permissions.
        /// </summary>
        /// <param name="repositories"></param>
        /// <returns></returns>
        public async Task ApplyPermissionsAsync()
            => await ApplyPermissionsAsync(CancellationToken.None);

        /// <summary>
        /// Applies all permissions.
        /// </summary>
        /// <param name="repositories"></param>
        /// <returns></returns>
        public async Task ApplyPermissionsAsync(CancellationToken ct)
            => await ApplyPermissionsAsync(ct, (await GetRepositoriesAsync()).Select(x => x.Name).ToArray());

        /// <summary>
        /// Applies all permissions for the given repositories.
        /// </summary>
        /// <param name="repositories"></param>
        /// <returns></returns>
        public async Task ApplyPermissionsAsync(CancellationToken ct, params string[] repositories)
        {
            OnProgressChanged(this, 0);
            try
            {

                var i = 0;
                foreach (var repo in repositories)
                {
                    ct.ThrowIfCancellationRequested();

                    await ApplyPermissionsToRepositoryAsync(repo);

                    i++;
                    OnProgressChanged(this, (i * 100) / repositories.Length);
                }

                Log("Terminé.");

            }
            catch (OperationCanceledException)
            {
                Log("Gestion des équipes annulée par l'utilisateur.");
            }
            catch (Exception ex)
            {
                Log($"Erreur lors de la gestion des équipes : {ex.Message}");
            }

        }

        /// <summary>
        /// Permissions to repository.
        /// </summary>
        /// <param name="repo"></param>
        /// <param name="clients"></param>
        /// <param name="teams"></param>
        /// <returns></returns>
        public async Task ApplyPermissionsToRepositoryAsync(string repo)
        {
            var clients = await GetClientsAsync();
            var teams = await GetTeamsAsync();
            var owner = await GetOwner();

            foreach (var client in clients
                .Where(x => x.HasTeam(teams))
                .Where(x => !x.IsOwner(Settings))
                )
            {
                // Clients repositories.
                if (repo.IsClient(client.Name))
                {
                    await AddRepositoriesToTeamAsync(client.ReadTeam, GithubPermission.Read, repo);
                    await AddRepositoriesToTeamAsync(client.TriageTeam, GithubPermission.Triage, repo);
                    await AddRepositoriesToTeamAsync(client.WriteTeam, GithubPermission.Write, repo);
                    await AddRepositoriesToTeamAsync(client.AdminTeam, GithubPermission.Admin, repo);
                }
                else
                {
                    await RemoveRepositoriesFromTeamAsync(client.ReadTeam, repo);
                    await RemoveRepositoriesFromTeamAsync(client.TriageTeam, repo);
                    await RemoveRepositoriesFromTeamAsync(client.WriteTeam, repo);
                    await RemoveRepositoriesFromTeamAsync(client.AdminTeam, repo);
                }

                // Extra repositories.
                if (client.ExtraRepositories.Any(x => x == repo))
                {
                    await AddRepositoriesToTeamAsync(client.WriteTeam, GithubPermission.Read, repo);
                    await AddRepositoriesToTeamAsync(client.AdminTeam, GithubPermission.Read, repo);
                }
                else
                {
                    await RemoveRepositoriesFromTeamAsync(client.ReadTeam, repo);
                    await RemoveRepositoriesFromTeamAsync(client.TriageTeam, repo);
                    await RemoveRepositoriesFromTeamAsync(client.WriteTeam, repo);
                    await RemoveRepositoriesFromTeamAsync(client.AdminTeam, repo);
                }

            }

            // NTLM admin and dev access.
            await AddRepositoriesToTeamAsync(owner.ReadTeam, GithubPermission.Read, repo);
            await AddRepositoriesToTeamAsync(owner.TriageTeam, GithubPermission.Triage, repo);
            await AddRepositoriesToTeamAsync(owner.WriteTeam, GithubPermission.Write, repo);
            await AddRepositoriesToTeamAsync(owner.AdminTeam, GithubPermission.Admin, repo);

        }


        /// <summary>
        /// The list of extra repositories the clients has read access to.
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public string[] GetClientExtraRepositories(string client)
            => (Settings.ClientSettingsRepository + client + ".txt")
                .GetRepositoryListFromFile(Token);

        #endregion


        /// <summary>
        /// Closes all the issues of all the repositories of the organization.
        /// </summary>
        /// <param name="repositories"></param>
        /// <returns></returns>
        public async Task CloseAllIssues()
        {
            var repositories = await GetRepositoriesAsync();
            foreach (var repo in repositories)
                await CloseIssues(repo.Name);
        }

        /// <summary>
        /// Closes all the issues.
        /// </summary>
        /// <param name="repositories"></param>
        /// <returns></returns>
        public async Task CloseIssues(params string[] repositories)
        {
            var client = GetGitHubClient();

            foreach (var repo in repositories)
            {

                try
                {
                    // Récupérer toutes les issues ouvertes
                    var issues = await client.Issue.GetAllForRepository(Settings.Organization, repo, new RepositoryIssueRequest
                    {
                        State = ItemStateFilter.Open
                    });

                    // Parcourir chaque issue et les fermer
                    foreach (var issue in issues)
                    {
                        if (issue.State == ItemState.Open)
                        {
                            var issueUpdate = new IssueUpdate
                            {
                                State = ItemState.Closed
                            };

                            await client.Issue.Update(Settings.Organization, repo, issue.Number, issueUpdate);
                            Log($"Issue #{issue.Number} fermée.");
                        }
                    }

                    Log("Toutes les issues ouvertes ont été fermées.");
                }
                catch (Exception ex)
                {
                    Warn($"Erreur: {ex.Message}");
                }


            }
        }


        #region Issues



        #endregion

        #region Security

        /// <summary>
        /// Validates a token.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<bool> ValidateToken(string token)
        {
            Token = token;
            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("PATValidator", "1.0"));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var requestUrl = "https://api.github.com/user";

            var response = await httpClient.GetAsync(requestUrl);

            return response.IsSuccessStatusCode;
        }

        #endregion

    }
}
