using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ConsolBox
{
    public class Exceptions
    {
        [XmlElement("ExceptionFolder")]
        public List<ExceptionFolder> exceptionFolder { get; set; }
    }
}
