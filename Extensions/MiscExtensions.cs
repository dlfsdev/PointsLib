using System;

namespace PointsLibUi.Extensions
{
    public static class MiscExtensions
    {
        /// <summary>
        /// Clamp an arbitrary <see cref="System.IComparable"/> between some <paramref name="min"/> and <paramref name="max"/>.
        /// </summary>
        public static T Clamp<T>(this T val, T min, T max) where T : IComparable<T>
        {
            if (val.CompareTo(min) < 0)
                return min;

            if (val.CompareTo(max) > 0)
                return max;

            return val;
        }

        // This is not a true extension method in order to avoid having it show up in intellisense for everything
        public static void CopySharedPublicProperties(object l, object r)
        {
            foreach (var lInfo in l.GetType().GetProperties())
            {
                if (lInfo.GetGetMethod() == null)
                    continue;
                var rInfo = r.GetType().GetProperty(lInfo.Name, lInfo.PropertyType);
                if (rInfo == null)
                    continue;
                if (rInfo.GetSetMethod() == null)
                    continue;
                object lValue = lInfo.GetValue(l);
                rInfo.SetValue(r, lValue);
            }
        }
    }
}
