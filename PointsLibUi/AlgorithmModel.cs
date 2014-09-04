using PointsLibInterop;
using PointsLibUi.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;

namespace PointsLibUi
{
    // Helps us avoid adding dozens of similar properties to MainFormModel without making Demeter too mad
    internal abstract class AlgorithmModel : IDisposable
    {
        private readonly AlgorithmRunner _runner;

        public event EventHandler SettingsChanged;
        public event EventHandler ProgressMade;

        public AlgorithmModel()
        {
            _runner = new AlgorithmRunner(Run);

            _runner.DataChanged += (s, e) => OnSettingsChanged();
            _runner.ProgressMade += (s, e) => OnProgressMade();
        }

        private bool _visible = true;
        public bool Visible
        {
            get { return _visible; }
            set { _visible = value; OnSettingsChanged(); }
        }

        private Color _color;
        public Color ResultColor
        {
            get { return _color; }
            set { _color = value; OnSettingsChanged(); }
        }

        public int ThrottleMs
        {
            get { return _runner.ThrottleMs; }
            set { _runner.ThrottleMs = value; OnSettingsChanged(); }
        }

        public abstract bool CanTrySolve(IEnumerable<PointF> points);

        public async Task SolveAsync(IEnumerable<PointF> points)
        {
            await _runner.RunAsync(points.SafeToPointsLibPoints());
        }

        public IEnumerable<PointF> Result
        {
            get { return _runner.Result.SafeToDrawingPoints(); }
        }

        public IEnumerable<PointF> ProvisionalResult
        {
            get { return _runner.ProvisionalResult.SafeToDrawingPoints(); }
        }

        public abstract bool ReportsDefiniteProgress
        {
            get;
        }

        public double? GetFractionComplete()
        {
            return _runner.ComputeEstimatedFractionComplete();
        }

        public TimeSpan? GetApproxTimeRemaining()
        {
            return _runner.ComputeApproxTimeRemaining();
        }

        public bool IsInProgress
        {
            get { return _runner.IsInProgress; }
        }

        public void ClearResults()
        {
            _runner.ClearResults();
        }

        public void Cancel()
        {
            _runner.Cancel();
        }

        protected abstract IEnumerable<Point2d> Run(
            IEnumerable<Point2d> points,
            IProgress<ProgressReport> progress,
            CancellationToken ct);

        protected void OnProgressMade()
        {
            ProgressMade.RaiseSafe(this, EventArgs.Empty);
        }

        protected void OnSettingsChanged()
        {
            SettingsChanged.RaiseSafe(this, EventArgs.Empty);
        }

        public void Dispose()
        {
            _runner.Dispose();
        }
    }
}
