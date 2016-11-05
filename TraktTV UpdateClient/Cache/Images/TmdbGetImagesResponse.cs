using Newtonsoft.Json;

namespace TraktTVUpdateClient.Cache
{
    /// <summary>
    /// Get images response that includes a list backdrop images, a list of poster images and the TMDb ID of the show
    /// </summary>
    public class TmdbGetImagesResponse
    {
        /// <summary>
        /// Gets or sets a list of backdrop images
        /// </summary>
        [JsonProperty(PropertyName = "backdrops")]
        public TmdbImage[] backdrops { get; set; }

        /// <summary>
        /// Gets or sets the tv show id
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public int id { get; set; }

        /// <summary>
        /// Gets or sets a list of poster images
        /// </summary>
        [JsonProperty(PropertyName = "posters")]
        public TmdbImage[] posters { get; set; }
    }
}
