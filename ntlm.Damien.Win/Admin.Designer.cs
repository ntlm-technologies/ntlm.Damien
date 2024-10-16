namespace ntlm.Damien.Win
{
    partial class Admin
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Admin));
            teamslbl = new Label();
            label2 = new Label();
            teamsbtn = new Button();
            secretsbtn = new Button();
            host = new TextBox();
            label1 = new Label();
            label3 = new Label();
            username = new TextBox();
            label4 = new Label();
            password = new TextBox();
            label5 = new Label();
            port = new TextBox();
            SuspendLayout();
            // 
            // teamslbl
            // 
            teamslbl.AutoSize = true;
            teamslbl.Location = new Point(12, 9);
            teamslbl.Name = "teamslbl";
            teamslbl.Size = new Size(238, 20);
            teamslbl.TabIndex = 0;
            teamslbl.Text = "Application des droits aux équipes";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 91);
            label2.Name = "label2";
            label2.Size = new Size(191, 20);
            label2.TabIndex = 1;
            label2.Text = "Téléchargement des secrets";
            // 
            // teamsbtn
            // 
            teamsbtn.Location = new Point(12, 32);
            teamsbtn.Name = "teamsbtn";
            teamsbtn.Size = new Size(94, 29);
            teamsbtn.TabIndex = 2;
            teamsbtn.Text = "Equipes";
            teamsbtn.UseVisualStyleBackColor = true;
            teamsbtn.Click += Teamsbtn_Click;
            // 
            // secretsbtn
            // 
            secretsbtn.Location = new Point(90, 255);
            secretsbtn.Name = "secretsbtn";
            secretsbtn.Size = new Size(94, 29);
            secretsbtn.TabIndex = 3;
            secretsbtn.Text = "Secrets";
            secretsbtn.UseVisualStyleBackColor = true;
            secretsbtn.Click += Secretsbtn_Click;
            // 
            // host
            // 
            host.Location = new Point(90, 123);
            host.Name = "host";
            host.Size = new Size(186, 27);
            host.TabIndex = 4;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(42, 126);
            label1.Name = "label1";
            label1.Size = new Size(40, 20);
            label1.TabIndex = 5;
            label1.Text = "Host";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(36, 159);
            label3.Name = "label3";
            label3.Size = new Size(46, 20);
            label3.TabIndex = 7;
            label3.Text = "Login";
            // 
            // username
            // 
            username.Location = new Point(90, 156);
            username.Name = "username";
            username.Size = new Size(186, 27);
            username.TabIndex = 6;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(14, 192);
            label4.Name = "label4";
            label4.Size = new Size(70, 20);
            label4.TabIndex = 9;
            label4.Text = "Password";
            // 
            // password
            // 
            password.Location = new Point(90, 189);
            password.Name = "password";
            password.PasswordChar = '*';
            password.Size = new Size(186, 27);
            password.TabIndex = 8;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(49, 225);
            label5.Name = "label5";
            label5.Size = new Size(35, 20);
            label5.TabIndex = 11;
            label5.Text = "Port";
            // 
            // port
            // 
            port.Location = new Point(90, 222);
            port.Name = "port";
            port.Size = new Size(48, 27);
            port.TabIndex = 10;
            // 
            // Admin
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(298, 298);
            Controls.Add(label5);
            Controls.Add(port);
            Controls.Add(label4);
            Controls.Add(password);
            Controls.Add(label3);
            Controls.Add(username);
            Controls.Add(label1);
            Controls.Add(host);
            Controls.Add(secretsbtn);
            Controls.Add(teamsbtn);
            Controls.Add(label2);
            Controls.Add(teamslbl);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "Admin";
            Text = "Admin";
            FormClosed += Admin_FormClosed;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label teamslbl;
        private Label label2;
        private Button teamsbtn;
        private Button secretsbtn;
        private TextBox host;
        private Label label1;
        private Label label3;
        private TextBox username;
        private Label label4;
        private TextBox password;
        private Label label5;
        private TextBox port;
    }
}