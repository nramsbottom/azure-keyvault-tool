using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace KeyVaultTool
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            var source = new AzurePublicCloudKeyVaultSecretSource();
            var secrets = source.GetAllSecrets(textBox1.Text);

            e.Result = secrets;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ClearSecretsView(true);

            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show(this, "Key vault name is required.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            SetUIState(false);
            backgroundWorker1.RunWorkerAsync();
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                // failed
                MessageBox.Show(this, "Unable to load key vault secrets.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                var secrets = e.Result as Dictionary<string, string>;
                RefreshSecretsView(secrets);
            }

            SetUIState(true);
        }

        private void SetUIState(bool enabled)
        {
            textBox1.Enabled = listView1.Enabled = button1.Enabled = enabled;
        }

        private void ClearSecretsView(bool endUpdate = false)
        {
            listView1.BeginUpdate();
            listView1.Items.Clear();
            if (endUpdate)
                listView1.EndUpdate();
        }

        private void RefreshSecretsView(Dictionary<string, string> secrets)
        {
            ClearSecretsView(false);

            foreach (var item in secrets)
            {
                var listViewItem = new ListViewItem()
                {
                    Text = item.Key,
                    Tag = item
                };

                listViewItem.SubItems.Add(new ListViewItem.ListViewSubItem(listViewItem, item.Value));
                listViewItem.Checked = true;
                listView1.Items.Add(listViewItem);
            }
            listView1.EndUpdate();
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
                return;
            var item = (KeyValuePair<string, string>)listView1.SelectedItems[0].Tag;
            using var frm = new KeyVaultSecretValueForm(item.Value);
            frm.ShowDialog();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SetCheckStateAll(true);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SetCheckStateAll(false);
        }

        void SetCheckStateAll(bool state)
        {
            foreach (ListViewItem item in listView1.Items)
                item.Checked = state;
        }

        private void fileToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "JSON File (*.json)|*.json|CSV File (*.csv)|*.csv";
            if (saveFileDialog1.ShowDialog(this) == DialogResult.OK)
            {
                var selectedSecrets = GetSelectedSecrets();
                var filename = saveFileDialog1.FileName;
                var json = new JsonSecretWriter().WriteSecrets(selectedSecrets);

                File.WriteAllText(filename, json);
            }
        }

        private void keyVaultToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using var frm = new KeyVaultNameEntryForm();
            if (frm.ShowDialog(this) == DialogResult.OK)
            {
                var targetKeyVaultName = frm.KeyVaultName;
                throw new NotImplementedException();
            }
            else
            {
                MessageBox.Show(this, "Export to Key Vault cancelled.", "Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void exportToolStripMenuItem1_Click(object sender, EventArgs e)
        {
        }

        private void userSecretsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var selectedSecrets = GetSelectedSecrets();
            var xml = new UserSecretsXmlSecretWriter().WriteSecrets(selectedSecrets);

            Clipboard.SetText(xml);
            MessageBox.Show(this, "User Secrets XML has been copied to clipboard.", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Returns a dictionary of all the user secrets currently checked within the list.
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> GetSelectedSecrets()
        {
            var selectedSecrets = new Dictionary<string, string>();
            foreach (ListViewItem item in listView1.Items)
            {
                if (!item.Checked)
                    continue;
                selectedSecrets.Add(item.Text, item.SubItems[1].Text);
            }
            return selectedSecrets;
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            // todo: show save dialog
            // todo: determine file type from extension
            // todo: write selected secrets to file
            throw new NotImplementedException();
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            // todo: show open dialog
            // todo: determine file type from extension
            // todo: read file contents to listview
            throw new NotImplementedException();
        }
    }
}
