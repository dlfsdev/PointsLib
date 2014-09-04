using PointsLibUi.Extensions;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace PointsLibUi.Controls
{
    /// <summary>
    /// A drawing canvas with built-in support for dragging and zooming
    /// </summary>
    public sealed class Canvas : PictureBox
    {
        private const float ZOOM_IN_RATE = 1.20f;

        private readonly HandToolDragger _dragger;

        // These are in CLIENT units
        // Positive values are akin to moving a scrollbar that far right/down
        private float _xScroll;
        private float _yScroll;

        // Applied AFTER scroll correction
        private float _zoomFactor;

        private bool _dragEnabled;

        // Fired when either a zoom or scroll causes the virtual bounds to change
        public event EventHandler BoundsChanged;

        public Canvas()
        {
            _xScroll = 0.0f;
            _yScroll = 0.0f;

            _zoomFactor = 1.0f;

            _dragger = new HandToolDragger(this);
            DragButton = MouseButtons.Left;
            DragEnabled = true;

            MouseWheel += OnMouseWheel;
        }

        [Browsable(true)]
        [DefaultValue(true)]
        public bool DragEnabled
        {
            get { return _dragEnabled; }

            set
            {
                if (value == DragEnabled)
                    return;

                _dragEnabled = value;

                if (value)
                    _dragger.Drag += OnDrag;
                else
                    _dragger.Drag -= OnDrag;
            }
        }

        [Browsable(true)]
        [DefaultValue(MouseButtons.Left)]
        public MouseButtons DragButton
        {
            get { return _dragger.Button; }
            set { _dragger.Button = value; }
        }

        // public so this can be attached to other controls' MouseWheel events, e.g. if you
        // want to wheel the control under the cursor rather than the one that has focus
        public void OnMouseWheel(object sender, MouseEventArgs e)
        {
            // This handler can be registered to more than one control, so be careful about
            // translating the location to client space
            Point clientPoint = PointToClient(((Control)sender).PointToScreen(e.Location));

            if (!ClientRectangle.Contains(clientPoint))
                return;

            float newZoom = _zoomFactor;

            if (e.Delta > 0)
                newZoom *= ZOOM_IN_RATE;
            else if (e.Delta < 0)
                newZoom /= ZOOM_IN_RATE;

            ZoomTo(newZoom, clientPoint);
        }

        public void ZoomTo(float newZoom, Point pivotClientPoint)
        {
            // What model point was under the pivot before the zoom?
            PointF preZoomVirtualPoint = ToVirtualPoint(pivotClientPoint);

            _zoomFactor = newZoom;

            // What client point corresponds to the model point that used to be under the pivot?
            PointF postZoomClientPoint = ToFractionalClientPoint(preZoomVirtualPoint);

            // how many client units did the pivot move?
            float dx = postZoomClientPoint.X - pivotClientPoint.X;
            float dy = postZoomClientPoint.Y - pivotClientPoint.Y;

            // scroll exactly enough to cancel the canvas space motion from the zoom
            _xScroll += dx;
            _yScroll += dy;

            OnBoundsChanged();
        }

        public void ScrollTo(float x, float y)
        {
            _xScroll = x;
            _yScroll = y;
            OnBoundsChanged();
        }

        public void ScrollBy(float dx, float dy)
        {
            ScrollTo(_xScroll + dx, _yScroll + dy);
        }

        private void OnDrag(object sender, HandToolDragEventArgs e)
        {
            // Why negative? because the viewport moves in the opposite direction of the drag motion.
            ScrollBy(-e.Dx, -e.Dy);
        }

        private void OnBoundsChanged()
        {
            BoundsChanged.RaiseSafe(this, EventArgs.Empty);
        }



        #region Conversions

        public float ToVirtualDistance(float clientDistance)
        {
            return clientDistance / _zoomFactor;
        }

        public int ToClientDistance(float virtualDistance)
        {
            return (int)Math.Round(ToFractionalClientDistance(virtualDistance));
        }

        public float ToFractionalClientDistance(float virtualDistance)
        {
            return virtualDistance * _zoomFactor;
        }

        public PointF ToVirtualPoint(Point clientPoint)
        {
            PointF p = clientPoint;
            p.X += _xScroll;
            p.Y += _yScroll;
            p.X /= _zoomFactor;
            p.Y /= _zoomFactor;
            return p;
        }

        public PointF ToFractionalClientPoint(PointF virtualPoint)
        {
            PointF p = virtualPoint;
            p.X *= _zoomFactor;
            p.Y *= _zoomFactor;
            p.X -= _xScroll;
            p.Y -= _yScroll;
            return p;
        }

        public Point ToClientPoint(PointF virtualPoint)
        {
            return ToFractionalClientPoint(virtualPoint).Round();
        }

        #endregion
    }
}
