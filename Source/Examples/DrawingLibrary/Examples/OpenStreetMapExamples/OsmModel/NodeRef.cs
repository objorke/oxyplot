namespace OsmLibrary
{
    using System.Xml.Serialization;

    /// <summary>
    /// Represents a node reference in a <see cref="Way" />.
    /// </summary>
    public struct NodeRef
    {
        /// <summary>
        /// Gets or sets the node reference.
        /// </summary>
        /// <value>The reference.</value>
        [XmlAttribute("ref")]
        public long Ref { get; set; }
    }
}