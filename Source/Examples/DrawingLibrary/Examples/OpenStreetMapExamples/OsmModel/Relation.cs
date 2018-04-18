namespace OsmLibrary
{
    using System.Collections.Generic;
    using System.Xml.Serialization;

    /// <summary>
    /// Represents a relation.
    /// </summary>
    public class Relation : Element
    {
        /// <summary>
        /// The members.
        /// </summary>
        private readonly List<Member> members;

        /// <summary>
        /// Initializes a new instance of the <see cref="Relation"/> class.
        /// </summary>
        public Relation()
        {
            this.members = new List<Member>();
        }

        /// <summary>
        /// Gets the members.
        /// </summary>
        /// <value>The members.</value>
        [XmlElement("member")]
        public List<Member> Members
        {
            get
            {
                return this.members;
            }
        }
    }
}