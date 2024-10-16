namespace ntlm.Damien
{
    using LibGit2Sharp;
    using System;
    using System.IO;
    using System.Net;

    /// <summary>
    /// FTP service.
    /// </summary>
    public class FtpService : BaseService
    {

        /// <summary>
        /// Only files modified after this date will be downloaded.
        /// </summary>
        public readonly DateTime StartDate = new(2024, 10, 16);
        public string? Host { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? LocalDirectory { get; set; }
        public int Port { get; set; }
        public string? UriSuffix { get; set; }

        /// <summary>
        /// Download release files from the specified repositories.
        /// </summary>
        /// <param name="repositories"></param>
        /// <param name="host"></param>
        /// <param name="ftpUsername"></param>
        /// <param name="ftpPassword"></param>
        /// <param name="localDownloadDirectory"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public RepositoryInfo[] DownloadReleaseFiles(CancellationToken ct, string[] repositories)
        {
            OnProgressChanged(this, 0);

            if (
                LocalDirectory == null ||
                Username == null ||
                Password == null
                ) return Array.Empty<RepositoryInfo>();
            if (!Directory.Exists(LocalDirectory))
                Directory.CreateDirectory(LocalDirectory);


            var list = new List<RepositoryInfo>();
            int i = 0;
            foreach (var repository in repositories)
            {


                try
                {
                    string uri = $"ftp://{Host}:{Port}/{repository}{UriSuffix}/";

                    // Utilisation de FtpWebRequest qui est obsolète mais supporté pour FTP
#pragma warning disable SYSLIB0014
                    FtpWebRequest request = (FtpWebRequest)WebRequest.Create(uri);
#pragma warning restore SYSLIB0014

                    request.Method = WebRequestMethods.Ftp.ListDirectory;
                    request.Credentials = new NetworkCredential(Username, Password);
                    request.UsePassive = true;

                    try
                    {

                        ct.ThrowIfCancellationRequested();

                        using FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                        using StreamReader reader = new(response.GetResponseStream());
                        string? line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            if (!string.IsNullOrWhiteSpace(line) && line.EndsWith(".release.json", StringComparison.OrdinalIgnoreCase))
                            {
                                string ftpFileUrl = $"{uri}/{line}";
                                string localFilePath = Path.Combine(LocalDirectory, line);

                                // Récupérer la date du fichier
                                DateTime fileDate = GetFileDate(ftpFileUrl, Username, Password);
                                Log($"Fichier : {line}, Date de dépôt : {fileDate}");
                                if (fileDate > StartDate)
                                {
                                    var fileDirectory = Path.GetDirectoryName(localFilePath);

                                    if (!string.IsNullOrWhiteSpace(fileDirectory))
                                    {
                                        var dir = Path.Combine(fileDirectory, repository);

                                        if (!Directory.Exists(dir))
                                            Directory.CreateDirectory(dir);

                                        var filePath = Path.Combine(
                                            dir,
                                            Path.GetFileName(localFilePath)
                                            );

                                        DownloadFile(ftpFileUrl, filePath, Username, Password);
                                        list.Add(new RepositoryInfo(repository) { ReleaseFileName = localFilePath });
                                        Log($"Le fichier {line} a été téléchargé.");
                                    }


                                }
                                else
                                {
                                    Log($"Le fichier {line}, trop ancien, n'a pas été téléchargé.");
                                }

                            }
                        }

                    }
                    catch (Exception)
                    {
                        Log($"L'Uri {uri} est introuvable.");
                    }
                    i++;
                    OnProgressChanged(this, (i * 100) / repositories.Length);
                }

                catch (OperationCanceledException)
                {
                    Log("Le téléchargement des secrets a été annulé par l'utilsiateur.");
                }
                catch (Exception ex)
                {
                    Log($"Erreur lors du téléchargement des secrts : {ex.Message}");
                }
            }
            return [.. list];
        }

        private DateTime GetFileDate(string ftpFileUrl, string ftpUsername, string ftpPassword)
        {
#pragma warning disable SYSLIB0014
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpFileUrl);
#pragma warning restore SYSLIB0014            
            request.Method = WebRequestMethods.Ftp.GetDateTimestamp; // Méthode pour obtenir la date de modification
            request.Credentials = new NetworkCredential(ftpUsername, ftpPassword);

            using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
            {
                return response.LastModified; // Retourne la date de modification du fichier
            }
        }


        private void DownloadFile(string ftpFileUrl, string localFilePath, string ftpUsername, string ftpPassword)
        {
#pragma warning disable SYSLIB0014
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpFileUrl);
#pragma warning restore SYSLIB0014

            request.Method = WebRequestMethods.Ftp.DownloadFile;
            request.Credentials = new NetworkCredential(ftpUsername, ftpPassword);

            using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
            using (Stream responseStream = response.GetResponseStream())
            using (FileStream outputStream = new(localFilePath, FileMode.Create))
            {
                responseStream.CopyTo(outputStream);
            }

        }


        public class RepositoryInfo(string name)
        {
            public string? Name { get; set; } = name;
            public string? ReleaseFileName { get; set; }
        }

    }

}
