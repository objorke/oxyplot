namespace DrawingLibrary.Examples
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    using DrawingDemo;

    using OxyPlot;
    using OxyPlot.Drawing;

    using SvgLibrary;

    public static class SvgExamples
    {
        [Example("SVG Tiger")]
        public static Example Tiger()
        {
            var drawing = new DrawingModel();

            var assembly = typeof(SvgExamples).GetTypeInfo().Assembly;
            using (var stream = assembly.GetManifestResourceStream("DrawingLibrary.Examples.SvgExamples.Ghostscript_Tiger.svg"))
            {
                var svg = Svg.Load(stream);
                RenderSvg(svg, drawing);
            }

            return new Example(drawing);
        }

        [Example("SVG Europe")]
        public static Example Europe()
        {
            var drawing = new DrawingModel();

            var assembly = typeof(SvgExamples).GetTypeInfo().Assembly;
            using (var stream = assembly.GetManifestResourceStream("DrawingLibrary.Examples.SvgExamples.Blank_map_of_Europe.svg"))
            {
                var svg = Svg.Load(stream);
                RenderSvg(svg, drawing);
            }

            return new Example(drawing);
        }

        [Example("SVG Path")]
        public static Example Path()
        {
            var drawing = new DrawingModel();
            var content = "<svg xmlns=\"http://www.w3.org/2000/svg\"><g stroke=\"#000\" stroke-width=\"1\"><path d=\"M100,200 C100,100 250,100 250,200 S400,300 400,200\" /></g></svg>";
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(content)))
            {
                var svg = Svg.Load(stream);
                RenderSvg(svg, drawing);
            }

            return new Example(drawing);
        }

        private static void RenderSvg(SvgGroup group, DrawingModel drawing, SvgStyle groupStyle = null, SvgTransform groupTransform = null)
        {
            if (groupStyle == null)
            {
                groupStyle = new SvgStyle();
            }

            if (groupTransform == null)
            {
                groupTransform = new SvgTransform();
            }

            Func<DataPoint, DataPoint> ytransform = x => new DataPoint(x.X, -x.Y);

            foreach (var element in group.Elements)
            {
                var computedStyle = groupStyle.Append(element.Style);
                var totalTransform = groupTransform.Append(element.Transform);
                var innerGroup = element as SvgGroup;
                if (innerGroup != null)
                {
                    RenderSvg(innerGroup, drawing, computedStyle, totalTransform);
                }

                Func<DataPoint, DataPoint> transform = x =>
                {
                    double xx, yy;
                    totalTransform.Transform(x.X, x.Y, out xx, out yy);
                    return new DataPoint(xx, -yy);
                };

                var path = element as SvgPath;
                if (path != null)
                {
                    var figures = GetPaths(path);
                    foreach (var f in figures)
                    {
                        var closed = double.IsNaN(f.Last().X);
                        if (closed)
                        {
                            f.RemoveAt(f.Count - 1);
                            var p = new Polygon(f.Select(transform))
                            {
                                Stroke = OxyColor.Parse(computedStyle.Stroke),
                                Thickness = computedStyle.StrokeWidth,
                                Fill = OxyColor.Parse(computedStyle.Fill)
                            };
                            drawing.Add(p);
                        }
                        else
                        {
                            var p = new Polyline(f.Select(transform))
                            {
                                Color = OxyColor.Parse(computedStyle.Stroke),
                                Thickness = computedStyle.StrokeWidth,
                            };
                            drawing.Add(p);
                        }
                    }
                }
            }
        }

        private static List<List<DataPoint>> GetPaths(SvgPath path, int n = 8)
        {
            var figures = new List<List<DataPoint>>();
            List<DataPoint> figure = null;

            var currentCommand = '\0';
            var currentPoint = new DataPoint();
            var previousControlPoint = new DataPoint();

            Action<double, double, bool> moveTo = (x, y, relative) =>
            {
                // Debug.WriteLine("moveTo({0},{1})", x, y);
                currentPoint = relative ? new DataPoint(currentPoint.X + x, currentPoint.Y + y) : new DataPoint(x, y);
                previousControlPoint = currentPoint;
                figure = new List<DataPoint> { currentPoint };
                figures.Add(figure);
            };
            Action<double, double, bool> lineTo = (x, y, relative) =>
            {
                // Debug.WriteLine("lineTo({0},{1})", x, y);
                currentPoint = relative ? new DataPoint(currentPoint.X + x, currentPoint.Y + y) : new DataPoint(x, y);
                figure.Add(currentPoint);
            };
            Action<double, double, double, double, double, double, bool> curveTo = (x1, y1, x2, y2, x, y, relative) =>
            {
                // Debug.WriteLine("curveTo({0},{1},{2},{3},{4},{5})", x1, y1, x2, y2, x, y);
                var p0 = currentPoint;
                var p1 = relative ? new DataPoint(currentPoint.X + x1, currentPoint.Y + y1) : new DataPoint(x1, y1);
                var p2 = relative ? new DataPoint(currentPoint.X + x2, currentPoint.Y + y2) : new DataPoint(x2, y2);
                var p3 = relative ? new DataPoint(currentPoint.X + x, currentPoint.Y + y) : new DataPoint(x, y);
                for (int i = 1; i <= n; i++)
                {
                    var t = (double)i / n;
                    var cube = t * t * t;
                    var square = t * t;
                    var ax = 3 * (p1.X - p0.X);
                    var ay = 3 * (p1.Y - p0.Y);
                    var bx = (3 * (p2.X - p1.X)) - ax;
                    var by = (3 * (p2.Y - p1.Y)) - ay;
                    var cx = p3.X - p0.X - ax - bx;
                    var cy = p3.Y - p0.Y - ay - by;
                    var xt = (cx * cube) + (bx * square) + (ax * t) + p0.X;
                    var yt = (cy * cube) + (by * square) + (ay * t) + p0.Y;
                    figure.Add(new DataPoint(xt, yt));
                }

                currentPoint = p3;
                previousControlPoint = p2;
            };
            Action<double, double, double, double, bool> smoothCurveTo = (x2, y2, x, y, relative) =>
            {
                // Debug.WriteLine("smoothCurveTo({0},{1},{2},{3})", x2, y2, x, y);

                // absolute coordinates
                var dx = currentPoint.X - previousControlPoint.X;
                var dy = currentPoint.Y - previousControlPoint.Y;
                var x1 = relative ? dx : currentPoint.X + dx;
                var y1 = relative ? dy : currentPoint.Y + dy;
                curveTo(x1, y1, x2, y2, x, y, relative);
            };

            Action close = () =>
            {
                figure.Add(new DataPoint(double.NaN, double.NaN));
                figure = null;
            };

            // Debug.WriteLine(path.PathData);
            var queue = new Queue<object>(PathLexer(path.PathData));
            Func<double> next = () => (double)queue.Dequeue();
            while (queue.Count > 0)
            {
                if (queue.Peek() is char)
                {
                    currentCommand = (char)queue.Dequeue();
                    // Debug.WriteLine(currentCommand);
                    continue;
                }

                bool relative = char.IsLower(currentCommand);
                double x, y, x1, y1, x2, y2;
                switch (char.ToLower(currentCommand))
                {
                    case 'm':
                        x = next();
                        y = next();
                        moveTo(x, y, relative);
                        currentCommand = relative ? 'l' : 'L';
                        break;

                    case 'l':
                        x = next();
                        y = next();
                        lineTo(x, y, relative);
                        break;

                    case 'h':
                        x = next();
                        lineTo(x, 0, relative);
                        break;

                    case 'v':
                        y = next();
                        lineTo(0, y, relative);
                        break;

                    case 'z':
                        close();
                        break;

                    case 's':
                        x2 = next();
                        y2 = next();
                        x = next();
                        y = next();
                        smoothCurveTo(x2, y2, x, y, relative);
                        break;

                    case 'c':
                        x1 = next();
                        y1 = next();
                        x2 = next();
                        y2 = next();
                        x = next();
                        y = next();
                        curveTo(x1, y1, x2, y2, x, y, relative);
                        break;
                    case 'a':
                        var rx = next();
                        var ry = next();
                        var xrot = next();
                        var largeArcFlag = next();
                        var sweepFlag = next();
                        x = next();
                        y = next();
                        lineTo(x, y, relative);
                        // arcTo(x1, y1, x2, y2, x, y, relative);
                        break;
                    default:
                        throw new NotImplementedException("The command " + currentCommand + " is not yet supported.");
                }
            }

            if (char.ToLower(currentCommand) == 'z')
            {
                close();
            }

            return figures;
        }

        private static IEnumerable<object> PathLexer(string path)
        {
            string number = null;

            foreach (char c in path)
            {
                if (number != null)
                {
                    if (char.IsNumber(c) || c == '.' || c == 'E' || c == 'e' || (c == '-' && number[number.Length - 1] == 'e'))
                    {
                        number += c;
                        continue;
                    }

                    yield return double.Parse(number, CultureInfo.InvariantCulture);
                    number = null;
                }

                if (char.IsNumber(c) || c == '.' || c == '-')
                {
                    number = c.ToString();
                    continue;
                }

                if (c == ' ' || c == ',')
                {
                    continue;
                }

                yield return c;
            }

            if (number != null)
            {
                yield return double.Parse(number, CultureInfo.InvariantCulture);
            }
        }
    }
}