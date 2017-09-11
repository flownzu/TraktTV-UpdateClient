using System;
using System.Collections;
using System.Windows.Forms;

namespace TraktTVUpdateClient.Extension
{
    class ListViewItemComparer : IComparer
    {
        public int Column;
        private SortOrder sortOrder;

        public ListViewItemComparer(int col, SortOrder order)
        {
            Column = col;
            sortOrder = order;
        }

        public int Compare(object x, object y)
        {
            int returnVal = -1;
            if(Column == 0)
            {
                returnVal = String.Compare((x as ListViewItem).SubItems[Column].Text, (y as ListViewItem).SubItems[Column].Text);
            }
            else if(Column == 1)
            {
                ProgressBarEx pb1 = ((x as ListViewItem).ListView as ListViewEx).GetEmbeddedControl(x as ListViewItem).ConvertTo<ProgressBarEx>();
                ProgressBarEx pb2 = ((y as ListViewItem).ListView as ListViewEx).GetEmbeddedControl(y as ListViewItem).ConvertTo<ProgressBarEx>();
                returnVal = ((double)pb1.Value / pb1.Maximum).CompareTo((double)pb2.Value / pb2.Maximum);
            }
            else
            {
                returnVal = int.Parse((x as ListViewItem).SubItems[Column].Text).CompareTo(int.Parse((y as ListViewItem).SubItems[Column].Text));
            }
            return sortOrder == SortOrder.Descending ? returnVal * -1 : returnVal;
        }
    }
}
