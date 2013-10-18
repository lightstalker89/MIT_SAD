using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ConsolBox
{
    public class ConfigFile
    {
        [XmlElement("Folders")]
        public Folders folders { get; set; }
    }
}
