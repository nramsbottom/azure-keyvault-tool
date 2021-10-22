using System;
using System.Windows.Forms;

namespace KeyVaultTool
{
    public partial class KeyVaultNameEntryForm : Form
    {
        public KeyVaultNameEntryForm()
        {
            InitializeComponent();
        }

        private void KeyVaultNameEntryForm_Load(object sender, EventArgs e)
        {

        }

        public string KeyVaultName
        {
            get
            {
                return textBox1.Text;
            }
        }
    }
}
