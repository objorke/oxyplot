namespace DrawingDemo
{
    using System;

    using OxyPlot;
    using OxyPlot.Drawing;

    public static class CircleExamples
    {
        [Example("Circle from three points")]
        public static Example CircleFromThreePoints()
        {
            var drawing = new DrawingModel();

            var p1 = new DataPoint(0, 0);
            var p2 = new DataPoint(100, 20);
            var p3 = new DataPoint(50, 50);

            var circle = new Ellipse { Fill = OxyColors.LightGray, Stroke = OxyColors.LightBlue, Thickness = -2 };
            drawing.Add(circle);
            
            var h1 = drawing.AddPoint(p1, OxyColors.LightBlue);
            var h2 = drawing.AddPoint(p2, OxyColors.LightBlue);
            var h3 = drawing.AddPoint(p3, OxyColors.LightBlue);

            Action updateCircle = () =>
            {
                DataPoint c;
                double r;
                FindCircle(h1.Center, h2.Center, h3.Center, out c, out r);
                circle.Center = c;
                circle.RadiusX = circle.RadiusY = r;
                drawing.Invalidate();
            };

            updateCircle();
            h1.OnDragged(updateCircle);
            h2.OnDragged(updateCircle);
            h2.OnDragged(updateCircle);
            return new Example(drawing);
        }

        public static void FindCircle(DataPoint a, DataPoint b, DataPoint c, out DataPoint cc, out double r)
        {
            // Get the perpendicular bisector of (x1, y1) and (x2, y2).
            var x1 = (b.X + a.X) / 2;
            var y1 = (b.Y + a.Y) / 2;
            var dy1 = b.X - a.X;
            var dx1 = -(b.Y - a.Y);

            // Get the perpendicular bisector of (x2, y2) and (x3, y3).
            var x2 = (c.X + b.X) / 2;
            var y2 = (c.Y + b.Y) / 2;
            var dy2 = c.X - b.X;
            var dx2 = -(c.Y - b.Y);

            // See where the lines intersect.
            var cx = ((y1 * dx1 * dx2) + (x2 * dx1 * dy2) - (x1 * dy1 * dx2) - (y2 * dx1 * dx2)) / ((dx1 * dy2) - (dy1 * dx2));
            var cy = ((cx - x1) * dy1 / dx1) + y1;

            var dx = cx - a.X;
            var dy = cy - a.Y;
            cc = new DataPoint(cx, cy);
            r = Math.Sqrt((dx * dx) + (dy * dy));
        }
    }
}