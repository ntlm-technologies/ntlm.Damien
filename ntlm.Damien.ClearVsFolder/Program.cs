using LibGit2Sharp;
using System.Diagnostics;

string baseDirectory = @"C:\Users\mambo\OneDrive\Desktop\ntlm.Damien"; // À adapter

// Recherche récursive des répertoires contenant des repositories Git
ProcessRepositories(baseDirectory);



static void ProcessRepositories(string directory)
{
    foreach (var dir in Directory.GetDirectories(directory))
    {
        if (Directory.Exists(Path.Combine(dir, ".git")))
        {
            // Il s'agit d'un repository Git
            ProcessGitRepository(dir);
        }

        // Appel récursif pour traiter les sous-répertoires
        ProcessRepositories(dir);
    }
}

static void ProcessGitRepository(string repoPath)
{
    Console.WriteLine($"Traitement du repository: {repoPath}");

    using var repo = new Repository(repoPath);
    // Vérification et mise à jour du fichier .gitignore
    string gitignorePath = Path.Combine(repoPath, ".gitignore");
    EnsureVsFolderIgnored(gitignorePath);

    // Liste des branches à traiter
    string[] branchesToCheck = ["master", "main", "dev", "test", "to-dotnet-8"];

    foreach (var branchName in branchesToCheck)
    {
        // Vérifier si la branche existe
        if (repo.Branches[branchName] != null)
        {
            // Changer de branche
            Commands.Checkout(repo, repo.Branches[branchName]);

            // Supprimer le dossier .vs/
            RunGitCommand(repoPath, "rm -r --cached .vs/");

            // Ajouter le commit
            RunGitCommand(repoPath, "commit -m \"Supprime le dossier .vs/ du suivi\"");

            // Pousser la branche
            RunGitCommand(repoPath, $"push origin {branchName}");
        }
    }
}

static void EnsureVsFolderIgnored(string gitignorePath)
{
    // Si le fichier .gitignore n'existe pas, le créer
    if (!File.Exists(gitignorePath))
    {
        File.WriteAllText(gitignorePath, ".vs/");
        Console.WriteLine($".gitignore créé avec '.vs/' à {gitignorePath}");
    }
    else
    {
        var lines = File.ReadAllLines(gitignorePath);
        if (Array.IndexOf(lines, ".vs/") == -1)
        {
            File.AppendAllText(gitignorePath, Environment.NewLine + ".vs/");
            Console.WriteLine($".vs/ ajouté à {gitignorePath}");
        }
    }
}

static void RunGitCommand(string repoPath, string arguments)
{
    ProcessStartInfo startInfo = new()
    {
        FileName = "git",
        Arguments = arguments,
        WorkingDirectory = repoPath,
        RedirectStandardOutput = true,
        RedirectStandardError = true,
        UseShellExecute = false,
        CreateNoWindow = true
    };

    using var process = Process.Start(startInfo);

    if (process == null) return;

    process.WaitForExit();
    string output = process.StandardOutput.ReadToEnd();
    string error = process.StandardError.ReadToEnd();

    if (process.ExitCode == 0)
    {
        Console.WriteLine($"Commande git réussie: {arguments}");
    }
    else
    {
        Console.WriteLine($"Erreur lors de l'exécution de git {arguments}: {error}");
    }
}

Console.ReadLine();