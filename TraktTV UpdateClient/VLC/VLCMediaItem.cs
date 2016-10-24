using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraktTVUpdateClient.VLC
{
    public class VLCMediaItem
    {
        public string Path;
        public string Title;
        public string State;
        public uint Length;
        public bool WatchedPercentReached;

        public VLCMediaItem()
        {
            Path = String.Empty;
            Title = String.Empty;
            State = String.Empty;
            Length = 0;
            WatchedPercentReached = false;
        }

        public VLCMediaItem(string path, string title, uint length)
        {
            Path = path;
            Title = title;
            Length = length;
            WatchedPercentReached = false;
            State = String.Empty;
        }
    }
}
