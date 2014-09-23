namespace OsmLibrary
{
    using System;
    using System.Xml.Serialization;

    /// <summary>
    /// Represents a base class for elements.
    /// </summary>
    public abstract class Element
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        [XmlAttribute("id")]
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Element"/> is visible.
        /// </summary>
        /// <value><c>true</c> if visible; otherwise, <c>false</c>.</value>
        [XmlAttribute("visible")]
        public bool Visible { get; set; }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>The version.</value>
        [XmlAttribute("version")]
        public int Version { get; set; }

        /// <summary>
        /// Gets or sets the changeset.
        /// </summary>
        /// <value>The changeset.</value>
        [XmlAttribute("changeset")]
        public long Changeset { get; set; }

        /// <summary>
        /// Gets or sets the timestamp.
        /// </summary>
        /// <value>The timestamp.</value>
        [XmlAttribute("timestamp")]
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Gets or sets the user.
        /// </summary>
        /// <value>The user.</value>
        [XmlAttribute("user")]
        public string User { get; set; }

        /// <summary>
        /// Gets or sets the user id.
        /// </summary>
        /// <value>The user id.</value>
        [XmlAttribute("uid")]
        public long Uid { get; set; }
    }
}