using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TraktApiSharp;
using TraktApiSharp.Enums;
using TraktApiSharp.Objects.Get.Ratings;
using TraktApiSharp.Objects.Get.Shows;
using TraktApiSharp.Objects.Get.Shows.Seasons;
using TraktApiSharp.Objects.Get.Watched;
using TraktApiSharp.Requests.Params;
using TraktTVUpdateClient.Extension;

namespace TraktTVUpdateClient
{
    public class Cache
    {
        internal IEnumerable<TraktWatchedShow> watchedList { get; private set; }
        internal IEnumerable<TraktRatingsItem> ratingList { get; private set; }
        internal Dictionary<string, TraktShowWatchedProgress> progressList { get; private set; }
        internal TraktClient Client;
        //internal DateTime lastSync;

        public Cache(TraktClient Client)
        {
            this.Client = Client;
        }

        public void Load()
        {
            try
            {
                if (File.Exists("watchedList.json"))
                {
                    using (StreamReader sr = File.OpenText("watchedList.json")) { watchedList = JsonConvert.DeserializeObject<IEnumerable<TraktWatchedShow>>(sr.ReadToEnd()); }
                }
                else { watchedList = Enumerable.Empty<TraktWatchedShow>(); }
                if (File.Exists("progressList.json"))
                {
                    using(StreamReader sr = File.OpenText("progressList.json")) { progressList = JsonConvert.DeserializeObject<Dictionary<string, TraktShowWatchedProgress>>(sr.ReadToEnd()); }
                }
                else { progressList = new Dictionary<string, TraktShowWatchedProgress>(); }
                if (File.Exists("ratingList.json"))
                {
                    using(StreamReader sr = File.OpenText("ratingList.json")) { ratingList = JsonConvert.DeserializeObject<List<TraktRatingsItem>>(sr.ReadToEnd()); }
                }
                else { ratingList = Enumerable.Empty<TraktRatingsItem>(); }
            }
            catch (Exception) { }
        }

        public async Task Sync()
        {
            if(watchedList.Count() == 0)
            {
                await UpdateWatchedShowList();
                foreach(TraktWatchedShow watchedShow in watchedList)
                {
                    Task.Run(() => SyncSeasonOverview(watchedShow.Show)).Forget();
                }
            }
            if(progressList.Count == 0 || progressList.Count != watchedList.Count())
            {
                foreach(TraktWatchedShow watchedShow in watchedList)
                {
                    Task.Run(() => SyncShowProgress(watchedShow.Show)).Forget();
                }
            }
            await UpdateRatingsList();
        }

        public void Save()
        {
            using (StreamWriter sw = File.CreateText("watchedList.json")) { new JsonSerializer().Serialize(sw, watchedList); }
            using (StreamWriter sw = File.CreateText("progressList.json")) { new JsonSerializer().Serialize(sw, progressList); }
            using (StreamWriter sw = File.CreateText("ratingList.json")) { new JsonSerializer().Serialize(sw, ratingList); }
        }

        public async Task Update()
        {
            await UpdateWatchedShowList();
            await UpdateProgressList();
            await UpdateRatingsList();
        }

        public async Task<bool> UpdateWatchedShowList()
        {
            try
            {
                watchedList = await Client.Users.GetWatchedShowsAsync("me", new TraktExtendedInfo().SetFull().SetImages());
                return true;
            }
            catch (Exception) { return false; }
        }

        public async Task<bool> UpdateRatingsList()
        {
            try
            {
                ratingList = await Client.Users.GetRatingsAsync("me", TraktRatingsItemType.Show);
                return true;
            }
            catch (Exception) { return false; }
        }

        public async Task<bool> UpdateProgressList()
        {
            try
            {
                if (watchedList != null)
                {
                    progressList = new Dictionary<string, TraktShowWatchedProgress>();
                    List<Task> taskList = new List<Task>();
                    foreach (TraktWatchedShow show in watchedList)
                    {
                        taskList.Add(Task.Run(() => SyncShowProgress(show.Show)));
                    }
                    await Task.WhenAll(taskList);
                    return true;
                }
                return false;
            }
            catch (Exception) { return false; }
        }

        private async Task SyncSeasonOverview(TraktShow show)
        {
            show.Seasons = await Client.Seasons.GetAllSeasonsAsync(show.Ids.Slug, new TraktExtendedInfo().SetFull());
            foreach (TraktSeason s in show.Seasons)
            {
                if (s.Number > 0)
                {
                    s.Episodes = await Client.Seasons.GetSeasonAsync(show.Ids.Slug, s.Number.Value, new TraktExtendedInfo().SetFull());
                }
            }
        }

        private async Task SyncShowProgress(TraktShow show)
        {
            TraktShowWatchedProgress progress = await Client.Shows.GetShowWatchedProgressAsync(show.Ids.Slug, false, false, false);
            if (progressList.ContainsKey(show.Title)) { progressList.Remove(show.Title); }
            progressList.Add(show.Title, progress);
        }
    }
}
