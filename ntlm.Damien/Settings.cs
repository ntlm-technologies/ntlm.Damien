namespace ntlm.Damien
{
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// Settings
    /// </summary>
    public class Settings
    {
        public Settings()
        {
            new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build()
                .Bind(this);
        }

        /// <summary>
        /// Organization name in Github.
        /// </summary>
        public string Organization { get; set; } = "nntlm-technologies";

        /// <summary>
        /// Github's url.
        /// </summary>
        public string? GithubUrl { get; set; }

        /// <summary>
        /// Github repository to store client's settings.
        /// </summary>
        public string? ClientSettingsRepository { get; set; }
    }

}
