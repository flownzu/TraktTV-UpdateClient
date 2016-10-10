using Newtonsoft.Json;
using MoreLinq;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TraktApiSharp;
using TraktApiSharp.Objects.Basic;
using TraktApiSharp.Objects.Get.Shows;
using TraktApiSharp.Objects.Get.Watched;
using TraktApiSharp.Objects.Post.Syncs.History;
using TraktSharp.Examples.Wpf.ViewModels;
using TraktSharp.Examples.Wpf.Views;
using TraktTVUpdateClient.Extension;
using TraktTVUpdateClient.Properties;
using TraktApiSharp.Objects.Post.Syncs.Ratings;

namespace TraktTVUpdateClient
{
    public partial class MainForm : Form
    {
        public TraktClient Client;
        public Cache TraktCache;
        public bool NoCache = true;

        public MainForm() {
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
            UpdateListView(true);
            TraktCache.SyncCompleted += TraktCache_SyncCompleted;
        }

        public Cache LoadCache(string cacheFile = "cache.json")
        {
            if (File.Exists(cacheFile))
            {
                using (StreamReader sr = File.OpenText(cacheFile)) { TraktCache = JsonConvert.DeserializeObject<Cache>(sr.ReadToEnd()); }
            }
            return TraktCache == null ? new Cache(Client) : TraktCache;
        }

        public async Task<bool> login() {
            var authorizeViewModel = new AuthorizeViewModel(Client);
            var window = new AuthorizeView(authorizeViewModel);
            window.ShowDialog();
            await Client.OAuth.GetAuthorizationAsync();
            return Client.Authentication.IsAuthorized;
        }

        private void MainForm_Shown(object sender, EventArgs e) {
            StartSTATask(() => loginThread());
        }

        private async void loginThread()
        {
            do
            {
                await login();
            } while (Client.Authentication.IsAuthorized == false);
            connectStatusLabel.Invoke(new MethodInvoker(() => connectStatusLabel.Text = connectStatusLabel.Text.Replace("not ", "")));
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

        private void UpdateListView(bool programStart = false)
        {
            lock (this.watchedListView)
            {
                if (programStart)
                {
                    foreach(TraktWatchedShow watchedShow in TraktCache.watchedList)
                    {
                        TraktShowWatchedProgress showProgress;
                        if(TraktCache.progressList.TryGetValue(watchedShow.Show.Ids.Slug, out showProgress))
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
                else
                {
                    foreach(TraktWatchedShow watchedShow in TraktCache.watchedList)
                    {
                        TraktShowWatchedProgress showProgress;
                        if(TraktCache.progressList.TryGetValue(watchedShow.Show.Ids.Slug, out showProgress))
                        {
                            var traktRating = TraktCache.ratingList.Where(x => x.Show.Ids.Slug.Equals(watchedShow.Show.Ids.Slug)).FirstOrDefault();
                            int showRating = (traktRating != null && traktRating.Rating.HasValue) ? traktRating.Rating.Value : 0;
                            var ListViewItem = watchedListView.FindItemWithText(watchedShow.Show.Title);
                            if(ListViewItem != null)
                            {
                                ProgressBar pb = (ProgressBar)watchedListView.GetEmbeddedControl(ListViewItem);
                                pb.Maximum = showProgress.Aired.Value;
                                pb.Value = showProgress.Completed.Value;
                                ListViewItem.SubItems[2].Text = showRating.ToString();
                            }
                            else
                            {
                                ProgressBar pb = new ProgressBar() { Minimum = 0, Maximum = showProgress.Aired.Value };
                                pb.Value = showProgress.Completed.Value;
                                ListViewItem = watchedListView.Items.Add(new ListViewItem(new string[] { watchedShow.Show.Title, "", showRating.ToString() }));
                                watchedListView.AddEmbeddedControl(pb, 1, ListViewItem.Index);
                            }
                        }
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
                        UpdateListView();
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
                    int seasonNumber = progress.Seasons.MaxBy(x => x.Number).Number.Value; //get latest season the user has watched
                    int episodeNumber = progress.Seasons.MaxBy(x => x.Number).Episodes.MaxBy(x => x.Number).Number.Value; //get latest episode the user has watched of the latest season
                    TraktSyncHistoryRemovePostBuilder historyRemoveBuilder = new TraktSyncHistoryRemovePostBuilder();
                    historyRemoveBuilder.AddEpisode(await Client.Episodes.GetEpisodeAsync(show.Show.Ids.Slug, seasonNumber, episodeNumber));
                    var removeEpisodeResponse = await Client.Sync.RemoveWatchedHistoryItemsAsync(historyRemoveBuilder.Build());
                    if (removeEpisodeResponse.Deleted.Episodes.HasValue && removeEpisodeResponse.Deleted.Episodes.Value >= 1)
                    {
                        await TraktCache.SyncShowProgress(show.Show);
                        UpdateListView();
                    }
                }
            }
        }

        private void addShowButton_Click(object sender, EventArgs e)
        {

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
                if (TraktCache.progressList.TryGetValue(show.Show.Ids.Slug, out progress) && show != null)
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
                    episodeProgressBar.Maximum = progress.Aired.GetValueOrDefault();
                    episodeProgressBar.Value = progress.Completed.GetValueOrDefault();
                }
            }
        }

        private void watchedListView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if(watchedListView.SelectedItems.Count == 1)
            {
                Process.Start("http://www.trakt.tv/shows/" + TraktCache.watchedList.ToList().Find(x => x.Show.Title.Equals(watchedListView.SelectedItems[0].Text)).Show.Ids.Slug);
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

        private void TraktCache_SyncCompleted(object sender, EventArgs e)
        {
            UpdateListView();
        }
    }
}
