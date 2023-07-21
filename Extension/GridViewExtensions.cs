using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HadesFormCommon.Extension
{
    public static class GridViewExtensions
    {
        public static void CreateColumn(this DataGridView dataGridView, string headerText, string name)
        {
            DataGridViewTextBoxCell cell = new();
            DataGridViewColumn col = new()
            {
                HeaderText = headerText,
                Name = name,
                DataPropertyName = name,
                CellTemplate = cell
            };
            dataGridView.Columns.Add(col);
        }
        public static void SetDataSource<T>(this DataGridView dataGridView, List<T> dataSource)
        {
            dataGridView.Invoke(new Action(() =>
            {
                dataGridView.DataSource = null;
                if (dataSource.Count > 0)
                {
                    dataGridView.DataSource = dataSource;
                    dataGridView.ClearSelection();
                }
            }));
        }
    }
}
