using TraktApiSharp.Objects.Get.Shows;
using TraktApiSharp.Objects.Get.Shows.Episodes;

namespace TraktTVUpdateClient.Cache
{
    public class TraktRequest
    {
        public TraktRequestAction action { get; set; }
        public TraktShow RequestShow { get; set; }
        public TraktEpisode RequestEpisode { get; set; }
        public string RequestValue { get; set; }
    }

    public enum TraktRequestAction
    {
        AddEpisode,
        RemoveEpisode,
        RateShow
    }
}
