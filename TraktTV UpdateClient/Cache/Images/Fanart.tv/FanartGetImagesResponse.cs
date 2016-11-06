using Newtonsoft.Json;

namespace TraktTVUpdateClient.Cache
{
    /// <summary>
    /// Stores the fanart.tv images of a show
    /// </summary>
    class FanartGetImagesResponse
    {
        /// <summary>
        /// Gets or sets the tv shows name.
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string name { get; set; }

        /// <summary>
        /// Gets or sets the TVDB ID.
        /// </summary>
        [JsonProperty(PropertyName = "thetvdb_id")]
        public string tvdbid { get; set; }

        /// <summary>
        /// Gets or sets the clearlogo images.
        /// </summary>
        [JsonProperty(PropertyName = "clearlogo")]
        public FanartImage[] clearlogo { get; set; }

        /// <summary>
        /// Gets or sets the hdtvlogo images.
        /// </summary>
        [JsonProperty(PropertyName = "hdtvlogo")]
        public FanartImage[] hdtvlogo { get; set; }

        /// <summary>
        /// Gets or sets the seasonposter images.
        /// </summary>
        [JsonProperty(PropertyName = "seasonposter")]
        public FanartImage[] seasonposter { get; set; }

        /// <summary>
        /// Gets or sets the tvthumb images.
        /// </summary>
        [JsonProperty(PropertyName = "tvthumb")]
        public FanartImage[] tvthumb { get; set; }

        /// <summary>
        /// Gets or sets the showbackground images.
        /// </summary>
        [JsonProperty(PropertyName = "showbackground")]
        public FanartImage[] showbackground { get; set; }

        /// <summary>
        /// Gets or sets the tvposter images.
        /// </summary>
        [JsonProperty(PropertyName = "tvposter")]
        public FanartImage[] tvposter { get; set; }

        /// <summary>
        /// Gets or sets the tvbanner images.
        /// </summary>
        [JsonProperty(PropertyName = "tvbanner")]
        public FanartImage[] tvbanner { get; set; }

        /// <summary>
        /// Gets or sets the clearart images.
        /// </summary>
        [JsonProperty(PropertyName = "clearart")]
        public FanartImage[] clearart { get; set; }

        /// <summary>
        /// Gets or sets the hdclearart images.
        /// </summary>
        [JsonProperty(PropertyName = "hdclearart")]
        public FanartImage[] hdclearart { get; set; }

        /// <summary>
        /// Gets or sets the seasonthumb images.
        /// </summary>
        [JsonProperty(PropertyName = "seasonthumb")]
        public FanartImage[] seasonthumb { get; set; }

        /// <summary>
        /// Gets or sets the characterart images.
        /// </summary>
        [JsonProperty(PropertyName = "characterart")]
        public FanartImage[] characterart { get; set; }

        /// <summary>
        /// Gets or sets the seasonbanner images.
        /// </summary>
        [JsonProperty(PropertyName = "seasonbanner")]
        public FanartImage[] seasonbanner { get; set; }
    }
}