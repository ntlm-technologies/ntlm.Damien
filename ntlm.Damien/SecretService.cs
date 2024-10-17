using LibGit2Sharp;
using static ntlm.Damien.FtpService;

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

            // Place copies in the repositories.
            if (ri != null)
                PlaceCopiesInReporitories(ri, ct);

            Log("Terminé.");
        }

        /// <summary>
        /// Places copies in the repositories.
        /// </summary>
        /// <param name="list"></param>
        public void PlaceCopiesInReporitories(RepositoryInfo[] list, CancellationToken ct)
        {
            OnProgressChanged(this, 0);

            int i = 0;
            foreach (var item in list)
            {
                try
                {
                    ct.ThrowIfCancellationRequested();

                    PlaceCopyInRepository(item);
                }
                catch (OperationCanceledException)
                {
                    Log("Le téléchargement des secrets a été annulé par l'utilsiateur.");
                    break;
                }

                i++;
                OnProgressChanged(this, (i * 100) / list.Length);
            }
        }

        /// <summary>
        /// Places a copy in the repository.
        /// </summary>
        /// <param name="info"></param>
        public void PlaceCopyInRepository(RepositoryInfo info)
        {
            if (info == null) return;
            if (!info.IsValid()) return;

            var directories = info
                .Name
                .GetSettingsDirectories(Git.BasePath);

            bool copied = false;

            // Loop through possible settings directories.
            foreach (var directory in directories)
            {
                if (Directory.Exists(directory))
                {
                    File.Copy(
                        info.ReleaseFileName,
                        Path.Combine(directory, Path.GetFileName(info.ReleaseFileName)),
                        true
                        );
                    Log($"Copie de {info.ReleaseFileName} vers {directory}.");
                    copied = true;
                }

            }

            if (!copied)
                Warn($"Impossible de copier {info.ReleaseFileName} vers un dossier de configuration.");
        }

        public GithubService Git { get; } = git;
        public FtpService Ftp { get; } = ftp;
    }
}
