using Newtonsoft.Json;

namespace TraktTVUpdateClient.Cache
{
    /// <summary>
    /// Contains the base url and valid sizes needed to build the image url
    /// </summary>
    public class TmbdImageConfiguration
    {
        /// <summary>
        /// Image base url
        /// </summary>
        [JsonProperty(PropertyName = "base_url")]
        public string BaseUrl { get; set; }

        /// <summary>
        /// Secure image base url
        /// </summary>
        [JsonProperty(PropertyName = "secure_base_url")]
        public string SecureBaseUrl { get; set; }

        /// <summary>
        /// Array of valid backdrop sizes
        /// </summary>
        [JsonProperty(PropertyName = "backdrop_sizes")]
        public string[] BackdropSizes { get; set; }

        /// <summary>
        /// Array of valid logo sizes
        /// </summary>
        [JsonProperty(PropertyName = "logo_sizes")]
        public string[] LogoSizes { get; set; }

        /// <summary>
        /// Array of valid poster sizes
        /// </summary>
        [JsonProperty(PropertyName = "poster_sizes")]
        public string[] PosterSizes { get; set; }

        /// <summary>
        /// Array of valid profile sizes
        /// </summary>
        [JsonProperty(PropertyName = "profile_sizes")]
        public string[] ProfileSizes { get; set; }

        /// <summary>
        /// Array of valid still sizes
        /// </summary>
        [JsonProperty(PropertyName = "still_sizes")]
        public string[] StillSizes { get; set; }
    }
}
