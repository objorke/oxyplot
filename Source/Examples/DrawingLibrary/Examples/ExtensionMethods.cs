namespace DrawingDemo
{
    using System;

    using OxyPlot;
    using OxyPlot.Drawing;

    public static class ExtensionMethods
    {
        public static Ellipse AddPoint(this DrawingModel drawing, DataPoint p, OxyColor color, double alpha = 0.25, double radius = 1)
        {
            var ellipse = new Ellipse
            {
                Center = p,
                RadiusX = radius,
                RadiusY = radius,
                Stroke = color,
                Thickness = -2,
                Fill = OxyColor.FromAColor((byte)(alpha * 255), color)
            };
            drawing.Add(ellipse);
            return ellipse;
        }

        public static void AddCross(this DrawingModel drawing, double x, double y, OxyColor color, double size = 1, double thickness = 0.1)
        {
            drawing.Add(new Lines(
                new[]
                {
                    new DataPoint(x - size, y), new DataPoint(x + size, y), new DataPoint(x, y - size),
                    new DataPoint(x, y + size)
                }) { Color = color, Thickness = thickness });
        }

        public static void OnDragged(this Ellipse ellipse, Action changed)
        {
            var downPoint = ScreenPoint.Undefined;
            var dragging = false;
            double originalSize = 0;
            ellipse.MouseDown += (s, e) =>
            {
                originalSize = ellipse.RadiusX;
                ellipse.RadiusX = ellipse.RadiusY = originalSize * 1.2;
                changed();
                downPoint = e.Position;
                e.Handled = true;
            };
            ellipse.MouseUp += (s, e) =>
            {
                ellipse.RadiusX = ellipse.RadiusY = originalSize;
                changed();
                dragging = false;
                e.Handled = true;
            };
            ellipse.MouseMove += (s, e) =>
            {
                if (!dragging && (e.Position - downPoint).Length > 10)
                {
                    dragging = true;
                }

                if (dragging)
                {
                    var view = (IDrawingView)e.View;
                    var vm = view.ActualViewModel;
                    ellipse.Center = vm.InverseTransform(e.Position);
                    changed();
                }

                e.Handled = true;
            };
        }

    }
}