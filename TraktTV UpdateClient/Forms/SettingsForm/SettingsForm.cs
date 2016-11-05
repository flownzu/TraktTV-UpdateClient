using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using TraktTVUpdateClient.Extension;

namespace TraktTVUpdateClient.Forms
{
    public partial class SettingsForm : Form
    {
        private float watchedPercent = Properties.Settings.Default.WatchedPercent;
        private MainForm mainForm;

        public SettingsForm(MainForm parent)
        {
            InitializeComponent();
            watchedPercentLbl.Text = Math.Round((double)Properties.Settings.Default.WatchedPercent*100) + "% of the title.";
            watchedPercentTrackBar.Value = (int)Math.Round((double)Properties.Settings.Default.WatchedPercent*100);
            enableVLCCheckBox.Checked = Properties.Settings.Default.VLCEnabled;
            mainForm = parent;
        }

        private void watchedPercentTrackBar_ValueChanged(object sender, EventArgs e)
        {
            watchedPercent = (float)watchedPercentTrackBar.Value / 100;
            watchedPercentLbl.Text = watchedPercentTrackBar.Value + "% of the title.";
        }

        private void SettingsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(e.CloseReason == CloseReason.UserClosing)
            {
                int vlcPort = Properties.Settings.Default.VLCPort;
                int.TryParse(vlcPortTxtBox.Text, out vlcPort);
                if (watchedPercent != Properties.Settings.Default.WatchedPercent || vlcPort != Properties.Settings.Default.VLCPort || Properties.Settings.Default.VLCEnabled != enableVLCCheckBox.Checked)
                {
                    var dialogResult = MessageBox.Show("There are unsaved changes, do you want to save them?", "Warning", MessageBoxButtons.YesNoCancel);
                    if(dialogResult == DialogResult.Yes)
                    {
                        Properties.Settings.Default.VLCEnabled = enableVLCCheckBox.Checked;
                        Properties.Settings.Default.WatchedPercent = watchedPercent;
                        Properties.Settings.Default.VLCPort = vlcPort;
                        Properties.Settings.Default.Save();
                        if (Properties.Settings.Default.VLCEnabled) { Task.Run(() => mainForm.waitForVLCConnection()).Forget(); }
                    }
                    else if(dialogResult == DialogResult.Cancel)
                    {
                        e.Cancel = true;
                    }
                }
            }
        }

        private void saveSettingsBtn_Click(object sender, EventArgs e)
        {
            int vlcPort = Properties.Settings.Default.VLCPort;
            int.TryParse(vlcPortTxtBox.Text, out vlcPort);
            Properties.Settings.Default.WatchedPercent = watchedPercent;
            Properties.Settings.Default.VLCPort = vlcPort;
            Properties.Settings.Default.VLCEnabled = enableVLCCheckBox.Checked;
            Properties.Settings.Default.Save();
            if (Properties.Settings.Default.VLCEnabled) { Task.Run(() => mainForm.waitForVLCConnection()).Forget(); }
        }
    }
}
