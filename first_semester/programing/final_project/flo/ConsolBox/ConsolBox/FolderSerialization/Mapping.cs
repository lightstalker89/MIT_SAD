using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ConsolBox
{
    public class Mapping
    {
        [XmlElement("SourceFolders")]
        public SourceFolders sourceFolders { get; set; }
        [XmlElement("DestinationFolders")]
        public DestinationFolders destinationFolders { get; set; }
    }
}
