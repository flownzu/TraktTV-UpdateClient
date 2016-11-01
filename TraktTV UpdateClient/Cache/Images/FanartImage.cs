using Newtonsoft.Json;
using TraktApiSharp.Attributes;

namespace TraktTVUpdateClient.Cache
{
    /// <summary>
    /// Image from fanart.tv
    /// </summary>
    class FanartImage
    {
        /// <summary>
        /// Gets or sets the image id.
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public string id { get; set; }

        /// <summary>
        /// Gets or sets the image url.
        /// </summary>
        [JsonProperty(PropertyName = "url")]
        public string url { get; set; }

        /// <summary>
        /// Gets or sets the image language.
        /// </summary>
        [JsonProperty(PropertyName = "lang")]
        public string lang { get; set; }

        /// <summary>
        /// Gets or sets the image likes.
        /// </summary>
        [JsonProperty(PropertyName = "likes")]
        public string likes { get; set; }

        /// <summary>
        /// Gets or sets the season the image is associated with.
        /// <para>Nullable</para>
        /// </summary>
        [JsonProperty(PropertyName = "season")]
        [Nullable]
        public string season { get; set; }
    }
}
