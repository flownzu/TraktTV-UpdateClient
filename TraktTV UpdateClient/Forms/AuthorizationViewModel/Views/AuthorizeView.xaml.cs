using System.Windows.Navigation;
using TraktTVUpdateClient.Extension;

namespace TraktTVUpdateClient.Forms
{

    internal partial class AuthorizeView {

		private AuthorizeViewModel ViewModel { get; set; }

		public AuthorizeView(AuthorizeViewModel viewModel) {
			InitializeComponent();
			ViewModel = viewModel;
			DataContext = ViewModel;
			Load();
		}

		private void Load() {
			AuthorizeBrowser.Navigate(ViewModel.Client.OAuth.CreateAuthorizationUrlNoPin());
		}

		private void AuthorizeBrowserNavigating(object sender, NavigatingCancelEventArgs e) { ViewModel.Navigating(this, e); }

	}

}