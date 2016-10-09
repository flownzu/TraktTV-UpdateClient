using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TraktApiSharp;
using TraktSharp.Examples.Wpf.ViewModels;
using TraktSharp.Examples.Wpf.Views;
using TraktTVUpdateClient.Properties;
using System.Diagnostics;

namespace TraktTVUpdateClient
{
    public partial class MainForm : Form
    {
        public TraktClient Client;
        public Cache TraktCache;

        public MainForm() {
            InitializeComponent();
            Client = new TraktClient(Resources.ClientID, Resources.ClientSecret);
            Client.Authentication.RedirectUri = "app://authorized";
            Client.Configuration.ForceAuthorization = true;
            TraktCache = new Cache(Client);
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
    }
}
