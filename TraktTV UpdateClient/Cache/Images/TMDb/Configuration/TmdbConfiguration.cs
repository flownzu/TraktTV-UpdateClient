using Newtonsoft.Json;

namespace TraktTVUpdateClient.Cache
{
    /// <summary>
    /// Contains the image configuration which is needed to build image URLs and change keys
    /// </summary>
    public class TmdbConfiguration
    {
        /// <summary>
        /// Contains the base url and valid sizes needed to build the image url
        /// </summary>
        [JsonProperty(PropertyName = "images")]
        public TmbdImageConfiguration ImageConfiguration { get; set; }

        /// <summary>
        /// Contains a list of change keys that can be used when consuming data from the change feed
        /// </summary>
        [JsonProperty(PropertyName = "change_keys")]
        public string[] changeKeys;
    }
}
