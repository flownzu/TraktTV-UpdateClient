using MoreLinq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
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
using TraktTVUpdateClient.Cache;
using TraktTVUpdateClient.Extension;
using TraktTVUpdateClient.Forms;
using TraktTVUpdateClient.Properties;
using TraktTVUpdateClient.VLC;

namespace TraktTVUpdateClient
{
    public partial class MainForm : Form
    {
        public TraktClient Client;
        public TraktCache TraktCache;
        public ImageCache ShowPosterCache;
        public VLCConnection vlcClient;
        public TraktShow CurrentShow;
        public TraktEpisode CurrentEpisode;

        private SettingsForm settingsForm;
        private SearchShowForm searchForm;
        private bool vlcThreadStarted;

        public MainForm()
        {
            InitializeComponent();
            Client = new TraktClient(Resources.ClientID, Resources.ClientSecret);
            Client.Authentication.RedirectUri = "app://authorized";
            Client.Configuration.ForceAuthorization = true;
            TraktCache = LoadCache();
            if(TraktCache.TraktClient == null) TraktCache.TraktClient = Client;
            TraktCache.SyncStarted += TraktCache_SyncStarted;
            TraktCache.SyncCompleted += TraktCache_SyncCompleted;
            TraktCache.RequestCached += TraktCache_RequestCached;
            if (Settings.Default.VLCEnabled) Task.Run(() => WaitForVlcConnection()).Forget();
            ShowPosterCache = new ImageCache();
            Task.Run(() => ShowPosterCache.Init()).Forget();
            Task.Run(() => TraktCache.RequestCacheThread());
            ShowPosterCache.SyncCompleted += ShowPosterCache_SyncCompleted;
        }

        private void SyncWatchedListViewWithRequestCache()
        {
            if(TraktCache.RequestCache != null && TraktCache.RequestCache.Count > 0)
            {
                foreach(TraktRequest request in TraktCache.RequestCache)
                {
                    this.InvokeIfRequired(() => TraktCache_RequestCached(this, new RequestCachedEventArgs(request)));
                }
            }
        }

        private void TraktCache_RequestCached(object sender, RequestCachedEventArgs e)
        {
            if (e.Request.Action == TraktRequestAction.RateShow)
            {
                var listViewItem = this.InvokeIfRequired(() => watchedListView.FindItemWithTextExact(e.Request.RequestShow.Title));
                this.InvokeIfRequired(() => listViewItem.SubItems[2].Text = e.Request.RequestValue);
            }
            else if (e.Request.Action == TraktRequestAction.AddEpisode || e.Request.Action == TraktRequestAction.RemoveEpisode)
            {
                var listViewItem = this.InvokeIfRequired(() => watchedListView.FindItemWithTextExact(e.Request.RequestShow.Title));
                var progressBar = this.InvokeIfRequired(() => watchedListView.GetEmbeddedControl(listViewItem)).ConvertTo<ProgressBarEx>();
                if (TraktCache.ProgressList.TryGetValue(e.Request.RequestShow.Ids.Slug, out TraktShowWatchedProgress showProgress))
                {
                    if (e.Request.RequestEpisode != null)
                    {
                        var episode = showProgress.Seasons.Where(x => x.Number.Equals(e.Request.RequestEpisode.SeasonNumber)).FirstOrDefault().Episodes.Where(x => x.Number.Equals(e.Request.RequestEpisode.Number)).FirstOrDefault();
                        if (episode != null)
                        {
                            episode.Completed = e.Request.Action.ToBool();
                            showProgress.Completed += e.Request.Action.ToInt();
                            this.InvokeIfRequired(() => progressBar.Value += e.Request.Action.ToInt());
                            this.InvokeIfRequired(() => progressBar.CustomText = showProgress.Completed + "/" + showProgress.Aired);
                            this.InvokeIfRequired(() => progressBar.Refresh());
                        }
                    }
                    else
                    {
                        Match m = Regex.Match(e.Request.RequestValue, @"S(\d+)E(\d+)");
                        int seasonNumber = int.Parse(m.Groups[1].Value);
                        int episodeNumber = int.Parse(m.Groups[2].Value);
                        var episode = showProgress.Seasons.Where(x => x.Number.Equals(seasonNumber)).FirstOrDefault().Episodes.Where(x => x.Number.Equals(episodeNumber)).FirstOrDefault();
                        if (episode != null)
                        {
                            episode.Completed = e.Request.Action.ToBool();
                            showProgress.Completed += e.Request.Action.ToInt();
                            this.InvokeIfRequired(() => progressBar.Value += e.Request.Action.ToInt());
                            this.InvokeIfRequired(() => progressBar.CustomText = showProgress.Completed + "/" + showProgress.Aired);
                            this.InvokeIfRequired(() => progressBar.Refresh());
                        }
                    }
                    showProgress.NextEpisode = null;
                    this.InvokeIfRequired(() => WatchedListView_SelectedIndexChanged(null, EventArgs.Empty));
                }
            }
        }

        public TraktCache LoadCache(string cacheFile = "cache.json")
        {
            try
            {
                if (File.Exists(cacheFile))
                {
                    using (StreamReader sr = File.OpenText(cacheFile)) { TraktCache = JsonConvert.DeserializeObject<TraktCache>(sr.ReadToEnd()); }
                }
                return TraktCache ?? new TraktCache(Client);
            }
            catch (Exception) { return new TraktCache(Client); }
        }

        public void WaitForVlcConnection()
        {
            if (vlcThreadStarted) return;
            while (Settings.Default.VLCEnabled)
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
                Thread.Sleep(1000);
            }
            if (Settings.Default.VLCEnabled)
            {
                vlcConnectStatusLabel.Invoke(new MethodInvoker(() => vlcConnectStatusLabel.Text = "VLC Status: connected"));
                vlcClient.ConnectionLost += VlcClient_ConnectionLost;
                vlcClient.WatchedPercentReached += VlcClient_WatchedPercentReached;
                vlcClient.MediaItemChanged += VlcClient_MediaItemChanged;
                Task.Run(() => vlcClient.ConnectionThread()).Forget();
            }
            vlcThreadStarted = false;
        }

        private async void VlcClient_MediaItemChanged(object sender, MediaItemChangedEventArgs e)
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
                TraktShow show = await GetClosestMatch(showName, 80);
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
                TraktShow show = await GetClosestMatch(parentFolderName, 80);
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
                    else
                    {
                        List<Task> taskList = new List<Task>();
                        foreach (TraktSeason season in show.Seasons.Where(x => x.Number > 0))
                            taskList.Add(Task.Run(async () => season.Episodes = await Client.Seasons.GetSeasonAsync(show.Ids.Slug, season.Number.Value)));
                        await Task.WhenAll(taskList);

                        m = Regex.Match(fileName, @".*-[_\s]?(\d+)");
                        if(m.Success)
                        {
                            int[] seasonAndEpisodeNumber = show.Seasons.GetEpisodeAndSeasonNumberFromAbsoluteNumber(Int16.Parse(m.Groups[1].Value));
                            CurrentShow = show;
                            CurrentEpisode = await Client.Episodes.GetEpisodeAsync(show.Ids.Slug, seasonAndEpisodeNumber[0], seasonAndEpisodeNumber[1]);
                        }
                    }
                }
                else
                {
                    string parentsParentFolderName = mediaPath[mediaPath.Length - 3];
                    show = await GetClosestMatch(parentsParentFolderName, 80);
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

        private async Task<TraktShow> GetClosestMatch(string showName, double minSimilarity = 0)
        {
            var searchResult = await Client.Search.GetTextQueryResultsAsync(TraktSearchResultType.Show, showName, TraktSearchField.Title, limitPerPage: 15);

            double highestSimilarity = 0;
            TraktShow highestSimilarityShow = null;
            foreach (TraktSearchResult searchResultItem in searchResult.Items)
            {
                double d = Extensions.GetSimilarityRatio(searchResultItem.Show.Title, showName, out double d1, out double d2);
                if (d > highestSimilarity) { highestSimilarity = d; highestSimilarityShow = searchResultItem.Show; }
            }
            if(highestSimilarity >= minSimilarity) return highestSimilarityShow;
            else { return null; }
        }

        private async void VlcClient_WatchedPercentReached(object sender, EventArgs e)
        {
            if (CurrentEpisode != null && CurrentShow != null)
            {
                try
                {
                    if(await Client.MarkEpisodeWatched(CurrentShow, CurrentEpisode))
                    {
                        Task.Run(() => TraktCache.SyncShowProgress(CurrentShow.Ids.Slug)).Forget();
                        this.InvokeIfRequired(() => toolStripEventLabel.Text = "Watched " + CurrentShow.Title + " S" + CurrentEpisode.SeasonNumber.ToString().PadLeft(2, '0') + "E" + CurrentEpisode.Number.ToString().PadLeft(2, '0'));
                    }
                }
                catch (Exception)
                {
                    TraktCache.AddRequestToCache(new TraktRequest() { Action = TraktRequestAction.AddEpisode, RequestEpisode = CurrentEpisode, RequestShow = CurrentShow });
                    this.InvokeIfRequired(() => toolStripEventLabel.Text = "Cached request because it failed!");
                }
            }
        }

        private void VlcClient_ConnectionLost(object sender, EventArgs e)
        {
            vlcConnectStatusLabel.Invoke(new MethodInvoker(() => vlcConnectStatusLabel.Text = "VLC Status: not connected"));
            Thread vlcConnectionThread = new Thread(WaitForVlcConnection)
            {
                IsBackground = true
            };
            vlcConnectionThread.Start();
        }

        public async Task<bool> Login()
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
            StartSTATask(() => LoginThread());
            UpdateListView();
        }

        private async void LoginThread()
        {
            this.InvokeIfRequired(() => toolStripEventLabel.Text = "Logging in...");
            if (File.Exists("auth.json")) { Client.Authorization = Extensions.LoadAuthorization(); }
            if (!Client.Authorization.IsValid || Client.Authorization.IsExpired)
            {
                try
                {
                    do
                    {
                        await Login();
                    } while (Client.Authentication.IsAuthorized == false);
                    Client.Authorization.Serialize();
                }
                catch (Exception) { this.InvokeIfRequired(() => toolStripEventLabel.Text = "Problems logging in, try again in a few minutes."); return; }
            }
            traktConnectStatusLabel.Invoke(new MethodInvoker(() => traktConnectStatusLabel.Text = traktConnectStatusLabel.Text.Replace("not ", "")));
            Task.Run(() => TraktCache.Sync()).Forget();
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
            foreach (TraktWatchedShow watchedShow in TraktCache.WatchedList)
            {
                if (TraktCache.ProgressList.TryGetValue(watchedShow.Show.Ids.Slug, out TraktShowWatchedProgress showProgress))
                {
                    ProgressBarEx pb = new ProgressBarEx() { Minimum = 0, Maximum = showProgress.Aired.Value, DisplayStyle = ProgressBarDisplayText.CustomText, CustomText = showProgress.Completed + "/" + showProgress.Aired };
                    pb.Value = showProgress.Completed.Value;
                    var traktRating = TraktCache.RatingList.Where(x => x.Show.Ids.Slug.Equals(watchedShow.Show.Ids.Slug)).FirstOrDefault();
                    int showRating = (traktRating != null && traktRating.Rating.HasValue) ? traktRating.Rating.Value : 0;
                    var ListViewItem = watchedListView.Items.Add(new ListViewItem(new string[] { watchedShow.Show.Title, "", showRating.ToString(CultureInfo.CurrentCulture) }));
                    watchedListView.AddEmbeddedControl(pb, 1, ListViewItem.Index);
                }
            }
        }

        private async void AddEpisodeButton_Click(object sender, EventArgs e)
        {
            if(watchedListView.SelectedItems.Count == 1 && Client.IsValidForUseWithAuthorization)
            {
                TraktWatchedShow show = TraktCache.WatchedList.Where(x => x.Show.Title.Equals(watchedListView.SelectedItems[0].Text)).FirstOrDefault();
                int seasonNumber = 0;
                int episodeNumber = 0;
                if(TraktCache.ProgressList.TryGetValue(show.Show.Ids.Slug, out TraktShowWatchedProgress progress) && show != null)
                {
                    try
                    {
                        if (progress.NextEpisode != null)
                        {
                            if (await Client.MarkEpisodeWatched(show.Show, progress.NextEpisode))
                            {
                                Task.Run(() => TraktCache.SyncShowProgress(show.Show.Ids.Slug)).Forget();
                                this.InvokeIfRequired(() => toolStripEventLabel.Text = "Watched " + show.Show.Title + " S" + progress.NextEpisode.SeasonNumber.ToString().PadLeft(2, '0') + "E" + progress.NextEpisode.Number.ToString().PadLeft(2, '0'));
                            }
                        }
                        else
                        {
                            seasonNumber = progress.Seasons.Where(x => x.Completed > 0).MaxBy(x => x.Number).Number.Value;
                            var season = progress.Seasons.Where(x => x.Number.Equals(seasonNumber)).First();
                            episodeNumber = 0;
                            if (season.Completed == season.Aired)
                            {
                                var nextSeason = progress.Seasons.Where(x => x.Number.Equals(seasonNumber + 1)).First();
                                if (nextSeason != null && nextSeason.Aired > 0)
                                {
                                    seasonNumber++;
                                    episodeNumber = 1;
                                }
                            }
                            else
                            {
                                episodeNumber = progress.Seasons.Where(x => x.Number == seasonNumber).First().Episodes.Where(x => x.Completed == true).MaxBy(x => x.Number).Number.Value + 1;
                            }
                            if (await Client.MarkEpisodeWatched(show.Show, seasonNumber, episodeNumber))
                            {
                                Task.Run(() => TraktCache.SyncShowProgress(show.Show.Ids.Slug)).Forget();
                                this.InvokeIfRequired(() => toolStripEventLabel.Text = "Watched " + show.Show.Title + " S" + seasonNumber.ToString().PadLeft(2, '0') + "E" + episodeNumber.ToString().PadLeft(2, '0'));
                            }                            
                        }
                    }
                    catch(Exception)
                    {
                        if (progress.NextEpisode != null)
                            TraktCache.AddRequestToCache(new TraktRequest() { Action = TraktRequestAction.AddEpisode, RequestEpisode = progress.NextEpisode, RequestShow = show.Show });
                        else
                            TraktCache.AddRequestToCache(new TraktRequest() { Action = TraktRequestAction.AddEpisode, RequestShow = show.Show, RequestValue = "S" + seasonNumber.ToString().PadLeft(2, '0') + "E" + episodeNumber.ToString().PadLeft(2, '0') });
                        this.InvokeIfRequired(() => toolStripEventLabel.Text = "Cached request because it failed!");
                    }
                }
            }
        }

        private async void RemoveEpisodeButton_Click(object sender, EventArgs e)
        {
            if (watchedListView.SelectedItems.Count == 1 && Client.IsValidForUseWithAuthorization)
            {
                TraktWatchedShow show = TraktCache.WatchedList.Where(x => x.Show.Title.Equals(watchedListView.SelectedItems[0].Text)).FirstOrDefault();
                if (TraktCache.ProgressList.TryGetValue(show.Show.Ids.Slug, out TraktShowWatchedProgress progress) && show != null)
                {
                    int seasonNumber = progress.Seasons.Where(x => x.Completed > 0).MaxBy(x => x.Number).Number.Value;
                    int episodeNumber = progress.Seasons.Where(x => x.Number == seasonNumber).First().Episodes.Where(x => x.Completed == true).MaxBy(x => x.Number).Number.Value;
                    try
                    {
                        if (await Client.RemoveWatchedEpisode(show.Show, seasonNumber, episodeNumber))
                        {
                            Task.Run(() => TraktCache.SyncShowProgress(show.Show.Ids.Slug)).Forget();
                            this.InvokeIfRequired(() => toolStripEventLabel.Text = "Removed " + show.Show.Title + " S" + seasonNumber.ToString().PadLeft(2, '0') + "E" + episodeNumber.ToString().PadLeft(2, '0'));
                        }

                    }
                    catch (Exception)
                    {
                        TraktCache.AddRequestToCache(new TraktRequest() { Action = TraktRequestAction.RemoveEpisode, RequestShow = show.Show, RequestValue = "S" + seasonNumber.ToString().PadLeft(2, '0') + "E" + episodeNumber.ToString().PadLeft(2, '0') });
                        this.InvokeIfRequired(() => toolStripEventLabel.Text = "Cached request because it failed!");
                    }
                }
            }
        }

        private void AddShowButton_Click(object sender, EventArgs e)
        {
            if (searchForm == null || searchForm.IsDisposed)
            {
                searchForm = new SearchShowForm(TraktCache)
                {
                    StartPosition = FormStartPosition.Manual,
                    Location = MousePosition
                };
                searchForm.Show();
            }
            else
                searchForm.Focus();
        }

        private void SyncButton_Click(object sender, EventArgs e)
        {
            if (Client.Authentication.IsAuthorized)
            {
                Task.Run(() => TraktCache.Sync()).Forget();
            }
        }

        private void WatchedListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (watchedListView.SelectedItems.Count == 1)
            {
                TraktWatchedShow show = TraktCache.WatchedList.Where(x => x.Show.Title.Equals(watchedListView.SelectedItems[0].Text)).FirstOrDefault();
                if (show != null && TraktCache.ProgressList.TryGetValue(show.Show.Ids.Slug, out TraktShowWatchedProgress progress))
                {
                    showNameLabel.Text = show.Show.Title;
                    episodeCountLabel.Text = "/ " + progress.Aired.ToString();
                    currentEpisodeTextBox.Text = progress.Completed.ToString();
                    yearLabel.Text = "Year: " + show.Show.Year.ToString();
                    scoreComboBox.SelectedIndex = scoreComboBox.FindStringExact(watchedListView.SelectedItems[0].SubItems[2].Text);
                    genreLabel.Text = "Genre: " + show.Show.Genres.ToGenreString();
                    if (progress.NextEpisode != null) nextUnwatchedEpisodeLbl.Text = "Next Episode: S" + progress.NextEpisode.SeasonNumber.ToString().PadLeft(2, '0') + "E" + progress.NextEpisode.Number.ToString().PadLeft(2, '0');
                    else nextUnwatchedEpisodeLbl.Text = "Next Episode:";
                    showPosterBox.ImageLocation = Path.Combine(ShowPosterCache.ImagePath, show.Show.Ids.Trakt + ".jpg");

                    if (sender != null)
                    {
                        seasonOverviewTreeView.Nodes.Clear();
                        foreach (TraktSeasonWatchedProgress season in progress.Seasons)
                        {
                            int index = seasonOverviewTreeView.Nodes.Add(new TreeNode("Season " + season.Number) { Checked = (season.Completed == season.Aired), Tag = season.Number });
                            foreach (TraktEpisodeWatchedProgress episode in season.Episodes)
                            {
                                seasonOverviewTreeView.Nodes[index].Nodes.Add(new TreeNode("Episode " + episode.Number) { Checked = episode.Completed.Value, Tag = episode.Number });
                            }
                        }
                    }
                    else
                    {
                        foreach (TreeNode seasonNode in seasonOverviewTreeView.Nodes)
                        {
                            var seasonProgress = progress.Seasons.Where(x => x.Number.Equals(seasonNode.Tag)).First();
                            seasonNode.Checked = (seasonProgress.Completed == seasonProgress.Aired);
                            foreach (TreeNode episodeNode in seasonNode.Nodes)
                            {
                                episodeNode.Checked = seasonProgress.Episodes.Where(x => x.Number.Equals(episodeNode.Tag)).First().Completed.Value;
                            }
                        }
                    }
                }
            }
        }

        private void WatchedListView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if(watchedListView.SelectedItems.Count == 1)
            {
                Process.Start("https://trakt.tv/shows/" + TraktCache.WatchedList.ToList().Find(x => x.Show.Title.Equals(watchedListView.SelectedItems[0].Text)).Show.Ids.Slug);
            }
        }

        private async void ScoreComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(watchedListView.SelectedItems.Count == 1 && Client.IsValidForUseWithAuthorization)
            {
                var traktRating = TraktCache.RatingList.Where(x => x.Show.Title.Equals(watchedListView.SelectedItems[0].Text)).FirstOrDefault();
                if(traktRating != null)
                {
                    if (!scoreComboBox.SelectedItem.ToString().Equals(traktRating.Rating.ToString()))
                    {
                        int showRating = int.Parse(scoreComboBox.SelectedItem.ToString());
                        try
                        {
                            if(await Client.RateShow(traktRating.Show, showRating))
                            {
                                this.InvokeIfRequired(() => TraktCache.UpdateRatingsList()).Forget();
                                this.InvokeIfRequired(() => watchedListView.FindItemWithTextExact(traktRating.Show.Title).SubItems[2].Text = showRating.ToString());
                                this.InvokeIfRequired(() => toolStripEventLabel.Text = "Rated " + traktRating.Show.Title + " " + showRating + "/10");
                            }
                        }
                        catch(Exception)
                        {
                            TraktCache.AddRequestToCache(new TraktRequest() { Action = TraktRequestAction.RateShow, RequestShow = traktRating.Show, RequestValue = showRating.ToString() });
                            this.InvokeIfRequired(() => toolStripEventLabel.Text = "Cached request because it failed!");
                        }
                    }
                }
                else
                {
                    TraktWatchedShow show = TraktCache.WatchedList.Where(x => x.Show.Title.Equals(watchedListView.SelectedItems[0].Text)).FirstOrDefault();
                    if (show != null && !scoreComboBox.SelectedItem.ToString().Equals("0"))
                    {
                        int showRating = int.Parse(scoreComboBox.SelectedItem.ToString());
                        try
                        {
                            if(await Client.RateShow(show.Show, showRating))
                            {
                                this.InvokeIfRequired(() => TraktCache.UpdateRatingsList()).Forget();
                                this.InvokeIfRequired(() => watchedListView.FindItemWithTextExact(traktRating.Show.Title).SubItems[2].Text = showRating.ToString());
                                this.InvokeIfRequired(() => toolStripEventLabel.Text = "Rated " + traktRating.Show.Title + " " + showRating + "/10");
                            }
                        }
                        catch(Exception)
                        {
                            TraktCache.AddRequestToCache(new TraktRequest() { Action = TraktRequestAction.RateShow, RequestShow = traktRating.Show, RequestValue = showRating.ToString() });
                            this.InvokeIfRequired(() => toolStripEventLabel.Text = "Cached request because it failed!");
                        }
                    }
                }
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            TraktCache.Save();
        }

        private void TraktCache_SyncStarted(object sender, SyncStartedEventArgs e)
        {
            if(e != SyncStartedEventArgs.PartialSync) this.InvokeIfRequired(() => toolStripEventLabel.Text = "Syncing with trakt.tv started...");
        }

        private void TraktCache_SyncCompleted(object sender, SyncCompletedEventArgs e)
        {
            if (e != SyncCompletedEventArgs.PartialSync)
            {
                this.InvokeIfRequired(() => toolStripEventLabel.Text = "Syncing show poster started...");
                Task.Run(() => ShowPosterCache.Sync(TraktCache)).Forget();
            }
            foreach (TraktWatchedShow watchedShow in TraktCache.WatchedList)
            {
                if (TraktCache.ProgressList.TryGetValue(watchedShow.Show.Ids.Slug, out TraktShowWatchedProgress showProgress))
                {
                    ListViewItem lvItem = new ListViewItem();
                    var traktRating = TraktCache.RatingList.Where(x => x.Show.Ids.Slug.Equals(watchedShow.Show.Ids.Slug)).FirstOrDefault();
                    int showRating = (traktRating != null && traktRating.Rating.HasValue) ? traktRating.Rating.Value : 0;
                    lvItem = this.InvokeIfRequired(() => watchedListView.FindItemWithTextExact(watchedShow.Show.Title));
                    if (lvItem != null)
                    {
                        this.InvokeIfRequired(() => lvItem.SubItems[2].Text = showRating.ToString(CultureInfo.CurrentCulture));
                        ProgressBarEx progressBar = this.InvokeIfRequired(() => watchedListView.GetEmbeddedControl(lvItem)).ConvertTo<ProgressBarEx>();
                        this.InvokeIfRequired(() => progressBar.Maximum = showProgress.Aired.Value);
                        this.InvokeIfRequired(() => progressBar.Value = showProgress.Completed.Value);
                        this.InvokeIfRequired(() => progressBar.CustomText = showProgress.Completed + "/" + showProgress.Aired);
                        this.InvokeIfRequired(() => progressBar.Refresh());
                    }
                    else
                    {
                        ProgressBarEx progressBar = new ProgressBarEx() { Maximum = showProgress.Aired.Value, Value = showProgress.Completed.Value, DisplayStyle = ProgressBarDisplayText.CustomText, CustomText = showProgress.Completed + "/" + showProgress.Aired };
                        this.InvokeIfRequired(() => watchedListView.Items.Insert(0, new ListViewItem(new string[] { watchedShow.Show.Title, "", showRating.ToString(CultureInfo.CurrentCulture) })));
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
                this.InvokeIfRequired(() => show = TraktCache.WatchedList.Where(x => x.Show.Title.Equals(showTitle)).FirstOrDefault());
                if(show == null)
                {
                    ListViewItem lvItem = new ListViewItem();
                    this.InvokeIfRequired(() => lvItem = watchedListView.Items[i]);
                    if (lvItem != default(ListViewItem)) removeList.Add(lvItem);
                }
            }
            foreach (ListViewItem lvItem in removeList)
                this.InvokeIfRequired(() => watchedListView.Items.Remove(lvItem));
            this.InvokeIfRequired(() => WatchedListView_SelectedIndexChanged(null, EventArgs.Empty));
        }

        private void ShowPosterCache_SyncCompleted(object sender, EventArgs e)
        {
            this.InvokeIfRequired(() => toolStripEventLabel.Text = "Cache synced.");
        }

        private void SettingButton_Click(object sender, EventArgs e)
        {
            if (settingsForm == null || settingsForm.IsDisposed)
            {
                settingsForm = new SettingsForm(this)
                {
                    StartPosition = FormStartPosition.Manual,
                    Location = MousePosition
                };
                settingsForm.Show();
            }
            else
                settingsForm.Focus();
        }

        private void SeasonOverviewTreeView_BeforeCheck(object sender, TreeViewCancelEventArgs e)
        {
            if(e.Action == TreeViewAction.ByKeyboard || e.Action == TreeViewAction.ByMouse)
                e.Cancel = true;
        }

        private void SeasonOverviewTreeView_DoubleClick(object sender, EventArgs e)
        {
            TreeNode selectedNode = (sender as TreeViewEx).SelectedNode;

            if (selectedNode != null)
            {
                if (selectedNode.Text.Contains("Season"))
                {
                    int seasonNumber = int.Parse(selectedNode.Text.Replace("Season ", ""), CultureInfo.CurrentCulture);
                    if (watchedListView.SelectedItems.Count == 1)
                    {
                        Process.Start("https://trakt.tv/shows/" + TraktCache.WatchedList.ToList().Find(x => x.Show.Title.Equals(watchedListView.SelectedItems[0].Text)).Show.Ids.Slug + "/seasons/" + seasonNumber);
                    }
                }
                else if (selectedNode.Text.Contains("Episode"))
                {
                    int seasonNumber = int.Parse(selectedNode.Parent.Text.Replace("Season ", ""), CultureInfo.CurrentCulture);
                    int episodeNumber = int.Parse(selectedNode.Text.Replace("Episode ", ""), CultureInfo.CurrentCulture);
                    if (watchedListView.SelectedItems.Count == 1)
                    {
                        Process.Start("https://trakt.tv/shows/" + TraktCache.WatchedList.ToList().Find(x => x.Show.Title.Equals(watchedListView.SelectedItems[0].Text)).Show.Ids.Slug + "/seasons/" + seasonNumber + "/episodes/" + episodeNumber);
                    }
                }
            }
        }

        private void RelogButton_Click(object sender, EventArgs e)
        {
            Client.Authorization = null;
            if(File.Exists("auth.json")) File.Delete("auth.json");
            StartSTATask(() => LoginThread());
        }        
    }
}
