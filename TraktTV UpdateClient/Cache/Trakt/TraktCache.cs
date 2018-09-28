using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TraktApiSharp;
using TraktApiSharp.Enums;
using TraktApiSharp.Objects.Basic;
using TraktApiSharp.Objects.Get.Collection;
using TraktApiSharp.Objects.Get.History;
using TraktApiSharp.Objects.Get.Ratings;
using TraktApiSharp.Objects.Get.Shows;
using TraktApiSharp.Objects.Get.Watched;
using TraktApiSharp.Requests.Params;
using TraktTVUpdateClient.Extension;

namespace TraktTVUpdateClient.Cache
{
    public class TraktCache
    {
        [JsonProperty(PropertyName = "WatchedList")]
        internal IEnumerable<TraktWatchedShow> WatchedList { get; private set; }

        [JsonProperty(PropertyName = "CollectionList")]
        internal IEnumerable<TraktCollectionShow> CollectionList { get; private set; }

        [JsonProperty(PropertyName = "RatingList")]
        internal IEnumerable<TraktRatingsItem> RatingList { get; private set; }

        [JsonProperty(PropertyName = "ShowWatchedProgress")]
        internal Dictionary<string, TraktShowWatchedProgress> ShowWatchedProgress { get; private set; }

        [JsonProperty(PropertyName = "ShowCollectionProgress")]
        internal Dictionary<string, TraktShowCollectionProgress> ShowCollectionProgress { get; private set; }

        [JsonProperty(PropertyName = "RequestCache")]
        internal List<TraktRequest> RequestCache { get; private set; }

        [JsonIgnore]
        internal TraktClient TraktClient { get; set; }

        [JsonProperty(PropertyName = "LastRating")]
        internal DateTime LastRating { get; set; }

        [JsonProperty(PropertyName = "LastWatched")]
        internal DateTime LastWatched { get; set; }

        [JsonProperty(PropertyName = "LastCollected")]
        internal DateTime LastCollected { get; set; }

        public event EventHandler<SyncStartedEventArgs> SyncStarted;
        public event EventHandler<SyncCompletedEventArgs> SyncCompleted;
        public event EventHandler<RequestCachedEventArgs> RequestCached;
        public event EventHandler<RequestCacheSyncedEventArgs> RequestCacheSynced;

        public TraktCache(TraktClient Client)
        {
            TraktClient = Client;
            RequestCache = new List<TraktRequest>();
            WatchedList = Enumerable.Empty<TraktWatchedShow>();
            CollectionList = Enumerable.Empty<TraktCollectionShow>();
            RatingList = Enumerable.Empty<TraktRatingsItem>();
            ShowWatchedProgress = new Dictionary<string, TraktShowWatchedProgress>();
            ShowCollectionProgress = new Dictionary<string, TraktShowCollectionProgress>();
        }

        public async Task RequestCacheThread()
        {
            while(true)
            {
                await SyncRequestCache();
                await Task.Delay(TimeSpan.FromMinutes(5));
            }
        }

        public async Task SyncRequestCache()
        {
            if(RequestCache.Count > 0)
            {
                List<TraktRequest> removeRequestList = new List<TraktRequest>();
                foreach (TraktRequest request in RequestCache)
                {
                    if (await request.Send(TraktClient))
                    {
                        removeRequestList.Add(request);
                    }
                }
                if (removeRequestList.Count() > 0)
                {
                    OnRequestCacheSynced(new RequestCacheSyncedEventArgs(removeRequestList));
                }
            }
        }

        public async Task Sync()
        {
            try
            {
                OnSyncStarted(SyncStartedEventArgs.CompleteSync);
                var lastActivites = await TraktClient.Sync.GetLastActivitiesAsync();

                if (lastActivites.Episodes.WatchedAt.HasValue && lastActivites.Episodes.WatchedAt != LastWatched)
                {
                    var oldWatchedList = WatchedList;
                    await UpdateWatchedShowList();
                    await UpdateWatchedProgress();
                    List<Task> taskList = new List<Task>();
                    foreach (TraktWatchedShow show in WatchedList)
                    {
                        if (show.Show.Seasons == null || show.Show.Seasons.Any(x => x.Episodes == null))
                        {
                            var oldShow = oldWatchedList.Where(x => x.Show.Title.Equals(show.Show.Title)).FirstOrDefault();
                            if (oldShow != null && oldShow.Show.Seasons != null)
                            {
                                show.Show.Seasons = oldShow.Show.Seasons;
                            }
                            else taskList.Add(Task.Run(() => show.Show.SyncShowOverview(TraktClient)));
                        }
                    }
                    await Task.WhenAll(taskList);
                    LastWatched = lastActivites.Episodes.WatchedAt.Value;
                }
                else await UpdateWatchedProgress();

                if (lastActivites.Episodes.CollectedAt.HasValue && lastActivites.Episodes.CollectedAt != LastCollected)
                {
                    await UpdateCollectedShowList();
                    await UpdateCollectionProgress();
                    LastCollected = lastActivites.Episodes.CollectedAt.Value;
                }
                else await UpdateCollectionProgress();

                if (lastActivites.Shows.RatedAt.HasValue && lastActivites.Shows.RatedAt != LastRating)
                {
                    await UpdateRatingsList();
                    LastRating = lastActivites.Shows.RatedAt.Value;
                }

                OnSyncCompleted(SyncCompletedEventArgs.CompleteSync);
            }
            catch (Exception) { OnSyncCompleted(SyncCompletedEventArgs.CompleteSync); }
        }

        public void AddRequestToCache(TraktRequest rq)
        {
            if (rq.Action == TraktRequestAction.RateShow)
            {
                var previousRequest = RequestCache.Where(x => x.Action.Equals(TraktRequestAction.RateShow) && x.RequestShow.Equals(rq.RequestShow)).FirstOrDefault();
                if (previousRequest != null)
                {
                    previousRequest.RequestValue = rq.RequestValue;
                }
                else
                {
                    RequestCache.Add(rq);
                    OnRequestCached(new RequestCachedEventArgs(rq));
                }
            }
            else if (rq.Action == TraktRequestAction.RemoveEpisode || rq.Action == TraktRequestAction.AddEpisode)
            {
                if (rq.RequestEpisode != null)
                {
                    var previousRequest = RequestCache.Where(x => x.Action.Equals(rq.Action) && x.RequestEpisode.Equals(rq.RequestEpisode)).FirstOrDefault();
                    if (previousRequest != null) return;
                    previousRequest = RequestCache.Where(x => x.Action.Equals(rq.Action.Invert()) && x.RequestEpisode.Equals(rq.RequestEpisode)).FirstOrDefault();
                    if (previousRequest != null)
                    {
                        RequestCache.Remove(previousRequest);
                        OnRequestCached(new RequestCachedEventArgs(rq));
                        return;
                    }
                }
                RequestCache.Add(rq);
                OnRequestCached(new RequestCachedEventArgs(rq));
            }
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

        protected virtual void OnRequestCached(RequestCachedEventArgs e)
        {
            Save();
            RequestCached?.Invoke(this, e);
        }

        protected virtual void OnRequestCacheSynced(RequestCacheSyncedEventArgs e)
        {
            foreach (TraktRequest request in e.RequestList) RequestCache.Remove(request);
            Save();
            RequestCacheSynced?.Invoke(this, e);
        }

        public void Save()
        {
            using (FileStream fs = File.Open("cache.json", FileMode.Create, FileAccess.ReadWrite))
            {
                var cache = System.Text.Encoding.Default.GetBytes(JsonConvert.SerializeObject(this));
                fs.Write(cache, 0, cache.Length);
            }
        }

        public async Task<IEnumerable<TraktHistoryItem>> GetWatchedHistory()
        {
            TraktPaginationListResult<TraktHistoryItem> episodeHistory = await TraktClient.Sync.GetWatchedHistoryAsync(TraktSyncItemType.Episode, startAt: LastWatched, endAt: DateTime.Now, limitPerPage: 100);
            if(episodeHistory.Page < episodeHistory.PageCount)
            {
                for(int i = episodeHistory.Page.Value + 1; i < episodeHistory.PageCount; i++)
                {
                    var tempList = await TraktClient.Sync.GetWatchedHistoryAsync(TraktSyncItemType.Episode, startAt: LastWatched, endAt: DateTime.Now, limitPerPage: 100, page: i);
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
                WatchedList = await TraktClient.Sync.GetWatchedShowsAsync(new TraktExtendedInfo().SetFull().SetImages());
                return true;
            }
            catch { return false; }
        }

        public async Task<bool> UpdateCollectedShowList()
        {
            try
            {
                CollectionList = await TraktClient.Sync.GetCollectionShowsAsync(new TraktExtendedInfo().SetFull().SetImages());
                return true;
            }
            catch { return false; }
        }

        public async Task<bool> UpdateRatingsList()
        {
            try
            {
                RatingList = await TraktClient.Sync.GetRatingsAsync(TraktRatingsItemType.Show);
                return true;
            }
            catch { return false; }
        }

        public async Task<bool> UpdateWatchedProgress()
        {
            try
            {
                if (WatchedList != null)
                {
                    List<Task> taskList = new List<Task>();
                    foreach (TraktWatchedShow show in WatchedList)
                    {
                        taskList.Add(Task.Run(() => SyncShowProgress(show.Show.Ids.Slug)));
                    }
                    await Task.WhenAll(taskList);
                    return true;
                }
                return false;
            }
            catch { return false; }
        }

        public async Task<bool> UpdateCollectionProgress()
        {
            try
            {
                if (ShowCollectionProgress != null)
                {
                    List<Task> taskList = new List<Task>();
                    foreach (TraktCollectionShow show in CollectionList)
                    {
                        taskList.Add(Task.Run(() => SyncShowCollection(show.Show.Ids.Slug)));
                    }
                    await Task.WhenAll(taskList);
                    return true;
                }
                return false;
            }
            catch { return false; }
        }

        public async Task SyncShowProgress(string showSlug, bool updateCache = false)
        {
            if (updateCache) OnSyncStarted(SyncStartedEventArgs.PartialSync);
            var progress = await TraktClient.Shows.GetShowWatchedProgressAsync(showSlug, false, false, false);
            if (ShowWatchedProgress.ContainsKey(showSlug)) { ShowWatchedProgress.Remove(showSlug); }
            ShowWatchedProgress.Add(showSlug, progress);
            if (updateCache) OnSyncCompleted(SyncCompletedEventArgs.PartialSync);
        }

        public async Task SyncShowCollection(string showSlug, bool updateCache = false)
        {
            if (updateCache) OnSyncStarted(SyncStartedEventArgs.PartialSync);
            var collection = await TraktClient.Shows.GetShowCollectionProgressAsync(showSlug, false, false, false);
            if (ShowCollectionProgress.ContainsKey(showSlug)) { ShowCollectionProgress.Remove(showSlug); }
            ShowCollectionProgress.Add(showSlug, collection);
            if (updateCache) OnSyncCompleted(SyncCompletedEventArgs.PartialSync);
        }
    }
}
