using HadesAIOCommon.WinAPI;
using System.Diagnostics;
using System.Reflection;

namespace HadesFormCommon.Utilities
{
    public class HadesUtils
    {
        public static void EmbedProcessToControl(Control control, Process proc, int w, int h, int marginY = 0)
        {
            control.Invoke(new Action(() =>
            {
                _ = WinApiUtils.SetWindowLong(proc.MainWindowHandle, WindowStyle.GWL_STYLE, WindowStyle.WS_VISIBLE);
                WinApiUtils.SetParent(proc.MainWindowHandle, control.Handle);
                WinApiUtils.MoveWindow(proc.MainWindowHandle, 0, 0 - marginY, w, h, false);
            }));
        }
        public static void InvokeCrossThread(Control control, Action action)
        {
            control.Invoke(() =>
            {
                action.Invoke();
            });
        }
        public static void ExecuteAsync(Action action)
        {
            Task.Run(() =>
            {
                try
                {
                    action.Invoke();
                }
                catch (Exception e)
                {
                    MsgBox.Err(e.Message);
                }
            });
        }
        public static void SetDataSourceGridView<T>(DataGridView dataGridView, IEnumerable<T>? dataSource)
        {
            dataGridView.Invoke(new Action(() =>
            {
                dataGridView.DataSource = null;
                if (dataSource?.Count() > 0)
                {
                    dataGridView.DataSource = dataSource;
                    //dataGridView.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                    dataGridView.ClearSelection();
                }
            }));
        }
        public static void SetDoubleBufferDataGridView(DataGridView dataGridView)
        {
            typeof(DataGridView).InvokeMember(
                   "DoubleBuffered",
                   BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty,
                   null,
                   dataGridView,
                   new object[] { true });
        }
        public static void RefreshDataGridView(DataGridView dataGridView)
        {
            dataGridView.Invoke(new Action(() =>
            {
                dataGridView.Refresh();
            }));
        }
        public static void FitSizeColumnsGridView(DataGridView dataGridView, DataGridViewAutoSizeColumnMode mode, params string[] exceptColumns)
        {
            foreach (DataGridViewColumn col in dataGridView.Columns)
            {
                if (exceptColumns.Contains(col.Name))
                {
                    continue;
                }
                col.AutoSizeMode = mode;
            }
        }
        public static void SetColumnsWidth(DataGridView dataGridView, int size, params string[] columns)
        {
            foreach (var colName in columns)
            {
                dataGridView.Columns[colName].Width = size;
            }
        }
        public static void BindingRowIndexGridView(DataGridView dataGridView, string IndexRowName = "STT")
        {
            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                row.Cells[IndexRowName].Value = row.Index + 1;
            }
        }
        public static void SortDataGridView<T>(DataGridView dataGridView, string colName, bool ascending)
        {
            var data = dataGridView.Rows
                .Cast<DataGridViewRow>()
                .Select(r => r.DataBoundItem)
                .Cast<T>();
            if (ascending)
            {
                dataGridView.DataSource = data.OrderBy(_ => _?.GetType()?.GetProperty(colName)?.GetValue(_)).ToList();
            }
            else
            {
                dataGridView.DataSource = data.OrderByDescending(_ => _?.GetType()?.GetProperty(colName)?.GetValue(_)).ToList();
            }
        }

        public static List<T> GetSelectedOwningRows<T>(DataGridView dataGridView)
        {
            return GetSelectedRows(dataGridView)
                .Select(row => row.DataBoundItem)
                .Cast<T>()
                .ToList();
        }
        public static List<DataGridViewRow> GetSelectedRows(DataGridView dataGridView)
        {
            var selectedRows = dataGridView
                .SelectedCells
                .Cast<DataGridViewCell>()
                .Select(cell => cell.OwningRow)
                .Distinct();

            return selectedRows
                .Reverse()
                .ToList();
        }
        public static Control? FindControl(Control control, string controlName)
        {
            var controls = control.Controls.Find(controlName, true);
            if (!controls.Any())
            {
                return null;
            }
            return controls.First();
        }

        public static string ChooseFile(string? title = null, string filter = "Select file |*")
        {
            string fileName = string.Empty;
            using (OpenFileDialog fileDialog = new())
            {
                fileDialog.AddExtension = true;
                fileDialog.AutoUpgradeEnabled = true;
                fileDialog.CheckFileExists = true;
                fileDialog.CheckPathExists = true;
                fileDialog.RestoreDirectory = true;
                fileDialog.DefaultExt = "txt";
                fileDialog.Title = title ?? "Chọn file txt";
                fileDialog.Filter = filter; // "Text file (*.txt)|*.txt";
                fileDialog.ShowDialog();
                fileName = fileDialog.FileName;
            }
            return fileName;
        }
        public static List<string> ChooseFiles(string? title = null, string filter = "Select files |*")
        {
            var files = new List<string>();
            using (var fileDialog = new OpenFileDialog())
            {
                fileDialog.AddExtension = true;
                fileDialog.AutoUpgradeEnabled = true;
                fileDialog.CheckFileExists = true;
                fileDialog.CheckPathExists = true;
                fileDialog.RestoreDirectory = true;
                fileDialog.Multiselect = true;
                fileDialog.Title = title ?? "Chọn files";
                fileDialog.Filter = filter;
                fileDialog.ShowDialog();
                files = fileDialog.FileNames.ToList();
            }
            return files;
        }

    }
}
