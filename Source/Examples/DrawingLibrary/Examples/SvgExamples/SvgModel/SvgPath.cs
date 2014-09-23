namespace SvgLibrary
{
    using System.Xml.Serialization;

    public class SvgPath : SvgElement
    {
        [XmlAttribute("d")]
        public string PathData { get; set; }
    }
}