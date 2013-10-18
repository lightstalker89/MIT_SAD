using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ConsolBox
{
    public class SourceFolder
    {
        [XmlAttribute("path")]
        public string path { get; set; }
        [XmlAttribute("recursion")]
        public string recursion { get; set; }
        [XmlElement("Exceptions")]
        public Exceptions exceptionFolder { get; set; }
    }
}
