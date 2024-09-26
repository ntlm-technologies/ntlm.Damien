namespace ntlm.Damien.Lgi.Tests
{

    using ntlm.Damien.Tests;

    [TestClass]
    public class LgiGithubTests
    {
        [TestMethod]
        public void Clone()
        {
            // Given
            var lgi = new LgiGithub(GithubTests.Directory, GithubTests.Token);

            // Then
            lgi.Clone();

            // Assert
            Assert.IsTrue(Github.Warnings.Count() == 0);
        }
    }
}