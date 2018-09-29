using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using TraktApiSharp.Attributes;

namespace TraktTVUpdateClient.Cache
{
    /// <summary>
    /// Contains the image information and file path of the image file
    /// </summary>
    public class TmdbImage
    {
        /// <summary>
        /// Gets or sets the aspect ratio of the image
        /// </summary>
        [JsonProperty(PropertyName = "aspect_ratio")]
        public decimal AspectRatio { get; set; }

        /// <summary>
        /// Gets or sets the file path of the image needed to build the image url
        /// </summary>
        [JsonProperty(PropertyName = "file_path")]
        public string FilePath { get; set; }

        /// <summary>
        /// Gets or sets the original image height
        /// </summary>
        [JsonProperty(PropertyName = "height")]
        public int Height { get; set; }

        /// <summary>
        /// Gets or sets the image language
        /// <para>Nullable</para>
        /// </summary>
        [Nullable]
        [JsonProperty(PropertyName = "iso_639_1")]
        public string Language { get; set; }

        /// <summary>
        /// Gets or sets the average vote for the image
        /// </summary>
        [JsonProperty(PropertyName = "vote_average")]
        public decimal VoteAverage { get; set; }

        /// <summary>
        /// Gets or sets the vote count of the image
        /// </summary>
        [JsonProperty(PropertyName = "vote_count")]
        public int VoteCount { get; set; }

        /// <summary>
        /// Gets or sets the original image width
        /// </summary>
        [JsonProperty(PropertyName = "width")]
        public int Width { get; set; }

        public async Task Save(string imagePath, string imageBaseURL, string size)
        {
            try
            {
                using (var httpClient = new HttpClient() { BaseAddress = new Uri(imageBaseURL) })
                {
                    using (var response = await httpClient.GetAsync(size + "/" + FilePath))
                    {
                        var image = await response.Content.ReadAsByteArrayAsync();
                        using (FileStream fs = new FileStream(imagePath + Path.GetExtension(FilePath), FileMode.Create))
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
