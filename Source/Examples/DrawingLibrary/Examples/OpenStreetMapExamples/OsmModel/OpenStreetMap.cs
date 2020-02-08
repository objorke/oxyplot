namespace OsmLibrary
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml.Serialization;

    /// <summary>
    /// Represents a OpenStreetMap file.
    /// </summary>
    [XmlRoot("osm")]
    [XmlInclude(typeof(Bounds))]
    [XmlInclude(typeof(Node))]
    [XmlInclude(typeof(Way))]
    [XmlInclude(typeof(Relation))]
    public class OpenStreetMap
    {
        /// <summary>
        /// The serializer.
        /// </summary>
        private static XmlSerializer serializer = new XmlSerializer(typeof(OpenStreetMap));

        /// <summary>
        /// Initializes a new instance of the <see cref="OpenStreetMap"/> class.
        /// </summary>
        public OpenStreetMap()
        {
            this.Nodes = new List<Node>();
            this.Ways = new List<Way>();
            this.Relations = new List<Relation>();
        }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>The version.</value>
        [XmlAttribute("version")]
        public string Version { get; set; }

        /// <summary>
        /// Gets or sets the generator.
        /// </summary>
        /// <value>The generator.</value>
        [XmlAttribute("generator")]
        public string Generator { get; set; }

        /// <summary>
        /// Gets or sets the copyright.
        /// </summary>
        /// <value>The copyright.</value>
        [XmlAttribute("copyright")]
        public string Copyright { get; set; }

        /// <summary>
        /// Gets or sets the attribution.
        /// </summary>
        /// <value>The attribution.</value>
        [XmlAttribute("attribution")]
        public string Attribution { get; set; }

        /// <summary>
        /// Gets or sets the license.
        /// </summary>
        /// <value>The license.</value>
        [XmlAttribute("license")]
        public string License { get; set; }

        /// <summary>
        /// Gets or sets the bounding box.
        /// </summary>
        /// <value>The bounding box.</value>
        [XmlElement("bounds")]
        public Bounds Bounds { get; set; }

        /// <summary>
        /// Gets the nodes.
        /// </summary>
        /// <value>The nodes.</value>
        [XmlElement("node")]
        public List<Node> Nodes { get; private set; }

        /// <summary>
        /// Gets the ways.
        /// </summary>
        /// <value>The ways.</value>
        [XmlElement("way")]
        public List<Way> Ways { get; private set; }

        /// <summary>
        /// Gets the relations.
        /// </summary>
        /// <value>The relations.</value>
        [XmlElement("relation")]
        public List<Relation> Relations { get; private set; }

        /// <summary>
        /// Loads a <see cref="OpenStreetMap" /> from the specified stream.
        /// </summary>
        /// <param name="s">The stream to load from.</param>
        /// <returns>A <see cref="OpenStreetMap" />.</returns>
        public static OpenStreetMap Load(Stream s)
        {
            return (OpenStreetMap)serializer.Deserialize(s);
        }

        /// <summary>
        /// Saves to the specified stream.
        /// </summary>
        /// <param name="s">The stream to write to.</param>
        public void Save(Stream s)
        {
            serializer.Serialize(s, this);
        }

        public IEnumerable<Node> GetNodes(List<NodeRef> nodes)
        {
            foreach (var nd in nodes)
            {
                var node = this.Nodes.FirstOrDefault(n => n.Id == nd.Ref);
                if (node != null)
                {
                    yield return node;
                }
            }
        }

        public void Query(Func<Way, bool> predicate, Action<Way, IEnumerable<Node>> action)
        {
            foreach (var way in this.Ways.Where(predicate))
            {
                var nodes = this.GetNodes(way.Nodes);
                action(way, nodes);
            }
        }
    }
}
