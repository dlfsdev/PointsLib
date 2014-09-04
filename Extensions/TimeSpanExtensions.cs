using System;

namespace PointsLibUi.Extensions
{
    public static class TimeSpanExtensions
    {
        /// <summary>
        /// Produces a user-friendly string describing a timespan
        /// </summary>
        public static string ToPrettyString(this TimeSpan t)
        {
            string s = "";

            if (t.Days > 0)
                s += string.Format(" {0}d", t.Days.ToString());
            if (s.Length > 0 || t.Hours > 0)
                s += string.Format(" {0}h", t.Hours.ToString());
            if (t.Days == 0 && (s.Length > 0 || t.Minutes > 0))
                s += string.Format(" {0}m", t.Minutes.ToString());
            if (t.Hours == 0 && t.Days == 0)
                s += string.Format(" {0}s", t.Seconds.ToString(s.Length == 0 ? "D" : "D2"));
            
            return s.TrimStart();
        }
    }
}
