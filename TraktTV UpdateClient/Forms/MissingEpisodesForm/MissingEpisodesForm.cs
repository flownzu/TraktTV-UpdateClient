using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TraktTVUpdateClient.Cache;

namespace TraktTVUpdateClient.Forms
{
    public partial class MissingEpisodeForm : Form
    {
        public MissingEpisodeForm(TraktCache cache)
        {
            InitializeComponent();
            foreach (var showProgress in cache.ShowCollectionProgress.OrderBy(x => x.Key))
            {
                if (showProgress.Value.Seasons.Any(x => x.Completed < x.Aired))
                {
                    var showTitle = cache.WatchedList.Where(x => x.Show.Ids.Slug == showProgress.Key).FirstOrDefault()?.Show.Title;
                    if (!string.IsNullOrEmpty(showTitle))
                    {
                        var showNode = treeViewMissingEpisodes.Nodes.Add(showTitle);
                        foreach (var season in showProgress.Value.Seasons)
                        {
                            if (season.Episodes.Any(x => !x.Completed ?? false))
                            {
                                var seasonNode = showNode.Nodes.Add("Season " + season.Number);
                                if (season.Episodes.All(x => !x.Completed ?? false)) seasonNode.Tag = "complete";
                                else seasonNode.Tag = "partial";
                                foreach (var episode in season.Episodes)
                                {
                                    if (!episode.Completed ?? false)
                                    {
                                        seasonNode.Nodes.Add("Episode " + episode.Number);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void ExportBtn_Click(object sender, System.EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "csv-file (*.csv)|*.csv",
                FileName = "MissingEpisodes" + DateTime.Now.ToString("ddMMyyyyhhmmss") + ".csv",
                RestoreDirectory = true,
                OverwritePrompt = true,
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
            };
            var dialogResult = saveFileDialog.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                try
                {
                    StringBuilder sb = new StringBuilder("SHOW;SEASON;EPISODE" + Environment.NewLine);
                    foreach (TreeNode showNode in treeViewMissingEpisodes.Nodes)
                    {
                        foreach (TreeNode seasonNode in showNode.Nodes)
                        {
                            if (checkBoxSeasonExport.Checked && seasonNode.Tag.ToString() == "complete")
                            {
                                sb.AppendLine(showNode.Text + ";" + seasonNode.Text.Replace("Season ", "") + ";");
                            }
                            else
                            {
                                foreach (TreeNode episodeNode in seasonNode.Nodes)
                                {
                                    sb.AppendLine(showNode.Text + ";" + seasonNode.Text.Replace("Season ", "") + ";" + episodeNode.Text.Replace("Episode ", ""));
                                }
                            }
                        }
                    }
                    File.WriteAllText(saveFileDialog.FileName, sb.ToString());
                }
                catch { }
            }
        }
    }
}
