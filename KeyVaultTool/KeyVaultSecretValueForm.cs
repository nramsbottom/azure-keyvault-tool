using System;
using System.Windows.Forms;

namespace KeyVaultTool
{
    public partial class KeyVaultSecretValueForm : Form
    {
        public readonly string _value;

        public KeyVaultSecretValueForm(string value)
        {
            _value = value;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void KeyVaultSecretValueForm_Load(object sender, EventArgs e)
        {
            textBox1.Text = _value;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(_value);
        }
    }
}
