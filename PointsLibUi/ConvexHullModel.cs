using PointsLibInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace PointsLibUi
{
    internal sealed class ConvexHullModel : AlgorithmModel
    {
        public override bool CanTrySolve(IEnumerable<System.Drawing.PointF> points)
        {
            return points.Any();
        }

        public override bool ReportsDefiniteProgress
        {
            get { return ConvexHullSolverFactory.Create().ReportsDefiniteProgress; }
        }

        protected override IEnumerable<Point2d> Run(
            IEnumerable<Point2d> points,
            IProgress<ProgressReport> progress,
            CancellationToken cancellationToken)
        {
            return ConvexHullSolverFactory.Create().Solve(points, progress, cancellationToken);
        }
    }
}
