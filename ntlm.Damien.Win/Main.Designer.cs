﻿namespace ntlm.Damien.Win
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
            Token = new TextBox();
            FolderBrowserDialog1 = new FolderBrowserDialog();
            label2 = new Label();
            BrowseBasePath = new Button();
            backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            Clone = new Button();
            EventConsole = new TextBox();
            ProgressBar1 = new ProgressBar();
            Fetch = new CheckBox();
            BasePath = new TextBox();
            Cancel = new Button();
            TokenQuestionMark = new PictureBox();
            BasePathQuestionMark = new PictureBox();
            TokenToolTip = new ToolTip(components);
            BasePathToolTip = new ToolTip(components);
            ShowWarnings = new Button();
            Profile = new ComboBox();
            ProfileQuestionMark = new PictureBox();
            label3 = new Label();
            ProfileToolTip = new ToolTip(components);
            ((System.ComponentModel.ISupportInitialize)TokenQuestionMark).BeginInit();
            ((System.ComponentModel.ISupportInitialize)BasePathQuestionMark).BeginInit();
            ((System.ComponentModel.ISupportInitialize)ProfileQuestionMark).BeginInit();
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
            // Token
            // 
            Token.Location = new Point(143, 13);
            Token.Name = "Token";
            Token.Size = new Size(392, 27);
            Token.TabIndex = 1;
            Token.Text = "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";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(17, 48);
            label2.Name = "label2";
            label2.Size = new Size(85, 20);
            label2.TabIndex = 2;
            label2.Text = "Destination";
            // 
            // BrowseBasePath
            // 
            BrowseBasePath.Location = new Point(536, 44);
            BrowseBasePath.Name = "BrowseBasePath";
            BrowseBasePath.Size = new Size(99, 29);
            BrowseBasePath.TabIndex = 4;
            BrowseBasePath.Text = "Parcourir";
            BrowseBasePath.UseVisualStyleBackColor = true;
            BrowseBasePath.Click += BrowseBasePath_Click;
            // 
            // Clone
            // 
            Clone.Enabled = false;
            Clone.Location = new Point(143, 143);
            Clone.Name = "Clone";
            Clone.Size = new Size(99, 29);
            Clone.TabIndex = 5;
            Clone.Text = "Cloner";
            Clone.UseVisualStyleBackColor = true;
            Clone.Click += Clone_Click;
            // 
            // EventConsole
            // 
            EventConsole.Location = new Point(12, 178);
            EventConsole.Multiline = true;
            EventConsole.Name = "EventConsole";
            EventConsole.ReadOnly = true;
            EventConsole.ScrollBars = ScrollBars.Vertical;
            EventConsole.Size = new Size(623, 208);
            EventConsole.TabIndex = 6;
            EventConsole.WordWrap = false;
            // 
            // ProgressBar1
            // 
            ProgressBar1.Location = new Point(12, 392);
            ProgressBar1.Name = "ProgressBar1";
            ProgressBar1.Size = new Size(623, 29);
            ProgressBar1.TabIndex = 7;
            // 
            // Fetch
            // 
            Fetch.AutoSize = true;
            Fetch.Location = new Point(143, 113);
            Fetch.Name = "Fetch";
            Fetch.Size = new Size(250, 24);
            Fetch.TabIndex = 8;
            Fetch.Text = "Mettre à jour les dépôts existants";
            Fetch.UseVisualStyleBackColor = true;
            // 
            // BasePath
            // 
            BasePath.Location = new Point(143, 46);
            BasePath.Name = "BasePath";
            BasePath.Size = new Size(392, 27);
            BasePath.TabIndex = 9;
            BasePath.Text = "C:\\Users\\Dell\\Desktop\\ntlm.Damien";
            // 
            // Cancel
            // 
            Cancel.Enabled = false;
            Cancel.Location = new Point(243, 143);
            Cancel.Name = "Cancel";
            Cancel.Size = new Size(99, 29);
            Cancel.TabIndex = 10;
            Cancel.Text = "Annuler";
            Cancel.UseVisualStyleBackColor = true;
            Cancel.Click += Cancel_Click;
            // 
            // TokenQuestionMark
            // 
            TokenQuestionMark.Location = new Point(108, 15);
            TokenQuestionMark.Name = "TokenQuestionMark";
            TokenQuestionMark.Size = new Size(29, 21);
            TokenQuestionMark.TabIndex = 11;
            TokenQuestionMark.TabStop = false;
            // 
            // BasePathQuestionMark
            // 
            BasePathQuestionMark.Location = new Point(108, 48);
            BasePathQuestionMark.Name = "BasePathQuestionMark";
            BasePathQuestionMark.Size = new Size(29, 21);
            BasePathQuestionMark.TabIndex = 12;
            BasePathQuestionMark.TabStop = false;
            // 
            // ShowWarnings
            // 
            ShowWarnings.AutoEllipsis = true;
            ShowWarnings.BackColor = SystemColors.Control;
            ShowWarnings.Enabled = false;
            ShowWarnings.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            ShowWarnings.ForeColor = Color.DarkOrange;
            ShowWarnings.Location = new Point(348, 143);
            ShowWarnings.Name = "ShowWarnings";
            ShowWarnings.Size = new Size(187, 29);
            ShowWarnings.TabIndex = 13;
            ShowWarnings.Text = "Avertissement";
            ShowWarnings.UseVisualStyleBackColor = false;
            ShowWarnings.Click += ShowWarnings_Click;
            // 
            // ProfileSelector
            // 
            Profile.DropDownStyle = ComboBoxStyle.DropDownList;
            Profile.FormattingEnabled = true;
            Profile.Location = new Point(143, 79);
            Profile.Name = "ProfileSelector";
            Profile.Size = new Size(151, 28);
            Profile.TabIndex = 14;
            // 
            // ProfileQuestionMark
            // 
            ProfileQuestionMark.Location = new Point(108, 82);
            ProfileQuestionMark.Name = "ProfileQuestionMark";
            ProfileQuestionMark.Size = new Size(29, 21);
            ProfileQuestionMark.TabIndex = 16;
            ProfileQuestionMark.TabStop = false;
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
            Controls.Add(ProfileQuestionMark);
            Controls.Add(label3);
            Controls.Add(Profile);
            Controls.Add(ShowWarnings);
            Controls.Add(BasePathQuestionMark);
            Controls.Add(TokenQuestionMark);
            Controls.Add(Cancel);
            Controls.Add(BasePath);
            Controls.Add(Fetch);
            Controls.Add(ProgressBar1);
            Controls.Add(EventConsole);
            Controls.Add(Clone);
            Controls.Add(BrowseBasePath);
            Controls.Add(label2);
            Controls.Add(Token);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Main";
            Text = "Ntlm Github Cloner";
            Load += Main_Load;
            ((System.ComponentModel.ISupportInitialize)TokenQuestionMark).EndInit();
            ((System.ComponentModel.ISupportInitialize)BasePathQuestionMark).EndInit();
            ((System.ComponentModel.ISupportInitialize)ProfileQuestionMark).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox Token;
        private FolderBrowserDialog FolderBrowserDialog1;
        private Label label2;
        private Button BrowseBasePath;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private Button Clone;
        private TextBox EventConsole;
        private ProgressBar ProgressBar1;
        private CheckBox Fetch;
        private TextBox BasePath;
        private Button Cancel;
        private PictureBox TokenQuestionMark;
        private PictureBox BasePathQuestionMark;
        private ToolTip TokenToolTip;
        private ToolTip BasePathToolTip;
        private Button ShowWarnings;
        private ComboBox Profile;
        private PictureBox ProfileQuestionMark;
        private Label label3;
        private ToolTip ProfileToolTip;
    }
}
