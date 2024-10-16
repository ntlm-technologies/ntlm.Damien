namespace ntlm.Damien
{

    /// <summary>
    /// Handles secrets.
    /// </summary>
    public class SecretService(
        GithubService git,
        FtpService ftp
            ) : BaseService
    {

        public const string SecretRepository = "ntlm.Damien.Secrets";

        /// <summary>
        /// Handles the secrets. Saves them in each local repository and the ntlm.Damien.Secrets repository.
        /// </summary>
        /// <returns></returns>
        public async Task Handle()
            => await Handle(CancellationToken.None);

        /// <summary>
        /// Handles the secrets. Saves them in each local repository and the ntlm.Damien.Secrets repository.
        /// </summary>
        /// <returns></returns>
        public async Task Handle(CancellationToken ct)
        {

            // Ensure the ftp service has the proper directory.
            Ftp.LocalDirectory = @$"{Git.BasePath}\ntlm\ntlm.Damien.Secrets";

            // Clone the secret repository
            Git.Clone(SecretRepository);

            // The repositories
            var repositories = await Git.GetRepositoriesAsync();

            // The repositories and their release files
            var ri = Ftp.DownloadReleaseFiles(repositories
                .Select(x => x.Name)
                .ToArray(),
                ct
                );

            if (ri != null)
            {
                // Placer dans les repositories locaux
            }

        }


        public GithubService Git { get; } = git;
        public FtpService Ftp { get; } = ftp;
    }
}
