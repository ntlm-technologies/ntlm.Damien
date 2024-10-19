namespace ntlm.Damien.Tests
{
    public class DamienTestsException : Exception
    {
        public DamienTestsException(string? message) : base(message)
        {
        }
    }

    public class TokenNotProvidedException : DamienTestsException
    {
        public TokenNotProvidedException() : base("A valid Github token is required to pass tests. Go to github.com to set the GithubServiceTests.Token property.")
        {
            
        }
    }
}
