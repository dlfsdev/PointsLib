using PointsLibInterop;
using PointsLibUi.Extensions;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PointsLibUi
{
    internal sealed class AlgorithmRunner : IDisposable
    {
        private const int MAX_PROGRESS_REPORTS_PER_SECOND = 10;

        public delegate IEnumerable<Point2d> StartAlgorithmDelegate(
            IEnumerable<Point2d> points, IProgress<ProgressReport> progress, CancellationToken ct);

        private readonly StartAlgorithmDelegate _startAlgorithm;

        private CancellationTokenSource _cts;

        private TimeRemainingEstimator _timeEstimator = new TimeRemainingEstimator();

        private SemaphoreSlim _runLock = new SemaphoreSlim(1);

        private object _locker = new object();

        public AlgorithmRunner(StartAlgorithmDelegate startAlgorithm)
        {
            _startAlgorithm = startAlgorithm;
        }

        public event EventHandler<EventArgs> DataChanged;
        public event EventHandler<EventArgs> ProgressMade;

        private int _throttleMs = 0;
        public int ThrottleMs
        {
            get { lock (_locker) return _throttleMs; }
            set { lock (_locker) _throttleMs = Math.Max(value, 0); }
        }

        private IEnumerable<Point2d> _result = null;
        public IEnumerable<Point2d> Result
        {
            get { lock (_locker) return _result; }
        }

        private IEnumerable<Point2d> _provisionalResult = null;
        public IEnumerable<Point2d> ProvisionalResult
        {
            get { lock (_locker) return _provisionalResult; }
        }

        public double? ComputeEstimatedFractionComplete()
        {
            return _timeEstimator.ComputeEstimatedFractionComplete();
        }

        public TimeSpan? ComputeApproxTimeRemaining()
        {
            return _timeEstimator.ComputeApproxTimeRemaining();
        }

        public bool IsInProgress
        {
            get { return _runLock.CurrentCount == 0; }
        }

        public void ClearResults()
        {
            lock (_locker)
            {
                _result = null;
                _provisionalResult = null;
            }
        }

        public async Task RunAsync(IEnumerable<Point2d> points)
        {
            Cancel();

            await _runLock.WaitAsync().ConfigureAwait(false);

            try
            {
                ClearResults();

                _cts = new CancellationTokenSource();
                
                _timeEstimator.OnStarting();
                
                OnDataChanged();
                OnProgressMade();

                var t = Task.Run(() => _startAlgorithm(points, BuildProgressHandler(), _cts.Token));

                try
                {
                    await t.ConfigureAwait(false);
                }
                catch (OperationCanceledException) { }

                if (t.IsCanceled || t.IsFaulted || t.Result == null)
                    return;

                lock(_locker)
                    _result = t.Result;
            }
            finally
            {
                _runLock.Release();
                OnProgressMade();
                OnDataChanged();
            }
        }

        public void Cancel()
        {
            if (_cts == null)
                return;
            _cts.Cancel();
        }

        private IProgress<ProgressReport> BuildProgressHandler()
        {
            // will run for progress reports that pass the throttler
            var afterThrottleProgress = new PassthroughProgress<ProgressReport>(OnAfterThrottleProgress);

            // filters out progress reports when they arrive too fast
            var throttler = new ProgressThrottler<ProgressReport>(afterThrottleProgress,
                TimeSpan.FromMilliseconds(1000 / MAX_PROGRESS_REPORTS_PER_SECOND));
            throttler.MayThrottle = r => !r.Progress.HasValue || r.Progress.Value < 1.0;

            // will run for all progress reports
            return new PassthroughProgress<ProgressReport>(r => OnBeforeThrottleProgress(r, throttler));
        }

        // receives all reports from the algorithm
        private void OnBeforeThrottleProgress(ProgressReport report, IProgress<ProgressReport> nextProgressHandler)
        {
            int delay = ThrottleMs;
            if (delay > 0)
                Thread.Sleep(delay);
            nextProgressHandler.Report(report);
        }

        // receives only the reports that pass the throttler
        private void OnAfterThrottleProgress(ProgressReport report)
        {
            lock (_locker)
                _provisionalResult = report.ProvisionalAnswer;

            // It is important not to update the time estimator until after the throttle delay 
            // or it won't be able to correctly include this time in its estimate
            _timeEstimator.OnProgress(report.Progress);

            OnProgressMade();
        }

        private void OnDataChanged()
        {
            DataChanged.RaiseSafe(this, EventArgs.Empty);
        }

        private void OnProgressMade()
        {
            ProgressMade.RaiseSafe(this, EventArgs.Empty);
        }

        public void Dispose()
        {
            _runLock.Dispose();
        }
    }
}
