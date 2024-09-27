namespace ntlm.Damien.Win
{
    using System.Runtime.CompilerServices;

    public partial class Main : Form
    {
        private CancellationTokenSource? cancellationTokenSource;

        public Main()
        {
            InitializeComponent();

            HandleCloneVisibility();
        }

        private void BrowseBasePath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog1.ShowDialog();
            BasePath.Text = FolderBrowserDialog1.SelectedPath;
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

        private void Main_Load(object sender, EventArgs e)
        {
        }
    }
}
