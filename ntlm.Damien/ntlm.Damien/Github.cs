namespace ntlm.Damien
{

    using LibGit2Sharp;
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

        public event EventHandler<int> ProgressChanged = delegate { };

        protected virtual void OnProgressChanged(object sender, int progress)
        {
            ProgressChanged?.Invoke(sender, progress);
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
        public GithubSettings? Settings { get; set; }

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
                Settings = builder.GetSection("AppSettings").Get<GithubSettings>();
            }
            catch (Exception ex)
            {
                Warn(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Clones repositories to local directory.
        /// </summary>
        public void Clone()
        {
            if (Settings?.UrlUrls != null)
                Clone(Settings.UrlUrls.GetRepositoryListFromFile());
            else
                Warn("Aucun fichier txt distant configuré pour obtenir la liste des url à cloner.");
        }

        /// <summary>
        /// Clones a repository to local directory.
        /// </summary>
        /// <param name="urls">Urls of Github directories.</param>
        public void Clone(params string[] urls)
        {
            OnProgressChanged(this, 0);

            var cloneOptions = new CloneOptions();
            cloneOptions.FetchOptions.CredentialsProvider =
                (_url, _user, _cred) => new UsernamePasswordCredentials
                {
                    Username = Token,
                    Password = string.Empty
                };

            int i = 0;

            foreach (var url in urls)
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

                    if (Directory.Exists(repoPath))
                        Log($"Le dépôt {repoName} existe déjà.");
                    else
                    {
                        Repository.Clone(tUrl, repoPath, cloneOptions);
                        Log($"Cloné avec succès : {repoName}");
                    }

                    i++;
                    OnProgressChanged(this, (i * 100) / urls.Length);
                }
                catch (Exception ex)
                {
                    Warn($"Erreur lors du clonage de {tUrl}: {ex.Message}");
                }
            }
            Log("Terminé.");
        }

        /// <summary>
        /// Transforms the url.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public virtual string TransformUrl(string url) => 
            Settings?.OrganizationUrl != null ?
            Path.Combine(Settings.OrganizationUrl, url) :
            url;

        /// <summary>
        /// Transforms the directory.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public virtual string GetClientDirectory(string repoName) =>
            Settings != null && 
            Settings.Clients.Contains(repoName.Split('.')[0]) ?
            repoName.Split('.')[0] : 
            string.Empty;

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
        public static List<string> Warnings => [];

        /// <summary>
        /// Warns.
        /// </summary>
        /// <param name="text"></param>
        public void Warn(string text)
        {
            Log(text);
            Warnings.Add(text);
        }

        /// <summary>
        /// Log progress.
        /// </summary>
        /// <param name="text"></param>
        public void Log(string text)
            => Logger?.WriteLine(text);



    }
}
