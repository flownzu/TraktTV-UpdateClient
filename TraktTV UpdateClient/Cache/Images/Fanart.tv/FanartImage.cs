using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
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
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the image url.
        /// </summary>
        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the image language.
        /// </summary>
        [JsonProperty(PropertyName = "lang")]
        public string Lang { get; set; }

        /// <summary>
        /// Gets or sets the image likes.
        /// </summary>
        [JsonProperty(PropertyName = "likes")]
        public string Likes { get; set; }

        /// <summary>
        /// Gets or sets the season the image is associated with.
        /// <para>Nullable</para>
        /// </summary>
        [JsonProperty(PropertyName = "season")]
        [Nullable]
        public string Season { get; set; }

        public string GetFileExtension()
        {
            return Path.GetExtension(Url);
        }

        public async Task Save(string imagePath)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync(Url))
                    {
                        var image = await response.Content.ReadAsByteArrayAsync();
                        using (FileStream fs = new FileStream(imagePath + GetFileExtension(), FileMode.Create))
                        {
                            using (BinaryWriter bw = new BinaryWriter(fs))
                            {
                                bw.Write(image);
                            }
                        }
                    }
                }
            }
            catch (Exception) { }
        }
    }
}