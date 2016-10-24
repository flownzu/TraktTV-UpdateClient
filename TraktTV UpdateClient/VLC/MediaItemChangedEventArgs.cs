using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraktTVUpdateClient.VLC
{
    public class MediaItemChangedEventArgs : EventArgs
    {
        public VLCMediaItem MediaItem { get; set; }

        public MediaItemChangedEventArgs(VLCMediaItem mediaItem)
        {
            MediaItem = mediaItem;
        }
    }
}
