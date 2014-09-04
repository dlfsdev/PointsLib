using System;

namespace PointsLibUi
{
    /// <summary>
    ///  A <see cref="System.IProgress"/> implementation that does not marshall to the original synchronization context
    /// </summary>
    internal sealed class PassthroughProgress<T> : IProgress<T>
    {
        Action<T> _f;

        public PassthroughProgress(Action<T> f)
        {
            _f = f;
        }

        void IProgress<T>.Report(T value)
        {
            _f(value);
        }
    }
}
