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
    public class ImageCache
    {
        internal Uri baseAdress = new Uri("https://api.themoviedb.org/3/");
        internal TmdbConfiguration configuration;

        public bool IsReadyForImageCaching = false;
        public string ImagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "images");


        public ImageCache()
        {

        }

        public async Task Init()
        {
            configuration = await GetConfiguration();
            IsReadyForImageCaching = true;
        }

        private async Task<TmdbConfiguration> GetConfiguration()
        {
            try
            {
                using(var httpClient = new HttpClient() { BaseAddress = baseAdress })
                {
                    using(var response = await httpClient.GetAsync("configuration?api_key=" + Resources.TMDbAPIKey))
                    {
                        return JsonConvert.DeserializeObject<TmdbConfiguration>(await response.Content.ReadAsStringAsync());
                    }
                }
            }
            catch (Exception) { return null; }
        }

        public async Task<TmdbGetImagesResponse> GetImages(string tmdb_id)
        {
            try
            {
                using (var httpClient = new HttpClient() { BaseAddress = baseAdress })
                {
                    using (var response = await httpClient.GetAsync("tv/" + tmdb_id + "/images?api_key=" + Resources.TMDbAPIKey))
                    {
                        return JsonConvert.DeserializeObject<TmdbGetImagesResponse>(await response.Content.ReadAsStringAsync());
                    }
                }
            }
            catch (Exception) { return null; }
        }

        public async Task SaveShowPoster(string tmdb_id, string traktid)
        {
            var imgList = await GetImages(tmdb_id);
            if(imgList.posters != null && imgList.posters.Length > 0)
            {                
                await imgList.posters[0].Save(Path.Combine(ImagePath, traktid), configuration.imageConfiguration.baseUrl, GetBestPosterSize(100));
            }
        }

        public string GetBestPosterSize(int size)
        {
            string bestPosterSize = "original";
            int smallestDifference = int.MaxValue;
            foreach (string posterSize in configuration.imageConfiguration.posterSizes)
            {
                if (posterSize.StartsWith("w"))
                {
                    int width = int.Parse(posterSize.Replace("w", ""));
                    if (width < size) continue;
                    else
                    {
                        int difference = width - size;
                        if (difference < smallestDifference)
                        {
                            smallestDifference = difference;
                            bestPosterSize = posterSize;
                        }
                    }

                }
            }
            return bestPosterSize;
        }

        public void Sync(TraktCache traktCache)
        {
            Directory.CreateDirectory(ImagePath);
            foreach (TraktWatchedShow show in traktCache.watchedList)
            {
                var dirInfo = new DirectoryInfo(ImagePath);
                var fileInfo = dirInfo.GetFiles(show.Show.Ids.Trakt + ".*");
                if(fileInfo.Length == 0)
                {
                    TmdbRateLimiter.CheckLimiter();
                    Task.Run(() => SaveShowPoster(show.Show.Ids.Tmdb.ToString(), show.Show.Ids.Trakt.ToString())).Forget();
                }
            }
        }
    }
}
