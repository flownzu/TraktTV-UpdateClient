using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace TraktTVUpdateClient.Extension
{
    class ProgressBarEx : ProgressBar
    {
        [DllImportAttribute("uxtheme.dll")]
        private static extern int SetWindowTheme(IntPtr hWnd, string appname, string idlist);

        public ProgressBarDisplayText DisplayStyle { get; set; }
        public String CustomText { get; set; }

        public ProgressBarEx()
        {
            if (ProgressBarRenderer.IsSupported) { SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true); }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (ProgressBarRenderer.IsSupported)
            {
                Rectangle rect = ClientRectangle;
                Graphics g = e.Graphics;
                ProgressBarRenderer.DrawHorizontalBar(g, rect);
                rect.Inflate(-3, -3);
                if (Value > 0)
                {
                    Rectangle clip = new Rectangle(rect.X, rect.Y, (int)Math.Round(((float)Value / Maximum) * rect.Width), rect.Height);
                    ProgressBarRenderer.DrawHorizontalChunks(g, clip);
                }
                string text = DisplayStyle == ProgressBarDisplayText.Percentage ? Value.ToString() + '%' : CustomText;
                using (Font f = new Font(FontFamily.GenericSerif, 10))
                {
                    SizeF len = g.MeasureString(text, f);
                    // Point location = new Point(Convert.ToInt32((rect.Width / 2) - (len.Width / 2)), Convert.ToInt32((rect.Height / 2) - (len.Height / 2)));
                    Point location = new Point(Convert.ToInt32((Width / 2) - len.Width / 2), Convert.ToInt32((Height / 2) - len.Height / 2));
                    g.DrawString(text, f, Brushes.Red, location);
                }
            }
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            SetWindowTheme(this.Handle, "", "");
            base.OnHandleCreated(e);
        }
    }

    public enum ProgressBarDisplayText
    {
        Percentage,
        CustomText
    }
}
