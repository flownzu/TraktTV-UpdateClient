using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TraktApiSharp.Objects.Get.Shows;
using TraktApiSharp.Objects.Get.Watched;
using TraktTVUpdateClient.Cache.Images.TheTVDB;
using TraktTVUpdateClient.Properties;

namespace TraktTVUpdateClient.Cache
{
    public class ImageCache
    {
        public static readonly Uri tmdbBaseAddress = new Uri("https://api.themoviedb.org/3/");
        public static readonly Uri fanartBaseAddress = new Uri("http://webservice.fanart.tv/v3/");
        public static readonly Uri tvdbBaseAddress = new Uri("https://api.thetvdb.com/");

        internal static JWTToken tvdbAuthToken;
        internal static TmdbConfiguration configuration;

        private bool IsReadyForImageCaching = false;

        public readonly string ImagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "images");
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

        private async Task<JWTToken> GetTvdbToken()
        {
            try
            {
                using (var httpClient = new HttpClient() { BaseAddress = tvdbBaseAddress })
                {
                    using (var response = await httpClient.PostAsync("login", new StringContent("{\"apikey\":\"" + Resources.TVDBAPIKey + "\"}", Encoding.UTF8, "application/json")))
                    {
                        return JsonConvert.DeserializeObject<JWTToken>(await response.Content.ReadAsStringAsync());
                    }
                }
            }
            catch { return null; }
        }

        private async Task<TvdbImageResponse> GetTvdbImages(string tvdb_id)
        {
            try
            {
                using (var httpClient = new HttpClient { BaseAddress = tvdbBaseAddress })
                {
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + tvdbAuthToken.Token);
                    httpClient.DefaultRequestHeaders.Add("Accept-Language", "en");
                    httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
                    using (var response = await httpClient.GetAsync("series/" + tvdb_id + "/images/query?keyType=poster"))
                    {
                        return JsonConvert.DeserializeObject<TvdbImageResponse>(await response.Content.ReadAsStringAsync());
                    }
                }
            }
            catch { return null; }
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
            if (ids.Tvdb.HasValue)
            {
                if (!string.IsNullOrEmpty(tvdbAuthToken?.Token))
                {
                    var imgList = await GetTvdbImages(ids.Tvdb.ToString());
                    if (imgList != null && imgList.Images != null && imgList.Images.Length > 0)
                    {
                        await imgList.Images[0].Save(Path.Combine(ImagePath, ids.Trakt.ToString()));
                    }
                }
            }
            else if (ids.Tmdb.HasValue)
            {
                TmdbRateLimiter.CheckLimiter();
                var imgList = await GetTmdbImages(ids.Tmdb.ToString());
                if (imgList != null && imgList.Posters != null && imgList.Posters.Length > 0 && IsReadyForImageCaching)
                {
                    await imgList.Posters[0].Save(Path.Combine(ImagePath, ids.Trakt.ToString()), configuration.ImageConfiguration.BaseUrl, GetBestPosterSize(100));
                }
            }
            else if (ids.Tvdb.HasValue)
            {
                var imgList = await GetFanartImages(ids.Tvdb.ToString());
                if(imgList != null && imgList.TvPoster != null && imgList.TvPoster.Length > 0)
                {
                    await imgList.TvPoster[0].Save(Path.Combine(ImagePath, ids.Trakt.ToString()));
                }
            }
        }

        private string GetBestPosterSize(int size)
        {
            string bestPosterSize = "original";
            int smallestDifference = int.MaxValue;
            foreach (string posterSize in configuration.ImageConfiguration.PosterSizes)
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
            if (tvdbAuthToken == null) tvdbAuthToken = await GetTvdbToken();
            Directory.CreateDirectory(ImagePath);
            List<Task> taskList = new List<Task>();
            foreach (TraktWatchedShow show in traktCache.WatchedList)
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
