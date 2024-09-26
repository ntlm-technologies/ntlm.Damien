using System.Runtime.CompilerServices;

namespace ntlm.Damien.Win
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void browseBasePath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog1.ShowDialog();
            BasePath.Text = FolderBrowserDialog1.SelectedPath;
            Clone.Enabled = !string.IsNullOrWhiteSpace(FolderBrowserDialog1.SelectedPath);
        }

        private void Clone_Click(object sender, EventArgs e)
        {
            Disable();
            var github = new Github(BasePath.Text, Token.Text) { 
                Logger = new TextBoxWriter(EventConsole)
            };
            github.ProgressChanged += ProgressChanged;
            github.Clone();
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
        }

        private void Disable()
        {
            Clone.Enabled = false;
            Token.Enabled = false;
        }
    }
}
