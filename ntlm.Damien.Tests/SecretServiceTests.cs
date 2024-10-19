namespace ntlm.Damien.Tests
{
    using LibGit2Sharp;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class SecretServiceTests
    {
        public static SecretService Get()
        {
            // Given
            var ftp = new FtpService()
            {
                Host = "185.50.52.242",
                Username = "ftp-ntlm",
                Password = "password",
                Port = 21,
                UriSuffix = ".test"
            };
            var git = new GithubService(
                @"C:\deploy\damien",
                "token"
                );
            return new SecretService(git, ftp);
        }

        [TestMethod]
        public async Task Handle()
        {
            // Given
            var secret = Get();

            // When
            await secret.Handle();
        }

        [TestMethod]
        public void PlaceCopyInRepository()
        {
            // Given
            var secret = Get();
            var repository = "lgi.social";
            var settings = @$"C:\Deploy\damien\ntlm\ntlm.Damien.Secrets\{repository}\appSettings.release.json";
            var destination = @$"C:\Deploy\damien\lgi\{repository}\{repository}.configuration";
            var settingsDirectory = Path.GetDirectoryName(settings) ?? string.Empty;
            if (!Directory.Exists(destination))
                Directory.CreateDirectory(destination);
            if (!Directory.Exists(settingsDirectory))
                Directory.CreateDirectory(settingsDirectory);
            if (!File.Exists(settings))
                File.WriteAllText(settings, "some content");

            var ri = new FtpService.RepositoryInfo(
                repository,
                settings
                );

            // When
            secret.PlaceCopyInRepository(ri);
        }

    }
}
