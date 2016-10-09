using Newtonsoft.Json;
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
using TraktSharp.Examples.Wpf.ViewModels;
using TraktSharp.Examples.Wpf.Views;
using TraktTVUpdateClient.Extension;
using TraktTVUpdateClient.Properties;

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

        private void addEpisodeButton_Click(object sender, EventArgs e)
        {

        }

        private void removeEpisodeButton_Click(object sender, EventArgs e)
        {

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

        }

        private void watchedListView_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }

        private void scoreComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

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
