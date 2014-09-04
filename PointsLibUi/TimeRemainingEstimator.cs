using System;

namespace PointsLibUi
{
    /// <summary>
    /// Estimates how much longer it will take to complete a task based on periodic progress
    /// updates provided to it.
    /// </summary>
    internal sealed class TimeRemainingEstimator
    {
        private object _locker = new object();
        
        private DateTime? _startTime;

        private double _lastReportedFractionComplete = 0.0;
        private DateTime? _lastReportedFractionCompleteTime;
                                
        /// <summary>
        /// Call when the task begins
        /// </summary>
        public void OnStarting()
        {
            lock (_locker)
            {
                _startTime = DateTime.Now;
                _lastReportedFractionComplete = 0.0;
                _lastReportedFractionCompleteTime = null;
            }
        }

        /// <summary>
        /// Call when the task provides progress updates
        /// </summary>
        /// <param name="fractionComplete">The fraction of the task that has been completed</param>
        public void OnProgress(double? fractionComplete)
        {
            if (fractionComplete.HasValue)
            {
                lock (_locker)
                {
                    _lastReportedFractionComplete = fractionComplete.Value;
                    _lastReportedFractionCompleteTime = DateTime.Now;
                }
            }
        }

        /// <summary>
        /// Get an estimate of how much longer it will take to complete the task
        /// </summary>
        public TimeSpan? ComputeApproxTimeRemaining()
        {
            double fractionComplete = ComputeEstimatedFractionComplete();
            if (fractionComplete == 0.0)
                return null;

            if (fractionComplete >= 1.0)
                return new TimeSpan(0);

            long elapsedTicks = DateTime.Now.Ticks - _startTime.Value.Ticks;
            long estimatedTotalTicks = (long)Math.Round(elapsedTicks / fractionComplete);
            return new TimeSpan(estimatedTotalTicks - elapsedTicks);
        }

        /// <summary>
        /// Estimate the fraction complete for the task based on its history of progress reports
        /// </summary>
        public double ComputeEstimatedFractionComplete()
        {
            if (!_startTime.HasValue || !_lastReportedFractionCompleteTime.HasValue || _lastReportedFractionComplete == 0.0)
                return 0.0;

            long elapsedTicksAtLastProgress = _lastReportedFractionCompleteTime.Value.Ticks - _startTime.Value.Ticks;
            double progressPerTick = _lastReportedFractionComplete / elapsedTicksAtLastProgress;
            long totalElapsedTicks = DateTime.Now.Ticks - _startTime.Value.Ticks;
            return totalElapsedTicks * progressPerTick;
        }
    }
}
