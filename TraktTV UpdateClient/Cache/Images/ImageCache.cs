using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using TraktTVUpdateClient.Properties;

namespace TraktTVUpdateClient.Cache
{
    static class ImageCache
    {
        private static Uri baseAdress = new Uri("http://webservice.fanart.tv/v3/");

        public static async Task<FanartShowImages> GetImages(string tvdbid)
        {
            try
            {
                using (var httpClient = new HttpClient() { BaseAddress = baseAdress })
                {
                    using (var response = await httpClient.GetAsync("tv/" + tvdbid + "?api_key=" + Resources.FanartAPIKey))
                    {
                        return JsonConvert.DeserializeObject<FanartShowImages>(await response.Content.ReadAsStringAsync());
                    }
                }
            }
            catch (Exception) { return null; }
        }

        public static async Task SaveImage(this FanartImage img)
        {
            try
            {
                using(var httpClient = new HttpClient())
                {
                    using(var response = await httpClient.GetAsync(img.url))
                    {
                        var image = await response.Content.ReadAsByteArrayAsync();
                        using(FileStream fs = new FileStream("testimage.png", FileMode.Create))
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
