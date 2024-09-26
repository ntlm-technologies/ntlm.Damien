namespace ntlm.Damien.Lgi
{
    /// <summary>
    /// Github to clone Lgi's repositories.
    /// </summary>
    public class LgiGithub(string basePath, string? token) : Github(basePath, token)
    {


        public const string NtlmGithubUrl = 
            "https://github.com/ntlm-technologies/";

        private const string urlsUrl =
            "https://raw.githubusercontent.com/ntlm-technologies/lgi.Repositories/refs/heads/main/repositories.txt";

        public void Clone()
            => Clone(urlsUrl.GetRepositoryListFromFile());

        public override string TransformUrl(string url)
            => Path.Combine(NtlmGithubUrl, url);

    }
}
