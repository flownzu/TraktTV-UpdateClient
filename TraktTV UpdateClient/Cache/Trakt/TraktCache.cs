using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TraktApiSharp;
using TraktApiSharp.Enums;
using TraktApiSharp.Objects.Basic;
using TraktApiSharp.Objects.Get.History;
using TraktApiSharp.Objects.Get.Ratings;
using TraktApiSharp.Objects.Get.Shows;
using TraktApiSharp.Objects.Get.Watched;
using TraktApiSharp.Requests.Params;

namespace TraktTVUpdateClient.Cache
{
    public class TraktCache
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

        public event EventHandler<SyncStartedEventArgs> SyncStarted;
        public event EventHandler<SyncCompletedEventArgs> SyncCompleted;

        public TraktCache(TraktClient Client)
        {
            TraktClient = Client;
            watchedList = Enumerable.Empty<TraktWatchedShow>();
            ratingList = Enumerable.Empty<TraktRatingsItem>();
            progressList = new Dictionary<string, TraktShowWatchedProgress>();
        }

        public async Task Sync(bool NoCache = false)
        {
            OnSyncStarted(SyncStartedEventArgs.CompleteSync);
            if (!NoCache) { await this.Update(); return; }
            await UpdateProgressList();
            var lastActivites = await TraktClient.Sync.GetLastActivitiesAsync();
            if(lastActivites.Shows.RatedAt.HasValue && lastActivites.Shows.RatedAt != lastRating)
            {
                await UpdateRatingsList();
                lastRating = lastActivites.Shows.RatedAt.Value;
            }
            if(lastActivites.Episodes.WatchedAt.HasValue && lastActivites.Episodes.WatchedAt != lastWatched)
            {
                var oldWatchedList = watchedList;
                await UpdateWatchedShowList();
                await UpdateProgressList();
                lastWatched = lastActivites.Episodes.WatchedAt.Value;
            }
            OnSyncCompleted(SyncCompletedEventArgs.CompleteSync);
        }

        protected virtual void OnSyncStarted(SyncStartedEventArgs e)
        {
            SyncStarted?.Invoke(this, e);
        }

        protected virtual void OnSyncCompleted(SyncCompletedEventArgs e)
        {
            Save();
            SyncCompleted?.Invoke(this, e);
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
            OnSyncCompleted(SyncCompletedEventArgs.CompleteSync);
        }

        public async Task<IEnumerable<TraktHistoryItem>> GetWatchedHistory()
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
                watchedList = await TraktClient.Sync.GetWatchedShowsAsync(new TraktExtendedInfo().SetFull().SetImages());
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

        public async Task SyncShowProgress(TraktShow show)
        {
            OnSyncStarted(SyncStartedEventArgs.PartialSync);
            TraktShowWatchedProgress progress = await TraktClient.Shows.GetShowWatchedProgressAsync(show.Ids.Slug, false, false, false);
            if (progressList.ContainsKey(show.Ids.Slug)) { progressList.Remove(show.Ids.Slug); }
            progressList.Add(show.Ids.Slug, progress);
            OnSyncCompleted(SyncCompletedEventArgs.PartialSync);
        }

        public async Task SyncShowProgress(string showSlug)
        {
            OnSyncStarted(SyncStartedEventArgs.PartialSync);
            TraktShowWatchedProgress progress = await TraktClient.Shows.GetShowWatchedProgressAsync(showSlug, false, false, false);
            if (progressList.ContainsKey(showSlug)) { progressList.Remove(showSlug); }
            progressList.Add(showSlug, progress);
            OnSyncCompleted(SyncCompletedEventArgs.PartialSync);
        }
    }
}
