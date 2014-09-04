using System;
using System.Runtime.CompilerServices;

namespace PointsLibUi.Extensions
{
    public static class EventHandlerExtensions
    {
        /// <summary>
        /// Raise a <see cref="System.EventHandler"/> without the need to copy it or check for null first.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void RaiseSafe(this EventHandler theEvent, object sender, EventArgs args)
        {
            if (theEvent == null)
                return;
            theEvent(sender, args);
        }

        /// <summary>
        /// Raise a <see cref="System.EventHandler"/> without the need to copy it or check for null first.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void RaiseSafe<T>(this EventHandler<T> theEvent, object sender, T args)
        {
            if (theEvent == null)
                return;
            theEvent(sender, args);
        }
    }
}
