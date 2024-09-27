namespace ntlm.Damien.Win
{
    using System.Runtime.CompilerServices;
    using Microsoft.Win32;

    public partial class Main : Form
    {
        private CancellationTokenSource? cancellationTokenSource;

        public Main()
        {
            InitializeComponent();

            HandleCloneVisibility();

            BasePath.Text = LoadPathFromRegistry();
        }

        private void BrowseBasePath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog1.ShowDialog();
            BasePath.Text = FolderBrowserDialog1.SelectedPath;
            SavePathToRegistry(BasePath.Text);
            HandleCloneVisibility();
        }

        private void HandleCloneVisibility()
        {
            Clone.Enabled = !string.IsNullOrWhiteSpace(BasePath.Text);
        }

        private async void Clone_Click(object sender, EventArgs e)
        {
            Disable();
            var github = new Github(BasePath.Text, Token.Text)
            {
                Logger = new TextBoxWriter(EventConsole),
                Fetch = Fetch.Checked
            };
            cancellationTokenSource = new CancellationTokenSource();
            github.ProgressChanged += ProgressChanged;
            await github.CloneAsync(cancellationTokenSource.Token);
            Enable();
        }

        private void ProgressChanged(object sender, int progress)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => ProgressBar1.Value = progress));
            }
            else
            {
                ProgressBar1.Value = progress;
            }
        }

        private void Enable()
        {
            Clone.Enabled = true;
            Token.Enabled = true;
            BasePath.Enabled = true;
            Cancel.Enabled = false;
        }

        private void Disable()
        {
            Clone.Enabled = false;
            Token.Enabled = false;
            BasePath.Enabled = false;
            Cancel.Enabled = true;
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            cancellationTokenSource?.Cancel();
        }

        public const string RegistryKey = @"Software\ntlm.Damien";

        public void SavePathToRegistry(string path)
        {
            // Ouvrir ou créer une sous-clé spécifique à l'application dans le registre
            RegistryKey key = Registry.CurrentUser.CreateSubKey(RegistryKey);

            // Enregistrer le chemin dans cette clé
            key.SetValue("LastPath", path);

            // Fermer la clé après utilisation
            key.Close();
        }

        public string? LoadPathFromRegistry()
        {
            // Ouvrir la sous-clé où le chemin est stocké
            RegistryKey? key = Registry.CurrentUser.OpenSubKey(RegistryKey);

            if (key != null)
            {
                // Lire la valeur du chemin (ou renvoyer une chaîne vide si elle n'existe pas)
                object path = key.GetValue("LastPath", string.Empty);
                key.Close();
                return path?.ToString();
            }

            return string.Empty; // Retourne une valeur par défaut si la clé n'existe pas
        }




        private void Main_Load(object sender, EventArgs e)
        {
        }
    }
}
