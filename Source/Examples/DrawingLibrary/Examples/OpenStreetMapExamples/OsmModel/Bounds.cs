namespace OsmLibrary
{
    using System.Xml.Serialization;

    /// <summary>
    /// Represents a bounding box.
    /// </summary>
    public struct Bounds
    {
        /// <summary>
        /// Gets or sets the minimum latitude.
        /// </summary>
        /// <value>The minimum latitude.</value>
        [XmlAttribute("minlat")]
        public double MinLat { get; set; }

        /// <summary>
        /// Gets or sets the minimum longitude.
        /// </summary>
        /// <value>The minimum longitude.</value>
        [XmlAttribute("minlon")]
        public double MinLon { get; set; }

        /// <summary>
        /// Gets or sets the maximum latitude.
        /// </summary>
        /// <value>The maximum latitude.</value>
        [XmlAttribute("maxlat")]
        public double MaxLat { get; set; }

        /// <summary>
        /// Gets or sets the maximum longitude.
        /// </summary>
        /// <value>The maximum longitude.</value>
        [XmlAttribute("maxlon")]
        public double MaxLon { get; set; }
    }
}