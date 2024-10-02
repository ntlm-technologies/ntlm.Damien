using LibGit2Sharp;
using System;
using System.Diagnostics;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        // Chemin de base à parcourir
        string baseDirectory = @"C:\Users\mambo\OneDrive\Desktop\ntlm.Damien"; ; // À adapter

        // Recherche récursive des répertoires contenant des repositories Git
        ProcessRepositories(baseDirectory);
    }

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
        if (repoPath.EndsWith("ntlm.Damien"))
            return;

        Console.WriteLine($"Traitement du repository: {repoPath}");

        using (var repo = new Repository(repoPath))
        {
            // Vérification et mise à jour du fichier .gitignore
            string gitignorePath = Path.Combine(repoPath, ".gitignore");
            bool gitignoreUpdated = EnsureVsFolderIgnored(gitignorePath);

            // Si le fichier .gitignore a été mis à jour, commit et push
            if (gitignoreUpdated)
            {
                // Ajouter et commit le fichier .gitignore mis à jour
                Commands.Stage(repo, gitignorePath);
                repo.Commit("Ignore .vs", new Signature("Automated Script", "script@example.com", DateTimeOffset.Now), new Signature("Automated Script", "script@example.com", DateTimeOffset.Now));

                // Push la branche courante avec le nouveau .gitignore
                var currentBranchName = repo.Head.FriendlyName;
                RunGitCommand(repoPath, $"push origin {currentBranchName}");
                Console.WriteLine($"Modifications du .gitignore poussées sur la branche {currentBranchName}");
            }

            // Supprimer le dossier .vs/ du suivi
            RunGitCommand(repoPath, "rm -r --cached .vs/");

            // Committer le retrait des fichiers .vs/
            RunGitCommand(repoPath, "commit -m \"Retire le dossier .vs/ du suivi de version\"");

            // Pousser la branche courante
            RunGitCommand(repoPath, $"push origin {repo.Head.FriendlyName}");
        }
    }

    static bool EnsureVsFolderIgnored(string gitignorePath)
    {
        bool isUpdated = false;

        // Si le fichier .gitignore n'existe pas, le créer
        if (!File.Exists(gitignorePath))
        {
            File.WriteAllText(gitignorePath, ".vs/");
            Console.WriteLine($".gitignore créé avec '.vs/' à {gitignorePath}");
            isUpdated = true;
        }
        else
        {
            var lines = File.ReadAllLines(gitignorePath);
            if (Array.IndexOf(lines, ".vs/") == -1)
            {
                File.AppendAllText(gitignorePath, Environment.NewLine + ".vs/");
                Console.WriteLine($".vs/ ajouté à {gitignorePath}");
                isUpdated = true;
            }
        }

        return isUpdated;
    }

    static void RunGitCommand(string repoPath, string arguments)
    {
        ProcessStartInfo startInfo = new ProcessStartInfo
        {
            FileName = "git",
            Arguments = arguments,
            WorkingDirectory = repoPath,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using (var process = Process.Start(startInfo))
        {
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
    }
}
