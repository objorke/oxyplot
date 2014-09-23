namespace OsmLibrary
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Serialization;

    /// <summary>
    /// Represents a way.
    /// </summary>
    public class Way : TaggedElement
    {
        /// <summary>
        /// The nodes
        /// </summary>
        private readonly List<NodeRef> nodes;

        /// <summary>
        /// Initializes a new instance of the <see cref="Way"/> class.
        /// </summary>
        public Way()
        {
            this.nodes = new List<NodeRef>();
        }

        /// <summary>
        /// Gets the nodes of the way.
        /// </summary>
        /// <value>The nodes.</value>
        [XmlElement("nd")]
        public List<NodeRef> Nodes
        {
            get
            {
                return this.nodes;
            }
        }

        public string this[string key]
        {
            get
            {
                var t = this.Tags.FirstOrDefault(tag => tag.Key == key);
                if (t != null)
                {
                    return t.Value;
                }
                return null;
            }
        }
    }
}