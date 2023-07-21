using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HadesFormCommon.Utilities
{
    public class MsgBox
    {
        public static DialogResult Err(string msg, Exception? e = null)
        {
            if (e != null)
            {
                msg += Environment.NewLine + e.StackTrace;
            }
            return MessageBox.Show(msg, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        public static DialogResult Warn(string msg)
        {
            return MessageBox.Show(msg, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        public static DialogResult Info(string msg)
        {
            return MessageBox.Show(msg, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        public static DialogResult Quest(string msg)
        {
            return MessageBox.Show(msg, "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }
    }
}
