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
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the TVDB ID.
        /// </summary>
        [JsonProperty(PropertyName = "thetvdb_id")]
        public string TvdbId { get; set; }

        /// <summary>
        /// Gets or sets the clearlogo images.
        /// </summary>
        [JsonProperty(PropertyName = "clearlogo")]
        public FanartImage[] ClearLogo { get; set; }

        /// <summary>
        /// Gets or sets the hdtvlogo images.
        /// </summary>
        [JsonProperty(PropertyName = "hdtvlogo")]
        public FanartImage[] HdtvLogo { get; set; }

        /// <summary>
        /// Gets or sets the seasonposter images.
        /// </summary>
        [JsonProperty(PropertyName = "seasonposter")]
        public FanartImage[] SeasonPoster { get; set; }

        /// <summary>
        /// Gets or sets the tvthumb images.
        /// </summary>
        [JsonProperty(PropertyName = "tvthumb")]
        public FanartImage[] TvThumb { get; set; }

        /// <summary>
        /// Gets or sets the showbackground images.
        /// </summary>
        [JsonProperty(PropertyName = "showbackground")]
        public FanartImage[] ShowBackground { get; set; }

        /// <summary>
        /// Gets or sets the tvposter images.
        /// </summary>
        [JsonProperty(PropertyName = "tvposter")]
        public FanartImage[] TvPoster { get; set; }

        /// <summary>
        /// Gets or sets the tvbanner images.
        /// </summary>
        [JsonProperty(PropertyName = "tvbanner")]
        public FanartImage[] TvBanner { get; set; }

        /// <summary>
        /// Gets or sets the clearart images.
        /// </summary>
        [JsonProperty(PropertyName = "clearart")]
        public FanartImage[] ClearArt { get; set; }

        /// <summary>
        /// Gets or sets the hdclearart images.
        /// </summary>
        [JsonProperty(PropertyName = "hdclearart")]
        public FanartImage[] HdClearArt { get; set; }

        /// <summary>
        /// Gets or sets the seasonthumb images.
        /// </summary>
        [JsonProperty(PropertyName = "seasonthumb")]
        public FanartImage[] SeasonThumb { get; set; }

        /// <summary>
        /// Gets or sets the characterart images.
        /// </summary>
        [JsonProperty(PropertyName = "characterart")]
        public FanartImage[] CharacterArt { get; set; }

        /// <summary>
        /// Gets or sets the seasonbanner images.
        /// </summary>
        [JsonProperty(PropertyName = "seasonbanner")]
        public FanartImage[] SeasonBanner { get; set; }
    }
}