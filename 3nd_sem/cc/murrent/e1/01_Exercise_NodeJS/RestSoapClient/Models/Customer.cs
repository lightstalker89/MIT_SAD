using System.Collections.Generic;

namespace RestSoapClient.Models
{
    public class Customer
    {
        public string Name { get; set; }
        public List<string> Orders { get; set; }
    }
}
