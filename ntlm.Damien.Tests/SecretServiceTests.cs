namespace ntlm.Damien.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class SecretServiceTests
    {
        [TestMethod]
        public async Task Handle()
        {
            // Given
            var ftp = new FtpService()
            {
                Host = "185.50.52.242",
                Username = "ftp-ntlm",
                Password = "SkV8pvF!cmk73\\+~",
                Port = 21,
                UriSuffix = ".test"
            };
            var git = new GithubService(
                @"C:\deploy\damien", 
                "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"
                );
            var secret = new SecretService(git, ftp);

            // When
            await secret.Handle();
        }


    }
}
