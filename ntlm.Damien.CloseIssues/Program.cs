// See https://aka.ms/new-console-template for more information

using ntlm.Damien;

var github = new GithubService()
{
    Logger = Console.Out,
    Token = "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"
};

await github.CloseAllIssues();