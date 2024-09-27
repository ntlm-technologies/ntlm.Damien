namespace ntlm.Damien.Win
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Win32;

    public partial class Main : Form
    {
        private CancellationTokenSource? cancellationTokenSource;

        /// <summary>
        /// Github service handling the clone operations.
        /// </summary>
        public Github Github { get; } = new Github();

        /// <summary>
        /// All the available settings.
        /// </summary>
        public GithubSettings[] Settings { get; private set; } = [];

        public Main()
        {
            InitializeComponent();

            Github.Logger = new TextBoxWriter(EventConsole);
            Github.ProgressChanged += ProgressChanged;

            HandleCloneVisibility();
            BindSettings();
            InitializeProfileSelector();

            ShowWarnings.Visible = false;

            BasePath.Text = GetFromRegistry(nameof(BasePath));
            Token.Text = GetFromRegistry(nameof(Token));

            Image reducedQuestionMark = ResizeImage(SystemIcons.Question.ToBitmap(), 20, 20);

            TokenQuestionMark.Image = reducedQuestionMark;
            BasePathQuestionMark.Image = reducedQuestionMark;
            ProfileQuestionMark.Image = reducedQuestionMark;


            TokenToolTip.SetToolTip(TokenQuestionMark, string.Join(Environment.NewLine, new[] {
                "Un 'personal access token' Github est nécessaire pour cloner les dépôts.",
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

            BasePathToolTip.SetToolTip(ProfileQuestionMark, "Un profil fait référence à un projet donné et une liste de dépôt à cloner.");

        }

        /// <summary>
        /// Init profile selector.
        /// </summary>
        private void InitializeProfileSelector()
        {
            // Définir la source de données du ComboBox comme la liste des settings
            Profile.DataSource = Settings;

            // Spécifier le champ à afficher dans le ComboBox (ici "Name" de GithubSettings)
            Profile.DisplayMember = "Name";

            // Gérer l'événement de sélection pour mettre à jour le clone manager
            Profile.SelectedIndexChanged += ProfileSelector_SelectedIndexChanged;

            var registry = GetFromRegistry(nameof(Profile));
            var registrySetting = Settings.FirstOrDefault(x => x.Name == registry);

            if (registrySetting != null)
                Profile.SelectedItem = registrySetting;
            else
                Profile.SelectedIndex = 0;


            Github.Setting = 
                Profile.SelectedItem as GithubSettings
                ;
        }

        /// <summary>
        /// When profile is changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProfileSelector_SelectedIndexChanged(object? sender, EventArgs e)
        {
            // Récupérer le setting sélectionné
            GithubSettings? selectedSetting = Profile.SelectedItem as GithubSettings;

            if (selectedSetting != null)
            {
                // Mettre à jour la propriété Settings du clone manager avec le setting sélectionné
                Github.Setting = selectedSetting;
                SaveToRegistry(nameof(Profile), selectedSetting.Name);
            }
        }


        /// <summary>
        /// Explores the *.sesttings.json of the projects and populates Settings.
        /// </summary>
        private void BindSettings()
        {
            Settings = GetSettings()
                .Select(file =>
                {
                    var builder = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile(file, optional: false, reloadOnChange: true)
                        .Build();
                    return builder
                    .GetSection("AppSettings")
                    .Get<GithubSettings>()
                    ?? new GithubSettings();
                    ;
                })
                .Where(x => x != null)
                .ToArray();
        }

        private void BrowseBasePath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog1.ShowDialog();
            BasePath.Text = FolderBrowserDialog1.SelectedPath;
            SaveToRegistry(nameof(BasePath), BasePath.Text);
            HandleCloneVisibility();
        }

        private void HandleCloneVisibility()
        {
            Clone.Enabled = !string.IsNullOrWhiteSpace(BasePath.Text);
        }

        private async void Clone_Click(object sender, EventArgs e)
        {
            SaveToRegistry(nameof(Token), Token.Text);
            Disable();
            Github.BasePath = BasePath.Text;
            Github.Token = Token.Text;
            Github.Fetch = Fetch.Checked;
            cancellationTokenSource = new CancellationTokenSource();
            await Github.CloneAsync(cancellationTokenSource.Token);
            Enable();
            ShowWarningsVisibility();
        }

        private void ShowWarningsVisibility()
        {
            var c = Github.Warnings.Count();
            ShowWarnings.Visible = c > 0;
            ShowWarnings.Enabled = c > 0;
            ShowWarnings.Text = string.Format("Avertissement{0} ({1})", c > 1 ? "s" : string.Empty, c);
        }

        private void ProgressChanged(object? sender, int progress)
        {
            ShowWarningsVisibility();
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
            BrowseBasePath.Enabled = true;
        }

        private void Disable()
        {
            Clone.Enabled = false;
            Token.Enabled = false;
            BasePath.Enabled = false;
            Cancel.Enabled = true;
            BrowseBasePath.Enabled = false;
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            cancellationTokenSource?.Cancel();
        }

        public const string RegistryKey = @"Software\ntlm.Damien";

        public void SaveToRegistry(string key, string? value)
        {
            // Ouvrir ou créer une sous-clé spécifique à l'application dans le registre
            RegistryKey rKey = Registry.CurrentUser.CreateSubKey(RegistryKey);

            // Enregistrer le chemin dans cette clé
            rKey.SetValue(key, value);

            // Fermer la clé après utilisation
            rKey.Close();
        }

        public string? GetFromRegistry(string key)
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
            var warnings = new WarningDialog(Github.Warnings.ToArray());
            warnings.ShowDialog();
        }

        /// <summary>
        /// Returns all the settings available in project.
        /// </summary>
        /// <returns></returns>
        public string[] GetSettings()
            => Directory.GetFiles(
                Directory.GetCurrentDirectory()
                , "*.settings.json",
                SearchOption.AllDirectories
                );


    }
}
