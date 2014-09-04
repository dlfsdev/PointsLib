using System;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace PointsLibUi.Extensions
{
    public static class ControlExtensions
    {
        public static GridItem TryGetRoot(this PropertyGrid grid)
        {
            var root = grid.SelectedGridItem;
            if (root == null)
                return null;
            while (root.Parent != null)
                root = root.Parent;
            return root;
        }

        public static bool SetExpanded(this PropertyGrid grid, string nodeName, bool expanded)
        {
            var root = grid.TryGetRoot();
            if (root == null)
                return false;

            foreach(var item in root.GridItems.Cast<GridItem>())
            {
                if (item == null)
                    continue;

                if(item.Label.EqualsOrdinal(nodeName) && item.Expandable)
                {
                    item.Expanded = expanded;
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Perform the given <see cref="System.Action"/> on the controls's main thread and block until it is complete
        /// </summary>
        public static void DoOnUiThreadBlocking(this Control control, Action action)
        {
            if (control.InvokeRequired)
                control.Invoke((MethodInvoker)delegate { action(); });
            else
                action();
        }

        /// <summary>
        /// Schedule the given <see cref="System.Action"/> to take place on the control's main thread and return immediately
        /// </summary>
        public static void DoOnUiThreadNonblocking(this Control control, Action action)
        {
            control.BeginInvoke((MethodInvoker)delegate { action(); });
        }

        /// <summary>
        /// Disable drawing until a subsequent call to <see cref="ResumeDrawing"/>
        /// </summary>
        public static void SuspendDrawing(this Control c)
        {
            SendMessage(c.Handle, WM_SETREDRAW, UIntPtr.Zero, IntPtr.Zero);
        }

        /// <summary>
        /// Resume drawing after a prior call to <see cref="SuspendDrawing"/>
        /// </summary>
        public static void ResumeDrawing(this Control c)
        {
            SendMessage(c.Handle, WM_SETREDRAW, (UIntPtr)1, IntPtr.Zero);
            c.Refresh();
        }

        /// <summary>
        /// Clamp <paramref name="value"/> to <paramref name="bar"/>'s Min/Max range and then set Value to it.
        /// </summary>
        public static void SafeSetValue(this ScrollProperties bar, int value)
        {
            bar.Value = value.Clamp(bar.Minimum, bar.Maximum);
        }

        /// <summary>
        /// Clamp <paramref name="value"/> to <paramref name="bar"/>'s Min/Max range and then set Value to it.
        /// </summary>
        public static void SafeSetValue(this ProgressBar bar, int value)
        {
            bar.Value = value.Clamp(bar.Minimum, bar.Maximum);
        }

        /// <summary>
        /// <see cref="System.Windows.Forms.ProgressBar"/> normally animates changes to its Value.
        /// But if progress is being made rapidly, these animations may lag significantly behind the actual amount of progress.
        /// Clamp <paramref name="value"/> to <paramref name="bar"/>'s Min/Max range and then set Value to it without animating.
        /// </summary>
        public static void SafeSetValueNoAnimate(this ProgressBar bar, int value)
        {
            int safeValue = value.Clamp(bar.Minimum, bar.Maximum);

            bar.Value = safeValue;

            if (safeValue != bar.Minimum)
            {
                bar.Value = safeValue - 1;
                bar.Value = safeValue;
            }
        }

        /// <summary>
        /// Round a <see cref="System.Drawing.PointF"/> to the nearest <see cref="System.Drawing.Point"/>/>
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static Point Round(this PointF p)
        {
            return new Point((int)Math.Round(p.X), (int)Math.Round(p.Y));
        }

        public static void ScrollToBottom(this TextBox tb)
        {
            SendMessage(tb.Handle, WM_VSCROLL, (UIntPtr)SB_BOTTOM, IntPtr.Zero);
        }

        private const UInt32 WM_VSCROLL = 0x115;
        private const UInt32 WM_SETREDRAW = 11;
        private const UInt32 SB_BOTTOM = 7;

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, UInt32 wMsg, UIntPtr wParam, IntPtr lParam);
    }
}
