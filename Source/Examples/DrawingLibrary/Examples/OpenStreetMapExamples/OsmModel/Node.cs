namespace OsmLibrary
{
    using System.Xml.Serialization;

    /// <summary>
    /// Represents a node.
    /// </summary>
    public class Node : TaggedElement
    {
        /// <summary>
        /// Gets or sets the latitude.
        /// </summary>
        /// <value>The latitude.</value>
        [XmlAttribute("lat")]
        public double Latitude { get; set; }

        /// <summary>
        /// Gets or sets the longitude.
        /// </summary>
        /// <value>The longitude.</value>
        [XmlAttribute("lon")]
        public double Longitude { get; set; }
    }
}