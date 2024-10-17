using LibGit2Sharp;
using System.IO.Compression;

class Program
{
    static void Main(string[] args)
    {
        // Spécifiez le chemin du dépôt Git
        string repoPath = @"C:\Applications\Ntlm\ntlm.Damien.Release\"; // Modifiez selon votre environnement
        string publishDirectory = Path.Combine(repoPath, "publish"); // Chemin vers le répertoire 'publish'
        string zipPath = Path.Combine(repoPath, "NtlmGithubManager.zip"); // Chemin vers le fichier ZIP

        // Message du commit
        string commitMessage = "Commit automatique après publication";

        // Informations de l'utilisateur Git
        string authorName = "Nicolas Taret";
        string authorEmail = "mambojoel@gmail.com";

        // Credentials pour le push (si nécessaire pour un dépôt distant privé)
        var username = "MamboJoel";
        var token = "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";

        try
        {
            // 1. Zipper le répertoire 'publish'
            if (Directory.Exists(publishDirectory))
            {
                if (File.Exists(zipPath))
                {
                    File.Delete(zipPath); // Supprimer le fichier ZIP s'il existe déjà
                }

                // Créer un fichier ZIP à partir du répertoire 'publish'
                ZipFile.CreateFromDirectory(publishDirectory, zipPath);
                Console.WriteLine($"Le répertoire '{publishDirectory}' a été zippé avec succès vers '{zipPath}'.");
            }
            else
            {
                Console.WriteLine($"Le répertoire '{publishDirectory}' n'existe pas.");
                return;
            }

            // 2. Ouvrir le dépôt Git
            using (var repo = new Repository(repoPath))
            {
                // Ajouter tous les fichiers modifiés
                Commands.Stage(repo, "*");

                // Créer le commit
                Signature author = new Signature(authorName, authorEmail, DateTime.Now);
                Signature committer = author;
                Commit commit = repo.Commit(commitMessage, author, committer);

                Console.WriteLine($"Commit effectué avec succès : {commit.Sha}");

                // Pousser les changements vers le dépôt distant (si nécessaire)
                var options = new PushOptions
                {
                    CredentialsProvider = (url, usernameFromUrl, types) =>
                        new UsernamePasswordCredentials { Username = username, Password = token }
                };

                // Pousser sur la branche 'main' (modifiez si nécessaire)
                repo.Network.Push(repo.Branches["main"], options);

                Console.WriteLine("Push effectué avec succès !");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Une erreur est survenue : {ex.Message}");
        }
    }
}
