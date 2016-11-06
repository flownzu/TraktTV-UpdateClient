using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace TraktTVUpdateClient.Cache
{
    public class TmdbRateLimiter
    {
        private static readonly List<DateTime> _requests = new List<DateTime>();

        private const double _perMillisecond = 10000.1;
        private const int _rateLimit = 40;
        private const int _rateLimitCooldownMs = 10000;

        public static void CheckLimiter()
        {
            _requests.Add(DateTime.Now);

            var requestDuringRateLimit = _requests.Where(x => (DateTime.Now - x).TotalMilliseconds < _perMillisecond).ToArray();
            if(requestDuringRateLimit.Count() >= _rateLimit)
            {
                Thread.Sleep(_rateLimitCooldownMs);
            }

            if (_requests.Count > 0) _requests.RemoveAll(x => (DateTime.Now - x).TotalMilliseconds >= _perMillisecond);
        }
    }
}
