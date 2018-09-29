using Newtonsoft.Json;

namespace TraktTVUpdateClient.Cache.Images.TheTVDB
{
    public class TvdbImageResponse
    {
        [JsonProperty(PropertyName = "data")]
        public TvdbImage[] Images { get; set; }

        public TvdbJsonErrors Errors { get; set; }
    }
}
