using System;

namespace TraktTVUpdateClient.Cache
{
    public class SyncStartedEventArgs : EventArgs
    {
        public static readonly SyncStartedEventArgs CompleteSync = new SyncStartedEventArgs(true);
        public static readonly SyncStartedEventArgs PartialSync = new SyncStartedEventArgs(false);
        public bool completeSync;

        public SyncStartedEventArgs(bool _completeSync)
        {
            completeSync = _completeSync;
        }
    }
}
