namespace DrawingDemo
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text.RegularExpressions;

    using OxyPlot;
    using OxyPlot.Drawing;

    public static class TileLayerExamples {

        [Example("Tile layer and gpx track log")]
        public static Example OpenStreetMap()
        {
            var drawing = new DrawingModel();

            var tileLayer = new TileLayer
            {
                Source = "http://opencache.statkart.no/gatekeeper/gk/gk.open_gmaps?layers=toporaster3&zoom={Z}&x={X}&y={Y}",
                CopyrightNotice = "Kartgrunnlag: Statens kartverk, Geovekst og kommuner.",
                Opacity = 0.7
            };
            drawing.Add(tileLayer);

            var assembly = typeof(TileLayerExamples).GetTypeInfo().Assembly;
            using (var stream = assembly.GetManifestResourceStream("DrawingLibrary.Resources.Tracklog.gpx"))
            {
                var track = LoadGpxTrack(stream).ToArray();

                var trackLine = new Polyline(track.Select(tileLayer.Transform))
                {
                    Color = OxyColor.FromAColor(180, OxyColors.Red),
                    Thickness = -3,
                    MinimumSegmentLength = 1
                };

                drawing.Add(trackLine);

                var milestones = GetMileStones(track, 5000);
                foreach (var kvp in milestones)
                {
                    var p = tileLayer.Transform(kvp.Value);
                    var milestoneEllipse = new Ellipse
                    {
                        Center = p,
                        RadiusX = -10,
                        RadiusY = -10,
                        Stroke = OxyColors.Black,
                        Fill = OxyColor.FromAColor(180, OxyColors.White),
                        Thickness = -1.5,
                        Text = string.Format(CultureInfo.InvariantCulture, "{0:0}", kvp.Key / 1000),
                        FontSize = 9
                    };

                    drawing.Add(milestoneEllipse);
                }
            }

            return new Example(drawing);
        }

        private static Dictionary<double, LatLon> GetMileStones(IList<LatLon> points, double distance)
        {
            var result = new Dictionary<double, LatLon>();
            double milestone = distance;
            double d0 = 0;
            for (int i = 1; i < points.Count; i++)
            {
                var d = points[i - 1].DistanceTo(points[i]);
                var d1 = d0 + d;
                if (milestone > d0 && milestone <= d1)
                {
                    double f = (milestone - d0) / (d1 - d0);
                    var lat = points[i - 1].Latitude + (f * (points[i].Latitude - points[i - 1].Latitude));
                    var lon = points[i - 1].Longitude + (f * (points[i].Longitude - points[i - 1].Longitude));
                    result.Add(milestone, new LatLon(lat, lon));
                    milestone += distance;
                }

                d0 = d1;
            }

            return result;
        }

        private static IEnumerable<LatLon> LoadGpxTrack(Stream s)
        {
            var r = new StreamReader(s);
            var content = r.ReadToEnd();
            foreach (Match m in Regex.Matches(content, "<trkpt lon=\"(.*)\" lat=\"(.*)\""))
            {
                var lon = double.Parse(m.Groups[1].Value, CultureInfo.InvariantCulture);
                var lat = double.Parse(m.Groups[2].Value, CultureInfo.InvariantCulture);
                yield return new LatLon(lat, lon);
            }
        }
    }
}