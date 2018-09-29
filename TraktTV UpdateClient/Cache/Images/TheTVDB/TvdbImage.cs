using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace TraktTVUpdateClient.Cache.Images.TheTVDB
{
    public class TvdbImage
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "keyType")]
        public string KeyType { get; set; }

        [JsonProperty(PropertyName = "subKey")]
        public string SubKey { get; set; }

        [JsonProperty(PropertyName = "fileName")]
        public string FileName { get; set; }

        [JsonProperty(PropertyName = "languageId")]
        public int LanguageId { get; set; }

        [JsonProperty(PropertyName = "resolution")]
        public string Resolution { get; set; }

        [JsonProperty(PropertyName = "ratingsInfo")]
        public RatingsInfo RatingsInfo { get; set; }

        [JsonProperty(PropertyName = "thumbnail")]
        public string Thumbnail { get; set; }

        public async Task Save(string imagePath)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync("https://thetvdb.com/banners/" + Thumbnail))
                    {
                        var image = await response.Content.ReadAsByteArrayAsync();
                        using (FileStream fs = new FileStream(imagePath + Path.GetExtension(Thumbnail), FileMode.Create))
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
