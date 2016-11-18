using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
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
            BackColor = Color.DarkTurquoise;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            LinearGradientBrush brush = null;
            Rectangle rec = ClientRectangle;
            double scaleFactor = ((double)Value - (double)Minimum) / ((double)Maximum - (double)Minimum);
            ProgressBarRenderer.DrawHorizontalBar(e.Graphics, rec);
            rec.Width = (int)((rec.Width * scaleFactor) - 4);
            rec.Height -= 4;
            if(rec.Width == 0) { rec.Width = -1; }
            brush = new LinearGradientBrush(rec, this.ForeColor, this.BackColor, LinearGradientMode.Vertical);
            e.Graphics.FillRectangle(brush, 2, 2, rec.Width, rec.Height);
            string text = DisplayStyle == ProgressBarDisplayText.Percentage ? Value.ToString() + '%' : CustomText;
            using (Font f = new Font("Calibri", 10))
            {
                SizeF len = e.Graphics.MeasureString(text, f);
                Point location = new Point(Convert.ToInt32((Width / 2) - len.Width / 2), Convert.ToInt32((Height / 2) - len.Height / 2));
                e.Graphics.DrawString(text, f, Brushes.Black, location);
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
