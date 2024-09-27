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

            BasePath.Text = LoadPathFromRegistry(nameof(BasePath));
            Token.Text = LoadPathFromRegistry(nameof(Token));
        }

        private void BrowseBasePath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog1.ShowDialog();
            BasePath.Text = FolderBrowserDialog1.SelectedPath;
            SavePathToRegistry(nameof(BasePath), BasePath.Text);
            HandleCloneVisibility();
        }

        private void HandleCloneVisibility()
        {
            Clone.Enabled = !string.IsNullOrWhiteSpace(BasePath.Text);
        }

        private async void Clone_Click(object sender, EventArgs e)
        {
            SavePathToRegistry(nameof(Token), Token.Text);
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

        public void SavePathToRegistry(string key, string path)
        {
            // Ouvrir ou cr�er une sous-cl� sp�cifique � l'application dans le registre
            RegistryKey rKey = Registry.CurrentUser.CreateSubKey(RegistryKey);

            // Enregistrer le chemin dans cette cl�
            rKey.SetValue(key, path);

            // Fermer la cl� apr�s utilisation
            rKey.Close();
        }

        public string? LoadPathFromRegistry(string key)
        {
            // Ouvrir la sous-cl� o� le chemin est stock�
            RegistryKey? rKey = Registry.CurrentUser.OpenSubKey(RegistryKey);

            if (rKey != null)
            {
                // Lire la valeur du chemin (ou renvoyer une cha�ne vide si elle n'existe pas)
                object path = rKey.GetValue(key, string.Empty);
                rKey.Close();
                return path?.ToString();
            }

            return string.Empty; // Retourne une valeur par d�faut si la cl� n'existe pas
        }




        private void Main_Load(object sender, EventArgs e)
        {
        }
    }
}
