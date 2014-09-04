using System;

namespace PointsLibUi
{
    /// <summary>
    /// A <see cref="System.IProgress" decorator that throttles the rate of progress notifications by dropping some/>
    /// </summary>
    internal sealed class ProgressThrottler<T> : IProgress<T>
    {
        readonly object _locker = new object();

        readonly IProgress<T> _decorated;
        readonly TimeSpan _minIntervalBetweenUpdates;
        
        /// <param name="decorated">The <see cref="System.IProgress"/> to decorate</param>
        /// <param name="minIntervalBetweenUpdates">The desired minimum amount of time between progress updates.
        /// Reports may still arrive more frequently than this if <see cref="MayThrottle"/> returns false.</param>
        public ProgressThrottler(IProgress<T> decorated, TimeSpan minIntervalBetweenUpdates)
        {
            _decorated = decorated;
            _minIntervalBetweenUpdates = minIntervalBetweenUpdates;
            MayThrottle = _ => true;
        }

        /// <summary>
        /// Some reports are too important to throttle. Return false to force an individual report to be delivered even if
        /// this means violating the requested minimum interval between updates.
        /// Default function returns true for everything.
        /// </summary>
        public Func<T, bool> MayThrottle { get; set; }

        public void Report(T value)
        {
            if (ShouldThrottleReport(value))
                return;
            LastReportTime = DateTime.Now;
            _decorated.Report(value);
        }

        DateTime? _lastReportTime;
        private DateTime? LastReportTime
        {
            get { lock (_locker) return _lastReportTime; }
            set { lock (_locker) _lastReportTime = value; }
        }

        private bool ShouldThrottleReport(T value)
        {
            if (!MayThrottle(value))
                return false;

            DateTime? lastReport = LastReportTime;
            if (!lastReport.HasValue)
                return false;

            if (DateTime.Now - lastReport.Value >= _minIntervalBetweenUpdates)
                return false;

            return true;
        }
    }
}
