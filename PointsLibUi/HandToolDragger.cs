using System;
using System.Drawing;
using System.Windows.Forms;

namespace PointsLibUi
{
    /// <summary>
    /// Provides "hand tool"-like drag notifications for the provided
    /// <see cref="System.Windows.Forms.Control"/>
    /// </summary>
    internal sealed class HandToolDragger
    {
        private readonly MouseButtons _button;
        private readonly Control _control;
        private readonly PerformDragDelegate _performDrag;

        private Point _lastMouseLocation;

        public delegate void PerformDragDelegate(int dx, int dy);

        public HandToolDragger(Control control, MouseButtons button, PerformDragDelegate performDrag)
        {
            _control = control;
            _button = button;
            _performDrag = performDrag;

            _control.MouseDown += MouseDown;
        }

        private void MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != _button)
                return;

            _lastMouseLocation = e.Location;
            _control.MouseMove += MouseMove;
            _control.MouseUp += MouseUp;
        }

        private void MouseMove(Object sender, MouseEventArgs e)
        {
            int dx = e.Location.X - _lastMouseLocation.X;
            int dy = e.Location.Y - _lastMouseLocation.Y;
            _lastMouseLocation = e.Location;
            _performDrag(dx, dy);
        }

        private void MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button != _button)
                return;

            _control.MouseMove -= MouseMove;
            _control.MouseUp -= MouseUp;
        }
    }
}
