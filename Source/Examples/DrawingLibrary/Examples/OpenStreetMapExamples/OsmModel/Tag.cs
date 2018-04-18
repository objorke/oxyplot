namespace OsmLibrary
{
    using System.Xml.Serialization;

    /// <summary>
    /// Represents a tag in a <see cref="TaggedElement" />.
    /// </summary>
    public class Tag
    {
        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>The key.</value>
        [XmlAttribute("k")]
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        [XmlAttribute("v")]
        public string Value { get; set; }
    }
}