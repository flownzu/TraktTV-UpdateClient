using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using TraktApiSharp.Authentication;
using TraktApiSharp.Services;
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
            catch (Exception) { return default(TraktAuthorization); }
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
            catch (Exception) { return default(T); }
        }

        public static TimeSpan ToTimeSpan(this float timeStamp)
        {
            return new TimeSpan(ticks: (long)timeStamp);
        }

        public static TResult InvokeIfRequired<T, TResult>(this T source, Func<TResult> func)
            where T : Control
        {
            TResult returnValue = default(TResult);
            try
            {
                if (!source.InvokeRequired)
                    returnValue = func();
                else
                {
                    TResult temp = default(TResult);
                    source.Invoke(new Action(() => temp = func()));
                    returnValue = temp;
                }
            }
            catch (Exception) { return default(TResult); }
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
    }
}
