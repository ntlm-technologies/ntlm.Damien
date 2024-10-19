// See https://aka.ms/new-console-template for more information

using ntlm.Damien;

var github = new GithubService()
{
    Logger = Console.Out,
    Token = "token"
};

await github.CloseAllIssues();