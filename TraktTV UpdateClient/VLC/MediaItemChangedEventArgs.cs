using System;

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
