using System;

namespace TraktTVUpdateClient.Cache
{
    public class RequestCachedEventArgs : EventArgs
    {
        public static new RequestCachedEventArgs Empty = default(RequestCachedEventArgs);
        public TraktRequest Request { get; set; }

        public RequestCachedEventArgs(TraktRequest _request)
        {
            Request = _request;
        }
    }
}
