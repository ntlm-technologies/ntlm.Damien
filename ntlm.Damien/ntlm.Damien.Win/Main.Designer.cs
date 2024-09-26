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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            label1 = new Label();
            Token = new TextBox();
            FolderBrowserDialog1 = new FolderBrowserDialog();
            label2 = new Label();
            BasePath = new Label();
            browseBasePath = new Button();
            backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            Clone = new Button();
            EventConsole = new TextBox();
            ProgressBar1 = new ProgressBar();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(160, 76);
            label1.Name = "label1";
            label1.Size = new Size(96, 20);
            label1.TabIndex = 0;
            label1.Text = "Github Token";
            // 
            // Token
            // 
            Token.Location = new Point(262, 73);
            Token.Name = "Token";
            Token.Size = new Size(456, 27);
            Token.TabIndex = 1;
            Token.Text = "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";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(165, 125);
            label2.Name = "label2";
            label2.Size = new Size(85, 20);
            label2.TabIndex = 2;
            label2.Text = "Destination";
            // 
            // BasePath
            // 
            BasePath.AutoSize = true;
            BasePath.Location = new Point(262, 125);
            BasePath.Name = "BasePath";
            BasePath.Size = new Size(37, 20);
            BasePath.TabIndex = 3;
            BasePath.Text = "       ";
            // 
            // browseBasePath
            // 
            browseBasePath.Location = new Point(262, 148);
            browseBasePath.Name = "browseBasePath";
            browseBasePath.Size = new Size(94, 29);
            browseBasePath.TabIndex = 4;
            browseBasePath.Text = "Parcourir";
            browseBasePath.UseVisualStyleBackColor = true;
            browseBasePath.Click += browseBasePath_Click;
            // 
            // Clone
            // 
            Clone.Enabled = false;
            Clone.Location = new Point(262, 198);
            Clone.Name = "Clone";
            Clone.Size = new Size(94, 29);
            Clone.TabIndex = 5;
            Clone.Text = "Cloner";
            Clone.UseVisualStyleBackColor = true;
            Clone.Click += Clone_Click;
            // 
            // EventConsole
            // 
            EventConsole.Location = new Point(12, 251);
            EventConsole.Multiline = true;
            EventConsole.Name = "EventConsole";
            EventConsole.ReadOnly = true;
            EventConsole.ScrollBars = ScrollBars.Vertical;
            EventConsole.Size = new Size(776, 133);
            EventConsole.TabIndex = 6;
            EventConsole.WordWrap = false;
            // 
            // ProgressBar1
            // 
            ProgressBar1.Location = new Point(12, 390);
            ProgressBar1.Name = "ProgressBar1";
            ProgressBar1.Size = new Size(776, 29);
            ProgressBar1.TabIndex = 7;
            // 
            // Main
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(ProgressBar1);
            Controls.Add(EventConsole);
            Controls.Add(Clone);
            Controls.Add(browseBasePath);
            Controls.Add(BasePath);
            Controls.Add(label2);
            Controls.Add(Token);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "Main";
            Text = "Ntlm Github Cloner";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox Token;
        private FolderBrowserDialog FolderBrowserDialog1;
        private Label label2;
        private Label BasePath;
        private Button browseBasePath;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private Button Clone;
        private TextBox EventConsole;
        private ProgressBar ProgressBar1;
    }
}
