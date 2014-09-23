namespace OsmLibrary
{
    using System.Xml.Serialization;

    /// <summary>
    /// Represents a member of a <see cref="Relation" />.
    /// </summary>
    public struct Member
    {
        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        [XmlAttribute("type")]
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the reference.
        /// </summary>
        /// <value>The reference.</value>
        [XmlAttribute("ref")]
        public long Ref { get; set; }

        /// <summary>
        /// Gets or sets the role.
        /// </summary>
        /// <value>The role.</value>
        [XmlAttribute("role")]
        public string Role { get; set; }
    }
}