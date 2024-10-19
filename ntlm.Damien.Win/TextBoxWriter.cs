namespace ntlm.Damien.Win
{

    using System;
    using System.IO;
    using System.Text;
    using System.Windows.Forms;

    public class TextBoxWriter(TextBox textBox) : TextWriter
    {
        private readonly TextBox _textBox = textBox;

        // Propriété obligatoire à surcharger pour la classe TextWriter
        public override Encoding Encoding => System.Text.Encoding.UTF8;

        // Redéfinition de la méthode Write pour écrire du texte dans la TextBox
        public override void Write(char value)
        {
            // Comme la TextBox appartient au thread d'interface utilisateur,
            // il faut utiliser Invoke si nécessaire pour éviter des erreurs de threading.
            if (_textBox.InvokeRequired)
            {
                _textBox.Invoke(new Action(() => _textBox.AppendText(value.ToString())));
            }
            else
            {
                _textBox.AppendText(value.ToString());
            }
        }

        // Redéfinition de WriteLine pour ajouter des lignes complètes dans la TextBox
        public override void WriteLine(string? value)
        {
            Write(value + Environment.NewLine);
        }
    }

}
