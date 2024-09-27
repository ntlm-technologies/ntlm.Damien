namespace ntlm.Damien.Win
{
    partial class WarningDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WarningDialog));
            WarningList = new TextBox();
            SuspendLayout();
            // 
            // WarningList
            // 
            WarningList.ForeColor = Color.DarkOrange;
            WarningList.Location = new Point(12, 12);
            WarningList.Multiline = true;
            WarningList.Name = "WarningList";
            WarningList.ReadOnly = true;
            WarningList.ScrollBars = ScrollBars.Vertical;
            WarningList.Size = new Size(627, 229);
            WarningList.TabIndex = 0;
            // 
            // WarningForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(651, 253);
            Controls.Add(WarningList);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "WarningForm";
            Text = "Avertissements";
            Load += Avertissements_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox WarningList;
    }
}