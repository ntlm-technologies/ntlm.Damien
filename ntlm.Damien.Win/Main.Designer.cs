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
            profile = new ComboBox();
            profileQuestionMark = new PictureBox();
            label3 = new Label();
            profileToolTip = new ToolTip(components);
            ((System.ComponentModel.ISupportInitialize)tokenQuestionMark).BeginInit();
            ((System.ComponentModel.ISupportInitialize)basePathQuestionMark).BeginInit();
            ((System.ComponentModel.ISupportInitialize)profileQuestionMark).BeginInit();
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
            token.Size = new Size(392, 27);
            token.TabIndex = 1;
            token.Text = "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";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(23, 49);
            label2.Name = "label2";
            label2.Size = new Size(85, 20);
            label2.TabIndex = 2;
            label2.Text = "Destination";
            // 
            // browseBasePath
            // 
            browseBasePath.Location = new Point(536, 45);
            browseBasePath.Name = "browseBasePath";
            browseBasePath.Size = new Size(99, 29);
            browseBasePath.TabIndex = 4;
            browseBasePath.Text = "Parcourir";
            browseBasePath.UseVisualStyleBackColor = true;
            browseBasePath.Click += BrowseBasePath_Click;
            // 
            // clone
            // 
            clone.Enabled = false;
            clone.Location = new Point(143, 143);
            clone.Name = "clone";
            clone.Size = new Size(99, 29);
            clone.TabIndex = 5;
            clone.Text = "Cloner";
            clone.UseVisualStyleBackColor = true;
            clone.Click += Clone_Click;
            // 
            // eventConsole
            // 
            eventConsole.Location = new Point(12, 178);
            eventConsole.Multiline = true;
            eventConsole.Name = "eventConsole";
            eventConsole.ReadOnly = true;
            eventConsole.ScrollBars = ScrollBars.Vertical;
            eventConsole.Size = new Size(623, 208);
            eventConsole.TabIndex = 6;
            eventConsole.WordWrap = false;
            // 
            // progressBar1
            // 
            progressBar1.Location = new Point(12, 392);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(623, 29);
            progressBar1.TabIndex = 7;
            // 
            // fetch
            // 
            fetch.AutoSize = true;
            fetch.Location = new Point(143, 113);
            fetch.Name = "fetch";
            fetch.Size = new Size(250, 24);
            fetch.TabIndex = 8;
            fetch.Text = "Mettre à jour les dépôts existants";
            fetch.UseVisualStyleBackColor = true;
            fetch.CheckedChanged += Fetch_CheckedChanged;
            // 
            // basePath
            // 
            basePath.Location = new Point(143, 46);
            basePath.Name = "basePath";
            basePath.Size = new Size(392, 27);
            basePath.TabIndex = 9;
            basePath.Text = "C:\\Users\\Dell\\Desktop\\ntlm.Damien";
            // 
            // cancel
            // 
            cancel.Enabled = false;
            cancel.Location = new Point(243, 143);
            cancel.Name = "cancel";
            cancel.Size = new Size(99, 29);
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
            basePathQuestionMark.Location = new Point(108, 48);
            basePathQuestionMark.Name = "basePathQuestionMark";
            basePathQuestionMark.Size = new Size(29, 21);
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
            showWarnings.Location = new Point(348, 143);
            showWarnings.Name = "showWarnings";
            showWarnings.Size = new Size(187, 29);
            showWarnings.TabIndex = 13;
            showWarnings.Text = "Avertissement";
            showWarnings.UseVisualStyleBackColor = false;
            showWarnings.Click += ShowWarnings_Click;
            // 
            // profile
            // 
            profile.DropDownStyle = ComboBoxStyle.DropDownList;
            profile.FormattingEnabled = true;
            profile.Location = new Point(143, 79);
            profile.Name = "profile";
            profile.Size = new Size(151, 28);
            profile.TabIndex = 14;
            // 
            // profileQuestionMark
            // 
            profileQuestionMark.Location = new Point(108, 82);
            profileQuestionMark.Name = "profileQuestionMark";
            profileQuestionMark.Size = new Size(29, 21);
            profileQuestionMark.TabIndex = 16;
            profileQuestionMark.TabStop = false;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(58, 82);
            label3.Name = "label3";
            label3.Size = new Size(44, 20);
            label3.TabIndex = 15;
            label3.Text = "Profil";
            // 
            // Main
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(651, 434);
            Controls.Add(profileQuestionMark);
            Controls.Add(label3);
            Controls.Add(profile);
            Controls.Add(showWarnings);
            Controls.Add(basePathQuestionMark);
            Controls.Add(tokenQuestionMark);
            Controls.Add(cancel);
            Controls.Add(basePath);
            Controls.Add(fetch);
            Controls.Add(progressBar1);
            Controls.Add(eventConsole);
            Controls.Add(clone);
            Controls.Add(browseBasePath);
            Controls.Add(label2);
            Controls.Add(token);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Main";
            Text = "Ntlm Github Cloner";
            Load += Main_Load;
            ((System.ComponentModel.ISupportInitialize)tokenQuestionMark).EndInit();
            ((System.ComponentModel.ISupportInitialize)basePathQuestionMark).EndInit();
            ((System.ComponentModel.ISupportInitialize)profileQuestionMark).EndInit();
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
        private ComboBox profile;
        private PictureBox profileQuestionMark;
        private Label label3;
        private ToolTip profileToolTip;
    }
}
