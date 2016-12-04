using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using TraktApiSharp.Objects.Get.Shows;
using TraktApiSharp.Objects.Get.Watched;
using TraktTVUpdateClient.Extension;
using TraktTVUpdateClient.Properties;

namespace TraktTVUpdateClient.Cache
{
    public class ImageCache
    {
        internal Uri tmdbBaseAddress = new Uri("https://api.themoviedb.org/3/");
        internal Uri fanartBaseAddress = new Uri("http://webservice.fanart.tv/v3/");
        internal TmdbConfiguration configuration;

        public bool IsReadyForImageCaching = false;
        public string ImagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "images");

        public event EventHandler SyncCompleted;


        public ImageCache()
        {

        }

        public async Task Init()
        {
            TmdbRateLimiter.CheckLimiter();
            configuration = await GetTmdbConfiguration();
            IsReadyForImageCaching = true;
        }

        private async Task<TmdbConfiguration> GetTmdbConfiguration()
        {
            try
            {
                using(var httpClient = new HttpClient() { BaseAddress = tmdbBaseAddress })
                {
                    using(var response = await httpClient.GetAsync("configuration?api_key=" + Resources.TMDbAPIKey))
                    {
                        return JsonConvert.DeserializeObject<TmdbConfiguration>(await response.Content.ReadAsStringAsync());
                    }
                }
            }
            catch (Exception) { return null; }
        }

        private async Task<TmdbGetImagesResponse> GetTmdbImages(string tmdb_id)
        {
            try
            {
                using (var httpClient = new HttpClient() { BaseAddress = tmdbBaseAddress })
                {
                    using (var response = await httpClient.GetAsync("tv/" + tmdb_id + "/images?api_key=" + Resources.TMDbAPIKey))
                    {
                        return JsonConvert.DeserializeObject<TmdbGetImagesResponse>(await response.Content.ReadAsStringAsync());
                    }
                }
            }
            catch (Exception) { return null; }
        }

        private async Task<FanartGetImagesResponse> GetFanartImages(string tvdb_id)
        {
            try
            {
                using (var httpClient = new HttpClient() { BaseAddress = fanartBaseAddress })
                {
                    using (var response = await httpClient.GetAsync("tv/" + tvdb_id + "?api_key=" + Resources.FanartAPIKey))
                    {
                        return JsonConvert.DeserializeObject<FanartGetImagesResponse>(await response.Content.ReadAsStringAsync());
                    }
                }
            }
            catch (Exception) { return null; }
        }

        private async Task SaveShowPoster(TraktShowIds ids)
        {
            if (ids.Tmdb.HasValue)
            {
                TmdbRateLimiter.CheckLimiter();
                var imgList = await GetTmdbImages(ids.Tmdb.ToString());
                if (imgList != null && imgList.posters != null && imgList.posters.Length > 0)
                {
                    await imgList.posters[0].Save(Path.Combine(ImagePath, ids.Trakt.ToString()), configuration.imageConfiguration.baseUrl, GetBestPosterSize(100));
                }
            }
            else if (ids.Tvdb.HasValue)
            {
                var imgList = await GetFanartImages(ids.Tvdb.ToString());
                if(imgList != null && imgList.tvposter != null && imgList.tvposter.Length > 0)
                {
                    await imgList.tvposter[0].Save(Path.Combine(ImagePath, ids.Trakt.ToString()));
                }
            }
        }

        private string GetBestPosterSize(int size)
        {
            string bestPosterSize = "original";
            int smallestDifference = int.MaxValue;
            foreach (string posterSize in configuration.imageConfiguration.posterSizes)
            {
                if (posterSize.StartsWith("w", StringComparison.CurrentCulture))
                {
                    int width = int.Parse(posterSize.Replace("w", ""), CultureInfo.InvariantCulture);
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

        public async void Sync(TraktCache traktCache)
        {
            Directory.CreateDirectory(ImagePath);
            List<Task> taskList = new List<Task>();
            foreach (TraktWatchedShow show in traktCache.watchedList)
            {
                var dirInfo = new DirectoryInfo(ImagePath);
                var fileInfo = dirInfo.GetFiles(show.Show.Ids.Trakt + ".*");
                if(fileInfo.Length == 0)
                {
                    taskList.Add(Task.Run(() => SaveShowPoster(show.Show.Ids)));
                }
                Thread.Sleep(10);
            }
            await Task.WhenAll(taskList);
            OnSyncCompleted();
        }

        protected virtual void OnSyncCompleted()
        {
            SyncCompleted?.Invoke(this, EventArgs.Empty);
        }
    }
}
