using Microsoft.VisualStudio.TestTools.UnitTesting;
using static System.Net.WebRequestMethods;

namespace ntlm.Damien.Tests
{


    [TestClass]
    public class GithubTests
    {

        public static readonly string Token = "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";
        public static readonly string Directory = @"C:\Users\Dell\Desktop\ntlm.Damien\";

        [TestMethod]
        public void Clone_one_directory()
        {
            // Given
            var github = new Github(Directory, Token);

            // When
            github.Clone("https://github.com/ntlm-technologies/ntlm.def");

            // Then
            Assert.AreEqual(0, Github.Warnings.Count);
        }

        [TestMethod]
        public void Clone_muliple_directories()
        {
            // Given
            var github = new Github(Directory, Token);

            // When
            github.Clone(
                "https://github.com/ntlm-technologies/ntlm.def",
                "https://github.com/ntlm-technologies/ntlm.imp",
                "https://github.com/ntlm-technologies/ntlm"
                );

            // Then
            Assert.AreEqual(0, Github.Warnings.Count);
        }


        [TestMethod]
        public void TransformUrl()
        {
            // Given
            var github = new Github(Directory, Token)
            {
                Settings = new GithubSettings()
                {
                    OrganizationUrl = "https://github.com/ntlm-technologies/"
                }
            };

            // When
            var url = github.TransformUrl("ntlm.def");

            // Then
            Assert.AreEqual("https://github.com/ntlm-technologies/ntlm.def", url);
        }


        [TestMethod]
        public void GetClientDirectory()
        {
            // Given
            var github = new Github(Directory, Token)
            {
                Settings = new GithubSettings()
                {
                    Clients = ["ntlm"]
                }
            };

            // When
            var directory = github.GetClientDirectory("ntlm.def");

            // Then
            Assert.AreEqual("ntlm", directory);
        }

        [TestMethod]
        public void Clone_with_branch()
        {
            // Given
            var github = new Github(Directory, Token)
            {
                Settings = new GithubSettings()
                {
                    OrganizationUrl = "https://github.com/ntlm-technologies/",
                    Branches = ["to-dotnet-8", "dev"]
                }
            };

            // When
            github.Clone("lgi.Time");

            // Then
            Assert.AreEqual(0, Github.Warnings.Count);
        }


        [TestMethod]
        public void Clone_with_settings()
        {
            // Given
            var github = new Github(Directory, Token)
            {
                Settings = new GithubSettings()
                {
                    OrganizationUrl = "https://github.com/ntlm-technologies/",
                    UrlUrls = "https://raw.githubusercontent.com/ntlm-technologies/lgi.Repositories/refs/heads/main/repositories.txt",
                    Clients = [ "ntlm", "lgi" ]
                }
            };

            // When
            github.Clone();

            // Then
            Assert.AreEqual(0, Github.Warnings.Count);
        }

        [TestMethod]
        public void Init_loads_default_settings()
        {
            // When
            var github = new Github(Directory, Token);

            // Then
            Assert.IsNotNull(github.Settings);
        }

        [TestMethod]
        public void LoadSettings()
        {
            // Given
            var github = new Github(Directory, Token)
            {
                Settings = null
            };

            // When
            github.LoadSettings("appSettings.json");

            // Then
            Assert.IsNotNull(github.Settings);
        }

        [TestMethod]
        public void Clone()
        {
            // Given
            var github = new Github(Directory, Token);

            // When
            github.Clone();

            // Then
            Assert.AreEqual(0, Github.Warnings.Count);
        }

    }
}