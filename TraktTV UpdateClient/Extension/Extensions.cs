using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using TraktApiSharp.Authentication;

namespace TraktTVUpdateClient.Extension
{
    static class Extensions
    {
        public static string CreateAuthorizationUrlNoPin(this TraktOAuth OAuth)
        {
            return OAuth.CreateAuthorizationUrl().Replace("redirect_uri=urn%3Aietf%3Awg%3Aoauth%3A2.0%3Aoob", "redirect_uri=app:%2f%2fauthorized");
        }

        public static string UpperCase(this string s)
        {
            char[] array = s.ToCharArray();
            if (array.Length >= 1)
            {
                if (char.IsLower(array[0]))
                {
                    array[0] = char.ToUpper(array[0]);
                }
            }
            for (int i = 1; i < array.Length; i++)
            {
                if (array[i - 1] == ' ' || array[i - 1] == '-')
                {
                    if (char.IsLower(array[i]))
                    {
                        array[i] = char.ToUpper(array[i]);
                    }
                }
            }
            return new string(array);
        }

        public static T ConvertTo<T>(this Control c)
        {
            try
            {
                return (T)Convert.ChangeType(c, typeof(T));
            }
            catch (Exception) { return default(T); }
        }

        public static void Forget(this Task t) { }
    }
}
