using System;
using System.Runtime.InteropServices;

namespace TraktTVUpdateClient.Forms
{

    internal static class NativeMethods {
        [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool InternetSetOption(int hInternet, int dwOption, IntPtr lpBuffer, int dwBufferLength);

		[DllImport("urlmon.dll"), PreserveSig]
		[return: MarshalAs(UnmanagedType.Error)]
		private static extern int CoInternetSetFeatureEnabled(int featureEntry, [MarshalAs(UnmanagedType.U4)] int dwFlags, bool fEnable);

		internal static void DisableInternetExplorerClickSounds() { CoInternetSetFeatureEnabled(21, 0x00000002, true); }
        internal static unsafe void SupressWinInetBehaviour()
        {
            int option = (int)3;
            int* optionPtr = &option;
            bool success = InternetSetOption(0, 81, new IntPtr(optionPtr), sizeof(int));
        }
	}

}