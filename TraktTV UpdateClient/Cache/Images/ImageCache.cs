using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using TraktApiSharp.Objects.Get.Watched;
using TraktTVUpdateClient.Extension;
using TraktTVUpdateClient.Properties;

namespace TraktTVUpdateClient.Cache
{
    static class ImageCache
    {
        private static Uri baseAdress = new Uri("http://webservice.fanart.tv/v3/");
        private static string imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "images");

        public static string ImagePath => imagePath;

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

        public static async Task SaveImage(this FanartImage img, string fileName)
        {
            try
            {
                using(var httpClient = new HttpClient())
                {
                    using(var response = await httpClient.GetAsync(img.url))
                    {
                        var image = await response.Content.ReadAsByteArrayAsync();
                        using(FileStream fs = new FileStream(Path.Combine(imagePath, fileName + GetImageFileExtensionFromURL(img)), FileMode.Create))
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

        public static async Task SaveShowPoster(string tvdbid, string traktid)
        {
            var imgList = await GetImages(tvdbid);
            if(imgList.tvposter != null && imgList.tvposter.Length > 0)
            {
                await imgList.tvposter[0].SaveImage(traktid);
            }
        }

        public static void Sync(TraktCache traktCache)
        {
            if (!Directory.Exists(imagePath)) { Directory.CreateDirectory(imagePath); }
            foreach (TraktWatchedShow show in traktCache.watchedList)
            {
                var dirInfo = new DirectoryInfo(imagePath);
                var fileInfo = dirInfo.GetFiles(show.Show.Ids.Trakt + ".*");
                if(fileInfo.Length == 0)
                {
                    Task.Run(() => SaveShowPoster(show.Show.Ids.Tvdb.ToString(), show.Show.Ids.Trakt.ToString())).Forget();
                }
            }
        }

        private static string GetImageFileExtensionFromURL(this FanartImage img)
        {
            return Path.GetExtension(img.url);
        }
    }
}
