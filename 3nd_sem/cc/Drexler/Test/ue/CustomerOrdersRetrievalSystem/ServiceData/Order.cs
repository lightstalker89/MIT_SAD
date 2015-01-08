using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ServiceData
{
    [DataContract]
    public class Order
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string CustomerName { get; set; }
    }
}
