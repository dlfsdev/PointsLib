using PointsLibUi.Extensions;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace PointsLibUi.Controls
{
    /// <summary>
    /// Provides "hand tool"-like drag notifications for the provided
    /// <see cref="System.Windows.Forms.Control"/>
    /// </summary>
    internal sealed class HandToolDragger
    {
        private readonly Control _control;

        private Point _lastMouseLocation;

        public event EventHandler<HandToolDragEventArgs> Drag;

        public HandToolDragger(Control control)
        {
            Button = MouseButtons.Left;
            _control = control;
            _control.MouseDown += MouseDown;
        }

        private MouseButtons _button;
        public MouseButtons Button
        {
            get { return _button; }
            
            set
            {
                _button = value;

                if(_control != null)
                    EndDrag();
            }
        }

        private void MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != Button)
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
            OnDrag(dx, dy);
        }

        private void MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button != Button)
                return;
            EndDrag();
        }

        private void OnDrag(int dx, int dy)
        {
            Drag.RaiseSafe(this, new HandToolDragEventArgs(dx, dy));
        }

        private void EndDrag()
        {
            _control.MouseMove -= MouseMove;
            _control.MouseUp -= MouseUp;
        }
    }

    internal sealed class HandToolDragEventArgs : EventArgs
    {
        public HandToolDragEventArgs(int dx, int dy)
        {
            _dx = dx;
            _dy = dy;
        }

        private int _dx;
        public int Dx { get { return _dx; } }

        private int _dy;
        public int Dy { get { return _dy; } }
    }
}
