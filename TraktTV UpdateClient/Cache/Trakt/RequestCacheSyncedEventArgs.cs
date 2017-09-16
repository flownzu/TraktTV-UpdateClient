using System;
using System.Collections.Generic;

namespace TraktTVUpdateClient.Cache
{
    public class RequestCacheSyncedEventArgs : EventArgs
    {
        public static new RequestCachedEventArgs Empty => default;
        public IEnumerable<TraktRequest> RequestList;

        public RequestCacheSyncedEventArgs(IEnumerable<TraktRequest> requestList)
        {
            RequestList = requestList;
        }
    }
}
