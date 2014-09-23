namespace DrawingLibrary.Examples
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using DrawingDemo;

    using OsmLibrary;

    using OxyPlot;
    using OxyPlot.Drawing;

    public static class OpenStreetMapExamples
    {
        [Example("OSM Høvik")]
        public static Example OsmHøvik()
        {
            var drawing = new DrawingModel();

            var tileLayer = new TileLayer
            {
                Source = "http://tile.openstreetmap.org/{Z}/{X}/{Y}.png",
                CopyrightNotice = "OpenStreetMap",
                Opacity = 0.2
            };
            drawing.Add(tileLayer);
            Func<IEnumerable<Node>, IEnumerable<DataPoint>> transform = nodes => nodes.Select(n => tileLayer.Transform(new LatLon(n.Latitude, n.Longitude)));
            var assembly = typeof(OpenStreetMapExamples).GetTypeInfo().Assembly;
            using (var stream = assembly.GetManifestResourceStream("DrawingLibrary.Examples.OpenStreetMapExamples.map.osm"))
            {
                var osm = OpenStreetMap.Load(stream);
                osm.Query(way => way["highway"] == "motorway", (way, nodes) => drawing.Add(new Polyline(transform(nodes)) { Thickness = -5 }));
                osm.Query(way => way["highway"] == "primary", (way, nodes) => drawing.Add(new Polyline(transform(nodes)) { Thickness = -3 }));
                osm.Query(way => way["highway"] == "secondary", (way, nodes) => drawing.Add(new Polyline(transform(nodes)) { Thickness = -2 }));
                osm.Query(way => way["highway"] == "residential", (way, nodes) => drawing.Add(new Polyline(transform(nodes)) { Thickness = -2, Color = OxyColors.Gray }));
                osm.Query(way => way["building"] != null, (way, nodes) => drawing.Add(new Polygon(transform(nodes)) { Fill = OxyColors.Gray }));
                osm.Query(way => way["amenity"] == "parking", (way, nodes) => drawing.Add(new Polygon(transform(nodes)) { Fill = OxyColors.LightBlue }));
            }

            return new Example(drawing);
        }

        [Example("OSM MTB")]
        public static Example OsmMtb()
        {
            // Ways with mtb:scale tags are extracted using http://overpass-turbo.eu/ with the following query
            /* 
<query type="way">
  <has-kv k="mtb:scale"/>
  <bbox-query {{bbox}}/>
</query>
<union>
    <item />
    <recurse type="way-node"/>
</union>
<print/>
             */

            var drawing = new DrawingModel();

            var tileLayer = new TileLayer
            {
                Source = "http://tile.openstreetmap.org/{Z}/{X}/{Y}.png",
                CopyrightNotice = "OpenStreetMap",
            };
            drawing.Add(tileLayer);
            Func<IEnumerable<Node>, IEnumerable<DataPoint>> transform = nodes => nodes.Select(n => tileLayer.Transform(new LatLon(n.Latitude, n.Longitude)));
            var assembly = typeof(OpenStreetMapExamples).GetTypeInfo().Assembly;
            using (var stream =assembly.GetManifestResourceStream("DrawingLibrary.Examples.OpenStreetMapExamples.mtbways.osm"))
            {
                var osm = OpenStreetMap.Load(stream);
                osm.Query(way => way["mtb:scale"] == "0", (way, nodes) => drawing.Add(new Polyline(transform(nodes)) { Thickness = -5, Color = OxyColor.FromAColor(80, OxyColors.Green) }));
                osm.Query(way => way["mtb:scale"] == "1", (way, nodes) => drawing.Add(new Polyline(transform(nodes)) { Thickness = -5, Color = OxyColor.FromAColor(80, OxyColors.Blue) }));
                osm.Query(way => way["mtb:scale"] == "2", (way, nodes) => drawing.Add(new Polyline(transform(nodes)) { Thickness = -5, Color = OxyColor.FromAColor(80, OxyColors.Red) }));
                osm.Query(way => way["mtb:scale"] == "3", (way, nodes) => drawing.Add(new Polyline(transform(nodes)) { Thickness = -5, Color = OxyColor.FromAColor(80, OxyColors.Magenta) }));
            }

            return new Example(drawing);
        }
    }
}