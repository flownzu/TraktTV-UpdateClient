using System;
using System.Linq;
using System.Threading;
using System.Windows.Navigation;
using TraktTVUpdateClient.Extension;
using TraktSharp.Examples.Wpf.ViewModels;

namespace TraktSharp.Examples.Wpf.Views {

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