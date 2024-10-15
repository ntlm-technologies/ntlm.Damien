namespace ntlm.Damien.Win
{
    public partial class WarningDialog : Form
    {
        public WarningDialog()
        {
            InitializeComponent();


        }

        public WarningDialog(IEnumerable<string> warnings)
        {
            InitializeComponent();

            warningList.Text = string.Join(Environment.NewLine, warnings);

        }

        private void Avertissements_Load(object sender, EventArgs e)
        {

        }

    }
}
