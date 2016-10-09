using System;
using System.Linq;
using System.Reflection;
using System.Windows.Navigation;
using Microsoft.Win32;
using TraktSharp.Examples.Wpf.Views;
using TraktApiSharp;
using TraktTVUpdateClient.Extension;
using TraktApiSharp.Authentication;
using System.Windows;
using System.Threading;

namespace TraktSharp.Examples.Wpf.ViewModels {

	internal class AuthorizeViewModel : ViewModelBase {

		internal AuthorizeViewModel(TraktClient traktClient) {

			// Teach the WebBrowser control some manners
			NativeMethods.DisableInternetExplorerClickSounds();

			Registry.SetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION",
				string.Format("{0}.exe", Assembly.GetExecutingAssembly().GetName().Name), 0, RegistryValueKind.DWord);
			Registry.SetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION",
				string.Format("{0}.vshost.exe", Assembly.GetExecutingAssembly().GetName().Name), 0, RegistryValueKind.DWord);

			Client = traktClient;
		}

		internal TraktClient Client { get; private set; }

		internal void Navigating(AuthorizeView sender, NavigatingCancelEventArgs e) {
			if (!e.Uri.AbsoluteUri.StartsWith(Client.Authentication.RedirectUri, StringComparison.CurrentCultureIgnoreCase)) {
				return;
			}
            Client.Authentication.OAuthAuthorizationCode = e.Uri.ToString().Replace("app://authorized/?code=", "");
            e.Cancel = true;
			sender.DialogResult = true;
			sender.Close();
		}

	}

}