namespace ntlm.Damien.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Newtonsoft.Json.Linq;

    [TestClass]
    public class ExtensionsTests
    {

        [TestMethod]
        public void GetClient()
        {
            // Given
            var repo = "ntlm.def";

            // When
            var client = repo.GetClient();

            // Then
            Assert.AreEqual("ntlm", client);
        }

        [TestMethod]
        public void GetRepositoryListFromFile()
        {
            // Given
            var url = "https://raw.githubusercontent.com/ntlm-technologies/ntlm.Damien.Data/refs/heads/main/lgi.txt";

            // When
            var urls = url.GetRepositoryListFromFile(GithubServiceTests.Token);

            // Then
            Assert.IsTrue(urls.Length > 0);
        }


    }
}
