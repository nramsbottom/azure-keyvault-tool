using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

namespace KeyVaultTool
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        static Dictionary<string, string> GetAllSecrets(string vaultUrl)
        {
            if (string.IsNullOrWhiteSpace(vaultUrl))
            {
                throw new ArgumentException($"'{nameof(vaultUrl)}' cannot be null or whitespace.", nameof(vaultUrl));
            }

            var secrets = new Dictionary<string, string>();
            var client = new SecretClient(vaultUri: new Uri(vaultUrl), credential: new DefaultAzureCredential());

            var allSecrets = client.GetPropertiesOfSecrets();

            foreach (SecretProperties property in allSecrets)
            {
                KeyVaultSecret secret = client.GetSecret(property.Name);
                secrets[secret.Name] = secret.Value;
            }

            return secrets;
        }

        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            var secrets = GetAllSecrets("https://" + textBox1.Text + ".vault.azure.net/");

            e.Result = secrets;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show(this, "Key vault name is required.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            textBox1.Enabled = button1.Enabled = false;
            backgroundWorker1.RunWorkerAsync();
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            var secrets = e.Result as Dictionary<string, string>;

            listView1.BeginUpdate();
            listView1.Items.Clear();
            foreach (var item in secrets)
            {
                var listViewItem = new ListViewItem()
                {
                    Text = item.Key,
                    Tag = item
                };

                listViewItem.SubItems.Add(new ListViewItem.ListViewSubItem(listViewItem, item.Value));
                listView1.Items.Add(listViewItem);
            }
            listView1.EndUpdate();
            textBox1.Enabled = button1.Enabled = true;
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
                return;

            var item = (KeyValuePair<string, string>)listView1.SelectedItems[0].Tag;
            using (var frm = new KeyVaultSecretValueForm(item.Value))
            {
                frm.ShowDialog();
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView1.Items)
            {
                item.Checked = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView1.Items)
            {
                item.Checked = false;
            }
        }
        private void fileToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "JSON File (*.json)|*.json|CSV File (*.csv)|*.csv";
            if (saveFileDialog1.ShowDialog(this) == DialogResult.OK)
            {
                throw new NotImplementedException();
            }
        }

        private void keyVaultToolStripMenuItem_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void exportToolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }
    }
}
