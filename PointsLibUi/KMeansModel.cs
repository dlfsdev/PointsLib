using PointsLibInterop;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;

namespace PointsLibUi
{
    internal sealed class KMeansModel : AlgorithmModel
    {
        int _k;
        public int K
        {
            get { return _k; }
            set { _k = Math.Max(value, 1); }
        }

        int _repetitions;
        public int Repetitions
        {
            get { return _repetitions; }
            set { _repetitions = Math.Max(value, 1); }
        }

        private KMeansInitialMeansStrategy _initialMeansStrategy = KMeansInitialMeansStrategy.Forgy;
        public KMeansInitialMeansStrategy InitialMeansStrategy
        {
            get { return _initialMeansStrategy; }
            set { _initialMeansStrategy = value; }
        }

        public override bool CanTrySolve(IEnumerable<PointF> points)
        {
            return points.Count() >= K;
        }

        public override bool ReportsDefiniteProgress
        {
            get { return KMeansSolverFactory.Create(K, Repetitions, InitialMeansStrategy).ReportsDefiniteProgress; }
        }

        protected override IEnumerable<Point2d> Run(
            IEnumerable<Point2d> points,
            IProgress<ProgressReport> progress,
            CancellationToken cancellationToken)
        {
            return KMeansSolverFactory.Create(K, Repetitions, InitialMeansStrategy).Solve(
                points, progress, cancellationToken);
        }
    }
}
