using LibGit2Sharp;
using ntlm.Damien.Publish;
using System.IO.Compression;

class Program
{
    static void Main()
    {
        Settings settings = new Settings();

        // Utilisation des propriétés de l'objet Settings
        string repoPath = settings.RepoPath;
        string publishDirectory = settings.PublishDirectory;
        string zipPath = settings.ZipPath;
        string commitMessage = settings.CommitMessage;
        string authorName = settings.GitUser.AuthorName;
        string authorEmail = settings.GitUser.AuthorEmail;
        string username = settings.Credentials.Username;
        string token = settings.Credentials.Token;

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
            using var repo = new Repository(repoPath);
            // Ajouter tous les fichiers modifiés
            Commands.Stage(repo, "*");

            // Créer le commit
            Signature author = new (authorName, authorEmail, DateTime.Now);
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
        catch (Exception ex)
        {
            Console.WriteLine($"Une erreur est survenue : {ex.Message}");
        }
    }
}
