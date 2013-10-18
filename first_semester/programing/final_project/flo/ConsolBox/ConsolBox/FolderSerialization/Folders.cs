using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ConsolBox
{
    public class Folders
    {
        [XmlElement("Mapping")]
        public List<Mapping> FolderMapping = new List<Mapping>();
    }
}
