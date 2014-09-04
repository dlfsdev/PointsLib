using System;
using System.Windows.Forms;

namespace PointsLibUi.Controls
{
    public sealed class BetterTrackBar : TrackBar
    {
        private bool _dragging = false;

        // Hide the focus rectange when we get focus; it's really not needed here
        // See MSDN documentation on WM_UPDATEUISTATE for details
        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            SendMessage(this.Handle, WM_UPDATEUISTATE, (UIntPtr)MakeDword(UIS_SET, UISF_HIDEFOCUS), (IntPtr)0);
        }

        // Put the thumb where the user clicked instead of just moving it a single tick in that direction
        // If we do this we need to manage dragging too or things get weird
        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _dragging = true;
                SetValueAt(e.X);
            }
            else
            {
                base.OnMouseDown(e);
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                _dragging = false;
            else
                base.OnMouseUp(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (_dragging)
                SetValueAt(e.X);
            else
                base.OnMouseMove(e);
        }

        private void SetValueAt(int x)
        {
            int fudge = 13; // the track doesn't quite occupy the full width
            int clickPos = x - fudge;
            int trackWidth = Width - 2 * fudge;
            float preciseNewValue = ((float)clickPos / trackWidth) * (Maximum - Minimum);
            int newValue = Minimum + (int)Math.Round(preciseNewValue);
            if (newValue != Value)
                Value = newValue;
        }

        private const uint WM_UPDATEUISTATE = 0x0128;
        private const ushort UIS_SET = 1;
        private const ushort UISF_HIDEFOCUS = 1;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, UInt32 wMsg, UIntPtr wParam, IntPtr lParam);

        private static UInt32 MakeDword(ushort loWord, ushort hiWord)
        {
            return ((UInt32)hiWord << 16) | ((UInt32)loWord & 0xffffu);
        }
    }
}
