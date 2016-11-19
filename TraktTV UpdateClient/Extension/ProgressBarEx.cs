using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace TraktTVUpdateClient.Extension
{
    class ProgressBarEx : ProgressBar
    {
        [DllImportAttribute("uxtheme.dll")]
        private static extern int SetWindowTheme(IntPtr hWnd, string appname, string idlist);

        /// <summary>
        /// Gets or sets the display style of the progress bar.
        /// <para><see cref="ProgressBarDisplayText.Percentage"/> shows the current progress bar value as percent on the control</para>
        /// <para><seealso cref="ProgressBarDisplayText.CustomText"/> shows a custom text on the control</para>
        /// </summary>
        public ProgressBarDisplayText DisplayStyle { get; set; }

        /// <summary>
        /// Sets the string that will be displayed when the <see cref="DisplayStyle"/> is set to <seealso cref="ProgressBarDisplayText.CustomText"/>.
        /// </summary>
        public String CustomText { get; set; }

        public ProgressBarEx()
        {
            if (ProgressBarRenderer.IsSupported)
            {
                SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
                BackColor = Color.DarkTurquoise;
            }
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
            using (Font f = new Font("Microsoft Sans Serif", 8))
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
        /// <summary>
        /// Shows the current progress bar value as percent on the control
        /// </summary>
        Percentage,
        /// <summary>
        /// Shows a custom text on the control
        /// </summary>
        CustomText
    }
}
