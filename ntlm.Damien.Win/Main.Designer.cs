namespace ntlm.Damien.Win
{
    partial class Main
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            label1 = new Label();
            token = new TextBox();
            folderBrowserDialog1 = new FolderBrowserDialog();
            label2 = new Label();
            browseBasePath = new Button();
            backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            clone = new Button();
            eventConsole = new TextBox();
            progressBar1 = new ProgressBar();
            fetch = new CheckBox();
            basePath = new TextBox();
            cancel = new Button();
            tokenQuestionMark = new PictureBox();
            basePathQuestionMark = new PictureBox();
            tokenToolTip = new ToolTip(components);
            basePathToolTip = new ToolTip(components);
            showWarnings = new Button();
            branchesQuestionMark = new PictureBox();
            label3 = new Label();
            branchesToolTip = new ToolTip(components);
            branches = new TextBox();
            label4 = new Label();
            clientsQuestionMark = new PictureBox();
            clientsToolTip = new ToolTip(components);
            connect = new Button();
            mainPanel = new Panel();
            admin = new Button();
            avatar = new PictureBox();
            userName = new Label();
            clients = new CheckedListBox();
            ((System.ComponentModel.ISupportInitialize)tokenQuestionMark).BeginInit();
            ((System.ComponentModel.ISupportInitialize)basePathQuestionMark).BeginInit();
            ((System.ComponentModel.ISupportInitialize)branchesQuestionMark).BeginInit();
            ((System.ComponentModel.ISupportInitialize)clientsQuestionMark).BeginInit();
            mainPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)avatar).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 16);
            label1.Name = "label1";
            label1.Size = new Size(96, 20);
            label1.TabIndex = 0;
            label1.Text = "Github Token";
            // 
            // token
            // 
            token.Location = new Point(143, 13);
            token.Name = "token";
            token.PasswordChar = '*';
            token.Size = new Size(392, 27);
            token.TabIndex = 1;
            token.Text = "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";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(11, 55);
            label2.Name = "label2";
            label2.Size = new Size(85, 20);
            label2.TabIndex = 2;
            label2.Text = "Destination";
            // 
            // browseBasePath
            // 
            browseBasePath.Location = new Point(525, 49);
            browseBasePath.Name = "browseBasePath";
            browseBasePath.Size = new Size(99, 33);
            browseBasePath.TabIndex = 4;
            browseBasePath.Text = "Parcourir";
            browseBasePath.UseVisualStyleBackColor = true;
            browseBasePath.Click += BrowseBasePath_Click;
            // 
            // clone
            // 
            clone.Enabled = false;
            clone.Location = new Point(131, 249);
            clone.Name = "clone";
            clone.Size = new Size(99, 33);
            clone.TabIndex = 5;
            clone.Text = "Cloner";
            clone.UseVisualStyleBackColor = true;
            clone.Click += Clone_Click;
            // 
            // eventConsole
            // 
            eventConsole.Location = new Point(4, 288);
            eventConsole.Multiline = true;
            eventConsole.Name = "eventConsole";
            eventConsole.ReadOnly = true;
            eventConsole.ScrollBars = ScrollBars.Vertical;
            eventConsole.Size = new Size(619, 223);
            eventConsole.TabIndex = 6;
            eventConsole.WordWrap = false;
            // 
            // progressBar1
            // 
            progressBar1.Location = new Point(4, 517);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(619, 29);
            progressBar1.TabIndex = 7;
            // 
            // fetch
            // 
            fetch.AutoSize = true;
            fetch.Location = new Point(131, 119);
            fetch.Name = "fetch";
            fetch.Size = new Size(250, 24);
            fetch.TabIndex = 8;
            fetch.Text = "Mettre à jour les dépôts existants";
            fetch.UseVisualStyleBackColor = true;
            fetch.CheckedChanged += Fetch_CheckedChanged;
            // 
            // basePath
            // 
            basePath.Location = new Point(131, 52);
            basePath.Name = "basePath";
            basePath.Size = new Size(392, 27);
            basePath.TabIndex = 9;
            basePath.Text = "C:\\Users\\Dell\\Desktop\\ntlm.Damien";
            // 
            // cancel
            // 
            cancel.Enabled = false;
            cancel.Location = new Point(236, 249);
            cancel.Name = "cancel";
            cancel.Size = new Size(99, 33);
            cancel.TabIndex = 10;
            cancel.Text = "Annuler";
            cancel.UseVisualStyleBackColor = true;
            cancel.Click += Cancel_Click;
            // 
            // tokenQuestionMark
            // 
            tokenQuestionMark.Location = new Point(108, 15);
            tokenQuestionMark.Name = "tokenQuestionMark";
            tokenQuestionMark.Size = new Size(29, 21);
            tokenQuestionMark.TabIndex = 11;
            tokenQuestionMark.TabStop = false;
            // 
            // basePathQuestionMark
            // 
            basePathQuestionMark.Location = new Point(96, 54);
            basePathQuestionMark.Name = "basePathQuestionMark";
            basePathQuestionMark.Size = new Size(29, 25);
            basePathQuestionMark.TabIndex = 12;
            basePathQuestionMark.TabStop = false;
            // 
            // tokenToolTip
            // 
            tokenToolTip.AutoPopDelay = 20000;
            tokenToolTip.InitialDelay = 500;
            tokenToolTip.ReshowDelay = 100;
            // 
            // showWarnings
            // 
            showWarnings.AutoEllipsis = true;
            showWarnings.BackColor = SystemColors.Control;
            showWarnings.Enabled = false;
            showWarnings.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            showWarnings.ForeColor = Color.DarkOrange;
            showWarnings.Location = new Point(341, 249);
            showWarnings.Name = "showWarnings";
            showWarnings.Size = new Size(182, 33);
            showWarnings.TabIndex = 13;
            showWarnings.Text = "Avertissement";
            showWarnings.UseVisualStyleBackColor = false;
            showWarnings.Click += ShowWarnings_Click;
            // 
            // branchesQuestionMark
            // 
            branchesQuestionMark.Location = new Point(96, 88);
            branchesQuestionMark.Name = "branchesQuestionMark";
            branchesQuestionMark.Size = new Size(29, 25);
            branchesQuestionMark.TabIndex = 16;
            branchesQuestionMark.TabStop = false;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(28, 88);
            label3.Name = "label3";
            label3.Size = new Size(68, 20);
            label3.TabIndex = 15;
            label3.Text = "Branches";
            // 
            // branches
            // 
            branches.Location = new Point(131, 86);
            branches.Name = "branches";
            branches.Size = new Size(392, 27);
            branches.TabIndex = 17;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(37, 149);
            label4.Name = "label4";
            label4.Size = new Size(53, 20);
            label4.TabIndex = 19;
            label4.Text = "Clients";
            // 
            // clientsQuestionMark
            // 
            clientsQuestionMark.Location = new Point(96, 148);
            clientsQuestionMark.Name = "clientsQuestionMark";
            clientsQuestionMark.Size = new Size(29, 25);
            clientsQuestionMark.TabIndex = 20;
            clientsQuestionMark.TabStop = false;
            // 
            // connect
            // 
            connect.Location = new Point(536, 12);
            connect.Name = "connect";
            connect.Size = new Size(99, 29);
            connect.TabIndex = 21;
            connect.Text = "Connecter";
            connect.UseVisualStyleBackColor = true;
            connect.Click += Connect_Click;
            // 
            // mainPanel
            // 
            mainPanel.Controls.Add(admin);
            mainPanel.Controls.Add(avatar);
            mainPanel.Controls.Add(userName);
            mainPanel.Controls.Add(clients);
            mainPanel.Controls.Add(progressBar1);
            mainPanel.Controls.Add(eventConsole);
            mainPanel.Controls.Add(clientsQuestionMark);
            mainPanel.Controls.Add(basePath);
            mainPanel.Controls.Add(label4);
            mainPanel.Controls.Add(label2);
            mainPanel.Controls.Add(browseBasePath);
            mainPanel.Controls.Add(branches);
            mainPanel.Controls.Add(clone);
            mainPanel.Controls.Add(branchesQuestionMark);
            mainPanel.Controls.Add(fetch);
            mainPanel.Controls.Add(label3);
            mainPanel.Controls.Add(cancel);
            mainPanel.Controls.Add(showWarnings);
            mainPanel.Controls.Add(basePathQuestionMark);
            mainPanel.Location = new Point(12, 45);
            mainPanel.Name = "mainPanel";
            mainPanel.Size = new Size(627, 549);
            mainPanel.TabIndex = 22;
            // 
            // admin
            // 
            admin.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            admin.ForeColor = Color.Red;
            admin.Location = new Point(524, 10);
            admin.Name = "admin";
            admin.Size = new Size(99, 29);
            admin.TabIndex = 25;
            admin.Text = "Admin";
            admin.UseVisualStyleBackColor = true;
            admin.Click += Admin_Click;
            // 
            // avatar
            // 
            avatar.Location = new Point(131, 1);
            avatar.Name = "avatar";
            avatar.Size = new Size(45, 45);
            avatar.TabIndex = 24;
            avatar.TabStop = false;
            // 
            // userName
            // 
            userName.AutoSize = true;
            userName.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            userName.ForeColor = Color.RoyalBlue;
            userName.Location = new Point(182, 14);
            userName.Name = "userName";
            userName.Size = new Size(51, 20);
            userName.TabIndex = 23;
            userName.Text = "label5";
            // 
            // clients
            // 
            clients.FormattingEnabled = true;
            clients.Location = new Point(131, 148);
            clients.Name = "clients";
            clients.Size = new Size(392, 92);
            clients.TabIndex = 21;
            // 
            // Main
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(651, 604);
            Controls.Add(mainPanel);
            Controls.Add(connect);
            Controls.Add(tokenQuestionMark);
            Controls.Add(token);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "Main";
            Text = "Ntlm Github Manager";
            Load += Main_Load;
            ((System.ComponentModel.ISupportInitialize)tokenQuestionMark).EndInit();
            ((System.ComponentModel.ISupportInitialize)basePathQuestionMark).EndInit();
            ((System.ComponentModel.ISupportInitialize)branchesQuestionMark).EndInit();
            ((System.ComponentModel.ISupportInitialize)clientsQuestionMark).EndInit();
            mainPanel.ResumeLayout(false);
            mainPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)avatar).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox token;
        private FolderBrowserDialog folderBrowserDialog1;
        private Label label2;
        private Button browseBasePath;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private Button clone;
        private TextBox eventConsole;
        private ProgressBar progressBar1;
        private CheckBox fetch;
        private TextBox basePath;
        private Button cancel;
        private PictureBox tokenQuestionMark;
        private PictureBox basePathQuestionMark;
        private ToolTip tokenToolTip;
        private ToolTip basePathToolTip;
        private Button showWarnings;
        private PictureBox branchesQuestionMark;
        private Label label3;
        private ToolTip branchesToolTip;
        private TextBox branches;
        private Label label4;
        private PictureBox clientsQuestionMark;
        private ToolTip clientsToolTip;
        private Button connect;
        private Panel mainPanel;
        private CheckedListBox clients;
        private Label userName;
        private PictureBox avatar;
        private Button admin;
    }
}
