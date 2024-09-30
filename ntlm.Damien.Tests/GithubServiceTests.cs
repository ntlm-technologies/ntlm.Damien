using Microsoft.VisualStudio.TestTools.UnitTesting;
using static System.Net.WebRequestMethods;

namespace ntlm.Damien.Tests
{


    [TestClass]
    public class GithubServiceTests
    {

        public static readonly string Token = "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";
        public static readonly string Directory = Environment.CurrentDirectory;
        public static readonly string Organization = "ntlm-technologies";

        [TestMethod]
        public async Task ApplyPermissionsAsync_Full()
        {
            // Given
            var github = new GithubService(Directory, Token);

            // When
            await github.ApplyPermissionsAsync(
                "ntlm",
                "ntlm.Nancy",
                "lgi.Products",
                "LesBellesLettres.Export"
                );
        }


        [TestMethod]
        public async Task GetClientsAsync()
        {
            // Given
            var github = new GithubService(Directory, Token);

            // When
            var list = await github.GetClientsAsync();

            // Then
            Assert.IsNotNull(list);
        }



        [TestMethod]
        public async Task GetRepositoriesAsync()
        {
            // Given
            var github = new GithubService(Directory, Token);

            // When
            var list = await github.GetRepositoriesAsync();

            // Then
            Assert.IsNotNull(list);
        }



        [TestMethod]
        public async Task ApplyPermissionsAsync()
        {
            // Given
            var github = new GithubService(Directory, Token);

            // When
            await github.ApplyPermissionsAsync("ntlm.dev",
                x => Octokit.TeamPermissionLegacy.Pull,
                "ntlm.dev",
                "ntlm",
                "ntlm.Nancy"
                );
        }


        [TestMethod]
        public async Task RemoveRepositoriesFromTeamAsync()
        {
            // Given
            var github = new GithubService(Directory, Token);

            // When
            await github.RemoveRepositoriesFromTeamAsync(
                "ntlm.dev",
                "ntlm"
                );
        }

        [TestMethod]
        public async Task AddRepositoriesToTeamAsync()
        {
            // Given
            var github = new GithubService(Directory, Token);

            // When
            await github.AddRepositoriesToTeamAsync(
                "ntlm.dev", 
                Octokit.TeamPermissionLegacy.Push, 
                "ntlm"
                );
        }

        [TestMethod]
        public async Task GetUserTeamsAsync()
        {
            // Given
            var github = new GithubService(Directory, Token);

            // When
            var teams = await github.GetUserTeamsAsync("MamboJoel");

            // Then
            Assert.IsNotNull(teams);
        }

        [TestMethod]
        public async Task GetTeamRepositoriesAsync()
        {
            // Given
            var github = new GithubService(Directory, Token);

            // When
            var repos = await github.GetTeamRepositoriesAsync("ntlm.dev");

            // Then
            Assert.IsNotNull(repos);
        }


        [TestMethod]
        public void Clone_one_directory()
        {
            // Given
            var github = new GithubService(Directory, Token);

            // When
            github.Clone("ntlm.def");

            // Then
            Assert.AreEqual(0, GithubService.Warnings.Count);
        }

        [TestMethod]
        public void Clone_muliple_directories()
        {
            // Given
            var github = new GithubService(Directory, Token);

            // When
            github.Clone(
                "ntlm.def",
                "ntlm.imp",
                "ntlm"
                );

            // Then
            Assert.AreEqual(0, GithubService.Warnings.Count);
        }


        [TestMethod]
        public void TransformUrl()
        {
            // Given
            var github = new GithubService(Directory, Token);

            // When
            var url = github.TransformUrl("ntlm.def");

            // Then
            Assert.AreEqual("https://github.com/ntlm-technologies/ntlm.def", url);
        }

        [TestMethod]
        public void Clone_with_branch()
        {
            // Given
            var github = new GithubService(Directory, Token);

            // When
            github.Clone("lgi.Time");

            // Then
            Assert.AreEqual(0, GithubService.Warnings.Count);
        }

    }
}