namespace OsmLibrary
{
    using System.Collections.Generic;
    using System.Xml.Serialization;

    /// <summary>
    /// Provides a base class for elements that contains tags.
    /// </summary>
    public abstract class TaggedElement : Element
    {
        /// <summary>
        /// The tags
        /// </summary>
        private List<Tag> tags;

        /// <summary>
        /// Initializes a new instance of the <see cref="TaggedElement"/> class.
        /// </summary>
        protected TaggedElement()
        {
            this.tags = new List<Tag>();
        }


        /// <summary>
        /// Gets the tags of the element.
        /// </summary>
        /// <value>The tags.</value>
        [XmlElement("tag")]
        public List<Tag> Tags
        {
            get
            {
                return this.tags;
            }
        }
    }
}