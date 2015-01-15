using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.OpenStack.RequestHandling
{
    [DataContract]
    public class StartServerRequest
    {
        [DataMember(Name="os-start")]
        public string OSStart { get; set; }
    }
}
