using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ConsolBox
{
    public class SourceFolders
    {
        [XmlElement("SourceFolder")]
        public List<SourceFolder> sourceFolder { get; set; }
    }
}
