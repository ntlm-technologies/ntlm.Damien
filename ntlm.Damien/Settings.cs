namespace ntlm.Damien
{
    using Microsoft.Extensions.Configuration;
    using System.Reflection;
    using System.Text;

    /// <summary>
    /// Settings
    /// </summary>
    public class Settings
    {

        public const string DamienRepository = "ntlm.Damien";

        public Settings()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = $"{DamienRepository}.Settings.json";

            using Stream? stream = assembly.GetManifestResourceStream(resourceName) ?? throw new InvalidOperationException($"Ressource intégrée '{resourceName}' introuvable.");
            if (stream != null)
            {
                using StreamReader reader = new(stream);
                string jsonContent = reader.ReadToEnd();

                var configuration = new ConfigurationBuilder()
                    .AddJsonStream(new MemoryStream(Encoding.UTF8.GetBytes(jsonContent)))
                    .Build();

                configuration.Bind(this);
            }
            else
            {
                throw new Exception($"Ressource intégrée '{resourceName}' introuvable.");
            }
        }

        /// <summary>
        /// The owner. Teams of the owner are prefixed with this value.
        /// ie: ntlm.write, ntlm.admin, etc.
        /// </summary>
        public string Owner { get; set; } = "ntlm";

        /// <summary>
        /// The prefered branches to checkout to.
        /// </summary>
        public string[] PreferedBranches { get; set; } = [];

        /// <summary>
        /// Organization name in Github.
        /// </summary>
        public string Organization { get; set; } = "ntlm-technologies";

        /// <summary>
        /// Repository that holds secrets.
        /// ie: *.release.json files.
        /// </summary>
        public string SecretRepository { get; set; } = "ntlm.Damien.Secrets";   

        /// <summary>
        /// Github's url.
        /// </summary>
        public string? GithubUrl { get; set; }

        /// <summary>
        /// Github repository to store client's settings.
        /// </summary>
        public string? ClientSettingsRepository { get; set; }

        /// <summary>
        /// Only files modified after this date will be downloaded.
        /// Because as of october 2024 a lot of *.release.json are deprecated.
        /// </summary>
        public DateTime DownloadReleaseSettingsNewerThan { get; set; } = DateTime.MinValue;
    }

}
