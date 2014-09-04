using PointsLibInterop;
using PointsLibUi.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;

namespace PointsLibUi
{
    internal sealed class TspModel : AlgorithmModel
    {
        private int _numThreads;
        public int NumThreads
        {
            get { return _numThreads; }
            set { _numThreads = value.Clamp(1, Environment.ProcessorCount); }
        }

        public override bool ReportsDefiniteProgress
        {
            get { return TspSolverFactory.Create(NumThreads).ReportsDefiniteProgress; }
        }

        public override bool CanTrySolve(IEnumerable<PointF> points)
        {
            int count = points.Count();
            return count > 0 && count <= 20;
        }

        protected override IEnumerable<Point2d> Run(
            IEnumerable<Point2d> points,
            IProgress<ProgressReport> progress,
            CancellationToken cancellationToken)
        {
            return TspSolverFactory.Create(NumThreads).Solve(points, progress, cancellationToken);
        }
    }
}
