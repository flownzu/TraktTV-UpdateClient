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
using TraktTVUpdateClient.Extension;

namespace TraktTVUpdateClient.Cache
{
    public class TraktCache
    {
        [JsonProperty(PropertyName = "WatchedList")]
        internal IEnumerable<TraktWatchedShow> WatchedList { get; private set; }

        [JsonProperty(PropertyName = "RatingList")]
        internal IEnumerable<TraktRatingsItem> RatingList { get; private set; }

        [JsonProperty(PropertyName = "ProgressList")]
        internal Dictionary<string, TraktShowWatchedProgress> ProgressList { get; private set; }

        [JsonProperty(PropertyName = "RequestCache")]
        internal List<TraktRequest> RequestCache { get; private set; }

        [JsonIgnore]
        internal TraktClient TraktClient { get; set; }

        [JsonProperty(PropertyName = "LastRating")]
        internal DateTime LastRating { get; set; }

        [JsonProperty(PropertyName = "LastWatched")]
        internal DateTime LastWatched { get; set; }

        public event EventHandler<SyncStartedEventArgs> SyncStarted;
        public event EventHandler<SyncCompletedEventArgs> SyncCompleted;
        public event EventHandler<RequestCachedEventArgs> RequestCached;
        public event EventHandler<RequestCacheSyncedEventArgs> RequestCacheSynced;

        public TraktCache(TraktClient Client)
        {
            TraktClient = Client;
            RequestCache = new List<TraktRequest>();
            WatchedList = Enumerable.Empty<TraktWatchedShow>();
            RatingList = Enumerable.Empty<TraktRatingsItem>();
            ProgressList = new Dictionary<string, TraktShowWatchedProgress>();
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
                    await UpdateProgressList();
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
                else await UpdateProgressList();
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
            catch (Exception) { return false; }
        }

        public async Task<bool> UpdateRatingsList()
        {
            try
            {
                RatingList = await TraktClient.Sync.GetRatingsAsync(TraktRatingsItemType.Show);
                return true;
            }
            catch (Exception) { return false; }
        }

        public async Task<bool> UpdateProgressList()
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
            catch (Exception) { return false; }
        }

        public async Task SyncShowProgress(string showSlug, bool updateCache = false)
        {
            if (updateCache) OnSyncStarted(SyncStartedEventArgs.PartialSync);
            var progress = await TraktClient.Shows.GetShowWatchedProgressAsync(showSlug, false, false, false);
            if (ProgressList.ContainsKey(showSlug)) { ProgressList.Remove(showSlug); }
            ProgressList.Add(showSlug, progress);
            if (updateCache) OnSyncCompleted(SyncCompletedEventArgs.PartialSync);
        }
    }
}
