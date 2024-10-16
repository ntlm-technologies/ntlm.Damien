using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;

namespace ntlm.Damien.Tests
{
    [TestClass]
    public class FtpServiceTests
    {
        [TestMethod]
        public void DownloadReleaseFiles()
        {
            // Given
            var ftp = new FtpService()
            {
                Host = "185.50.52.242",
                Username = "ftp-ntlm",
                Password = "SkV8pvF!cmk73\\+~",
                LocalDirectory = @"C:\deploy\damien",
                Port = 21
            };
            string[] directories = ["lgi.time.test", "lgi.people.test"];

            // When
            var list = ftp.DownloadReleaseFiles(directories);
        
            // Then
            Assert.AreEqual(2, list?.Length);
        }


    }
}
