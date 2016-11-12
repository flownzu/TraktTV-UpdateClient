﻿using System.Runtime.InteropServices;

namespace TraktTVUpdateClient.Forms
{

    internal static class NativeMethods {

		[DllImport("urlmon.dll"), PreserveSig]
		[return: MarshalAs(UnmanagedType.Error)]
		private static extern int CoInternetSetFeatureEnabled(int featureEntry, [MarshalAs(UnmanagedType.U4)] int dwFlags, bool fEnable);

		internal static void DisableInternetExplorerClickSounds() { CoInternetSetFeatureEnabled(21, 0x00000002, true); }

	}

}