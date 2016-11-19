using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraktTVUpdateClient.Cache
{
    public class SyncCompletedEventArgs : EventArgs
    {
        public static readonly SyncCompletedEventArgs CompleteSync = new SyncCompletedEventArgs(true);
        public static readonly SyncCompletedEventArgs PartialSync = new SyncCompletedEventArgs(false);
        public bool completeSync;

        public SyncCompletedEventArgs(bool _completeSync)
        {
            completeSync = _completeSync;
        }
    }
}
