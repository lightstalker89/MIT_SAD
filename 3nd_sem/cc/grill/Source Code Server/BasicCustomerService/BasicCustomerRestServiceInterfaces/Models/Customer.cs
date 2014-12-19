using System.Collections.Generic;
using System.Runtime.Serialization;

namespace BasicCustomerRestServiceInterfaces.Models
{
    [DataContract]
    public class Customer
    {
        [DataMember]
        public string CustomerName { get; set; }

        [DataMember]
        public List<List<string>> CustomerOrders { get; set; }

        public Customer()
        {
            this.CustomerOrders = new List<List<string>>();
        }
    }
}
