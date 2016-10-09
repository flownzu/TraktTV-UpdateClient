using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TraktApiSharp;
using TraktApiSharp.Enums;
using TraktApiSharp.Extensions;
using TraktApiSharp.Objects.Basic;
using TraktApiSharp.Objects.Get.History;
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
        [JsonProperty(PropertyName = "WatchedList")]
        internal IEnumerable<TraktWatchedShow> watchedList { get; private set; }

        [JsonProperty(PropertyName = "RatingList")]
        internal IEnumerable<TraktRatingsItem> ratingList { get; private set; }

        [JsonProperty(PropertyName = "ProgressList")]
        internal Dictionary<string, TraktShowWatchedProgress> progressList { get; private set; }

        [JsonIgnore]
        internal TraktClient TraktClient { get; set; }

        [JsonProperty(PropertyName = "LastRating")]
        internal DateTime lastRating { get; set; }

        [JsonProperty(PropertyName = "LastWatched")]
        internal DateTime lastWatched { get; set; }

        public event EventHandler SyncCompleted;

        public Cache(TraktClient Client)
        {
            TraktClient = Client;
            watchedList = Enumerable.Empty<TraktWatchedShow>();
            ratingList = Enumerable.Empty<TraktRatingsItem>();
            progressList = new Dictionary<string, TraktShowWatchedProgress>();
        }

        public async Task Sync(bool NoCache = false)
        {
            if (!NoCache) { await this.Update(); return; }
            foreach(TraktWatchedShow show in watchedList)
            {
                List<Task> taskList = new List<Task>();
                if(show.Show.Status != TraktShowStatus.Canceled && show.Show.Status != TraktShowStatus.Ended)
                {
                    taskList.Add(Task.Run(() => SyncShow(show.Show)));
                }
                await Task.WhenAll(taskList);
            }
            var lastActivites = await TraktClient.Sync.GetLastActivitiesAsync();
            if(lastActivites.Shows.RatedAt.HasValue && lastActivites.Shows.RatedAt > lastRating)
            {
                await UpdateRatingsList();
                lastRating = lastActivites.Shows.RatedAt.Value;
            }
            if(lastActivites.Episodes.WatchedAt.HasValue && lastActivites.Episodes.WatchedAt > lastWatched)
            {
                var watchedHistory = await getWatchedHistory();
                var distinctShowSlugs = watchedHistory.Select(x => x.Show.Ids.Slug).Distinct();
                List<Task> taskList = new List<Task>();
                foreach (string slug in distinctShowSlugs)
                {
                    if(watchedList.Where(x => x.Show.Ids.Slug.Equals(slug)).FirstOrDefault() != null)
                    {
                        taskList.Add(Task.Run(() => SyncShowProgress(slug)));
                    }
                }
                await Task.WhenAll(taskList);
                lastWatched = lastActivites.Episodes.WatchedAt.Value;
            }
            OnSyncCompleted();
        }

        protected virtual void OnSyncCompleted()
        {
            SyncCompleted?.Invoke(this, EventArgs.Empty);
        }

        public void Save()
        {
            using (StreamWriter sw = File.CreateText("cache.json")) { new JsonSerializer().Serialize(sw, this); }
        }

        public async Task Update()
        {
            await UpdateWatchedShowList();
            await UpdateProgressList();
            await UpdateRatingsList();
            var lastActivites = await TraktClient.Sync.GetLastActivitiesAsync();
            lastRating = lastActivites.Shows.RatedAt.Value;
            lastWatched = lastActivites.Episodes.WatchedAt.Value;
        }

        public async Task<IEnumerable<TraktHistoryItem>> getWatchedHistory()
        {
            TraktPaginationListResult<TraktHistoryItem> episodeHistory = await TraktClient.Sync.GetWatchedHistoryAsync(TraktSyncItemType.Episode, startAt: lastWatched, endAt: DateTime.Now, limitPerPage: 100);
            if(episodeHistory.Page < episodeHistory.PageCount)
            {
                for(int i = episodeHistory.Page.Value + 1; i < episodeHistory.PageCount; i++)
                {
                    var tempList = await TraktClient.Sync.GetWatchedHistoryAsync(TraktSyncItemType.Episode, startAt: lastWatched, endAt: DateTime.Now, limitPerPage: 100, page: i);
                    episodeHistory.Items.Union(tempList.Items);
                    episodeHistory.ItemCount += tempList.ItemCount;
                }
            }
            return episodeHistory.Items;
        }

        public async Task<bool> UpdateWatchedShowList()
        {
            try
            {
                var newWatchedList = await TraktClient.Sync.GetWatchedShowsAsync(new TraktExtendedInfo().SetFull().SetImages());
                List<Task> taskList = new List<Task>();
                foreach (TraktWatchedShow show in newWatchedList.Except(watchedList))
                {
                    taskList.Add(Task.Run(() => SyncSeasonOverview(show.Show)));
                }
                await Task.WhenAll(taskList);
                if(watchedList.Count() == 0) { watchedList = newWatchedList; }
                else
                {
                    watchedList.Union(newWatchedList.Except(watchedList));
                }
                return true;
            }
            catch (Exception) { return false; }
        }

        public async Task<bool> UpdateRatingsList()
        {
            try
            {
                ratingList = await TraktClient.Sync.GetRatingsAsync(TraktRatingsItemType.Show);
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

        private async Task SyncShow(TraktShow show)
        {
            show = await TraktClient.Shows.GetShowAsync(show.Ids.Slug, new TraktExtendedInfo().SetFull());
            await SyncSeasonOverview(show);
        }

        private async Task SyncSeasonOverview(TraktShow show)
        {
            show.Seasons = await TraktClient.Seasons.GetAllSeasonsAsync(show.Ids.Slug, new TraktExtendedInfo().SetFull());
            foreach (TraktSeason s in show.Seasons)
            {
                if (s.Number > 0)
                {
                    s.Episodes = await TraktClient.Seasons.GetSeasonAsync(show.Ids.Slug, s.Number.Value, new TraktExtendedInfo().SetFull());
                }
            }
        }

        private async Task SyncShowProgress(TraktShow show)
        {
            TraktShowWatchedProgress progress = await TraktClient.Shows.GetShowWatchedProgressAsync(show.Ids.Slug, false, false, false);
            if (progressList.ContainsKey(show.Ids.Slug)) { progressList.Remove(show.Ids.Slug); }
            progressList.Add(show.Ids.Slug, progress);
        }

        private async Task SyncShowProgress(string showSlug)
        {
            TraktShowWatchedProgress progress = await TraktClient.Shows.GetShowWatchedProgressAsync(showSlug, false, false, false);
            if (progressList.ContainsKey(showSlug)) { progressList.Remove(showSlug); }
            progressList.Add(showSlug, progress);
        }
    }
}
