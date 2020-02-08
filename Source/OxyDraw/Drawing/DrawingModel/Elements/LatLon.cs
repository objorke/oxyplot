// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LatLon.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// <summary>
//   Represents a position.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OxyPlot.Drawing
{
    using System;

    /// <summary>
    /// Represents a position.
    /// </summary>
    public struct LatLon
    {
        /// <summary>
        /// The latitude
        /// </summary>
        private readonly double latitude;

        /// <summary>
        /// The longitude
        /// </summary>
        private readonly double longitude;

        /// <summary>
        /// Initializes a new instance of the <see cref="LatLon"/> struct.
        /// </summary>
        /// <param name="latitude">The latitude.</param>
        /// <param name="longitude">The longitude.</param>
        public LatLon(double latitude, double longitude)
        {
            this.latitude = latitude;
            this.longitude = longitude;
        }

        /// <summary>
        /// Gets the latitude.
        /// </summary>
        /// <value>
        /// The latitude.
        /// </value>
        public double Latitude
        {
            get
            {
                return this.latitude;
            }
        }

        /// <summary>
        /// Gets the longitude.
        /// </summary>
        /// <value>
        /// The longitude.
        /// </value>
        public double Longitude
        {
            get
            {
                return this.longitude;
            }
        }

        /// <summary>
        /// Calculates the distance to the specified point by the Haversine formula.
        /// </summary>
        /// <param name="other">The point to calculate the distance to.</param>
        /// <returns>
        /// The distance in meter.
        /// </returns>
        /// <a href="https://en.wikipedia.org/wiki/Haversine_formula" />
        /// <a href="https://www.movable-type.co.uk/scripts/gis-faq-5.1.html" />
        public double DistanceTo(LatLon other)
        {
            // radius of the earth (km)
            const double Deg2Rad = Math.PI / 180;
            double dlat = (other.Latitude - this.Latitude) * Deg2Rad;
            double dlon = (other.Longitude - this.Longitude) * Deg2Rad;
            double a = (Math.Sin(dlat / 2) * Math.Sin(dlat / 2)) + (Math.Cos(this.Latitude * Deg2Rad) * Math.Cos(other.Latitude * Deg2Rad) * Math.Sin(dlon / 2) * Math.Sin(dlon / 2));
            double c = 2 * Math.Asin(Math.Min(1, Math.Sqrt(a)));
            return 6371000 * c;
        }
    }
}
