namespace ntlm.Damien
{

    using LibGit2Sharp;
    using LibGit2Sharp.Handlers;
    using Microsoft.Extensions.Configuration;
    using System;

    /// <summary>
    /// Represents a github service to clone repositories to local drive.
    /// </summary>
    public class Github
    {

        public Github(string basePath, string? token)
        {
            BasePath = basePath;
            Token = token;
            LoadDefaultSettings();
        }

        public Github(string basePath) : this(basePath, null)
        {
        }

        public Github() : this(Directory.GetCurrentDirectory())
        {
        }

        public event EventHandler<int> ProgressChanged = delegate { };

        protected virtual void OnProgressChanged(object sender, int progress)
        {
            ProgressChanged?.Invoke(sender, progress);
        }

        public event EventHandler<string> Warned = delegate { };

        protected virtual void OnWarned(object sender, string text)
        {
            Warned?.Invoke(sender, text);
        }

        /// <summary>
        /// Loads default settings if appSettings.json is found in root.
        /// </summary>
        protected virtual void LoadDefaultSettings()
        {
            var file = "appSettings.json";
            if (File.Exists(Path.Combine(Directory.GetCurrentDirectory(), file)))
                LoadSettings(file);
        }

        /// <summary>
        /// The settings.
        /// </summary>
        public GithubSettings? Setting { get; set; }

        /// <summary>
        /// Logger.
        /// </summary>
        public TextWriter Logger { get; set; } = Console.Out;

        /// <summary>
        /// Github personal access token.
        /// </summary>
        public string? Token { get; set; }

        /// <summary>
        /// Local directory.
        /// </summary>
        public string BasePath { get; set; }

        /// <summary>
        /// Loads settings.
        /// </summary>
        /// <param name="filePath"></param>
        public void LoadSettings(string filePath)
        {
            try
            {
                var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile(filePath, optional: false, reloadOnChange: true)
                    .Build();
                Setting = builder.GetSection("AppSettings").Get<GithubSettings>();
            }
            catch (Exception ex)
            {
                Warn(ex.Message);
                throw;
            }
        }


        /// <summary>
        /// Clones repositories to local directory analysing settings.
        /// </summary>
        public async Task CloneAsync(CancellationToken ct)
        {
            if (Setting?.UrlUrls != null)
                await CloneAsync(
                    Setting
                        .UrlUrls
                        .GetRepositoryListFromFile(Token)
                        .GroupBy(x => x)
                        .Select(x => x.Key)
                        .ToArray()
                    , ct);
            else
                Warn("Aucun fichier txt distant configuré pour obtenir la liste des url à cloner.");
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
        public void Clone(string url)
        {
            var tUrl = TransformUrl(url);
            try
            {
                string repoName = GetRepositoryNameFromUrl(tUrl);
                string repoPath = Path.Combine(
                    BasePath,
                    GetClientDirectory(repoName),
                    repoName
                    );

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

                    Repository.Clone(tUrl, repoPath, cloneOptions);
                    Checkout(repoPath);
                    Log($"Cloné avec succès : {repoName}");
                }

            }
            catch (Exception ex)
            {
                Warn($"Erreur lors du clonage de {tUrl}: {ex.Message}");
            }

        }

        /// <summary>
        /// Clones a list repositories to local directory.
        /// </summary>
        /// <param name="urls">Urls of Github directories.</param>
        public async Task CloneAsync(string[] urls, CancellationToken ct)
        {
            Warnings.Clear();

            OnProgressChanged(this, 0);

            int i = 0;

            try
            {
                foreach (var url in urls)
                {
                    ct.ThrowIfCancellationRequested();

                    await Task.Run(() => Clone(url));

                    i++;
                    OnProgressChanged(this, (i * 100) / urls.Length);
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
            if (Setting?.Branches != null)
            {
                var repo = new Repository(repoPath);

                // Si des fichiers sont modifiés, effectuer un stash
                if (repo.RetrieveStatus().IsDirty)
                {
                    Log("Des modifications non validées détectées, création d'un stash...");
                    repo.Stashes.Add(repo.Config.BuildSignature(DateTimeOffset.Now), "Sauvegarde temporaire");
                }

                foreach (var branchName in Setting.Branches)
                {
                    // Vérifier si la branche existe localement
                    var branch = repo.Branches[branchName];

                    if (branch == null)
                    {
                        // Si la branche n'existe pas localement, vérifiez si elle existe sur le dépôt distant
                        var remoteBranch = repo.Branches[$"origin/{branchName}"];
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
            if (!Repository.IsValid(repoPath))
            {
                Console.WriteLine("Le dossier spécifié n'est pas un dépôt Git.");
                return;
            }

            var repo = new Repository(repoPath);

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
            var signature = new Signature("Your Name", "youremail@example.com", DateTimeOffset.Now);
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
        /// <param name="url"></param>
        /// <returns></returns>
        public virtual string TransformUrl(string url) =>
            Setting?.OrganizationUrl != null ?
            Path.Combine(Setting.OrganizationUrl, url) :
            url;

        /// <summary>
        /// Transforms the directory.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public virtual string GetClientDirectory(string repoName)
        {
            if (Setting == null) return string.Empty;

            if (Setting.Clients.Contains(repoName.Split('.')[0]))
                return repoName.Split('.')[0];

            if (Setting.Clients.Contains(repoName.Split('-')[0]))
                return repoName.Split('-')[0];

            return string.Empty;
        }


        /// <summary>
        /// Transforms the directory.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public virtual string TransformDirectory(string repoName) => repoName;

        /// <summary>
        /// Returns the name of a repository from its url.
        /// </summary>
        /// <param name="repoUrl"></param>
        /// <returns></returns>
        static string GetRepositoryNameFromUrl(string repoUrl)
            => repoUrl[(repoUrl.LastIndexOf('/') + 1)..];

        /// <summary>
        /// The warnings.
        /// </summary>
        public static List<string> Warnings { get; } = [];

        /// <summary>
        /// Warns.
        /// </summary>
        /// <param name="text"></param>
        public void Warn(string text)
        {
            Log(text);
            Warnings.Add(text);
            OnWarned(this, text);
        }

        /// <summary>
        /// Log progress.
        /// </summary>
        /// <param name="text"></param>
        public void Log(string text)
            => Logger?.WriteLine(text);



    }
}
