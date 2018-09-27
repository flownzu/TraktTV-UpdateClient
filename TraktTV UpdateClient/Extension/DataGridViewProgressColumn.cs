using System;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;

namespace TraktTVUpdateClient.Extension
{
    public class DataGridViewProgressColumn : DataGridViewImageColumn
    {
        public static Color _ProgressBarColor;

        public DataGridViewProgressColumn()
        {
            CellTemplate = new DataGridViewProgressCell(); ;
        }

        public override DataGridViewCell CellTemplate
        {
            get
            {
                return base.CellTemplate;
            }
            set
            {
                if (value != null && !value.GetType().IsAssignableFrom(typeof(DataGridViewProgressCell)))
                {
                    throw new InvalidCastException("Must be a DataGridViewProgressCell");
                }
                base.CellTemplate = value;
            }
        }

        [Browsable(true)]
        public Color ProgressBarColor
        {
            get
            {
                if (ProgressBarCellTemplate == null)
                {
                    throw new InvalidOperationException("Operation cannot be completed because this DataGridViewColumn does not have a CellTemplate.");
                }
                return ProgressBarCellTemplate.ProgressBarColor;
            }
            set
            {
                if (ProgressBarCellTemplate == null)
                {
                    throw new InvalidOperationException("Operation cannot be completed because this DataGridViewColumn does not have a CellTemplate.");
                }
                ProgressBarCellTemplate.ProgressBarColor = value;
                if (DataGridView != null)
                {
                    DataGridViewRowCollection dataGridViewRows = this.DataGridView.Rows;
                    int rowCount = dataGridViewRows.Count;
                    for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
                    {
                        DataGridViewRow dataGridViewRow = dataGridViewRows.SharedRow(rowIndex);
                        if (dataGridViewRow.Cells[Index] is DataGridViewProgressCell dataGridViewCell)
                        {
                            dataGridViewCell.SetProgressBarColor(rowIndex, value);
                        }
                    }
                    DataGridView.InvalidateColumn(Index);
                    // TODO: This column and/or grid rows may need to be autosized depending on their
                    //       autosize settings. Call the autosizing methods to autosize the column, rows, 
                    //       column headers / row headers as needed.
                }
            }
        }

        private DataGridViewProgressCell ProgressBarCellTemplate
        {
            get
            {
                return (DataGridViewProgressCell)CellTemplate;
            }
        }
    }
}
