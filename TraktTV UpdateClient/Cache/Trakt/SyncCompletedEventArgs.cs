using System;
using System.Collections.Generic;
using TraktApiSharp.Objects.Get.Ratings;
using TraktApiSharp.Objects.Get.Shows;

namespace TraktTVUpdateClient
{
    public class SyncCompletedEventArgs : EventArgs
    {
        public IEnumerable<TraktShow> Shows;
        public IEnumerable<TraktRatingsItem> Ratings;

        public SyncCompletedEventArgs(IEnumerable<TraktShow> changedShowList, IEnumerable<TraktRatingsItem> changedRatingsList)
        {

        }

        public SyncCompletedEventArgs()
        {

        }
    }
}
