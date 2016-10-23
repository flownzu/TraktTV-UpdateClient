using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TraktTVUpdateClient.Forms
{
    public partial class SettingsForm : Form
    {
        private float watchedPercent = Properties.Settings.Default.WatchedPercent;
        public SettingsForm()
        {
            InitializeComponent();
            watchedPercentLbl.Text = Math.Round((double)Properties.Settings.Default.WatchedPercent*100) + "% of the title.";
            watchedPercentTrackBar.Value = (int)Math.Round((double)Properties.Settings.Default.WatchedPercent*100);
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
                if (watchedPercent != Properties.Settings.Default.WatchedPercent || vlcPort != Properties.Settings.Default.VLCPort)
                {
                    var dialogResult = MessageBox.Show("There are unsaved changes, do you want to save them?", "Warning", MessageBoxButtons.YesNoCancel);
                    if(dialogResult == DialogResult.Yes)
                    {
                        Properties.Settings.Default.WatchedPercent = watchedPercent;
                        Properties.Settings.Default.VLCPort = vlcPort;
                        Properties.Settings.Default.Save();
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
            Properties.Settings.Default.Save();
        }
    }
}
