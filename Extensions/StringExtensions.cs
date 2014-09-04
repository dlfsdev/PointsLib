using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PointsLibUi.Extensions
{
    public static class StringExtensions
    {
        public static bool EqualsOrdinal(this String s1, String s2)
        {
            return s1.Equals(s2, StringComparison.Ordinal);
        }
    }
}
