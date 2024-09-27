namespace ntlm.Damien.Win
{
    using System.Runtime.CompilerServices;
    using Microsoft.Win32;

    public partial class Main : Form
    {
        private CancellationTokenSource? cancellationTokenSource;

        /// <summary>
        /// Github service handling the clone operations.
        /// </summary>
        public Github Github { get; } = new Github();

        public Main()
        {
            InitializeComponent();

            Github.Logger = new TextBoxWriter(EventConsole);

            HandleCloneVisibility();

            ShowWarnings.Visible = false;

            BasePath.Text = LoadPathFromRegistry(nameof(BasePath));
            Token.Text = LoadPathFromRegistry(nameof(Token));

            Image reducedQuestionMark = ResizeImage(SystemIcons.Question.ToBitmap(), 20, 20);

            TokenQuestionMark.Image = reducedQuestionMark;
            BasePathQuestionMark.Image = reducedQuestionMark;


            TokenToolTip.SetToolTip(TokenQuestionMark, string.Join(Environment.NewLine, new[] {
                "Un personal access token Github est nécessaire pour cloner les dépôts.",
                "Pour en générer un :",
                "- Github.com,",
                "- icône utilisateur en haut à droite,",
                "- Settings,",
                "- Developer settings,",
                "- Personal access tokens,",
                "- Tokens (classic),",
                "- Generate new token (classic),",
                "- attrtibuer un nom,",
                "- attribuer le scope repo,",
                "- copier le token ici.",
            }));

            BasePathToolTip.SetToolTip(BasePathQuestionMark, "Le répertoire local où seront clonés les dépôts.");

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
            Github.BasePath = BasePath.Text;
            Github.Token = Token.Text;
            Github.Fetch = Fetch.Checked;
            cancellationTokenSource = new CancellationTokenSource();
            Github.ProgressChanged += ProgressChanged;
            await Github.CloneAsync(cancellationTokenSource.Token);
            Enable();
        }

        private void ProgressChanged(object? sender, int progress)
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
            browseBasePath.Enabled = true;
            ShowWarnings.Visible = true;
        }

        private void Disable()
        {
            Clone.Enabled = false;
            Token.Enabled = false;
            BasePath.Enabled = false;
            Cancel.Enabled = true;
            browseBasePath.Enabled = false;
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            cancellationTokenSource?.Cancel();
        }

        public const string RegistryKey = @"Software\ntlm.Damien";

        public void SavePathToRegistry(string key, string path)
        {
            // Ouvrir ou créer une sous-clé spécifique à l'application dans le registre
            RegistryKey rKey = Registry.CurrentUser.CreateSubKey(RegistryKey);

            // Enregistrer le chemin dans cette clé
            rKey.SetValue(key, path);

            // Fermer la clé après utilisation
            rKey.Close();
        }

        public string? LoadPathFromRegistry(string key)
        {
            // Ouvrir la sous-clé où le chemin est stocké
            RegistryKey? rKey = Registry.CurrentUser.OpenSubKey(RegistryKey);

            if (rKey != null)
            {
                // Lire la valeur du chemin (ou renvoyer une chaîne vide si elle n'existe pas)
                object path = rKey.GetValue(key, string.Empty);
                rKey.Close();
                return path?.ToString();
            }

            return string.Empty; // Retourne une valeur par défaut si la clé n'existe pas
        }




        private void Main_Load(object sender, EventArgs e)
        {
        }
        private Image ResizeImage(Image image, int width, int height)
        {
            // Créer une nouvelle Bitmap avec les dimensions spécifiées
            Bitmap resizedImage = new Bitmap(width, height);

            // Utiliser Graphics pour dessiner l'image redimensionnée
            using (Graphics graphics = Graphics.FromImage(resizedImage))
            {
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                graphics.DrawImage(image, 0, 0, width, height);
            }

            return resizedImage;
        }

        private void ShowWarnings_Click(object sender, EventArgs e)
        {

        }
    }
}
