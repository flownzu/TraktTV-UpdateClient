using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using TraktApiSharp.Enums;
using TraktApiSharp.Objects.Basic;
using TraktApiSharp.Objects.Get.Shows;
using TraktApiSharp.Objects.Get.Shows.Episodes;
using TraktApiSharp.Objects.Get.Shows.Seasons;
using TraktApiSharp.Objects.Post.Syncs.History;
using TraktApiSharp.Requests.Params;
using TraktTVUpdateClient.Cache;
using TraktTVUpdateClient.Extension;

namespace TraktTVUpdateClient.Forms
{
    public partial class SearchShowForm : Form
    {
        TraktCache traktCache;
        List<TraktShow> lastSearch = new List<TraktShow>();

        public SearchShowForm(TraktCache cache)
        {
            InitializeComponent();
            traktCache = cache;
        }

        private void addEpisodesContextMenu_Opening(object sender, CancelEventArgs e)
        {
            if (foundShowsListView.SelectedItems.Count == 0)
            {
                foreach (ToolStripMenuItem item in addEpisodesContextMenu.Items)
                    item.Enabled = false;
            }
            else
            {
                addSpecificSeasonToolStripMenuItem.DropDownItems.Clear();
                addSpecificEpisodeToolStripMenuItem.DropDownItems.Clear();
                TraktShow selectedShow = lastSearch.Find(x => x.Title.Equals(foundShowsListView.SelectedItems[0].SubItems[0].Text));
                if (selectedShow.Seasons != null)
                {
                    var seasons = selectedShow.Seasons.Where(x => x.Number > 0 && x.Episodes != null);
                    if (seasons != null)
                    {
                        ToolStripMenuItem[] episodeMenu = new ToolStripMenuItem[seasons.Count()];
                        ToolStripMenuItem[] seasonMenu = new ToolStripMenuItem[seasons.Count()];
                        int index = 0;
                        int airedEpisodes = selectedShow.AiredEpisodes.Value;
                        foreach (TraktSeason season in seasons)
                        {
                            episodeMenu[index] = new ToolStripMenuItem();
                            episodeMenu[index].Text = "Season " + season.Number.Value;
                            episodeMenu[index].Name = "s" + season.Number.Value;
                            episodeMenu[index].Click += new EventHandler(MenuItemClickHandler);
                            seasonMenu[index] = new ToolStripMenuItem();
                            seasonMenu[index].Text = "Season " + season.Number.Value;
                            seasonMenu[index].Name = "s" + season.Number.Value;
                            seasonMenu[index].Click += new EventHandler(MenuItemClickHandler);
                            seasonMenu[index].Tag = "season" + season.Number.Value;
                            ToolStripMenuItem[] episodeList = new ToolStripMenuItem[season.Episodes.Count()];
                            for (int a = 0; a < season.Episodes.Count() && airedEpisodes > 0; a++)
                            {
                                episodeList[a] = new ToolStripMenuItem();
                                episodeList[a].Text = "Episode " + (a + 1);
                                episodeList[a].Tag = "s" + season.Number.Value + "e" + (a + 1);
                                episodeList[a].Click += new EventHandler(MenuItemClickHandler);
                                airedEpisodes--;
                            }
                            episodeList = episodeList.Where(x => x != null).ToArray();
                            episodeMenu[index].DropDownItems.AddRange(episodeList);
                            if (airedEpisodes <= 0) { break; }
                            index++;
                        }
                        seasonMenu = seasonMenu.Where(x => x != null).ToArray();
                        episodeMenu = episodeMenu.Where(x => x != null).ToArray();
                        addSpecificSeasonToolStripMenuItem.DropDownItems.AddRange(seasonMenu);
                        addSpecificEpisodeToolStripMenuItem.DropDownItems.AddRange(episodeMenu);
                    }
                }
            }
        }

        private async void MenuItemClickHandler(object sender, EventArgs e)
        {
            if (traktCache.TraktClient.IsValidForUseWithAuthorization)
            {
                TraktShow selectedShow = lastSearch.Find(x => x.Title.Equals(foundShowsListView.SelectedItems[0].SubItems[0].Text));
                ToolStripMenuItem clickedItem = (ToolStripMenuItem)sender;
                String tag = (string)clickedItem.Tag;
                TraktSyncHistoryPostBuilder historyPostBuilder = new TraktSyncHistoryPostBuilder();
                if (tag.Contains("season"))
                {
                    int seasonnumber = Int32.Parse(tag.Replace("season", ""));
                    historyPostBuilder.AddShow(selectedShow, seasonnumber);
                    var addHistoryResponse = await traktCache.TraktClient.Sync.AddWatchedHistoryItemsAsync(historyPostBuilder.Build());
                    if((addHistoryResponse.Added.Seasons.HasValue && addHistoryResponse.Added.Seasons.Value >= 1) || (addHistoryResponse.Added.Episodes.HasValue && addHistoryResponse.Added.Episodes.Value >= 1))
                    {
                        await traktCache.SyncShowProgress(selectedShow.Ids.Slug);
                        Task.Run(() => traktCache.Sync()).Forget();
                    }
                    MessageBox.Show("The selected season was added to the watched list.");
                }
                else if (Regex.Match(tag, @"s\d+e\d+").Success)
                {
                    int seasonnumber = Int32.Parse(Regex.Replace(tag, @"s(\d+)e\d+", "$1"));
                    int episodenumber = Int32.Parse(Regex.Replace(tag, @"s\d+e(\d+)", "$1"));
                    TraktEpisode ep = selectedShow.Seasons.Where(x => x.Number.Equals(seasonnumber)).First().Episodes.Where(x => x.Number.Equals(episodenumber)).First();
                    historyPostBuilder.AddEpisode(ep);
                    var addHistoryResponse = await traktCache.TraktClient.Sync.AddWatchedHistoryItemsAsync(historyPostBuilder.Build());
                    if(addHistoryResponse.Added.Episodes.HasValue && addHistoryResponse.Added.Episodes.Value >= 1)
                    {
                        await traktCache.SyncShowProgress(selectedShow.Ids.Slug);
                        Task.Run(() => traktCache.Sync()).Forget();
                    }
                    MessageBox.Show("The selected episode was added to the watched list.");
                }
            }
        }

        private async void searchBtn_Click(object sender, EventArgs e)
        {
            if (searchShowNameTxtBox.Text != String.Empty)
            {
                foundShowsListView.Items.Clear();
                int maxResults = 5;
                if (searchLimitTxtBox.Text != String.Empty) Int32.TryParse(searchLimitTxtBox.Text, out maxResults);
                lastSearch = await SearchShows(searchShowNameTxtBox.Text, maxResults);
                List<Task> taskList = new List<Task>();
                foreach(TraktShow show in lastSearch)
                {
                    taskList.Add(Task.Run(() => SyncSeasonOverview(show)));
                    ListViewItem lvi = foundShowsListView.Items.Add(new ListViewItem(new string[] { show.Title, "" }));
                    if (show.Year.HasValue) lvi.SubItems[1].Text = show.Year.ToString();
                }
                await Task.WhenAll(taskList);
            }
        }

        private async Task SyncSeasonOverview(TraktShow show)
        {
            show.Seasons = await traktCache.TraktClient.Seasons.GetAllSeasonsAsync(show.Ids.Slug);
            List<Task> taskList = new List<Task>();
            foreach(TraktSeason season in show.Seasons)
            {
               if(season.Number > 0)
                {
                    taskList.Add(Task.Run(() => SyncSeasonEpisodes(show.Ids.Slug, season)));
                }
            }
            await Task.WhenAll(taskList);
        }

        private async Task SyncSeasonEpisodes(string showIdOrSlug, TraktSeason season)
        {
            season.Episodes = await traktCache.TraktClient.Seasons.GetSeasonAsync(showIdOrSlug, season.Number.Value);
        }

        private async Task<List<TraktShow>> SearchShows(String title, int maxSearchResults = 5)
        {
            List<TraktShow> showList = new List<TraktShow>();
            var searchResult = await traktCache.TraktClient.Search.GetTextQueryResultsAsync(TraktSearchResultType.Show, title, TraktSearchField.Title, limitPerPage: maxSearchResults, extendedInfo: new TraktExtendedInfo().SetFull());
            if (searchResult != null)
            {
                foreach(TraktSearchResult searchItem in searchResult.Items)
                    showList.Add(searchItem.Show);
            }
            return showList;
        }

        private async void add1stEpisodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (traktCache.TraktClient.IsValidForUseWithAuthorization)
            {
                TraktShow selectedShow = lastSearch.Find(x => x.Title.Equals(foundShowsListView.SelectedItems[0].SubItems[0].Text));
                TraktSeason firstSeason = selectedShow.Seasons.Where(x => x.Number.Value.Equals(1)).First();
                TraktEpisode firstEpisode = firstSeason.Episodes.Where(x => x.Number.Value.Equals(1)).First();
                TraktSyncHistoryPostBuilder historyPostBuilder = new TraktSyncHistoryPostBuilder();
                historyPostBuilder.AddEpisode(firstEpisode);
                var addEpisodeResponse = await traktCache.TraktClient.Sync.AddWatchedHistoryItemsAsync(historyPostBuilder.Build());
                if(addEpisodeResponse.Added.Episodes.HasValue && addEpisodeResponse.Added.Episodes.Value >= 1)
                {
                    await traktCache.SyncShowProgress(selectedShow.Ids.Slug);
                    Task.Run(() => traktCache.Sync()).Forget();
                }
            }
        }

        private async void addAllEpisodesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (traktCache.TraktClient.IsValidForUseWithAuthorization)
            {
                TraktShow selectedShow = lastSearch.Find(x => x.Title.Equals(foundShowsListView.SelectedItems[0].SubItems[0].Text));
                TraktSyncHistoryPostBuilder historyPostBuilder = new TraktSyncHistoryPostBuilder();
                historyPostBuilder.AddShow(selectedShow);
                var addShowResponse = await traktCache.TraktClient.Sync.AddWatchedHistoryItemsAsync(historyPostBuilder.Build());
                if((addShowResponse.Added.Shows.HasValue && addShowResponse.Added.Shows.Value >= 1) || (addShowResponse.Added.Seasons.HasValue && addShowResponse.Added.Seasons.Value >= 1) || (addShowResponse.Added.Episodes.HasValue && addShowResponse.Added.Episodes.Value >= 1))
                {
                    await traktCache.SyncShowProgress(selectedShow.Ids.Slug);
                    Task.Run(() => traktCache.Sync()).Forget();
                }
            }
        }

        private void searchShowNameTxtBox_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter) { searchBtn_Click(this, EventArgs.Empty); }
        }

        private void foundShowsListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(foundShowsListView.SelectedItems.Count == 1)
            {
                TraktShow selectedShow = lastSearch.Find(X => X.Title.Equals(foundShowsListView.SelectedItems[0].SubItems[0].Text));
                showSummaryRTxtBox.ResetText();
                showSummaryRTxtBox.Text =
                    "Title: " + selectedShow.Title + " (" + selectedShow.Year + ")" + Environment.NewLine + Environment.NewLine +
                    "Rating: " + Math.Round(selectedShow.Rating.Value) + Environment.NewLine +
                    "Genres: " + selectedShow.Genres.ToGenreString() + Environment.NewLine + Environment.NewLine +
                    "Synopsis: " + selectedShow.Overview;
                seasonOverviewTreeView.Nodes.Clear();
                foreach(TraktSeason season in selectedShow.Seasons.Where(x => x.Number > 0))
                {
                    var seasonNode = seasonOverviewTreeView.Nodes.Add("Season " + season.Number);
                    foreach(TraktEpisode episode in season.Episodes)
                        seasonNode.Nodes.Add("Episode " + episode.Number);
                }                    
            }
            else
            {
                showSummaryRTxtBox.Clear();
                seasonOverviewTreeView.Nodes.Clear();
            }
        }

        private void seasonOverviewTreeView_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Text.Contains("Season"))
            {
                foreach (TreeNode episodeNode in e.Node.Nodes)
                    episodeNode.Checked = e.Node.Checked;
            }
        }

        private async void addCompleteShowBtn_Click(object sender, EventArgs e)
        {
            if (traktCache.TraktClient.IsValidForUseWithAuthorization && foundShowsListView.SelectedItems.Count == 1)
            {
                TraktShow selectedShow = lastSearch.Find(x => x.Title.Equals(foundShowsListView.SelectedItems[0].SubItems[0].Text));
                TraktSyncHistoryPostBuilder historyPostBuilder = new TraktSyncHistoryPostBuilder();
                historyPostBuilder.AddShow(selectedShow);
                var addShowResponse = await traktCache.TraktClient.Sync.AddWatchedHistoryItemsAsync(historyPostBuilder.Build());
                if ((addShowResponse.Added.Shows.HasValue && addShowResponse.Added.Shows.Value >= 1) || (addShowResponse.Added.Seasons.HasValue && addShowResponse.Added.Seasons.Value >= 1) || (addShowResponse.Added.Episodes.HasValue && addShowResponse.Added.Episodes.Value >= 1))
                {
                    await traktCache.SyncShowProgress(selectedShow.Ids.Slug);
                    Task.Run(() => traktCache.Sync()).Forget();
                }
            }
        }

        private async void addSelectedEpisodes_Click(object sender, EventArgs e)
        {
            if(traktCache.TraktClient.IsValidForUseWithAuthorization && foundShowsListView.SelectedItems.Count == 1)
            {
                TraktShow selectedShow = lastSearch.Find(x => x.Title.Equals(foundShowsListView.SelectedItems[0].SubItems[0].Text));
                TraktSyncHistoryPostBuilder historyPostBuilder = new TraktSyncHistoryPostBuilder();
                foreach(TreeNode seasonNode in seasonOverviewTreeView.Nodes)
                {
                    int seasonNumber = int.Parse(seasonNode.Text.Replace("Season ", ""));
                    if (seasonNode.Checked) historyPostBuilder.AddShow(selectedShow, seasonNumber);
                    else
                    {
                        foreach(TreeNode episodeNode in seasonNode.Nodes)
                        {
                            int episodeNumber = int.Parse(episodeNode.Text.Replace("Episode ", ""));
                            TraktEpisode episode = selectedShow.Seasons.Where(x => x.Number.Equals(seasonNumber)).First().Episodes.Where(x => x.Number.Equals(episodeNumber)).First();
                            if (episodeNode.Checked) historyPostBuilder.AddEpisode(episode);
                        }
                    }
                }
                try
                {
                    var addSelectedEpisodesResponse = await traktCache.TraktClient.Sync.AddWatchedHistoryItemsAsync(historyPostBuilder.Build());
                    if ((addSelectedEpisodesResponse.Added.Shows.HasValue && addSelectedEpisodesResponse.Added.Shows.Value >= 1) || (addSelectedEpisodesResponse.Added.Seasons.HasValue && addSelectedEpisodesResponse.Added.Seasons.Value >= 1) || (addSelectedEpisodesResponse.Added.Episodes.HasValue && addSelectedEpisodesResponse.Added.Episodes.Value >= 1))
                    {
                        await traktCache.SyncShowProgress(selectedShow.Ids.Slug);
                        Task.Run(() => traktCache.Sync()).Forget();
                    }
                }
                catch (Exception) { }
            }
        }
    }
}
