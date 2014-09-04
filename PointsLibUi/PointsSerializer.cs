using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace PointsLibUi
{
    internal static class PointsSerializer
    {
        public static IEnumerable<PointF> Parse(String pointsText)
        {
            var points = new List<PointF>();

            foreach(String fullLine in pointsText.Split(Environment.NewLine.ToCharArray()))
            {
                var line = fullLine.Trim();
                if (line.StartsWith("//") || line.StartsWith("#"))
                    continue;

                line = line.Replace("(", "").Replace(")", "");

                string[] pieces = line.Split(',');
                if (pieces.Length != 2)
                    continue;

                float x, y;
                if (!float.TryParse(pieces[0], out x))
                    continue;
                if (!float.TryParse(pieces[1], out y))
                    continue;

                points.Add(new PointF(x, y));
            }

            return points;
        }

        public static String ToString(PointF point)
        {
            var sb = new StringBuilder(APPROX_CHARS_PER_POINT);
            return sb.Append(point.X.ToString()).Append(',').Append(point.Y.ToString()).ToString();
        }

        public static String ToString(IEnumerable<PointF> points)
        {
            var pointsList = points.ToList();
            var sb = new StringBuilder(APPROX_CHARS_PER_POINT * pointsList.Count);
            foreach (PointF p in pointsList)
                sb.Append(ToString(p)).Append(Environment.NewLine);
            return sb.ToString();
        }

        public static IEnumerable<PointF> SerializeIn(TextReader reader)
        {
            var points = new List<PointF>();

            string line;
            while ((line = reader.ReadLine()) != null)
                points.AddRange(Parse(line));

            return points;
        }

        public static void SerializeOut(IEnumerable<PointF> points, TextWriter writer)
        {
            foreach (PointF p in points)
                writer.WriteLine(ToString(p));
        }

        private const int APPROX_CHARS_PER_POINT = 25;
    }
}
