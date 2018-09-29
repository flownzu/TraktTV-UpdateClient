using Newtonsoft.Json;

namespace TraktTVUpdateClient.Cache.Images.TheTVDB
{
    public class RatingsInfo
    {
        [JsonProperty(PropertyName = "average")]
        public double Average { get; set; }

        [JsonProperty(PropertyName = "count")]
        public int Count { get; set; }
    }
}
