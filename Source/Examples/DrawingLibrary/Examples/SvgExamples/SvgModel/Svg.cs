namespace SvgLibrary
{
    using System.IO;
    using System.Xml.Serialization;

    /// <summary>
    /// Represents a OpenStreetMap file.
    /// </summary>
    [XmlRoot("svg", Namespace = "http://www.w3.org/2000/svg")]
    public class Svg : SvgGroup
    {
        /// <summary>
        /// The serializer.
        /// </summary>
        private static XmlSerializer serializer = new XmlSerializer(typeof(Svg));

        /// <summary>
        /// Gets or sets the view box.
        /// </summary>
        /// <value>The view box.</value>
        [XmlAttribute("viewBox")]
        public string ViewBox { get; set; }

        /// <summary>
        /// Loads a <see cref="Svg" /> from the specified stream.
        /// </summary>
        /// <param name="s">The stream to load from.</param>
        /// <returns>An <see cref="Svg" /> instance.</returns>
        public static Svg Load(Stream s)
        {
            return (Svg)serializer.Deserialize(s);
        }

        /// <summary>
        /// Saves to the specified stream.
        /// </summary>
        /// <param name="s">The stream to write to.</param>
        public void Save(Stream s)
        {
            serializer.Serialize(s, this);
        }
    }
}
