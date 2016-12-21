using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
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
        internal IEnumerable<TraktWatchedShow> watchedList { get; private set; }

        [JsonProperty(PropertyName = "RatingList")]
        internal IEnumerable<TraktRatingsItem> ratingList { get; private set; }

        [JsonProperty(PropertyName = "ProgressList")]
        internal Dictionary<string, TraktShowWatchedProgress> progressList { get; private set; }

        [JsonProperty(PropertyName = "RequestCache")]
        internal List<TraktRequest> requestCache { get; private set; }

        [JsonIgnore]
        internal TraktClient TraktClient { get; set; }

        [JsonProperty(PropertyName = "LastRating")]
        internal DateTime lastRating { get; set; }

        [JsonProperty(PropertyName = "LastWatched")]
        internal DateTime lastWatched { get; set; }

        public event EventHandler<SyncStartedEventArgs> SyncStarted;
        public event EventHandler<SyncCompletedEventArgs> SyncCompleted;
        public event EventHandler<RequestCachedEventArgs> RequestCached;

        public TraktCache(TraktClient Client)
        {
            TraktClient = Client;
            requestCache = new List<TraktRequest>();
            watchedList = Enumerable.Empty<TraktWatchedShow>();
            ratingList = Enumerable.Empty<TraktRatingsItem>();
            progressList = new Dictionary<string, TraktShowWatchedProgress>();
        }

        public async Task RequestCacheThread()
        {
            await Task.Delay(TimeSpan.FromSeconds(10));
            while(true)
            {
                if (requestCache.Count > 0)
                {
                    OnSyncStarted(SyncStartedEventArgs.PartialSync);
                    List<TraktRequest> removeRequestList = new List<TraktRequest>();
                    foreach (TraktRequest request in requestCache)
                    {
                        if (await request.Send(TraktClient))
                        {
                            removeRequestList.Add(request);
                        }
                    }
                    OnSyncCompleted(SyncCompletedEventArgs.PartialSync);
                    foreach (TraktRequest request in removeRequestList)
                        requestCache.Remove(request);
                }
                await Task.Delay(TimeSpan.FromMinutes(5));
            }
        }

        public async Task Sync()
        {
            try
            {
                OnSyncStarted(SyncStartedEventArgs.CompleteSync);
                await UpdateProgressList();
                var lastActivites = await TraktClient.Sync.GetLastActivitiesAsync();
                if (lastActivites.Shows.RatedAt.HasValue && lastActivites.Shows.RatedAt != lastRating)
                {
                    await UpdateRatingsList();
                    lastRating = lastActivites.Shows.RatedAt.Value;
                }
                if (lastActivites.Episodes.WatchedAt.HasValue && lastActivites.Episodes.WatchedAt != lastWatched)
                {
                    var oldWatchedList = watchedList;
                    await UpdateWatchedShowList();
                    await UpdateProgressList();
                    lastWatched = lastActivites.Episodes.WatchedAt.Value;
                }
                OnSyncCompleted(SyncCompletedEventArgs.CompleteSync);
            }
            catch (Exception) { OnSyncCompleted(SyncCompletedEventArgs.CompleteSync); }
        }

        public void AddRequestToCache(TraktRequest rq)
        {
            if (rq.action == TraktRequestAction.RateShow)
            {
                var previousRequest = requestCache.Where(x => x.action.Equals(TraktRequestAction.RateShow) && x.RequestShow.Equals(rq.RequestShow)).FirstOrDefault();
                if (previousRequest != null)
                {
                    previousRequest.RequestValue = rq.RequestValue;
                }
                else
                {
                    requestCache.Add(rq);
                    OnRequestCached(new RequestCachedEventArgs(rq));
                }
            }
            else if (rq.action == TraktRequestAction.RemoveEpisode || rq.action == TraktRequestAction.AddEpisode)
            {
                if (rq.RequestEpisode != null)
                {
                    var previousRequest = requestCache.Where(x => x.action.Equals(rq.action) && 
                                          ((x.RequestEpisode != null && x.RequestEpisode.Equals(rq.RequestEpisode))) ||
                                          ((x.RequestShow != null && x.RequestShow.Equals(rq.RequestShow)) &&
                                          x.RequestValue.Equals("S" + rq.RequestEpisode.SeasonNumber.ToString().PadLeft(2, '0') + "E" + rq.RequestEpisode.Number.ToString().PadLeft(2, '0'))))
                                          .FirstOrDefault();
                    if (previousRequest != null) return;
                    previousRequest = requestCache.Where(x => x.action.Equals(rq.action.Invert()) &&
                                      ((x.RequestEpisode != null && x.RequestEpisode.Equals(rq.RequestEpisode))) ||
                                      ((x.RequestShow != null && x.RequestShow.Equals(rq.RequestShow)) &&
                                      x.RequestValue.Equals("S" + rq.RequestEpisode.SeasonNumber.ToString().PadLeft(2, '0') + "E" + rq.RequestEpisode.Number.ToString().PadLeft(2, '0'))))
                                      .FirstOrDefault();
                    if (previousRequest != null)
                    {
                        requestCache.Remove(previousRequest);
                        OnRequestCached(new RequestCachedEventArgs(rq));
                        return;
                    }
                }
                else
                {
                    Match m = Regex.Match(rq.RequestValue, @"S(\d+)E(\d+)");
                    int seasonNumber = int.Parse(m.Groups[1].Value);
                    int episodeNumber = int.Parse(m.Groups[2].Value);
                    var previousRequest = requestCache.Where(x => x.action.Equals(rq.action) && 
                                          ((x.RequestShow != null && x.RequestShow.Ids.Slug.Equals(rq.RequestShow.Ids.Slug) && x.RequestValue != null && x.RequestValue.Equals(rq.RequestValue)) || 
                                          (x.RequestEpisode != null && (x.RequestEpisode.Number.Equals(episodeNumber) && x.RequestEpisode.SeasonNumber.Equals(seasonNumber)))))
                                          .FirstOrDefault();
                    if (previousRequest != null) return;
                    previousRequest = requestCache.Where(x => x.action.Equals(rq.action.Invert()) && 
                                      ((x.RequestShow != null && x.RequestShow.Ids.Slug.Equals(rq.RequestShow.Ids.Slug) && x.RequestValue != null && x.RequestValue.Equals(rq.RequestValue)) || 
                                      (x.RequestEpisode != null && (x.RequestEpisode.Number.Equals(episodeNumber) && x.RequestEpisode.SeasonNumber.Equals(seasonNumber)))))
                                      .FirstOrDefault();
                    if (previousRequest != null)
                    {
                        requestCache.Remove(previousRequest);
                        OnRequestCached(new RequestCachedEventArgs(rq));
                        return;
                    }
                }
                requestCache.Add(rq);
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

        public void Save()
        {
            using (StreamWriter sw = File.CreateText("cache.json")) { new JsonSerializer().Serialize(sw, this); }
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
                        taskList.Add(Task.Run(() => SyncShowProgress(show.Show.Ids.Slug)));
                    }
                    await Task.WhenAll(taskList);
                    return true;
                }
                return false;
            }
            catch (Exception) { return false; }
        }

        public async Task SyncShowProgress(string showSlug)
        {
            OnSyncStarted(SyncStartedEventArgs.PartialSync);
            var progress = await TraktClient.Shows.GetShowWatchedProgressAsync(showSlug, false, false, false);
            if (progressList.ContainsKey(showSlug)) { progressList.Remove(showSlug); }
            progressList.Add(showSlug, progress);
            OnSyncCompleted(SyncCompletedEventArgs.PartialSync);
        }
    }
}
