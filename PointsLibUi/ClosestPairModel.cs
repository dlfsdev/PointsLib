using PointsLibInterop;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;

namespace PointsLibUi
{
    internal sealed class ClosestPairModel : AlgorithmModel
    {
        public override bool CanTrySolve(IEnumerable<PointF> points)
        {
            return points.Count() >= 2;
        }

        public override bool ReportsDefiniteProgress
        {
            get { return ClosestPairSolverFactory.Create().ReportsDefiniteProgress; }
        }

        protected override IEnumerable<Point2d> Run(
            IEnumerable<Point2d> points,
            IProgress<ProgressReport> progress,
            CancellationToken ct)
        {
            return ClosestPairSolverFactory.Create().Solve(points, progress, ct);
        }
    }
}
