namespace ntlm.Damien.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class SettingsTests
    {

        [TestMethod]
        public void Init()
        {
            // Given
            var s = new Settings();

            // Then
            Assert.IsNotNull(s.GithubUrl);
            Assert.IsNotNull(s.Organization);
            Assert.IsNotNull(s.ClientSettingsRepository);
            Assert.IsTrue(s.DownloadReleaseSettingsNewerThan > DateTime.MinValue);
        }


    }
}
