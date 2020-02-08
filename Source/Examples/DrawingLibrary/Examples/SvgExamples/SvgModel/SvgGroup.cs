namespace SvgLibrary
{
    using System.Collections.Generic;
    using System.Xml.Serialization;

    [XmlInclude(typeof(SvgPath))]
    [XmlInclude(typeof(SvgGroup))]
    public class SvgGroup : SvgElement
    {
        public SvgGroup()
        {
            this.Elements = new List<SvgElement>();
        }

        /// <summary>
        /// Gets the elements.
        /// </summary>
        /// <value>The elements.</value>
        [XmlElement(typeof(SvgGroup), ElementName = "g")]
        [XmlElement(typeof(SvgPath), ElementName = "path")]
        public List<SvgElement> Elements { get; private set; }
    }
}