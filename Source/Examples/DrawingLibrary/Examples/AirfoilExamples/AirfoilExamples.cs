namespace DrawingLibrary.Examples
{
    using System.Collections.Generic;
    using System.Linq;

    using DrawingDemo;

    using OxyPlot;
    using OxyPlot.Drawing;

    public static class AirfoilExamples
    {
        [Example("NACA 0012")]
        public static Example Naca0012()
        {
            return Naca4("0012");
        }

        [Example("NACA 2412")]
        public static Example Naca2412()
        {
            return Naca4("2412");
        }

        [Example("NACA 6412")]
        public static Example Naca6412()
        {
            return Naca4("6412");
        }

        public static Example Naca4(string id)
        {
            var airfoil = new NacaAirfoil(id);
            DataPoint[] camberLine;
            DataPoint[] upper;
            DataPoint[] lower;
            DataPoint[] thickness;
            airfoil.GetProfile(81, 100, out camberLine, out thickness, out upper, out lower);

            var profile = new List<DataPoint>(upper.Reverse());
            profile.AddRange(lower);

            var drawing = new DrawingModel() { Background = OxyColor.FromRgb(0, 128, 196) };
            drawing.Add(new Grid() { MajorColor = OxyColor.FromAColor(20, OxyColors.White), MinorColor = OxyColor.FromAColor(10, OxyColors.White) });
            drawing.Add(new Polygon(profile) { Stroke = OxyColors.Blue, Fill = OxyColor.FromAColor(30, OxyColors.White), Thickness = -2 });
            drawing.Add(new Polyline(camberLine) { Color = OxyColors.Red, Thickness = -2 });
            drawing.Add(new Polyline(thickness) { Color = OxyColors.Purple, Thickness = -2 });
            drawing.Add(new Text { Point = new DataPoint(0, 20), Content = "Airfoil example", FontSize = 4, FontWeight = FontWeights.Bold });
            drawing.Add(new Text { Point = new DataPoint(0, -7), Content = airfoil.ToString(), FontSize = 3, FontWeight = FontWeights.Bold });
            drawing.Add(new Text { Point = new DataPoint(80, -4), Content = "Camber line", FontSize = 2, Color = OxyColors.Red });
            drawing.Add(new Text { Point = new DataPoint(80, -7), Content = "Thickness", FontSize = 2, Color = OxyColors.Purple });
            drawing.Add(new Rectangle { MinimumX = -2, MaximumX = 102, MinimumY = -12, MaximumY = 22, Stroke = OxyColors.Black });
            return new Example(drawing);
        }
    }
}
