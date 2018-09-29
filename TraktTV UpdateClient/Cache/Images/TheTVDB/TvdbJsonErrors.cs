using Newtonsoft.Json;

namespace TraktTVUpdateClient.Cache.Images.TheTVDB
{
    public class TvdbJsonErrors
    {
        [JsonProperty(PropertyName = "invalidFilters")]
        public string[] InvalidFilters { get; set; }

        [JsonProperty(PropertyName = "invalidLanguage")]
        public string InvalidLanguage { get; set; }

        [JsonProperty(PropertyName = "invalidParams")]
        public string[] InvalidQueryParams { get; set; }
    }
}
