using Newtonsoft.Json;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TraktApiSharp;
using TraktApiSharp.Enums;
using TraktApiSharp.Objects.Basic;
using TraktApiSharp.Objects.Get.Shows;
using TraktApiSharp.Objects.Get.Shows.Episodes;
using TraktApiSharp.Objects.Get.Shows.Seasons;
using TraktApiSharp.Objects.Get.Watched;
using TraktApiSharp.Objects.Post.Syncs.History;
using TraktApiSharp.Objects.Post.Syncs.Ratings;
using TraktSharp.Examples.Wpf.ViewModels;
using TraktSharp.Examples.Wpf.Views;
using TraktTVUpdateClient.Cache;
using TraktTVUpdateClient.Forms;
using TraktTVUpdateClient.Extension;
using TraktTVUpdateClient.Properties;
using TraktTVUpdateClient.VLC;

namespace TraktTVUpdateClient
{
    public partial class MainForm : Form
    {
        public TraktClient Client;
        public TraktCache TraktCache;
        public VLCConnection vlcClient;
        public TraktShow CurrentShow;
        public TraktEpisode CurrentEpisode;
        public bool NoCache = true;

        private SettingsForm settingsForm;

        public MainForm()
        {
            InitializeComponent();
            Client = new TraktClient(Resources.ClientID, Resources.ClientSecret);
            Client.Authentication.RedirectUri = "app://authorized";
            Client.Configuration.ForceAuthorization = true;
            TraktCache = LoadCache();
            if(TraktCache.TraktClient == null)
            {
                TraktCache.TraktClient = Client;
                NoCache = false;
            }
            TraktCache.SyncCompleted += TraktCache_SyncCompleted;
            Thread vlcConnectionThread = new Thread(waitForVLCConnection);
            vlcConnectionThread.IsBackground = true;
            vlcConnectionThread.Start();
        }

        public TraktCache LoadCache(string cacheFile = "cache.json")
        {
            try
            {
                if (File.Exists(cacheFile))
                {
                    using (StreamReader sr = File.OpenText(cacheFile)) { TraktCache = JsonConvert.DeserializeObject<TraktCache>(sr.ReadToEnd()); }
                }
                return TraktCache == null ? new TraktCache(Client) : TraktCache;
            }
            catch (Exception) { return new TraktCache(Client); }
        }

        public void waitForVLCConnection()
        {
            while (true)
            {
                if(Process.GetProcessesByName("vlc").Count() > 0)
                {
                    try
                    {
                        vlcClient = new VLCConnection(Settings.Default.VLCPort);
                    }
                    catch(Exception) { }
                }
                if(vlcClient != null && vlcClient.Connected) { break; }
                Thread.Sleep(100);
            }
            vlcConnectStatusLabel.Invoke(new MethodInvoker(() => vlcConnectStatusLabel.Text = "VLC Status: connected"));
            vlcClient.ConnectionLost += vlcClient_ConnectionLost;
            vlcClient.WatchedPercentReached += vlcClient_WatchedPercentReached;
            vlcClient.MediaItemChanged += vlcClient_MediaItemChanged;
            Thread vlcThread = new Thread(vlcClient.ConnectionThread);
            vlcThread.IsBackground = true;
            vlcThread.Start();
        }

        private async void vlcClient_MediaItemChanged(object sender, MediaItemChangedEventArgs e)
        {
            await GetShowAndEpisodeFromMediaItem(e.MediaItem);
        }

        private async Task GetShowAndEpisodeFromMediaItem(VLCMediaItem mediaItem)
        {
            string fileName = Regex.Match(mediaItem.Path, @".*\/(.*)").Groups[1].Value;
            Match m = Regex.Match(fileName, @"(.*)[sS](\d+)[eE](\d+)");
            if (m.Success)
            {
                string showName = m.Groups[1].Value.Replace('.', ' ').Trim();
                int seasonNumber = Int16.Parse(m.Groups[2].Value);
                int episodeNumber = Int16.Parse(m.Groups[3].Value);
                TraktShow show = await getClosestMatch(showName, 80);
                if(show != null)
                {
                    CurrentShow = show;
                    CurrentEpisode = await Client.Episodes.GetEpisodeAsync(show.Ids.Slug, seasonNumber, episodeNumber);
                }
            }
            string[] mediaPath = Regex.Split(mediaItem.Path.Replace(@"file:///", ""), @"\/");
            try
            {
                string parentFolderName = mediaPath[mediaPath.Length - 2];
                TraktShow show = await getClosestMatch(parentFolderName, 80);
                if(show != null)
                {
                    show.Seasons = await Client.Seasons.GetAllSeasonsAsync(show.Ids.Slug);
                    if (show.Seasons.Count() == 1)
                    {
                        int seasonNumber = 1;
                        m = Regex.Match(fileName, @".*-[_\s]?(\d+)");
                        if (m.Success)
                        {
                            int episodeNumber = Int16.Parse(m.Groups[1].Value);
                            CurrentShow = show;
                            CurrentEpisode = await Client.Episodes.GetEpisodeAsync(show.Ids.Slug, seasonNumber, episodeNumber);
                        }
                        else
                        {
                            m = Regex.Match(fileName, @".*?(\d+)");
                            if(m.Success)
                            {
                                int episodeNumber = Int16.Parse(m.Groups[1].Value);
                                CurrentShow = show;
                                CurrentEpisode = await Client.Episodes.GetEpisodeAsync(show.Ids.Slug, seasonNumber, episodeNumber);
                            }
                        }
                    }
                }
                else
                {
                    string parentsParentFolderName = mediaPath[mediaPath.Length - 3];
                    show = await getClosestMatch(parentsParentFolderName, 80);
                    if(show != null)
                    {
                        m = Regex.Match(parentFolderName, @"(\d+)");
                        if(m.Success)
                        {
                            int seasonNumber = Int16.Parse(m.Groups[1].Value);
                            m = Regex.Match(fileName, @".*-[_\s]?(\d+)");
                            if (m.Success)
                            {
                                int episodeNumber = Int16.Parse(m.Groups[1].Value);
                                CurrentShow = show;
                                CurrentEpisode = await Client.Episodes.GetEpisodeAsync(show.Ids.Slug, seasonNumber, episodeNumber);
                            }
                        }
                    }
                }
            }
            catch (Exception) { }
        }

        private async Task<TraktShow> getClosestMatch(string showName, double minSimilarity = 0)
        {
            var searchResult = await Client.Search.GetTextQueryResultsAsync(TraktSearchResultType.Show, showName, TraktSearchField.Title, limitPerPage: 15);

            double highestSimilarity = 0;
            TraktShow highestSimilarityShow = null;
            foreach (TraktSearchResult searchResultItem in searchResult.Items)
            {
                double d1 = 0;
                double d2 = 0;
                double d = Extensions.GetSimilarityRatio(searchResultItem.Show.Title, showName, out d1, out d2);
                if (d > highestSimilarity) { highestSimilarity = d; highestSimilarityShow = searchResultItem.Show; }
            }
            if(highestSimilarity >= minSimilarity) return highestSimilarityShow;
            else { return null; }
        }

        private async void vlcClient_WatchedPercentReached(object sender, EventArgs e)
        {
            if (CurrentEpisode != null && CurrentShow != null)
            {
                TraktSyncHistoryPostBuilder historyPostBuilder = new TraktSyncHistoryPostBuilder();
                historyPostBuilder.AddEpisode(CurrentEpisode);
                var addEpisodeResponse = await Client.Sync.AddWatchedHistoryItemsAsync(historyPostBuilder.Build());
                if (addEpisodeResponse.Added.Episodes.HasValue && addEpisodeResponse.Added.Episodes.Value >= 1)
                {
                    await TraktCache.SyncShowProgress(CurrentShow);
                    Task.Run(() => TraktCache.Sync()).Forget();
                }
            }
        }

        private void vlcClient_ConnectionLost(object sender, EventArgs e)
        {
            vlcConnectStatusLabel.Invoke(new MethodInvoker(() => vlcConnectStatusLabel.Text = "VLC Status: not connected"));
            Thread vlcConnectionThread = new Thread(waitForVLCConnection);
            vlcConnectionThread.IsBackground = true;
            vlcConnectionThread.Start();
        }

        public async Task<bool> login()
        {
            if (Client.Authorization.IsRefreshPossible)
            {
                await Client.OAuth.RefreshAuthorizationAsync();
            }
            if (!Client.Authentication.IsAuthorized)
            {
                var authorizeViewModel = new AuthorizeViewModel(Client);
                var window = new AuthorizeView(authorizeViewModel);
                window.ShowDialog();
                await Client.OAuth.GetAuthorizationAsync();
            }
            return Client.Authentication.IsAuthorized;
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            StartSTATask(() => loginThread());
            UpdateListView();
        }

        private async void loginThread()
        {
            if (File.Exists("auth.json")) { Client.Authorization = Extensions.LoadAuthorization(); }
            if (!Client.Authorization.IsValid || Client.Authorization.IsExpired)
            {
                try
                {
                    do
                    {
                        await login();
                    } while (Client.Authentication.IsAuthorized == false);
                    Client.Authorization.Serialize();
                }
                catch (Exception) { return; }
            }
            traktConnectStatusLabel.Invoke(new MethodInvoker(() => traktConnectStatusLabel.Text = traktConnectStatusLabel.Text.Replace("not ", "")));
            Task.Run(() => TraktCache.Sync(NoCache)).Forget();
            NoCache = !NoCache;
        }

        public static Task StartSTATask(Action action)
        {
            var tcs = new TaskCompletionSource<object>();
            Thread thread = new Thread(() =>
            {
                try
                {
                    action();
                    tcs.SetResult(new object());
                }
                catch (Exception e)
                {
                    tcs.SetException(e);
                }
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            return tcs.Task;
        }

        private void UpdateListView()
        {
            lock (watchedListView)
            {
                foreach (TraktWatchedShow watchedShow in TraktCache.watchedList)
                {
                    TraktShowWatchedProgress showProgress;
                    if (TraktCache.progressList.TryGetValue(watchedShow.Show.Ids.Slug, out showProgress))
                    {
                        ProgressBar pb = new ProgressBar() { Minimum = 0, Maximum = showProgress.Aired.Value };
                        pb.Value = showProgress.Completed.Value;
                        var traktRating = TraktCache.ratingList.Where(x => x.Show.Ids.Slug.Equals(watchedShow.Show.Ids.Slug)).FirstOrDefault();
                        int showRating = (traktRating != null && traktRating.Rating.HasValue) ? traktRating.Rating.Value : 0;
                        var ListViewItem = watchedListView.Items.Add(new ListViewItem(new string[] { watchedShow.Show.Title, "", showRating.ToString() }));
                        watchedListView.AddEmbeddedControl(pb, 1, ListViewItem.Index);
                    }
                }
            }
        }

        private async void addEpisodeButton_Click(object sender, EventArgs e)
        {
            if(watchedListView.SelectedItems.Count == 1 && Client.IsValidForUseWithAuthorization)
            {
                TraktWatchedShow show = TraktCache.watchedList.Where(x => x.Show.Title.Equals(watchedListView.SelectedItems[0].Text)).FirstOrDefault();
                TraktShowWatchedProgress progress;
                if(TraktCache.progressList.TryGetValue(show.Show.Ids.Slug, out progress) && show != null)
                {
                    TraktSyncHistoryPostBuilder historyPostBuilder = new TraktSyncHistoryPostBuilder();
                    historyPostBuilder.AddEpisode(progress.NextEpisode);
                    var addEpisodeResponse = await Client.Sync.AddWatchedHistoryItemsAsync(historyPostBuilder.Build());
                    if(addEpisodeResponse.Added.Episodes.HasValue && addEpisodeResponse.Added.Episodes.Value >= 1)
                    {
                        await TraktCache.SyncShowProgress(show.Show);
                        Task.Run(() => TraktCache.Sync()).Forget();
                    }
                }
            }
        }

        private async void removeEpisodeButton_Click(object sender, EventArgs e)
        {
            if (watchedListView.SelectedItems.Count == 1 && Client.IsValidForUseWithAuthorization)
            {
                TraktWatchedShow show = TraktCache.watchedList.Where(x => x.Show.Title.Equals(watchedListView.SelectedItems[0].Text)).FirstOrDefault();
                TraktShowWatchedProgress progress;
                if (TraktCache.progressList.TryGetValue(show.Show.Ids.Slug, out progress) && show != null)
                {
                    int seasonNumber = progress.Seasons.Where(x => x.Completed > 0).MaxBy(x => x.Number).Number.Value;
                    int episodeNumber = progress.Seasons.Where(x => x.Number == seasonNumber).First().Episodes.Where(x => x.Completed == true).MaxBy(x => x.Number).Number.Value;
                    TraktSyncHistoryRemovePostBuilder historyRemoveBuilder = new TraktSyncHistoryRemovePostBuilder();
                    historyRemoveBuilder.AddEpisode(await Client.Episodes.GetEpisodeAsync(show.Show.Ids.Slug, seasonNumber, episodeNumber));
                    var removeEpisodeResponse = await Client.Sync.RemoveWatchedHistoryItemsAsync(historyRemoveBuilder.Build());
                    if (removeEpisodeResponse.Deleted.Episodes.HasValue && removeEpisodeResponse.Deleted.Episodes.Value >= 1)
                    {
                        await TraktCache.SyncShowProgress(show.Show);
                        Task.Run(() => TraktCache.Sync()).Forget();
                    }
                }
            }
        }

        private void addShowButton_Click(object sender, EventArgs e)
        {
            SearchShowForm searchShowForm = new SearchShowForm(TraktCache);
            searchShowForm.Show();
        }

        private async void updateButton_Click(object sender, EventArgs e)
        {
            if (Client.Authentication.IsAuthorized)
            {
                await TraktCache.Update();
                TraktCache.Save();
            }
        }

        private void watchedListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (watchedListView.SelectedItems.Count == 1)
            {
                TraktWatchedShow show = TraktCache.watchedList.Where(x => x.Show.Title.Equals(watchedListView.SelectedItems[0].Text)).FirstOrDefault();
                TraktShowWatchedProgress progress;
                if (show != null && TraktCache.progressList.TryGetValue(show.Show.Ids.Slug, out progress))
                {
                    showNameLabel.Text = show.Show.Title;
                    episodeCountLabel.Text = "/ " + progress.Aired.ToString();
                    currentEpisodeTextBox.Text = progress.Completed.ToString();
                    yearLabel.Text = "Year: " + show.Show.Year.ToString();
                    scoreComboBox.SelectedIndex = scoreComboBox.FindStringExact(watchedListView.SelectedItems[0].SubItems[2].Text);
                    string genres = String.Empty;
                    foreach(String genre in show.Show.Genres)
                    {
                        genres += genre.UpperCase() + ", ";
                    }
                    genreLabel.Text = genres != String.Empty ? "Genre: " + genres.Substring(0, genres.Length - 2) : "Genre: unspecified";
                    if (progress.NextEpisode != null) nextUnwatchedEpisodeLbl.Text = "Next Episode: S" + progress.NextEpisode.SeasonNumber.ToString().PadLeft(2, '0') + "E" + progress.NextEpisode.Number.ToString().PadLeft(2, '0');
                    else nextUnwatchedEpisodeLbl.Text = "Next Episode:";
                    showPosterBox.ImageLocation = Path.Combine(ImageCache.ImagePath, show.Show.Ids.Trakt + ".jpg");

                    if (sender != null)
                    {
                        seasonOverviewTreeView.Nodes.Clear();
                        foreach (TraktSeasonWatchedProgress season in progress.Seasons)
                        {
                            int index = seasonOverviewTreeView.Nodes.Add(new TreeNode("Season " + season.Number) { Checked = (season.Completed == season.Aired) , Tag = season.Number });
                            foreach (TraktEpisodeWatchedProgress episode in season.Episodes)
                            {
                                seasonOverviewTreeView.Nodes[index].Nodes.Add(new TreeNode("Episode " + episode.Number) { Checked = episode.Completed.Value , Tag = episode.Number });
                            }
                        }
                    }
                    else
                    {
                        foreach(TreeNode seasonNode in seasonOverviewTreeView.Nodes)
                        {
                            var seasonProgress = progress.Seasons.Where(x => x.Number.Equals(seasonNode.Tag)).First();
                            seasonNode.Checked = (seasonProgress.Completed == seasonProgress.Aired);
                            foreach(TreeNode episodeNode in seasonNode.Nodes)
                            {
                                episodeNode.Checked = seasonProgress.Episodes.Where(x => x.Number.Equals(episodeNode.Tag)).First().Completed.Value;
                            }
                        }
                    }
                }
            }
        }

        private void watchedListView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if(watchedListView.SelectedItems.Count == 1)
            {
                Process.Start("https://trakt.tv/shows/" + TraktCache.watchedList.ToList().Find(x => x.Show.Title.Equals(watchedListView.SelectedItems[0].Text)).Show.Ids.Slug);
            }
        }

        private async void scoreComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(watchedListView.SelectedItems.Count == 1 && Client.IsValidForUseWithAuthorization)
            {
                var traktRating = TraktCache.ratingList.Where(x => x.Show.Title.Equals(watchedListView.SelectedItems[0].Text)).FirstOrDefault();
                if(traktRating != null)
                {
                    if (!scoreComboBox.SelectedItem.ToString().Equals(traktRating.Rating.ToString()))
                    {
                        int showRating = int.Parse(scoreComboBox.SelectedItem.ToString());
                        TraktSyncRatingsPostBuilder ratingsPostBuilder = new TraktSyncRatingsPostBuilder();
                        if (showRating == 0)
                        {
                            ratingsPostBuilder.AddShow(traktRating.Show);
                            var ratingResponse = await Client.Sync.RemoveRatingsAsync(ratingsPostBuilder.Build());
                            if(ratingResponse.Deleted.Shows.HasValue && ratingResponse.Deleted.Shows.Value >= 1)
                            {
                                await TraktCache.UpdateRatingsList();
                                watchedListView.SelectedItems[0].SubItems[2].Text = scoreComboBox.SelectedItem.ToString();
                            }
                        }
                        else
                        {
                            ratingsPostBuilder.AddShowWithRating(traktRating.Show, showRating);
                            var ratingResponse = await Client.Sync.AddRatingsAsync(ratingsPostBuilder.Build());
                            if (ratingResponse.Added.Shows.HasValue && ratingResponse.Added.Shows.Value >= 1)
                            {
                                traktRating.Rating = int.Parse(scoreComboBox.SelectedItem.ToString());
                                watchedListView.SelectedItems[0].SubItems[2].Text = traktRating.Rating.ToString();
                            }
                        }

                    }
                }
                else
                {
                    TraktWatchedShow show = TraktCache.watchedList.Where(x => x.Show.Title.Equals(watchedListView.SelectedItems[0].Text)).FirstOrDefault();
                    if (show != null && !scoreComboBox.SelectedItem.ToString().Equals("0"))
                    {
                        TraktSyncRatingsPostBuilder ratingsPostBuilder = new TraktSyncRatingsPostBuilder();
                        ratingsPostBuilder.AddShowWithRating(show.Show, int.Parse(scoreComboBox.SelectedItem.ToString()));
                        var ratingResponse = await Client.Sync.AddRatingsAsync(ratingsPostBuilder.Build());
                        if(ratingResponse.Added.Shows.HasValue && ratingResponse.Added.Shows.Value >= 1)
                        {
                            await TraktCache.UpdateRatingsList();
                            watchedListView.SelectedItems[0].SubItems[2].Text = scoreComboBox.SelectedItem.ToString();
                        }
                    }
                }
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            TraktCache.Save();
        }

        private void TraktCache_SyncCompleted(object sender, SyncCompletedEventArgs e)
        {
            foreach (TraktWatchedShow watchedShow in TraktCache.watchedList)
            {
                TraktShowWatchedProgress showProgress;
                if (TraktCache.progressList.TryGetValue(watchedShow.Show.Ids.Slug, out showProgress))
                {
                    ListViewItem lvItem = new ListViewItem();
                    var traktRating = TraktCache.ratingList.Where(x => x.Show.Ids.Slug.Equals(watchedShow.Show.Ids.Slug)).FirstOrDefault();
                    int showRating = (traktRating != null && traktRating.Rating.HasValue) ? traktRating.Rating.Value : 0;
                    lvItem = this.InvokeIfRequired(() => watchedListView.FindItemWithTextExact(watchedShow.Show.Title));
                    if (lvItem != null)
                    {
                        this.InvokeIfRequired(() => lvItem.SubItems[2].Text = showRating.ToString());
                        ProgressBar progressBar = this.InvokeIfRequired(() => watchedListView.GetEmbeddedControl(lvItem)).ConvertTo<ProgressBar>();
                        this.InvokeIfRequired(() => progressBar.Maximum = showProgress.Aired.Value);
                        this.InvokeIfRequired(() => progressBar.Value = showProgress.Completed.Value);
                    }
                    else
                    {
                        ProgressBar progressBar = new ProgressBar() { Maximum = showProgress.Aired.Value, Value = showProgress.Completed.Value };
                        this.InvokeIfRequired(() => watchedListView.Items.Insert(0, new ListViewItem(new string[] { watchedShow.Show.Title, "", showRating.ToString() })));
                        this.InvokeIfRequired(() => watchedListView.AddEmbeddedControl(progressBar, 1, 0));
                    }
                }
            }
            List<ListViewItem> removeList = new List<ListViewItem>();
            for (int i = 0; i < watchedListView.Items.Count; i++)
            {
                TraktWatchedShow show = null;
                string showTitle = string.Empty;
                this.InvokeIfRequired(() => showTitle = watchedListView.Items[i].Text);
                this.InvokeIfRequired(() => show = TraktCache.watchedList.Where(x => x.Show.Title.Equals(showTitle)).FirstOrDefault());
                if(show == null)
                {
                    ListViewItem lvItem = new ListViewItem();
                    this.InvokeIfRequired(() => lvItem = watchedListView.Items[i]);
                    if (lvItem != default(ListViewItem)) removeList.Add(lvItem);
                }
            }
            foreach (ListViewItem lvItem in removeList)
                this.InvokeIfRequired(() => watchedListView.Items.Remove(lvItem));
            this.InvokeIfRequired(() => watchedListView_SelectedIndexChanged(null, EventArgs.Empty));
        }

        private void settingButton_Click(object sender, EventArgs e)
        {
            if (settingsForm == null || settingsForm.IsDisposed)
            {
                settingsForm = new SettingsForm();
                settingsForm.StartPosition = FormStartPosition.Manual;
                settingsForm.Location = MousePosition;
                settingsForm.Show();
            }
            else
                settingsForm.Focus();
        }

        private void seasonOverviewTreeView_BeforeCheck(object sender, TreeViewCancelEventArgs e)
        {
            if(e.Action == TreeViewAction.ByKeyboard || e.Action == TreeViewAction.ByMouse)
                e.Cancel = true;
        }
    }
}
