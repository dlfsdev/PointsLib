using PointsLibUi.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace PointsLibUi
{
    class PointsModel
    {
        private readonly HashSet<PointF> _points = new HashSet<PointF>();

        public event EventHandler PointsChanged;
        public event EventHandler SettingsChanged;

        public IEnumerable<PointF> Points
        {
            get { return _points; }
        }

        public void Add(PointF point)
        {
            if (!_points.Add(point))
                return;
            OnPointsChanged();
        }

        public void SetPoints(IEnumerable<PointF> points)
        {
            if (points.SequenceEqual(_points))
                return;

            _points.Clear();
            _points.UnionWith(points);

            OnPointsChanged();
        }

        public void Remove(PointF point, float radius)
        {
            if (_points.RemoveWhere(p => p.DistanceTo(point) <= radius) == 0)
                return;
            OnPointsChanged();
        }

        private bool _visible = true;
        public bool Visible
        {
            get { return _visible; }
            set { _visible = value; OnSettingsChanged(); }
        }

        private int _radius;
        public int Radius
        {
            get { return _radius; }
            set { _radius = value.Clamp(0, 5); OnSettingsChanged(); }
        }

        private Color _color;
        public Color Color
        {
            get { return _color; }
            set { _color = value; OnSettingsChanged(); }
        }

        private void OnPointsChanged()
        {
            PointsChanged.RaiseSafe(this, EventArgs.Empty);
        }

        private void OnSettingsChanged()
        {
            SettingsChanged.RaiseSafe(this, EventArgs.Empty);
        }
    }
}
