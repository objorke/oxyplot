namespace DrawingDemo
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using OxyPlot;
    using OxyPlot.Drawing;

    public static class BezierExamples
    {
        [Example("Bézier curve (by De Casteljau's algorithm)")]
        public static Example BézierDeCasteljau()
        {
            // https://en.wikipedia.org/wiki/De_Casteljau's_algorithm
            // http://pomax.github.io/bezierinfo/
            Func<IList<DataPoint>, double, DataPoint> bezier = (points, t) =>
            {
                while (points.Count() > 1)
                {
                    var newPoints = new DataPoint[points.Count - 1];
                    for (int i = 0; i < newPoints.Length; i++)
                    {
                        newPoints[i] = new DataPoint(((1 - t) * points[i].X) + (t * points[i + 1].X), ((1 - t) * points[i].Y) + (t * points[i + 1].Y));
                    }

                    points = newPoints;
                }

                return points[0];
            };

            var controlPoints = new[] { new DataPoint(65, 25), new DataPoint(5, 150), new DataPoint(80, 290), new DataPoint(220, 235), new DataPoint(250, 150), new DataPoint(135, 125) };
            return CreateBezierExample(controlPoints, bezier);
        }

        [Example("Bézier curve (3 control points)")]
        public static Example Bézier3()
        {
            // http://paulbourke.net/geometry/bezier/index.html
            Func<IList<DataPoint>, double, DataPoint> bezier3 = (p, mu) =>
            {
                var mu2 = mu * mu;
                var mum1 = 1 - mu;
                var mum12 = mum1 * mum1;

                return new DataPoint(p[0].X * mum12 + 2 * p[1].X * mum1 * mu + p[2].X * mu2, p[0].Y * mum12 + 2 * p[1].Y * mum1 * mu + p[2].Y * mu2);
            };

            var controlPoints = new[] { new DataPoint(65, 25), new DataPoint(5, 150), new DataPoint(80, 290) };
            return CreateBezierExample(controlPoints, bezier3);
        }

        [Example("Bézier curve (4 control points)")]
        public static Example Bézier4()
        {
            // http://paulbourke.net/geometry/bezier/index.html
            Func<IList<DataPoint>, double, DataPoint> bezier4 = (p, mu) =>
            {
                var mum1 = 1 - mu;
                var mum13 = mum1 * mum1 * mum1;
                var mu3 = mu * mu * mu;
                var p1 = p[0];
                var p2 = p[1];
                var p3 = p[2];
                var p4 = p[3];
                var px = mum13 * p1.X + 3 * mu * mum1 * mum1 * p2.X + 3 * mu * mu * mum1 * p3.X + mu3 * p4.X;
                var py = mum13 * p1.Y + 3 * mu * mum1 * mum1 * p2.Y + 3 * mu * mu * mum1 * p3.Y + mu3 * p4.Y;

                return new DataPoint(px, py);
            };

            var controlPoints = new[] { new DataPoint(65, 25), new DataPoint(5, 150), new DataPoint(80, 290), new DataPoint(220, 235) };
            return CreateBezierExample(controlPoints, bezier4);
        }

        [Example("Bézier curve (cubic)")]
        public static Example CubicBézier()
        {
            // http://paulbourke.net/geometry/bezier/index.html
            Func<IList<DataPoint>, double, DataPoint> cubicBezier = (p, mu) =>
            {
                // Piecewise cubic bezier curve as defined by Adobe in Postscript
                // The two end points are p0 and p3
                // Their associated control points are p1 and p2
                var p0 = p[0];
                var p1 = p[1];
                var p2 = p[2];
                var p3 = p[3];

                var cx = 3 * (p1.X - p0.X);
                var cy = 3 * (p1.Y - p0.Y);
                var bx = 3 * (p2.X - p1.X) - cx;
                var by = 3 * (p2.Y - p1.Y) - cy;
                var ax = p3.X - p0.X - cx - bx;
                var ay = p3.Y - p0.Y - cy - by;

                var px = ax * mu * mu * mu + bx * mu * mu + cx * mu + p0.X;
                var py = ay * mu * mu * mu + by * mu * mu + cy * mu + p0.Y;

                return new DataPoint(px, py);
            };

            var controlPoints = new[] { new DataPoint(65, 25), new DataPoint(5, 150), new DataPoint(80, 290), new DataPoint(220, 235) };
            return CreateBezierExample(controlPoints, cubicBezier);
        }

        [Example("Bézier curve (general)")]
        public static Example GeneralBézier()
        {
            // http://paulbourke.net/geometry/bezier/index.html
            Func<IList<DataPoint>, double, DataPoint> solveBezier = (p, mu) =>
            {
                if (1 - mu < 1e-8)
                {
                    return p.Last();
                }

                int k, kn, nn, nkn;
                double blend, muk, munk;
                double bx = 0, by = 0;
                int n = p.Count - 1;

                muk = 1;
                munk = Math.Pow(1 - mu, n);

                for (k = 0; k <= n; k++)
                {
                    nn = n;
                    kn = k;
                    nkn = n - k;
                    blend = muk * munk;
                    muk *= mu;
                    munk /= (1 - mu);
                    while (nn >= 1)
                    {
                        blend *= nn;
                        nn--;
                        if (kn > 1)
                        {
                            blend /= kn;
                            kn--;
                        }

                        if (nkn > 1)
                        {
                            blend /= nkn;
                            nkn--;
                        }
                    }

                    bx += p[k].X * blend;
                    by += p[k].Y * blend;
                }

                return new DataPoint(bx, by);
            };

            var controlPoints = new[] { new DataPoint(65, 25), new DataPoint(5, 150), new DataPoint(80, 290) }; //, new DataPoint(220, 235), new DataPoint(250, 150), new DataPoint(135, 125) };

            return CreateBezierExample(controlPoints, solveBezier);
        }

        private static Example CreateBezierExample(DataPoint[] initialPoints, Func<IList<DataPoint>, double, DataPoint> solveBezier)
        {
            var drawing = new DrawingModel();
            var bezierLine = new Polyline { Thickness = 2, Color = OxyColors.Green };
            var controlPointsLine = new Polyline { Thickness = 2, Color = OxyColors.Red };
            drawing.Add(bezierLine);
            drawing.Add(controlPointsLine);
            var controlPoints = new List<Ellipse>();
            var evaluatedPoints = new List<Ellipse>();

            Action update = () =>
            {
                foreach (var e in evaluatedPoints)
                {
                    drawing.Remove(e);
                }

                controlPointsLine.Points.Clear();
                controlPointsLine.Points.AddRange(controlPoints.Select(c => c.Center));

                var bezierPoints = CreateCurve(controlPointsLine.Points, 100, solveBezier);
                bezierLine.Points.Clear();
                bezierLine.Points.AddRange(bezierPoints);
                
                foreach (var p in bezierPoints)
                {
                    evaluatedPoints.Add(drawing.AddPoint(p, OxyColors.White, 1, 0.8));
                }

                drawing.Invalidate();
            };

            foreach (var p in initialPoints)
            {
                var cp = drawing.AddPoint(p, OxyColors.Blue, radius: 3);
                cp.OnDragged(update);
                controlPoints.Add(cp);
            }

            update();

            return new Example(drawing);
        }

        private static DataPoint[] CreateCurve(IList<DataPoint> controlPoints, int n, Func<IList<DataPoint>, double, DataPoint> solver)
        {
            var points = new DataPoint[n];
            for (int i = 0; i < n; i++)
            {
                double t = (double)i / (n - 1);
                points[i] = solver(controlPoints, t);
            }

            return points;
        }
    }
}