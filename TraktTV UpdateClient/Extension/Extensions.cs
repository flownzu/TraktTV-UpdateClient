using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using TraktApiSharp;
using TraktApiSharp.Authentication;
using TraktApiSharp.Exceptions;
using TraktApiSharp.Objects.Get.Shows;
using TraktApiSharp.Objects.Get.Shows.Episodes;
using TraktApiSharp.Objects.Get.Shows.Seasons;
using TraktApiSharp.Objects.Post.Syncs.History;
using TraktApiSharp.Objects.Post.Syncs.Ratings;
using TraktApiSharp.Services;
using TraktTVUpdateClient.Cache;
using static System.Windows.Forms.ListView;

namespace TraktTVUpdateClient.Extension
{
    static class Extensions
    {
        public static string CreateAuthorizationUrlNoPin(this TraktOAuth OAuth)
        {
            return OAuth.CreateAuthorizationUrl().Replace("redirect_uri=urn%3Aietf%3Awg%3Aoauth%3A2.0%3Aoob", "redirect_uri=app:%2f%2fauthorized");
        }

        public static void Serialize(this TraktAuthorization auth)
        {
            using (StreamWriter sw = File.CreateText("auth.json")) { sw.Write(TraktSerializationService.Serialize(auth)); }
        }

        public static TraktAuthorization LoadAuthorization(string file = "auth.json")
        {
            try
            {
                using (StreamReader sr = File.OpenText(file)) { return TraktSerializationService.DeserializeAuthorization(sr.ReadToEnd()); }
            }
            catch (Exception) { return default; }
        }

        public static string ToGenreString(this IEnumerable<string> genres)
        {
            string returnString = String.Empty;
            foreach(String genre in genres)
            {
                returnString += genre.UpperCase() + ", ";
            }
            return !String.IsNullOrEmpty(returnString) ? returnString.Substring(0, returnString.Length - 2) : "unspecified";
        }

        public static (int season, int episode) GetEpisodeAndSeasonNumberFromAbsoluteNumber(this IEnumerable<TraktSeason> seasons, int absoluteEpisodeNumber)
        {
            int episodeNumber = 0;
            int seasonNumber = 0;
            foreach(TraktSeason season in seasons.Where(x => x.Number > 0))
            {
                if (absoluteEpisodeNumber - season.Episodes.Count() > 0)
                {
                    absoluteEpisodeNumber -= season.Episodes.Count();
                    continue;
                }
                seasonNumber = season.Number.Value;
                episodeNumber = absoluteEpisodeNumber;
                break;
            }
            return (seasonNumber, episodeNumber);
        }

        public static (int season, int episode) GetEpisodeAndSeasonNumberFromAbsoluteNumber(this IEnumerable<TraktSeasonWatchedProgress> seasons, int absoluteEpisodeNumber)
        {
            int episodeNumber = 0;
            int seasonNumber = 0;
            foreach(TraktSeasonWatchedProgress season in seasons.Where(x => x.Number > 0))
            {
                if(absoluteEpisodeNumber - season.Episodes.Count() > 0)
                {
                    absoluteEpisodeNumber -= season.Episodes.Count();
                    continue;
                }
                seasonNumber = season.Number.Value;
                episodeNumber = absoluteEpisodeNumber;
                break;
            }
            return (seasonNumber, episodeNumber);
        }

        public static string UpperCase(this string s)
        {
            char[] array = s.ToCharArray();
            if (array.Length >= 1)
            {
                if (char.IsLower(array[0]))
                {
                    array[0] = char.ToUpper(array[0], CultureInfo.CurrentCulture);
                }
            }
            for (int i = 1; i < array.Length; i++)
            {
                if (array[i - 1] == ' ' || array[i - 1] == '-')
                {
                    if (char.IsLower(array[i]))
                    {
                        array[i] = char.ToUpper(array[i], CultureInfo.CurrentCulture);
                    }
                }
            }
            return new string(array);
        }

        public static T ConvertTo<T>(this Control c)
        {
            try
            {
                return (T)Convert.ChangeType(c, typeof(T), CultureInfo.CurrentCulture);
            }
            catch (Exception) { return default; }
        }

        public static TimeSpan ToTimeSpan(this float timeStamp)
        {
            return new TimeSpan(ticks: (long)timeStamp);
        }

        public static TraktRequestAction Invert(this TraktRequestAction action)
        {
            if (action == TraktRequestAction.AddEpisode) return TraktRequestAction.RemoveEpisode;
            else if (action == TraktRequestAction.RemoveEpisode) return TraktRequestAction.AddEpisode;
            else return default;
        }

        public static bool ToBool(this TraktRequestAction action)
        {
            if (action == TraktRequestAction.AddEpisode) return true;
            else return false;
        }

        public static int ToInt(this TraktRequestAction action)
        {
            if (action == TraktRequestAction.AddEpisode) return 1;
            else return -1;
        }

        public static TResult InvokeIfRequired<T, TResult>(this T source, Func<TResult> func)
            where T : Control
        {
            TResult returnValue = default;
            try
            {
                if (!source.InvokeRequired)
                    returnValue = func();
                else
                {
                    TResult temp = default;
                    source.Invoke(new Action(() => temp = func()));
                    returnValue = temp;
                }
            }
            catch (Exception) { }
            return returnValue;
        }

        public static void InvokeIfRequired<T>(this T source, Action action)
            where T : Control
        {
            try
            {
                if (!source.InvokeRequired)
                    action();
                else
                    source.Invoke(new Action(() => action()));
            }
            catch (Exception) { return; }
        }

        public static ListViewItem FindItemWithTextExact(this ListView lv, string searchText)
        {
            ListViewItem[] items = lv.Items.Find(searchText);
            if(items.Length > 1)
            {
                return null;
            }
            else if(items.Length == 1) { return items[0]; }
            return null;
        }

        public static ListViewItem[] Find(this ListViewItemCollection Items, string searchText)
        {
            List<ListViewItem> foundItems = new List<ListViewItem>();
            foreach(ListViewItem lvi in Items)
            {
                if (lvi.Text.Equals(searchText)) { foundItems.Add(lvi); }
            }
            return foundItems.ToArray();
        }

        public static void Forget(this Task t) { }

        public static double GetSimilarityRatio(String FullString1, String FullString2, out double WordsRatio, out double RealWordsRatio)
        {
            double theResult = 0;
            String[] Splitted1 = FullString1.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            String[] Splitted2 = FullString2.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (Splitted1.Length < Splitted2.Length)
            {
                String[] Temp = Splitted2;
                Splitted2 = Splitted1;
                Splitted1 = Temp;
            }
            int[,] theScores = new int[Splitted1.Length, Splitted2.Length];
            int[] BestWord = new int[Splitted1.Length];

            for (int loop = 0; loop < Splitted1.Length; loop++)
            {
                for (int loop1 = 0; loop1 < Splitted2.Length; loop1++) theScores[loop, loop1] = 1000;
                BestWord[loop] = -1;
            }
            int WordsMatched = 0;
            for (int loop = 0; loop < Splitted1.Length; loop++)
            {
                String String1 = Splitted1[loop];
                for (int loop1 = 0; loop1 < Splitted2.Length; loop1++)
                {
                    String String2 = Splitted2[loop1];
                    int LevenshteinDistance = Compute(String1, String2);
                    theScores[loop, loop1] = LevenshteinDistance;
                    if (BestWord[loop] == -1 || theScores[loop, BestWord[loop]] > LevenshteinDistance) BestWord[loop] = loop1;
                }
            }

            for (int loop = 0; loop < Splitted1.Length; loop++)
            {
                if (theScores[loop, BestWord[loop]] == 1000) continue;
                for (int loop1 = loop + 1; loop1 < Splitted1.Length; loop1++)
                {
                    if (theScores[loop1, BestWord[loop1]] == 1000) continue;
                    if (BestWord[loop] == BestWord[loop1])
                    {
                        if (theScores[loop, BestWord[loop]] <= theScores[loop1, BestWord[loop1]])
                        {
                            theScores[loop1, BestWord[loop1]] = 1000;
                            int CurrentBest = -1;
                            int CurrentScore = 1000;
                            for (int loop2 = 0; loop2 < Splitted2.Length; loop2++)
                            {
                                if (CurrentBest == -1 || CurrentScore > theScores[loop1, loop2])
                                {
                                    CurrentBest = loop2;
                                    CurrentScore = theScores[loop1, loop2];
                                }
                            }
                            BestWord[loop1] = CurrentBest;
                        }
                        else
                        {
                            theScores[loop, BestWord[loop]] = 1000;
                            int CurrentBest = -1;
                            int CurrentScore = 1000;
                            for (int loop2 = 0; loop2 < Splitted2.Length; loop2++)
                            {
                                if (CurrentBest == -1 || CurrentScore > theScores[loop, loop2])
                                {
                                    CurrentBest = loop2;
                                    CurrentScore = theScores[loop, loop2];
                                }
                            }
                            BestWord[loop] = CurrentBest;
                        }

                        loop = -1;
                        break;
                    }
                }
            }
            for (int loop = 0; loop < Splitted1.Length; loop++)
            {
                if (theScores[loop, BestWord[loop]] == 1000) theResult += Splitted1[loop].Length;
                else
                {
                    theResult += theScores[loop, BestWord[loop]];
                    if (theScores[loop, BestWord[loop]] == 0) WordsMatched++;
                }
            }
            int theLength = (FullString1.Replace(" ", "").Length > FullString2.Replace(" ", "").Length) ? FullString1.Replace(" ", "").Length : FullString2.Replace(" ", "").Length;
            if (theResult > theLength) theResult = theLength;
            theResult = (1 - (theResult / theLength)) * 100;
            WordsRatio = ((double)WordsMatched / (double)Splitted2.Length) * 100;
            RealWordsRatio = ((double)WordsMatched / (double)Splitted1.Length) * 100;
            return theResult;
        }

        public static int Compute(string s, string t)
        {
            int n = s.Length;
            int m = t.Length;
            int[,] d = new int[n + 1, m + 1];
            if (n == 0)
            {
                return m;
            }

            if (m == 0)
            {
                return n;
            }
            for (int i = 0; i <= n; d[i, 0] = i++)
            {
            }

            for (int j = 0; j <= m; d[0, j] = j++)
            {
            }
            for (int i = 1; i <= n; i++)
            {
                for (int j = 1; j <= m; j++)
                {
                    int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;
                    d[i, j] = Math.Min(
                        Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                        d[i - 1, j - 1] + cost);
                }
            }
            return d[n, m];
        }

        public static async Task<bool> MarkEpisodeWatched(this TraktClient Client, TraktShow show, TraktEpisode episode)
        {
            try
            {
                var historyPostBuilder = new TraktSyncHistoryPostBuilder();
                historyPostBuilder.AddEpisode(episode);
                var addEpisodeResponse = await Client.Sync.AddWatchedHistoryItemsAsync(historyPostBuilder.Build());
                if (addEpisodeResponse.Added.Episodes.HasValue && addEpisodeResponse.Added.Episodes.Value >= 1)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                if (ex.GetType() == typeof(System.Net.Http.HttpRequestException) || ex.GetType() == typeof(TraktServerException) || ex.GetType() == typeof(TraktServerUnavailableException))
                {
                    throw ex;
                }
            }
            return false;
        }

        public static async Task<bool> MarkEpisodeWatched(this TraktClient Client, TraktShow show, int seasonNumber, int episodeNumber)
        {
            try
            {
                var historyPostBuilder = new TraktSyncHistoryPostBuilder();
                var episode = await Client.Episodes.GetEpisodeAsync(show.Ids.Slug, seasonNumber, episodeNumber);
                historyPostBuilder.AddEpisode(episode);
                var addEpisodeResponse = await Client.Sync.AddWatchedHistoryItemsAsync(historyPostBuilder.Build());
                if (addEpisodeResponse.Added.Episodes.HasValue && addEpisodeResponse.Added.Episodes.Value >= 1)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                if (ex.GetType() == typeof(System.Net.Http.HttpRequestException) || ex.GetType() == typeof(TraktServerException) || ex.GetType() == typeof(TraktServerUnavailableException))
                {
                    throw ex;
                }
            }
            return false;
        }

        public static async Task<bool> MarkEpisodesWatched(this TraktClient Client, TraktShow show, List<TraktEpisode> episodeList)
        {
            try
            {
                var historyPostBuilder = new TraktSyncHistoryPostBuilder();
                foreach (var episode in episodeList) historyPostBuilder.AddEpisode(episode);
                var addEpisodeResponse = await Client.Sync.AddWatchedHistoryItemsAsync(historyPostBuilder.Build());
                if (addEpisodeResponse.Added.Episodes.HasValue && addEpisodeResponse.Added.Episodes.Value >= 1)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                if (ex.GetType() == typeof(System.Net.Http.HttpRequestException) || ex.GetType() == typeof(TraktServerException) || ex.GetType() == typeof(TraktServerUnavailableException))
                {
                    throw ex;
                }
            }
            return false;
        }

        public static async Task<bool> RemoveWatchedEpisode(this TraktClient Client, TraktShow show, TraktEpisode episode)
        {
            try
            {
                var historyRemoveBuilder = new TraktSyncHistoryRemovePostBuilder();
                historyRemoveBuilder.AddEpisode(episode);
                var removeEpisodeResponse = await Client.Sync.RemoveWatchedHistoryItemsAsync(historyRemoveBuilder.Build());
                if (removeEpisodeResponse.Deleted.Episodes.HasValue && removeEpisodeResponse.Deleted.Episodes.Value >= 1)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                if (ex.GetType() == typeof(System.Net.Http.HttpRequestException) || ex.GetType() == typeof(TraktServerException) || ex.GetType() == typeof(TraktServerUnavailableException))
                {
                    throw ex;
                }
            }
            return false;
        }

        public static async Task<bool> RemoveWatchedEpisode(this TraktClient Client, TraktShow show, int seasonNumber, int episodeNumber)
        {
            try
            {
                var historyRemoveBuilder = new TraktSyncHistoryRemovePostBuilder();
                var episode = await Client.Episodes.GetEpisodeAsync(show.Ids.Slug, seasonNumber, episodeNumber);
                historyRemoveBuilder.AddEpisode(episode);
                var removeHistoryResponse = await Client.Sync.RemoveWatchedHistoryItemsAsync(historyRemoveBuilder.Build());
                if (removeHistoryResponse.Deleted.Episodes.HasValue && removeHistoryResponse.Deleted.Episodes.Value >= 1)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                if (ex.GetType() == typeof(System.Net.Http.HttpRequestException) || ex.GetType() == typeof(TraktServerException) || ex.GetType() == typeof(TraktServerUnavailableException))
                {
                    throw ex;
                }
            }
            return false;
        }

        public static async Task<bool> RemoveWatchedEpisodes(this TraktClient Client, TraktShow show, List<TraktEpisode> episodeList)
        {
            try
            {
                var historyRemoveBuilder = new TraktSyncHistoryRemovePostBuilder();
                foreach(var episode in episodeList) historyRemoveBuilder.AddEpisode(episode);
                var removeHistoryResponse = await Client.Sync.RemoveWatchedHistoryItemsAsync(historyRemoveBuilder.Build());
                if(removeHistoryResponse.Deleted.Episodes.HasValue && removeHistoryResponse.Deleted.Episodes.Value >= 1)
                {
                    return true;
                }
            }
            catch(Exception ex)
            {
                if (ex.GetType() == typeof(System.Net.Http.HttpRequestException) || ex.GetType() == typeof(TraktServerException) || ex.GetType() == typeof(TraktServerUnavailableException))
                {
                    throw ex;
                }
            }
            return false;
        }

        public static async Task<bool> RateShow(this TraktClient Client, TraktShow show, int rating)
        {
            try
            {
                TraktSyncRatingsPostBuilder ratingsPostBuilder = new TraktSyncRatingsPostBuilder();
                if (rating == 0)
                {
                    ratingsPostBuilder.AddShow(show);
                    var ratingResponse = await Client.Sync.RemoveRatingsAsync(ratingsPostBuilder.Build());
                    if (ratingResponse.Deleted.Shows.HasValue && ratingResponse.Deleted.Shows.Value >= 1)
                    {
                        return true;
                    }
                }
                else
                {
                    ratingsPostBuilder.AddShowWithRating(show, rating);
                    var ratingResponse = await Client.Sync.AddRatingsAsync(ratingsPostBuilder.Build());
                    if (ratingResponse.Added.Shows.HasValue && ratingResponse.Added.Shows.Value >= 1)
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.GetType() == typeof(System.Net.Http.HttpRequestException) || ex.GetType() == typeof(TraktServerException) || ex.GetType() == typeof(TraktServerUnavailableException))
                {
                    throw ex;
                }
            }
            return false;
        }

        public static async Task SyncShowOverview(this TraktShow show, TraktClient Client)
        {
            show.Seasons = await Client.Seasons.GetAllSeasonsAsync(show.Ids.Slug);
            List<Task> taskList = new List<Task>();
            foreach (TraktSeason season in show.Seasons)
            {
                if (season.Number > 0)
                {
                    taskList.Add(Task.Run(() => SyncSeasonEpisodes(Client, show.Ids.Slug, season)));
                }
            }
            await Task.WhenAll(taskList);
        }

        private static async Task SyncSeasonEpisodes(TraktClient Client, string showIdOrSlug, TraktSeason season)
        {
            season.Episodes = await Client.Seasons.GetSeasonAsync(showIdOrSlug, season.Number.Value);
        }
    }
}
