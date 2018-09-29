using Newtonsoft.Json;

namespace TraktTVUpdateClient.Cache.Images.TheTVDB
{
    public class JWTToken
    {
        [JsonProperty(PropertyName = "token")]
        public string Token { get; set; }
    }
}
