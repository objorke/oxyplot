namespace DrawingDemo
{
    using System;

    using OxyPlot;
    using OxyPlot.Drawing;

    public static class MiscExamples
    {
        [Example("Planets")]
        public static Example Planets()
        {
            var drawing = new DrawingModel();
            var au = 1.496e11;
            Action<string, double, double, OxyColor> add =
                (name, distance, radius, color) =>
                    drawing.Add(
                        new Ellipse
                        {
                            RadiusX = radius / au,
                            RadiusY = radius / au,
                            Center = new DataPoint(distance / au, 0),
                            Fill = color,
                            Text = name
                        });
            add("Sun", 0, 696342e3, OxyColors.Yellow);
            add("Mercury", 0.4 * au, 2439.7e3, OxyColors.Maroon);
            add("Venus", 0.7 * au, 6051.8e3, OxyColors.Violet);
            add("Earth", 1 * au, 6371e3, OxyColors.AliceBlue);
            add("Mars", 1.5 * au, 3389.5e3, OxyColors.Magenta);
            add("Jupiter", 5.2 * au, 69911e3, OxyColors.Peru);
            add("Saturn", 9.5 * au, 58232e3, OxyColors.Salmon);
            add("Uranus", 19.2 * au, 25362e3, OxyColors.OrangeRed);
            add("Neptune", 30 * au, 24622e3, OxyColors.Blue);
            add("Pluto", 39 * au, 1184e3, OxyColors.Black);

            return new Example(drawing);
        }

        [Example("Grid")]
        public static Example Grid()
        {
            var drawing = new DrawingModel();
            drawing.Add(new Grid());
            for (int i = 50; i >= 10; i -= 5)
            {
                drawing.Add(new Ellipse { Center = new DataPoint(i, 0), RadiusX = i, RadiusY = i, Fill = OxyColor.FromAColor(40, OxyColors.Gray) });
            }

            return new Example(drawing);
        }

        [Example("Optical illusion")]
        public static Example OpticalIllusion()
        {
            var drawing = new DrawingModel();
            drawing.Background = OxyColors.Black;
            int n = 10;

            for (int i = 0; i + 1 < n; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    double x = 1.5 + Math.Sin(i * Math.PI * 2 / (n - 1)) * 1 + j * 3;
                    drawing.Add(
                        new Rectangle
                        {
                            MinimumX = x,
                            MinimumY = i,
                            MaximumX = x + 1,
                            MaximumY = i + 1,
                            Fill = OxyColors.White
                        });
                }
            }

            for (int i = 0; i < n; i++)
            {
                drawing.Add(new Lines(0, i, 16, i) { Color = OxyColors.Gray, Thickness = -2 });
            }

            return new Example(drawing);
        }

        [Example("Arrows")]
        public static Example Arrows()
        {
            var drawing = new DrawingModel();
            drawing.Add(new Arrow { StartPoint = new DataPoint(0, 0), EndPoint = new DataPoint(40, 10) });
            drawing.Add(new Arrow { StartPoint = new DataPoint(0, 10), EndPoint = new DataPoint(40, 20), Veeness = 0 });
            drawing.Add(new Arrow { StartPoint = new DataPoint(0, 20), EndPoint = new DataPoint(40, 30), Veeness = -1 });
            return new Example(drawing);
        }

        [Example("Snurr")]
        public static Example Snurr()
        {
            var p = new DataPoint[5];
            p[0] = new DataPoint(0, 0);
            p[1] = new DataPoint(1, 0);
            p[2] = new DataPoint(1, 1);
            p[3] = new DataPoint(0, 1);
            p[4] = p[0];
            var drawing = new DrawingModel { Background = OxyColors.Black };
            Func<DataPoint, DataPoint, double, DataPoint> lerp = (p1, p2, f) => new DataPoint(p1.X * (1 - f) + p2.X * f, p1.Y * (1 - f) + p2.Y * f);
            for (int i = 0; i < 255; i++)
            {
                var c = OxyColor.FromHsv(Math.Abs(Math.Sin(i * 0.03)), 1, 1);
                drawing.Add(new Polyline(p) { Color = c, Thickness = -1.8 });
                p[1] = lerp(p[1], p[2], 0.02);
                p[2] = lerp(p[2], p[3], 0.02);
                p[3] = lerp(p[3], p[0], 0.02);
                p[0] = lerp(p[0], p[1], 0.02);
                p[4] = p[0];
            }

            return new Example(drawing);
        }

        [Example("Blueprint")]
        public static Example Blueprint()
        {
            var drawing = new DrawingModel { Background = OxyColor.FromRgb(0, 128, 196) };
            drawing.Add(
                new RoundedRectangle
                {
                    MinimumX = 0,
                    MinimumY = 0,
                    MaximumX = 300,
                    MaximumY = 200,
                    CornerRadius = 3,
                    Stroke = OxyColors.White,
                    Fill = OxyColor.FromAColor(20, OxyColors.White)
                });

            for (int i = 1; i < 30; i++)
            {
                drawing.Add(new Lines(i * 10, 30, i * 10, 200) { Color = OxyColor.FromAColor(50, OxyColors.White), Thickness = 0.1 });
            }

            for (int i = 4; i < 20; i++)
            {
                drawing.Add(new Lines(0, i * 10, 300, i * 10) { Color = OxyColor.FromAColor(50, OxyColors.White), Thickness = 0.1 });
            }

            drawing.Add(new Lines(0, 30, 300, 30) { Color = OxyColors.White, Aliased = true });
            drawing.Add(new Lines(200, 0, 200, 30) { Color = OxyColors.White, Aliased = true });
            drawing.Add(new Lines(200, 15, 300, 15) { Color = OxyColors.White, Aliased = true });
            drawing.Add(new Text { Point = new DataPoint(5, 195), Content = "TOP VIEW", Color = OxyColors.White });
            drawing.Add(
                new Text
                {
                    Point = new DataPoint(5, 20),
                    Content = "OxyPlot drawing model",
                    Color = OxyColors.White,
                    FontSize = 10,
                    FontWeight = FontWeights.Bold
                });
            drawing.Add(
                new Text { Point = new DataPoint(205, 27), Content = "NAME", Color = OxyColors.White, FontSize = 6 });
            drawing.Add(
                new Text
                {
                    Point = new DataPoint(205, 12),
                    Content = DateTime.Now.ToString("yyyy-MM-dd"),
                    Color = OxyColors.White,
                    FontSize = 6
                });
            return new Example(drawing);
        }

        [Example("Genealogy tree")]
        public static Example GenealogyTree()
        {
            var drawing = new DrawingModel();
            var øystein = new Person(
                "Øystein",
                new Person(
                    "Olav",
                    new Person(
                        "Sigurd",
                        new Person("Martin", new Person("Olav"), new Person("Marta")),
                        new Person("Brita", new Person("Trond"), new Person("Helga"))),
                    new Person(
                        "Hjørdis",
                        new Person("Ola", new Person("Jon"), new Person("Anna")),
                        new Person("Guro", new Person("Nils"), new Person("Blansa")))),
                new Person(
                    "Sylvi",
                    new Person(
                        "Jonas",
                        new Person("Peder", new Person("Kristoffer"), new Person("Karen")),
                        new Person("Guri", new Person("Aslak"), new Person("Johanne"))),
                    new Person(
                        "Eline Brynhild",
                        new Person("Johan", new Person("Bernt"), new Person("Eline")),
                        new Person("Susanne", new Person("Severin"), new Person("Ragnhild")))));

            øystein.Render(drawing, 1, -10, 190);

            return new Example(drawing);
        }

        /// <summary>
        /// Shows a venn diagram.
        /// </summary>
        /// <returns>An example definition.</returns>
        /// <seealso cref="http://www.colinharman.com/portfolio/how-would-you-like-your-graphic-design/" />
        [Example("Venn diagram")]
        public static Example Diagram()
        {
            var drawing = new DrawingModel();

            drawing.Add(
                new Text
                {
                    Point = new DataPoint(0, 460),
                    Content = "HOW WOULD YOU LIKE",
                    Color = OxyColors.Black,
                    FontSize = 60,
                    FontWeight = FontWeights.Bold,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = OxyPlot.VerticalAlignment.Middle
                });
            drawing.Add(
                new Text
                {
                    Point = new DataPoint(0, 400),
                    Content = "YOUR GRAPHIC DESIGN?",
                    Color = OxyColors.Black,
                    FontSize = 60,
                    FontWeight = FontWeights.Bold,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = OxyPlot.VerticalAlignment.Middle
                });
            drawing.Add(
                new Text
                {
                    Point = new DataPoint(0, 350),
                    Content = "(YOU MAY PICK TWO)",
                    Color = OxyColors.Black,
                    FontSize = 20,
                    FontWeight = FontWeights.Bold,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = OxyPlot.VerticalAlignment.Middle
                });

            drawing.Add(
                new Ellipse
                {
                    Center = new DataPoint(-120, 100),
                    RadiusX = 185,
                    RadiusY = 185,
                    Fill = OxyColor.FromAColor(180, OxyColors.Red)
                });
            drawing.Add(
                new Ellipse
                {
                    Center = new DataPoint(120, 100),
                    RadiusX = 185,
                    RadiusY = 185,
                    Fill = OxyColor.FromAColor(180, OxyColors.Gold)
                });
            drawing.Add(
                new Ellipse
                {
                    Center = new DataPoint(0, 100 - 230),
                    RadiusX = 185,
                    RadiusY = 185,
                    Fill = OxyColor.FromAColor(180, OxyColors.SkyBlue)
                });
            drawing.Add(
                new Ellipse
                {
                    Center = new DataPoint(-205, -105),
                    RadiusX = 90,
                    RadiusY = 90,
                    Fill = OxyColor.FromAColor(180, OxyColors.Black)
                });
            drawing.Add(
                new Text
                {
                    Point = new DataPoint(-180, 132),
                    Content = "FAST",
                    Color = OxyColors.White,
                    FontSize = 60,
                    FontWeight = FontWeights.Bold,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = OxyPlot.VerticalAlignment.Middle
                });
            drawing.Add(
                new Text
                {
                    Point = new DataPoint(180, 132),
                    Content = "CHEAP",
                    Color = OxyColors.White,
                    FontSize = 60,
                    FontWeight = FontWeights.Bold,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = OxyPlot.VerticalAlignment.Middle
                });
            drawing.Add(
                new Text
                {
                    Point = new DataPoint(0, -160),
                    Content = "GREAT",
                    Color = OxyColors.White,
                    FontSize = 60,
                    FontWeight = FontWeights.Bold,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = OxyPlot.VerticalAlignment.Middle
                });
            drawing.Add(
                new Text
                {
                    Point = new DataPoint(-240, -100),
                    Content = "FREE",
                    Color = OxyColors.White,
                    FontSize = 40,
                    FontWeight = FontWeights.Bold,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = OxyPlot.VerticalAlignment.Middle
                });

            return new Example(drawing);
        }
    }

    public class Person
    {
        public Person(string name, Person father = null, Person mother = null)
        {
            this.Name = name;
            this.Father = father;
            this.Mother = mother;
        }

        public string Name { get; private set; }
        public Person Father { get; private set; }
        public Person Mother { get; private set; }
    }

    public static class PersonExtensions
    {
        public static void Render(this Person person, DrawingModel drawing, int generation, double startAngle, double endAngle)
        {
            var arc = new Polyline();
            var r0 = (generation - 1) * 100;
            var r1 = generation * 100;
            arc.Points.AddRange(Interpolation.Arc(new DataPoint(0, 0), r1, r1, startAngle, endAngle));
            drawing.Add(arc);
            var lines = new Lines();
            var midAngle = (startAngle + endAngle) / 2;
            var th0 = startAngle / 180 * Math.PI;
            var th1 = endAngle / 180 * Math.PI;
            var th2 = midAngle / 180 * Math.PI;
            lines.Add(Math.Cos(th0) * r0, Math.Sin(th0) * r0, Math.Cos(th0) * r1, Math.Sin(th0) * r1);
            lines.Add(Math.Cos(th1) * r0, Math.Sin(th1) * r0, Math.Cos(th1) * r1, Math.Sin(th1) * r1);
            drawing.Add(lines);

            var r2 = (r0 + r1) / 2;
            drawing.Add(new Text
            {
                Point = new DataPoint(Math.Cos(th2) * r2, Math.Sin(th2) * r2),
                Content = person.Name,
                FontSize = 20,
                FontFamily = "Times New Roman",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Middle,
                Rotate = 90 - midAngle
            });

            if (person.Father != null)
            {
                person.Father.Render(drawing, generation + 1, midAngle, endAngle);
            }

            if (person.Mother != null)
            {
                person.Mother.Render(drawing, generation + 1, startAngle, midAngle);
            }
        }
    }
}
