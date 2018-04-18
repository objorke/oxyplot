namespace DrawingDemo
{
    using System;
    using System.Collections.Generic;

    using OxyPlot;
    using OxyPlot.Drawing;

    public static class IntersectionExamples
    {
        [Example("Intersection of two circles")]
        public static Example IntersectionOfTwoCircles()
        {
            // http://paulbourke.net/geometry/circlesphere/
            var drawing = new DrawingModel();
            drawing.Add(new Ellipse { Center = new DataPoint(0, 0), RadiusX = 6, RadiusY = 6 });
            drawing.Add(new Ellipse { Center = new DataPoint(5, 1), RadiusX = 7, RadiusY = 7 });
            var p0 = new DataPoint(0, 0);
            var p1 = new DataPoint(5, 1);
            foreach (var i in GetCircleCircleIntersections(p0, p1, 6, 7))
            {
                drawing.AddPoint(i, OxyColors.Red);
            }

            return new Example(drawing);
        }

        [Example("Intersection of line and circle")]
        public static Example IntersectionOfLineAndCircles()
        {
            // http://paulbourke.net/geometry/circlesphere/
            var drawing = new DrawingModel();
            drawing.Add(new Ellipse { Center = new DataPoint(0, 0), RadiusX = 6, RadiusY = 6 });
            drawing.Add(new Lines(-8, 0, 12, 3));
            foreach (var i in GetRayCircleIntersections(new DataPoint(-8, 0), new DataPoint(12, 3), new DataPoint(0, 0), 6))
            {
                drawing.AddPoint(i, OxyColors.Red);
            }

            return new Example(drawing);
        }

        private static IEnumerable<DataPoint> GetCircleCircleIntersections(DataPoint p0, DataPoint p1, double r0, double r1)
        {
            var d = p0.DistanceTo(p1);
            var a = (r0 * r0 - r1 * r1 + d * d) / (2 * d);
            var h = Math.Sqrt(r0 * r0 - a * a);
            var p2 = new DataPoint(p0.X + a * (p1.X - p0.X) / d, p0.Y + a * (p1.Y - p0.Y) / d);
            yield return new DataPoint(p2.X + h * (p1.Y - p0.Y) / d, p2.Y - h * (p1.X - p0.X) / d);
            yield return new DataPoint(p2.X - h * (p1.Y - p0.Y) / d, p2.Y + h * (p1.X - p0.X) / d);
        }

        private static IEnumerable<DataPoint> GetRayCircleIntersections(DataPoint p1, DataPoint p2, DataPoint sc, double r)
        {
            double bb4ac;

            var dx = p2.X - p1.X;
            var dy = p2.Y - p1.Y;
            var a = dx * dx + dy * dy;
            var b = 2 * (dx * (p1.X - sc.X) + dy * (p1.Y - sc.Y));
            var c = sc.X * sc.X + sc.Y * sc.Y;
            c += p1.X * p1.X + p1.Y * p1.Y;
            c -= 2 * (sc.X * p1.X + sc.Y * p1.Y);
            c -= r * r;
            bb4ac = b * b - 4 * a * c;
            if (Math.Abs(a) < 1e-8 || bb4ac < 0)
            {
                yield break;
            }

            var mu1 = (-b + Math.Sqrt(bb4ac)) / (2 * a);
            var mu2 = (-b - Math.Sqrt(bb4ac)) / (2 * a);
            yield return new DataPoint(p1.X + mu1 * dx, p1.Y + mu1 * dy);
            yield return new DataPoint(p1.X + mu2 * dx, p1.Y + mu2 * dy);
        }
    }
}