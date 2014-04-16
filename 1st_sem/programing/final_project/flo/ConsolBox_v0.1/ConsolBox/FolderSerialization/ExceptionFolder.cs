using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ConsolBox
{
    public class ExceptionFolder
    {
        [XmlAttribute("path")]
        public string path { get; set; }
    }
}
