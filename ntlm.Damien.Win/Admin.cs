using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ntlm.Damien.Win
{
    public partial class Admin : Form
    {
        public Admin(
            Main main
            )
        {
            InitializeComponent();
            Main = main;

            host.Text = Main.GetFromRegistry(nameof(host));
            username.Text = Main.GetFromRegistry(nameof(username));
            password.Text = Main.GetFromRegistry(nameof(password));
            port.Text = Main.GetFromRegistry(nameof(port)) ?? "21";

        }

        public Main Main { get; }

        private void Admin_FormClosed(object sender, EventArgs e)
        {
            Main.CancellationTokenSource?.Cancel();
        }

        private async void Teamsbtn_Click(object sender, EventArgs e)
        {
            Main.Work();

            await Task.Run(async () =>
            {
                await Main.Github.ApplyPermissionsAsync(Main.CancellationTokenSource.Token);

            });

            Main.Done();
        }

        private async void Secretsbtn_Click(object sender, EventArgs e)
        {   
            Main.Work();

            await Task.Run(async () =>
            {
                Main.SaveToRegistry(nameof(host), host.Text);
                Main.SaveToRegistry(nameof(username), username.Text);
                Main.SaveToRegistry(nameof(password), password.Text);
                Main.SaveToRegistry(nameof(port), port.Text);
                Main.Secrets.Ftp.Host = host.Text;
                Main.Secrets.Ftp.Username = username.Text;
                Main.Secrets.Ftp.Password = password.Text;

                _ = int.TryParse(port.Text, out int p);
                Main.Secrets.Ftp.Port = p;

                await Main.Secrets.Handle(Main.CancellationTokenSource.Token);

            });


            Main.Done();
        }
    }
}
