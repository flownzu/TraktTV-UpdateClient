using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
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
            await Task.Delay(TimeSpan.FromSeconds(10));
            while(true)
            {
                if (RequestCache.Count > 0)
                {
                    OnSyncStarted(SyncStartedEventArgs.PartialSync);
                    List<TraktRequest> removeRequestList = new List<TraktRequest>();
                    foreach (TraktRequest request in RequestCache)
                    {
                        if (await request.Send(TraktClient))
                        {
                            removeRequestList.Add(request);
                        }
                    }
                    OnSyncCompleted(SyncCompletedEventArgs.PartialSync);
                    foreach (TraktRequest request in removeRequestList)
                        RequestCache.Remove(request);
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
                if (lastActivites.Shows.RatedAt.HasValue && lastActivites.Shows.RatedAt != LastRating)
                {
                    await UpdateRatingsList();
                    LastRating = lastActivites.Shows.RatedAt.Value;
                }
                if (lastActivites.Episodes.WatchedAt.HasValue && lastActivites.Episodes.WatchedAt != LastWatched)
                {
                    var oldWatchedList = WatchedList;
                    await UpdateWatchedShowList();
                    await UpdateProgressList();
                    List<Task> taskList = new List<Task>();
                    foreach(TraktWatchedShow show in WatchedList)
                    {
                        if(show.Show.Seasons == null)
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
                    var previousRequest = RequestCache.Where(x => x.Action.Equals(rq.Action) && 
                                          ((x.RequestEpisode != null && x.RequestEpisode.Equals(rq.RequestEpisode))) ||
                                          ((x.RequestShow != null && x.RequestShow.Equals(rq.RequestShow)) &&
                                          x.RequestValue.Equals("S" + rq.RequestEpisode.SeasonNumber.ToString().PadLeft(2, '0') + "E" + rq.RequestEpisode.Number.ToString().PadLeft(2, '0'))))
                                          .FirstOrDefault();
                    if (previousRequest != null) return;
                    previousRequest = RequestCache.Where(x => x.Action.Equals(rq.Action.Invert()) &&
                                      ((x.RequestEpisode != null && x.RequestEpisode.Equals(rq.RequestEpisode))) ||
                                      ((x.RequestShow != null && x.RequestShow.Equals(rq.RequestShow)) &&
                                      x.RequestValue.Equals("S" + rq.RequestEpisode.SeasonNumber.ToString().PadLeft(2, '0') + "E" + rq.RequestEpisode.Number.ToString().PadLeft(2, '0'))))
                                      .FirstOrDefault();
                    if (previousRequest != null)
                    {
                        RequestCache.Remove(previousRequest);
                        OnRequestCached(new RequestCachedEventArgs(rq));
                        return;
                    }
                }
                else
                {
                    Match m = Regex.Match(rq.RequestValue, @"S(\d+)E(\d+)");
                    int seasonNumber = int.Parse(m.Groups[1].Value);
                    int episodeNumber = int.Parse(m.Groups[2].Value);
                    var previousRequest = RequestCache.Where(x => x.Action.Equals(rq.Action) && 
                                          ((x.RequestShow != null && x.RequestShow.Ids.Slug.Equals(rq.RequestShow.Ids.Slug) && x.RequestValue != null && x.RequestValue.Equals(rq.RequestValue)) || 
                                          (x.RequestEpisode != null && (x.RequestEpisode.Number.Equals(episodeNumber) && x.RequestEpisode.SeasonNumber.Equals(seasonNumber)))))
                                          .FirstOrDefault();
                    if (previousRequest != null) return;
                    previousRequest = RequestCache.Where(x => x.Action.Equals(rq.Action.Invert()) && 
                                      ((x.RequestShow != null && x.RequestShow.Ids.Slug.Equals(rq.RequestShow.Ids.Slug) && x.RequestValue != null && x.RequestValue.Equals(rq.RequestValue)) || 
                                      (x.RequestEpisode != null && (x.RequestEpisode.Number.Equals(episodeNumber) && x.RequestEpisode.SeasonNumber.Equals(seasonNumber)))))
                                      .FirstOrDefault();
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

        public void Save()
        {
            using (StreamWriter sw = File.CreateText("cache.json")) { new JsonSerializer().Serialize(sw, this); }
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

        public async Task SyncShowProgress(string showSlug)
        {
            OnSyncStarted(SyncStartedEventArgs.PartialSync);
            var progress = await TraktClient.Shows.GetShowWatchedProgressAsync(showSlug, false, false, false);
            if (ProgressList.ContainsKey(showSlug)) { ProgressList.Remove(showSlug); }
            ProgressList.Add(showSlug, progress);
            OnSyncCompleted(SyncCompletedEventArgs.PartialSync);
        }
    }
}
