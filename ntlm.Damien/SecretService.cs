namespace ntlm.Damien
{

    /// <summary>
    /// Handles secrets.
    /// </summary>
    public class SecretService : BaseService
    {

        public const string SecretRepository = "ntlm.Damien.Secrets";

        public SecretService(
            GithubService git,
            FtpService ftp
            )
        {
            Git = git;
            Ftp = ftp;
        }

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
            var ri = Ftp.DownloadReleaseFiles(ct, repositories
                .Select(x => x.Name)
                .ToArray()
                );

        }


        public GithubService Git { get; }
        public FtpService Ftp { get; }
    }
}
