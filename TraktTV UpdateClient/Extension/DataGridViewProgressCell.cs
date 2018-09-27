using System;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;

namespace TraktTVUpdateClient.Extension
{
    class DataGridViewProgressCell : DataGridViewImageCell
    {
        static Image emptyImage;
        static Color _ProgressBarColor;

        public Color ProgressBarColor
        {
            get { return _ProgressBarColor; }
            set { _ProgressBarColor = value; }
        }

        static DataGridViewProgressCell()
        {
            emptyImage = new Bitmap(1, 1, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
        }

        public DataGridViewProgressCell()
        {
            ValueType = typeof(int);
        }

        protected override object GetFormattedValue(object value, int rowIndex, ref DataGridViewCellStyle cellStyle, TypeConverter valueTypeConverter, TypeConverter formattedValueTypeConverter, DataGridViewDataErrorContexts context)
        {
            return emptyImage;
        }

        protected override void Paint(Graphics g, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates cellState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
        {
            if (value is int[] values)
            {
                float percentage = (float)values[0] / values[1];
                Brush backColorBrush = new SolidBrush(cellStyle.BackColor);
                Brush foreColorBrush = new SolidBrush(cellStyle.ForeColor);
                base.Paint(g, clipBounds, cellBounds, rowIndex, cellState, value, formattedValue, errorText, cellStyle, advancedBorderStyle, (paintParts & ~DataGridViewPaintParts.ContentForeground));
                float posX = cellBounds.X;
                float posY = cellBounds.Y;
                string pbText = string.Join("/", values);
                float textWidth = TextRenderer.MeasureText(pbText, cellStyle.Font).Width;
                float textHeight = TextRenderer.MeasureText(pbText, cellStyle.Font).Height;
                switch (cellStyle.Alignment)
                {
                    case DataGridViewContentAlignment.BottomCenter:
                        posX = cellBounds.X + (cellBounds.Width / 2) - textWidth / 2;
                        posY = cellBounds.Y + cellBounds.Height - textHeight;
                        break;
                    case DataGridViewContentAlignment.BottomLeft:
                        posX = cellBounds.X;
                        posY = cellBounds.Y + cellBounds.Height - textHeight;
                        break;
                    case DataGridViewContentAlignment.BottomRight:
                        posX = cellBounds.X + cellBounds.Width - textWidth;
                        posY = cellBounds.Y + cellBounds.Height - textHeight;
                        break;
                    case DataGridViewContentAlignment.MiddleCenter:
                        posX = cellBounds.X + (cellBounds.Width / 2) - textWidth / 2;
                        posY = cellBounds.Y + (cellBounds.Height / 2) - textHeight / 2;
                        break;
                    case DataGridViewContentAlignment.MiddleLeft:
                        posX = cellBounds.X;
                        posY = cellBounds.Y + (cellBounds.Height / 2) - textHeight / 2;
                        break;
                    case DataGridViewContentAlignment.MiddleRight:
                        posX = cellBounds.X + cellBounds.Width - textWidth;
                        posY = cellBounds.Y + (cellBounds.Height / 2) - textHeight / 2;
                        break;
                    case DataGridViewContentAlignment.TopCenter:
                        posX = cellBounds.X + (cellBounds.Width / 2) - textWidth / 2;
                        posY = cellBounds.Y;
                        break;
                    case DataGridViewContentAlignment.TopLeft:
                        posX = cellBounds.X;
                        posY = cellBounds.Y;
                        break;

                    case DataGridViewContentAlignment.TopRight:
                        posX = cellBounds.X + cellBounds.Width - textWidth;
                        posY = cellBounds.Y;
                        break;
                }
                if (percentage >= 0.0)
                {
                    g.FillRectangle(new SolidBrush(_ProgressBarColor), cellBounds.X + 2, cellBounds.Y + 2, Convert.ToInt32(percentage * cellBounds.Width) - 5, cellBounds.Height / 1 - 5);
                    g.DrawString(pbText, cellStyle.Font, foreColorBrush, posX, posY);
                }
                else
                {
                    if (DataGridView.CurrentRow.Index == rowIndex)
                    {
                        g.DrawString(pbText, cellStyle.Font, new SolidBrush(cellStyle.SelectionForeColor), posX, posX);
                    }
                    else
                    {
                        g.DrawString(pbText, cellStyle.Font, foreColorBrush, posX, posY);
                    }
                }
            }
        }

        public override object Clone()
        {
            DataGridViewProgressCell dataGridViewCell = base.Clone() as DataGridViewProgressCell;
            if (dataGridViewCell != null)
            {
                dataGridViewCell.ProgressBarColor = ProgressBarColor;
            }
            return dataGridViewCell;
        }

        internal void SetProgressBarColor(int rowIndex, Color value)
        {
            ProgressBarColor = value;
        }
    }
}
