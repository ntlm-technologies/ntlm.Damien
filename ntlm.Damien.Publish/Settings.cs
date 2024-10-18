namespace ntlm.Damien.Publish
{
    using Microsoft.Extensions.Configuration;

    public class Settings
    {
        public string RepoPath { get; set; } = string.Empty;
        public string PublishDirectory { get; set; } = string.Empty;
        public string ZipPath { get; set; } = string.Empty;
        public string CommitMessage { get; set; } = string.Empty;
        public GitUser GitUser { get; set; } = new GitUser();
        public GitCredentials Credentials { get; set; } = new GitCredentials();

        public Settings()
        {
            var settingsFilePath = Path.Combine(Directory.GetCurrentDirectory(), "settings.json");
            var ntlmSettingsFilePath = Path.Combine(Directory.GetCurrentDirectory(), "settings.ntlm.json");

            if (File.Exists(settingsFilePath))
            {
                try
                {
                    // Configuration builder pour charger le fichier JSON
                    var configuration = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile(settingsFilePath, optional: false, reloadOnChange: true)
                        .AddJsonFile(ntlmSettingsFilePath, optional: true, reloadOnChange: true)
                        .Build();

                    // Lier la configuration au modèle Settings
                    configuration.Bind(this);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erreur lors de la désérialisation : {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("Le fichier de configuration JSON est introuvable.");
            }
        }
    }

    public class GitUser
    {
        public string AuthorName { get; set; } = string.Empty;
        public string AuthorEmail { get; set; } = string.Empty;
    }

    public class GitCredentials
    {
        public string Username { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
    }
}
