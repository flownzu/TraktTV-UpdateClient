using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace TraktTVUpdateClient
{
    public partial class TreeViewEx : TreeView
    {
        public TreeViewEx()
        {
            
        }

        protected override void WndProc(ref Message m)
        {
            if(m.Msg == 0x203 && CheckBoxes)
            {
                int x = m.LParam.ToInt32() & 0xffff;
                int y = (m.LParam.ToInt32() >> 16) & 0xffff;
                var hitTestInfo = HitTest(x, y);

                if(hitTestInfo.Node != null && hitTestInfo.Location == TreeViewHitTestLocations.StateImage)
                {
                    m.Result = IntPtr.Zero;
                    return;
                }
            }
            base.WndProc(ref m);
        }
    }
}
