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
                "Un 'personal access token' Github est n�cessaire pour cloner les d�p�ts.",
                "Pour en g�n�rer un :",
                "- Github.com,",
                "- ic�ne utilisateur en haut � droite,",
                "- Settings,",
                "- Developer settings,",
                "- Personal access tokens,",
                "- Tokens (classic),",
                "- Generate new token (classic),",
                "- attrtibuer un nom,",
                "- attribuer le scope repo,",
                "- copier le token ici.",
            }));

            BasePathToolTip.SetToolTip(BasePathQuestionMark, "Le r�pertoire local o� seront clon�s les d�p�ts.");

            BasePathToolTip.SetToolTip(ProfileQuestionMark, "Un profil fait r�f�rence � un projet donn� et une liste de d�p�t � cloner.");

        }

        /// <summary>
        /// Init profile selector.
        /// </summary>
        private void InitializeProfileSelector()
        {
            // D�finir la source de donn�es du ComboBox comme la liste des settings
            Profile.DataSource = Settings;

            // Sp�cifier le champ � afficher dans le ComboBox (ici "Name" de GithubSettings)
            Profile.DisplayMember = "Name";

            // G�rer l'�v�nement de s�lection pour mettre � jour le clone manager
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
            // R�cup�rer le setting s�lectionn�
            GithubSettings? selectedSetting = Profile.SelectedItem as GithubSettings;

            if (selectedSetting != null)
            {
                // Mettre � jour la propri�t� Settings du clone manager avec le setting s�lectionn�
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
            // Ouvrir ou cr�er une sous-cl� sp�cifique � l'application dans le registre
            RegistryKey rKey = Registry.CurrentUser.CreateSubKey(RegistryKey);

            // Enregistrer le chemin dans cette cl�
            rKey.SetValue(key, value);

            // Fermer la cl� apr�s utilisation
            rKey.Close();
        }

        public string? GetFromRegistry(string key)
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
        private Image ResizeImage(Image image, int width, int height)
        {
            // Cr�er une nouvelle Bitmap avec les dimensions sp�cifi�es
            Bitmap resizedImage = new Bitmap(width, height);

            // Utiliser Graphics pour dessiner l'image redimensionn�e
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
