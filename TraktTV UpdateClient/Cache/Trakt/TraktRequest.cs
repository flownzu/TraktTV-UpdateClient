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
        public TraktRequestAction action { get; set; }
        public TraktShow RequestShow { get; set; }
        public TraktEpisode RequestEpisode { get; set; }
        public string RequestValue { get; set; }

        public async Task<bool> Send(TraktClient traktClient)
        {
            try
            {
                if (action == TraktRequestAction.RateShow)
                {
                    await traktClient.RateShow(RequestShow, int.Parse(RequestValue));
                }
                else if (action == TraktRequestAction.AddEpisode)
                {
                    if (RequestEpisode != null)
                    {
                        await traktClient.MarkEpisodeWatched(RequestShow, RequestEpisode);
                    }
                }
                else if (action == TraktRequestAction.RemoveEpisode)
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
