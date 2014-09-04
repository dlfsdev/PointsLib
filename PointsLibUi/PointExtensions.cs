using PointsLibInterop;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;

namespace PointsLibUi
{
    internal static class PointExtensions
    {
        public static IEnumerable<Point2d> SafeToPointsLibPoints(this IEnumerable<PointF> points)
        {
            if (points == null)
                return null;
            return points.Select(p => new Point2d(p.X, p.Y));
        }

        public static IEnumerable<PointF> SafeToDrawingPoints(this IEnumerable<Point2d> points)
        {
            if (points == null)
                return null;
            return points.Select(p => new PointF((float)p.X, (float)p.Y));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float DistanceTo(this PointF p1, PointF p2)
        {
            return (float)Math.Sqrt(p1.SquaredDistanceTo(p2));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float SquaredDistanceTo(this PointF p1, PointF p2)
        {
            float dx = p2.X - p1.X;
            float dy = p2.Y - p1.Y;
            return dx * dx + dy * dy;
        }
    }
}
