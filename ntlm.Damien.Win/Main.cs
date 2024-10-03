namespace ntlm.Damien.Win
{
    using Microsoft.Win32;
    using Octokit;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Windows.Forms;

    public partial class Main : Form
    {
        private readonly CancellationTokenSource cancellationTokenSource = new();

        /// <summary>
        /// Github service handling the clone operations.
        /// </summary>
        public GithubService Github { get; private set; }

        public GithubService InitGithub()
        {
            var gs = Github = new GithubService()
            {
                Logger = new TextBoxWriter(eventConsole)
            };
            Github.ProgressChanged += ProgressChanged;
            return gs;
        }



        public Main()
        {
            InitializeComponent();

            Github = InitGithub();

            userName.Visible = false;
            mainPanel.Visible = false;
            teams.Visible = false;
            avatar.Visible = false;


            avatar.SizeMode = PictureBoxSizeMode.StretchImage; // Ajuste l'image exactement aux dimensions spécifiées
            avatar.BorderStyle = BorderStyle.FixedSingle; // Ajout d'une bordure pour visualiser les dimensions
            avatar.Dock = DockStyle.None; // Empêche le PictureBox de prendre toute la place du formulaire

            HandleCloneVisibility();

            fetch.Checked = bool.TryParse(GetFromRegistry(nameof(fetch)), out bool isChecked) && isChecked;

            showWarnings.Visible = false;

            basePath.Text = GetFromRegistry(nameof(basePath));
            token.Text = GetFromRegistry(nameof(token));
            branches.Text = GetFromRegistry(nameof(branches), "to-dotnet-8, dev, test");

            if (!string.IsNullOrWhiteSpace(token.Text))
                connect.PerformClick();

            Image reducedQuestionMark = ResizeImage(SystemIcons.Question.ToBitmap(), 20, 20);

            tokenQuestionMark.Image = reducedQuestionMark;
            basePathQuestionMark.Image = reducedQuestionMark;
            branchesQuestionMark.Image = reducedQuestionMark;


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

            branchesToolTip.SetToolTip(branchesQuestionMark, "Liste de noms de branches séparées par des virgules. Le programme tentera un checkout selon cette priorité.");
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
                cancellationTokenSource.Token
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

        private void Done()
        {
            Cursor = Cursors.Default;
            connect.Enabled = true;
            teams.Enabled = true;
            clone.Enabled = true;
            token.Enabled = true;
            basePath.Enabled = true;
            cancel.Enabled = false;
            browseBasePath.Enabled = true;
            branches.Enabled = true;
            clients.Enabled = true;
            fetch.Enabled = true;
        }

        private void Work()
        {
            Cursor = Cursors.WaitCursor;
            connect.Enabled = false;
            teams.Enabled = false;
            clone.Enabled = false;
            token.Enabled = false;
            basePath.Enabled = false;
            cancel.Enabled = true;
            browseBasePath.Enabled = false;
            branches.Enabled = false;
            clients.Enabled = false;
            fetch.Enabled = false;
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

        public static string? GetFromRegistry(string key, string? d = null)
        {
            // Ouvrir la sous-clé où le chemin est stocké
            RegistryKey? rKey = Registry.CurrentUser.OpenSubKey(RegistryKey);

            if (rKey != null)
            {
                // Lire la valeur du chemin (ou renvoyer une chaîne vide si elle n'existe pas)
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

            token.Enabled = false;
            connect.Enabled = false;
            Cursor = Cursors.WaitCursor;
            var r = await Github.ValidateToken(token.Text);
            SaveToRegistry(nameof(token), token.Text);
            if (r)
            {
                await BindClients();
                mainPanel.Visible = true;
                teams.Visible =
                    ((await Github.GetUserTeamsAsync()).IsNtlm())
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
                teams.Visible = false;
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

                // Télécharger les données de l'image avec HttpClient
                var imageBytes = await httpClient.GetByteArrayAsync(user.AvatarUrl);

                // Convertir les données en un flux d'image
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
            var isNtlm = teamList.Any(t => t.IsNtlm());
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

        private async void Teams_Click(object sender, EventArgs e)
        {
            Work();
            await Github.ApplyPermissionsAsync(cancellationTokenSource.Token);
            Done();
        }
    }
}
