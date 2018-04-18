// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TileLayer.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents a tile map layer.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Drawing
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Net;
    using System.Threading;

    /// <summary>
    /// Represents a tile map layer.
    /// </summary>
    public class TileLayer : DrawingElement
    {
        /// <summary>
        /// The image cache.
        /// </summary>
        private readonly Dictionary<string, OxyImage> images = new Dictionary<string, OxyImage>();

        /// <summary>
        /// The download queue.
        /// </summary>
        private readonly Queue<string> queue = new Queue<string>();

        /// <summary>
        /// The current number of downloads
        /// </summary>
        private int numberOfDownloads;

        /// <summary>
        /// Initializes a new instance of the <see cref="TileLayer"/> class.
        /// </summary>
        public TileLayer()
        {
            this.Source = "http://tile.openstreetmap.org/{Z}/{X}/{Y}.png";
            this.CopyrightNotice = "OpenStreetMap";

            this.TileSize = 256;
            this.MinZoomLevel = 0;
            this.MaxZoomLevel = 20;
            this.Opacity = 1d;
            this.FontFamily = "Arial";
            this.FontSize = 12;
            this.FontWeight = FontWeights.Normal;
            this.MaxNumberOfDownloads = 8;
        }

        /// <summary>
        /// Gets or sets the max number of simultaneous downloads.
        /// </summary>
        /// <value>The max number of downloads.</value>
        public int MaxNumberOfDownloads { get; set; }

        /// <summary>
        /// Gets or sets the source.
        /// </summary>
        /// <value>
        /// The source.
        /// </value>
        public string Source { get; set; }

        /// <summary>
        /// Gets or sets the copyright notice.
        /// </summary>
        /// <value>
        /// The copyright notice.
        /// </value>
        public string CopyrightNotice { get; set; }

        /// <summary>
        /// Gets or sets the opacity.
        /// </summary>
        /// <value>
        /// The opacity.
        /// </value>
        public double Opacity { get; set; }

        /// <summary>
        /// Gets or sets the size of the tile.
        /// </summary>
        /// <value>
        /// The size of the tile.
        /// </value>
        public int TileSize { get; set; }

        /// <summary>
        /// Gets or sets the minimum zoom level.
        /// </summary>
        /// <value>
        /// The minimum zoom level.
        /// </value>
        public int MinZoomLevel { get; set; }

        /// <summary>
        /// Gets or sets the maximum zoom level.
        /// </summary>
        /// <value>
        /// The maximum zoom level.
        /// </value>
        public int MaxZoomLevel { get; set; }

        /// <summary>
        /// Transforms a position to a coordinate (x,y).
        /// </summary>
        /// <param name="latlon">The latitude and longitude.</param>
        /// <returns>
        /// Tile coordinates.
        /// </returns>
        public static DataPoint ToPoint(LatLon latlon)
        {
            double x;
            double y;
            LatLonToTile(latlon.Latitude, latlon.Longitude, 0, out x, out y);
            return new DataPoint(x, -y);
        }

        /// <summary>
        /// Transforms a coordinate (x,y) to a position.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <returns>
        /// A <see cref="LatLon" /> structure.
        /// </returns>
        public static LatLon ToLatLon(DataPoint point)
        {
            double latitude;
            double longitude;
            TileToLatLon(point.X, -point.Y, 0, out latitude, out longitude);
            return new LatLon(latitude, longitude);
        }

        /// <summary>
        /// Transforms the specified latitude and longitude to a <see cref="DataPoint" />.
        /// </summary>
        /// <param name="latLon">The latitude and longitude.</param>
        /// <returns>The transformed point.</returns>
        public DataPoint Transform(LatLon latLon)
        {
            return ToPoint(latLon);
        }

        /// <summary>
        /// Creates the presentation model for the element.
        /// </summary>
        /// <param name="v">The parent presentation model.</param>
        /// <returns>
        /// The presentation model.
        /// </returns>
        protected override Presenter CreatePresenter(DrawingViewModel v)
        {
            return new TileLayerPresenter(this, v);
        }

        /// <summary>
        /// Transforms a position to a tile coordinate.
        /// </summary>
        /// <param name="latitude">The latitude.</param>
        /// <param name="longitude">The longitude.</param>
        /// <param name="zoom">The zoom.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        private static void LatLonToTile(double latitude, double longitude, int zoom, out double x, out double y)
        {
            // http://wiki.openstreetmap.org/wiki/Slippy_map_tilenames
            int n = 1 << zoom;
            double lat = latitude / 180 * Math.PI;
            x = (longitude + 180.0) / 360.0 * n;
            y = (1.0 - (Math.Log(Math.Tan(lat) + (1.0 / Math.Cos(lat))) / Math.PI)) / 2.0 * n;
        }

        /// <summary>
        /// Transforms a tile coordinate (x,y) to a position.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="zoom">The zoom.</param>
        /// <param name="latitude">The latitude.</param>
        /// <param name="longitude">The longitude.</param>
        private static void TileToLatLon(double x, double y, int zoom, out double latitude, out double longitude)
        {
            int n = 1 << zoom;
            longitude = (x / n * 360.0) - 180.0;
            double lat = Math.Atan(Math.Sinh(Math.PI * (1 - (2 * y / n))));
            latitude = lat * 180.0 / Math.PI;
        }

        /// <summary>
        /// Gets the image from the specified uri.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="asyncLoading">Get the image asynchronously if set to <c>true</c>. The plot model will be invalidated when the image has been downloaded.</param>
        /// <param name="onDownloadCompleted">The on download completed delegate.</param>
        /// <returns>
        /// The image.
        /// </returns>
        /// <remarks>
        /// This method gets the image from cache, or starts an async download.
        /// </remarks>
        private OxyImage GetImage(string uri, bool asyncLoading, Action onDownloadCompleted)
        {
            OxyImage img;
            if (this.images.TryGetValue(uri, out img))
            {
                return img;
            }

            if (!asyncLoading)
            {
                return this.Download(uri);
            }

            lock (this.queue)
            {
                // 'reserve' an image (otherwise multiple downloads of the same uri may happen)
                this.images[uri] = null;
                this.queue.Enqueue(uri);
            }

            this.BeginDownload(onDownloadCompleted);
            return null;
        }

        /// <summary>
        /// Downloads the image from the specified URI.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <returns>The image</returns>
        private OxyImage Download(string uri)
        {
            OxyImage img = null;
            var mre = new ManualResetEvent(false);
/*            var request = (HttpWebRequest)WebRequest.Create(uri);
            request.Method = "GET";
            request.BeginGetResponse(
               r =>
               {
                   try
                   {
                       if (request.HaveResponse)
                       {
                           var response = request.EndGetResponse(r);
                           var stream = response.GetResponseStream();

                           var ms = new MemoryStream();
                           stream.CopyTo(ms);
                           var buffer = ms.ToArray();

                           img = new OxyImage(buffer);
                           this.images[uri] = img;
                       }
                   }
                   catch (Exception e)
                   {
                       var ie = e;
                       while (ie != null)
                       {
                           System.Diagnostics.Debug.WriteLine(ie.Message);
                           ie = ie.InnerException;
                       }
                   }
                   finally
                   {
                       mre.Set();
                   }
               },
               request);
               */
            mre.WaitOne();
            return img;
        }

        /// <summary>
        /// Starts the next download in the queue.
        /// </summary>
        /// <param name="onDownloadCompleted">The download completed delegate.</param>
        private void BeginDownload(Action onDownloadCompleted)
        {
            if (this.numberOfDownloads >= this.MaxNumberOfDownloads)
            {
                return;
            }

            string uri = this.queue.Dequeue();
/*            var request = (HttpWebRequest)WebRequest.Create(uri);
            request.Method = "GET";
            Interlocked.Increment(ref this.numberOfDownloads);
            request.BeginGetResponse(
                r =>
                {
                    Interlocked.Decrement(ref this.numberOfDownloads);
                    try
                    {
                        if (request.HaveResponse)
                        {
                            var response = request.EndGetResponse(r);
                            var stream = response.GetResponseStream();
                            this.DownloadCompleted(uri, stream, onDownloadCompleted);
                        }
                    }
                    catch (Exception e)
                    {
                        var ie = e;
                        while (ie != null)
                        {
                            System.Diagnostics.Debug.WriteLine(ie.Message);
                            ie = ie.InnerException;
                        }
                    }
                },
                request);*/
        }

        /// <summary>
        /// The download completed, set the image.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="result">The result.</param>
        /// <param name="onDownloadCompleted">The download completed delegate.</param>
        private void DownloadCompleted(string uri, Stream result, Action onDownloadCompleted)
        {
            if (result == null)
            {
                return;
            }

            var ms = new MemoryStream();
            result.CopyTo(ms);
            var buffer = ms.ToArray();

            var img = new OxyImage(buffer);
            this.images[uri] = img;

            lock (this.queue)
            {
                // Clear old items in the queue, new ones will be added when the plot is refreshed
                foreach (var queuedUri in this.queue)
                {
                    // Remove the 'reserved' image
                    this.images.Remove(queuedUri);
                }

                this.queue.Clear();
            }

            onDownloadCompleted();
            if (this.queue.Count > 0)
            {
                this.BeginDownload(onDownloadCompleted);
            }
        }

        /// <summary>
        /// Gets the tile URI.
        /// </summary>
        /// <param name="x">The tile x.</param>
        /// <param name="y">The tile y.</param>
        /// <param name="zoom">The zoom.</param>
        /// <returns>The uri.</returns>
        private string GetTileUri(int x, int y, int zoom)
        {
            string url = this.Source.Replace("{X}", x.ToString(CultureInfo.InvariantCulture));
            url = url.Replace("{Y}", y.ToString(CultureInfo.InvariantCulture));
            return url.Replace("{Z}", zoom.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Represents the presentation model for the <see cref="TileLayer" />.
        /// </summary>
        private class TileLayerPresenter : Presenter<TileLayer>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="TileLayerPresenter"/> class.
            /// </summary>
            /// <param name="model">The model.</param>
            /// <param name="v">The presentation model of the parent <see cref="DrawingModel" />.</param>
            public TileLayerPresenter(TileLayer model, DrawingViewModel v)
                : base(model, v)
            {
            }

            /// <summary>
            /// Gets the bounding box of the element.
            /// </summary>
            /// <param name="rc">The render context.</param>
            /// <returns>
            /// The bounding box.
            /// </returns>
            public override BoundingBox GetBounds(IRenderContext rc)
            {
                return new BoundingBox();
            }

            /// <summary>
            /// Updates the element.
            /// </summary>
            /// <param name="rc">The render context.</param>
            public override void Update(IRenderContext rc)
            {
            }

            /// <summary>
            /// Renders the element.
            /// </summary>
            /// <param name="rc">The render context.</param>
            public override void Render(IRenderContext rc)
            {
                var clientRect = this.DrawingViewModel.ClientArea;
                var topLeft = this.InverseTransform(new ScreenPoint(clientRect.Left, clientRect.Top));
                DataPoint bottomRight;
                bottomRight = this.InverseTransform(new ScreenPoint(clientRect.Right, clientRect.Bottom));
                var latlon0 = ToLatLon(topLeft);
                var latlon1 = ToLatLon(bottomRight);
                var lon0 = latlon0.Longitude;
                var lon1 = latlon1.Longitude;
                var lat0 = latlon0.Latitude;
                var lat1 = latlon1.Latitude;

                // the desired number of tiles horizontally
                double tilesx = this.DrawingViewModel.ClientArea.Width / this.Model.TileSize;

                // calculate the desired zoom level
                var n = tilesx / (((lon1 + 180) / 360) - ((lon0 + 180) / 360));
                var zoom = (int)Math.Round(Math.Log(n) / Math.Log(2));
                if (zoom < this.Model.MinZoomLevel)
                {
                    zoom = this.Model.MinZoomLevel;
                }

                if (zoom > this.Model.MaxZoomLevel)
                {
                    zoom = this.Model.MaxZoomLevel;
                }

                // find tile coordinates for the corners
                double x0, y0;
                TileLayer.LatLonToTile(lat0, lon0, zoom, out x0, out y0);
                double x1, y1;
                TileLayer.LatLonToTile(lat1, lon1, zoom, out x1, out y1);

                double xmax = Math.Max(x0, x1);
                double xmin = Math.Min(x0, x1);
                double ymax = Math.Max(y0, y1);
                double ymin = Math.Min(y0, y1);

                // Add the tiles
                for (var x = (int)xmin; x < xmax; x++)
                {
                    for (var y = (int)ymin; y < ymax; y++)
                    {
                        string uri = this.Model.GetTileUri(x, y, zoom);
                        var img = this.Model.GetImage(uri, rc.RendersToScreen, () => this.DrawingViewModel.Invalidate());

                        if (img == null)
                        {
                            continue;
                        }

                        // transform from tile coordinates to lat/lon
                        double latitude0, latitude1, longitude0, longitude1;
                        TileLayer.TileToLatLon(x, y, zoom, out latitude0, out longitude0);
                        TileLayer.TileToLatLon(x + 1, y + 1, zoom, out latitude1, out longitude1);

                        // transform from lat/lon to screen coordinates
                        var s00 = this.Transform(ToPoint(new LatLon(latitude0, longitude0)));
                        var s11 = this.Transform(ToPoint(new LatLon(latitude1, longitude1)));

                        var r = OxyRect.Create(s00.X, s00.Y, s11.X, s11.Y);

                        // draw the image
                        rc.DrawClippedImage(clientRect, img, r.Left, r.Top, r.Width, r.Height, this.Model.Opacity, true);

                        // Tile coordinates
                        // rc.DrawText(r.Center, string.Format("{0},{1}", x, y), OxyColors.Black, this.Model.FontFamily, this.Model.FontSize, this.Model.FontWeight, 0, HorizontalAlignment.Center, VerticalAlignment.Middle);
                    }
                }

                // draw the copyright notice
                var p = new ScreenPoint(clientRect.Right - 5, clientRect.Bottom - 5);
                var textSize = rc.MeasureText(this.Model.CopyrightNotice, this.Model.FontFamily, this.Model.FontSize, this.Model.FontWeight);
                rc.DrawRectangle(new OxyRect(p.X - textSize.Width - 2, p.Y - textSize.Height - 2, textSize.Width + 4, textSize.Height + 4), OxyColor.FromAColor(200, OxyColors.White), OxyColors.Undefined);

                rc.DrawText(
                    p,
                    this.Model.CopyrightNotice,
                    OxyColors.Black,
                    this.Model.FontFamily,
                    this.Model.FontSize,
                    this.Model.FontWeight,
                    0,
                    HorizontalAlignment.Right,
                    VerticalAlignment.Bottom);
            }
        }
    }
}
