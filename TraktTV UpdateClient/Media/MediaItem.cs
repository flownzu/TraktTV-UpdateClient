using System.Diagnostics;

namespace TraktTVUpdateClient.Media
{
    public class MediaItem
    {
        public string FilePath { get; set; }

        public bool AbsoluteNumber { get; set; }

        public int Season { get; set; }

        public int EpisodeNumberStart { get; set; }

        public int EpisodeNumberEnd { get; set; }

        public int AbsoluteNumberStart { get; set; }

        public int AbsoluteNumberEnd { get; set; }

        public void Play()
        {
            try
            {
                Process.Start(FilePath);
            }
            catch { }
        }
    }
}
