using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ntlm.Damien.Win
{
    public partial class WarningDialog : Form
    {
        public WarningDialog()
        {
            InitializeComponent();


        }

        public WarningDialog(string[] warnings)
        {
            InitializeComponent();

            warningList.Text = string.Join(Environment.NewLine, warnings);

        }

        private void Avertissements_Load(object sender, EventArgs e)
        {

        }

    }
}
