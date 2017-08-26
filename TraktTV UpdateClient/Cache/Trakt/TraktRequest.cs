using System;
using System.Threading.Tasks;
using TraktApiSharp;
using TraktApiSharp.Objects.Get.Shows;
using TraktApiSharp.Objects.Get.Shows.Episodes;
using TraktTVUpdateClient.Extension;

namespace TraktTVUpdateClient.Cache
{
    public class TraktRequest
    {
        public TraktRequestAction Action { get; set; }
        public TraktShow RequestShow { get; set; }
        public TraktEpisode RequestEpisode { get; set; }
        public string RequestValue { get; set; }

        public async Task<bool> Send(TraktClient traktClient)
        {
            try
            {
                if (Action == TraktRequestAction.RateShow)
                {
                    await traktClient.RateShow(RequestShow, int.Parse(RequestValue));
                }
                else if (Action == TraktRequestAction.AddEpisode)
                {
                    if (RequestEpisode != null)
                    {
                        await traktClient.MarkEpisodeWatched(RequestShow, RequestEpisode);
                    }
                }
                else if (Action == TraktRequestAction.RemoveEpisode)
                {
                    if (RequestEpisode != null)
                    {
                        await traktClient.RemoveWatchedEpisode(RequestShow, RequestEpisode);
                    }
                }
                return true;
            }
            catch (Exception) { return false; }
        }
    }

    public enum TraktRequestAction
    {
        AddEpisode,
        RemoveEpisode,
        RateShow
    }
}
