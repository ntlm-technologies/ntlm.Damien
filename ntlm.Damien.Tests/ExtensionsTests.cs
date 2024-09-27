namespace ntlm.Damien.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ExtensionsTests
    {

        [TestMethod]
        public void GetRepositoryListFromFile()
        {
            // Given
            var url = "https://raw.githubusercontent.com/ntlm-technologies/lgi.Repositories/refs/heads/main/repositories.txt";

            // When
            var urls = url.GetRepositoryListFromFile();

            // Then
            Assert.IsTrue(urls.Count() > 0);
        }


    }
}
