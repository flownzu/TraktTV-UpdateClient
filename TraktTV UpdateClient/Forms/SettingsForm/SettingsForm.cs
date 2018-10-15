using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using TraktTVUpdateClient.Extension;
using TraktTVUpdateClient.Media;

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
            bindingSourceLibraries.DataSource = mainForm.librarySettings.Libraries;
            dataGridViewLibraries.DataSource = bindingSourceLibraries;
        }

        private void WatchedPercentTrackBar_ValueChanged(object sender, EventArgs e)
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
                        if (Properties.Settings.Default.VLCEnabled) { Task.Run(() => mainForm.WaitForVlcConnection()).Forget(); }
                    }
                    else if(dialogResult == DialogResult.Cancel)
                    {
                        e.Cancel = true;
                    }
                }
            }
        }

        private void SaveSettingsBtn_Click(object sender, EventArgs e)
        {
            int vlcPort = Properties.Settings.Default.VLCPort;
            int.TryParse(vlcPortTxtBox.Text, out vlcPort);
            Properties.Settings.Default.WatchedPercent = watchedPercent;
            Properties.Settings.Default.VLCPort = vlcPort;
            Properties.Settings.Default.VLCEnabled = enableVLCCheckBox.Checked;
            Properties.Settings.Default.Save();
            if (Properties.Settings.Default.VLCEnabled) { Task.Run(() => mainForm.WaitForVlcConnection()).Forget(); }
        }

        private void ButtonAddLibrary_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog
            {
                RootFolder = Environment.SpecialFolder.MyComputer,
                ShowNewFolderButton = false,
                Description = "Choose the folder where your media files are located."
            };
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                mainForm.librarySettings.Libraries.Add(new MediaLibrary(folderBrowserDialog.SelectedPath, ""));
                mainForm.librarySettings.Save();
                bindingSourceLibraries.ResetBindings(false);
            }
        }

        private void ButtonRemoveLibrary_Click(object sender, EventArgs e)
        {
            if (dataGridViewLibraries.SelectedCells.Count > 0)
            {
                if (bindingSourceLibraries.List[dataGridViewLibraries.SelectedCells[0].RowIndex] is MediaLibrary mediaLib)
                {
                    mainForm.librarySettings.Libraries.Remove(mediaLib);
                    mainForm.librarySettings.Save();
                }
                bindingSourceLibraries.ResetBindings(false);
            }
        }

        private void DataGridViewLibraries_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            mainForm.librarySettings.Save();
        }

        private void DataGridViewLibraries_SelectionChanged(object sender, EventArgs e)
        {
            treeViewFoundSeries.Nodes.Clear();
            if (dataGridViewLibraries.SelectedCells.Count > 0)
            {
                MediaLibrary lib = bindingSourceLibraries.List[dataGridViewLibraries.SelectedCells[0].RowIndex] as MediaLibrary;
                foreach (var series in lib.VideoFiles)
                {
                    var seriesNode = treeViewFoundSeries.Nodes.Add(series.Key);
                    var episodeList = series.Value.OrderBy(x => x.Season).ThenBy(x => x.AbsoluteNumber ? x.AbsoluteNumberStart : x.EpisodeNumberStart);
                    foreach (int season in episodeList.Select(x => x.Season).Distinct())
                    {
                        var seasonNode = seriesNode.Nodes.Add("Season " + season);
                        foreach (var mediaItem in episodeList.Where(x => x.Season == season))
                        {
                            var episodeNode = seasonNode.Nodes.Add("Episode " + (mediaItem.AbsoluteNumber ? mediaItem.AbsoluteNumberStart + (mediaItem.AbsoluteNumberEnd > 0 ? ("-" + mediaItem.AbsoluteNumberEnd) : "") : mediaItem.EpisodeNumberStart + (mediaItem.EpisodeNumberEnd > 0 ? ("-" + mediaItem.EpisodeNumberEnd) : "")));
                            episodeNode.Tag = mediaItem;
                        }
                        seasonNode.Expand();
                    }
                }
            }
        }

        private void ButtonReload_Click(object sender, EventArgs e)
        {
            if (dataGridViewLibraries.SelectedCells.Count > 0)
            {
                MediaLibrary lib = bindingSourceLibraries.List[dataGridViewLibraries.SelectedCells[0].RowIndex] as MediaLibrary;
                treeViewFoundSeries.Nodes.Clear();
                Cursor = Cursors.WaitCursor;
                Task.Run(() => lib.GetAllFiles()).ContinueWith((Task) => this.InvokeIfRequired(() => DataGridViewLibraries_SelectionChanged(null, EventArgs.Empty))).ContinueWith((Task) => this.InvokeIfRequired(() => Cursor = Cursors.Default));
            }
        }

        private void TreeViewFoundSeries_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Tag is MediaItem mediaItem)
            {
                richTextBoxMediaInfo.Text = mediaItem.FilePath + " parsed to " + e.Node.Parent.Parent.Text + " " + (mediaItem.Season > 0 ? "S" + mediaItem.Season.ToString().PadLeft(2, '0') : "") + "E" + (mediaItem.AbsoluteNumber ? mediaItem.AbsoluteNumberStart + (mediaItem.AbsoluteNumberEnd > 0 ? ("-" + mediaItem.AbsoluteNumberEnd) : "") : mediaItem.EpisodeNumberStart + (mediaItem.EpisodeNumberEnd > 0 ? ("-" + mediaItem.EpisodeNumberEnd) : "")).PadLeft(2, '0');
            }
        }

        private void ButtonReportParsingError_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/flownzu/TraktTV-UpdateClient/issues/25");
        }
    }
}
