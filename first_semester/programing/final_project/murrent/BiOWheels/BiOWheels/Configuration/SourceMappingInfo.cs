using System.Xml.Serialization;

namespace BiOWheels.Configuration
{
    public class SourceMappingInfo
    {
        [XmlAttribute]
        public bool Recursive { get; set; }

        public string SourceDirectory { get; set; }
    }
}
