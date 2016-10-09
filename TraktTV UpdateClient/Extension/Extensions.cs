using System.Threading.Tasks;
using TraktApiSharp.Authentication;

namespace TraktTVUpdateClient.Extension
{
    static class Extensions
    {
        public static string CreateAuthorizationUrlNoPin(this TraktOAuth OAuth)
        {
            return OAuth.CreateAuthorizationUrl().Replace("redirect_uri=urn%3Aietf%3Awg%3Aoauth%3A2.0%3Aoob", "redirect_uri=app:%2f%2fauthorized");
        }

        public static void Forget(this Task t) { }
    }
}
