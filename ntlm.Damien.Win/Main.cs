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

            Github.Logger = new TextBoxWriter(eventConsole);
            Github.ProgressChanged += ProgressChanged;

            HandleCloneVisibility();
            BindSettings();
            InitializeProfileSelector();

            fetch.Checked = bool.TryParse(GetFromRegistry(nameof(fetch)), out bool isChecked) && isChecked;

            showWarnings.Visible = false;

            basePath.Text = GetFromRegistry(nameof(basePath));
            token.Text = GetFromRegistry(nameof(token));

            Image reducedQuestionMark = ResizeImage(SystemIcons.Question.ToBitmap(), 20, 20);

            tokenQuestionMark.Image = reducedQuestionMark;
            basePathQuestionMark.Image = reducedQuestionMark;
            profileQuestionMark.Image = reducedQuestionMark;


            tokenToolTip.SetToolTip(tokenQuestionMark, string.Join(Environment.NewLine, [
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
            ]));

            basePathToolTip.SetToolTip(basePathQuestionMark, "Le répertoire local où seront clonés les dépôts.");

            basePathToolTip.SetToolTip(profileQuestionMark, "Un profil fait référence à un projet donné et une liste de dépôts à cloner.");

        }

        /// <summary>
        /// Init profile selector.
        /// </summary>
        private void InitializeProfileSelector()
        {
            // Définir la source de données du ComboBox comme la liste des settings
            profile.DataSource = Settings;

            // Spécifier le champ à afficher dans le ComboBox (ici "Name" de GithubSettings)
            profile.DisplayMember = "Name";

            // Gérer l'événement de sélection pour mettre à jour le clone manager
            profile.SelectedIndexChanged += ProfileSelector_SelectedIndexChanged;

            var registry = GetFromRegistry(nameof(profile));
            var registrySetting = Settings.FirstOrDefault(x => x.Name == registry);

            if (registrySetting != null)
                profile.SelectedItem = registrySetting;
            else
                profile.SelectedIndex = 0;


            Github.Setting =
                profile.SelectedItem as GithubSettings
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

            if (profile.SelectedItem is GithubSettings selectedSetting)
            {
                // Mettre à jour la propriété Settings du clone manager avec le setting sélectionné
                Github.Setting = selectedSetting;
                SaveToRegistry(nameof(profile), selectedSetting.Name);
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
            folderBrowserDialog1.ShowDialog();
            basePath.Text = folderBrowserDialog1.SelectedPath;
            SaveToRegistry(nameof(basePath), basePath.Text);
            HandleCloneVisibility();
        }

        private void HandleCloneVisibility()
        {
            clone.Enabled = !string.IsNullOrWhiteSpace(basePath.Text);
        }

        private async void Clone_Click(object sender, EventArgs e)
        {
            SaveToRegistry(nameof(token), token.Text);
            Disable();
            Github.BasePath = basePath.Text;
            Github.Token = token.Text;
            Github.Fetch = fetch.Checked;
            cancellationTokenSource = new CancellationTokenSource();
            await Github.CloneAsync(cancellationTokenSource.Token);
            Enable();
            ShowWarningsVisibility();
        }

        private void ShowWarningsVisibility()
        {
            var c = Github.Warnings.Count;
            showWarnings.Visible = c > 0;
            showWarnings.Enabled = c > 0;
            showWarnings.Text = string.Format("Avertissement{0} ({1})", c > 1 ? "s" : string.Empty, c);
        }

        private void ProgressChanged(object? sender, int progress)
        {
            ShowWarningsVisibility();
            if (InvokeRequired)
            {
                Invoke(new Action(() => progressBar1.Value = progress));
            }
            else
            {
                progressBar1.Value = progress;
            }
        }

        private void Enable()
        {
            clone.Enabled = true;
            token.Enabled = true;
            basePath.Enabled = true;
            cancel.Enabled = false;
            browseBasePath.Enabled = true;
        }

        private void Disable()
        {
            clone.Enabled = false;
            token.Enabled = false;
            basePath.Enabled = false;
            cancel.Enabled = true;
            browseBasePath.Enabled = false;
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            cancellationTokenSource?.Cancel();
        }

        public const string RegistryKey = @"Software\ntlm.Damien";

        public static void SaveToRegistry(string key, string? value)
        {
            // Ouvrir ou créer une sous-clé spécifique à l'application dans le registre
            RegistryKey rKey = Registry.CurrentUser.CreateSubKey(RegistryKey);

            // Enregistrer le chemin dans cette clé
            rKey.SetValue(key, value ?? string.Empty);

            // Fermer la clé après utilisation
            rKey.Close();
        }

        public static string? GetFromRegistry(string key)
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
        private static Bitmap ResizeImage(Image image, int width, int height)
        {
            // Créer une nouvelle Bitmap avec les dimensions spécifiées
            Bitmap resizedImage = new(width, height);

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
            var warnings = new WarningDialog(Github.Warnings);
            warnings.ShowDialog();
        }

        /// <summary>
        /// Returns all the settings available in project.
        /// </summary>
        /// <returns></returns>
        public static string[] GetSettings()
            => Directory.GetFiles(
                Directory.GetCurrentDirectory()
                , "*.settings.json",
                SearchOption.AllDirectories
                );

        private void Fetch_CheckedChanged(object sender, EventArgs e)
        {
            SaveToRegistry(nameof(fetch), fetch.Checked.ToString());
        }
    }
}
