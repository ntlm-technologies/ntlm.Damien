namespace ntlm.Damien.Win
{
    using Microsoft.Win32;
    using Octokit;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Windows.Forms;

    public partial class Main : Form
    {
        public CancellationTokenSource CancellationTokenSource = new();

        /// <summary>
        /// Github service handling the clone operations.
        /// </summary>
        public GithubService Github { get; private set; }

        /// <summary>
        /// Secret service .
        /// </summary>
        public SecretService Secrets { get; private set; }

        public GithubService InitGithub()
        {
            var gs = new GithubService()
            {
                Logger = new TextBoxWriter(eventConsole)
            };
            gs.ProgressChanged += ProgressChanged;
            return gs;
        }

        public SecretService InitSecrets()
        {
            var secrets = new SecretService(Github, new FtpService()
            {
                Logger = new TextBoxWriter(eventConsole)
            })
            {
                Logger = new TextBoxWriter(eventConsole)
            };
            secrets.ProgressChanged += ProgressChanged;
            secrets.Ftp.ProgressChanged += ProgressChanged;
            return secrets;
        }

        public Main()
        {

            InitializeComponent();
            Github = InitGithub();
            Secrets = InitSecrets();

            userName.Visible = false;
            mainPanel.Visible = false;
            avatar.Visible = false;


            avatar.SizeMode = PictureBoxSizeMode.StretchImage; // Ajuste l'image exactement aux dimensions sp�cifi�es
            avatar.BorderStyle = BorderStyle.FixedSingle; // Ajout d'une bordure pour visualiser les dimensions
            avatar.Dock = DockStyle.None; // Emp�che le PictureBox de prendre toute la place du formulaire

            HandleCloneVisibility();

            fetch.Checked = bool.TryParse(GetFromRegistry(nameof(fetch)), out bool isChecked) && isChecked;

            showWarnings.Visible = false;

            basePath.Text = GetFromRegistry(nameof(basePath));
            token.Text = GetFromRegistry(nameof(token));
            branches.Text = GetFromRegistry(nameof(branches),
                string.Join(',', Github.Settings.PreferedBranches)
                );

            if (!string.IsNullOrWhiteSpace(token.Text))
                connect.PerformClick();

            Image reducedQuestionMark = ResizeImage(SystemIcons.Question.ToBitmap(), 20, 20);

            tokenQuestionMark.Image = reducedQuestionMark;
            basePathQuestionMark.Image = reducedQuestionMark;
            branchesQuestionMark.Image = reducedQuestionMark;


            tokenToolTip.SetToolTip(tokenQuestionMark, string.Join(Environment.NewLine, [
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
            ]));

            basePathToolTip.SetToolTip(basePathQuestionMark, "Le r�pertoire local o� seront clon�s les d�p�ts.");

            branchesToolTip.SetToolTip(branchesQuestionMark, "Liste de noms de branches s�par�es par des virgules. Le programme tentera un checkout selon cette priorit�.");
            clientsToolTip.SetToolTip(clientsQuestionMark, "Cochez les clients dont vous souhaitez cloner les repositories.");

            clone.Focus();

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
            Cursor = Cursors.WaitCursor;
            SaveToRegistry(nameof(token), token.Text);
            SaveToRegistry(nameof(branches), branches.Text);
            SaveToRegistry(nameof(basePath), basePath.Text);
            Work();
            Github.BasePath = basePath.Text;
            Github.Token = token.Text;
            Github.Fetch = fetch.Checked;
            Github.Branches = branches.Text.Split(',').Select(b => b.Trim()).ToArray();
            await Github.CloneClientsAsync(
                clients.CheckedItems.Cast<string>().ToArray(),
                CancellationTokenSource.Token
                );
            Done();
            ShowWarningsVisibility();
            Cursor = Cursors.Default;
        }

        private void ShowWarningsVisibility()
        {
            var c = GithubService.Warnings.Count;
            showWarnings.Visible = c > 0;
            showWarnings.Enabled = c > 0;
            showWarnings.Text = string.Format("Avertissement{0} ({1})", c > 1 ? "s" : string.Empty, c);
        }

        private void ProgressChanged(object? sender, int progress)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => {
                    ShowWarningsVisibility();
                    progressBar1.Value = progress;
                }));
            }
            else
            {
                ShowWarningsVisibility();
                progressBar1.Value = progress;
            }
        }

        public void Done()
        {
            Cursor = Cursors.Default;
            connect.Enabled = true;
            admin.Enabled = true;
            clone.Enabled = true;
            token.Enabled = true;
            basePath.Enabled = true;
            cancel.Enabled = false;
            browseBasePath.Enabled = true;
            branches.Enabled = true;
            clients.Enabled = true;
            fetch.Enabled = true;
            if (Admin != null)
                foreach (var item in Admin.Controls.OfType<Control>())
                    item.Enabled = true;
        }

        public void Work()
        {
            Cursor = Cursors.WaitCursor;
            connect.Enabled = false;
            admin.Enabled = false;
            clone.Enabled = false;
            token.Enabled = false;
            basePath.Enabled = false;
            cancel.Enabled = true;
            browseBasePath.Enabled = false;
            branches.Enabled = false;
            clients.Enabled = false;
            fetch.Enabled = false;
            if (Admin != null)
                foreach (var item in Admin.Controls.OfType<Control>())
                    item.Enabled = false;
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            Cancel();
        }

        public void Cancel()
        {
            CancellationTokenSource?.Cancel();
            CancellationTokenSource = new();
        }

        public const string RegistryKey = @"Software\ntlm.Damien";

        public static void SaveToRegistry(string key, string? value)
        {
            // Ouvrir ou cr�er une sous-cl� sp�cifique � l'application dans le registre
            RegistryKey rKey = Registry.CurrentUser.CreateSubKey(RegistryKey);

            // Enregistrer le chemin dans cette cl�
            rKey.SetValue(key, value ?? string.Empty);

            // Fermer la cl� apr�s utilisation
            rKey.Close();
        }

        public static string? GetFromRegistry(string key, string? d = null)
        {
            // Ouvrir la sous-cl� o� le chemin est stock�
            RegistryKey? rKey = Registry.CurrentUser.OpenSubKey(RegistryKey);

            if (rKey != null)
            {
                // Lire la valeur du chemin (ou renvoyer une cha�ne vide si elle n'existe pas)
                object value = rKey.GetValue(key, string.Empty);
                rKey.Close();
                return
                    string.IsNullOrWhiteSpace(value.ToString()) ?
                    d :
                    value?.ToString();
            }

            return d;
        }




        private void Main_Load(object sender, EventArgs e)
        {
        }
        private static Bitmap ResizeImage(Image image, int width, int height)
        {
            // Cr�er une nouvelle Bitmap avec les dimensions sp�cifi�es
            Bitmap resizedImage = new(width, height);

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
            var warnings = new WarningDialog(GithubService.Warnings);
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

        private async void Connect_Click(object sender, EventArgs e)
        {
            mainPanel.Visible = false;
            Github = InitGithub();
            Secrets = InitSecrets();

            token.Enabled = false;
            connect.Enabled = false;
            Cursor = Cursors.WaitCursor;
            var r = await Github.ValidateToken(token.Text);
            SaveToRegistry(nameof(token), token.Text);
            if (r)
            {
                await BindClients();
                mainPanel.Visible = true;
                admin.Visible =
                    ((await Github.GetUserTeamsAsync()).IsOwner(Github.Settings))
                    ;
                var user = await Github.GetUser();
                userName.Text = user?.Login;
                userName.Visible = true;
                LoadAvatar(user);
                avatar.Visible = true;
                mainPanel.Visible = true;
            }
            else
            {
                MessageBox.Show("Token invalide.");
                mainPanel.Visible = false;
                admin.Visible = false;
            }
            token.Enabled = true;
            connect.Enabled = true;
            Cursor = Cursors.Default;
        }

        private async void LoadAvatar(User? user)
        {
            if (user == null) return;
            try
            {
                var httpClient = new HttpClient();

                // T�l�charger les donn�es de l'image avec HttpClient
                var imageBytes = await httpClient.GetByteArrayAsync(user.AvatarUrl);

                // Convertir les donn�es en un flux d'image
                using var ms = new System.IO.MemoryStream(imageBytes);
                avatar.Image = Image.FromStream(ms);
            }
            catch (Exception)
            {
                //MessageBox.Show($"Erreur lors du chargement de l'image : {ex.Message}");
            }
        }


        private async Task BindClients()
        {
            clients.Items.Clear();
            var clientList = await Github.GetClientsAsync();
            var teamList = await Github.GetTeamsAsync();
            var isNtlm = teamList.Any(t => t.IsOwner(Github.Settings));
            foreach (var client in clientList)
            {
                if (isNtlm || client.HasTeam(teamList))
                {
                    var i = clients.Items.Add(client.Name);
                    if (client.HasTeam(teamList))
                        clients.SetItemChecked(i, true);
                }
            }
        }

        //public async Task ApplyPermissionsAsync()
        //{
        //    Work();
        //    await Github.ApplyPermissionsAsync(CancellationTokenSource.Token);
        //    Done();
        //}

        //public async Task DowloadSecretsAsync()
        //{
        //    Work();
        //    await Secrets.Handle(CancellationTokenSource.Token);
        //    Done();
        //}


        private void Admin_Click(object sender, EventArgs e)
        {
            Admin ??= new Admin(this);
            if (Admin.IsDisposed) Admin = new Admin(this);
            Admin.Show();
        }

        public string GetBasePath() => basePath.Text;

        public Admin? Admin { get; private set; }

    }
}
