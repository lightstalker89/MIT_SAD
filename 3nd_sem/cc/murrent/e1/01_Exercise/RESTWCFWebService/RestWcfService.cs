using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Web.Script.Serialization;
using ServiceModels;

namespace RESTWCFWebService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in both code and config file together.
    public class RestWcfService : IService
    {
        private List<Customer> customers = new List<Customer>();
        private List<Order> orders = new List<Order>();
        private JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();

        public string GetCustomers()
        {
            Console.WriteLine("Received GetCustomers request");
            return javaScriptSerializer.Serialize(customers);
        }

        public string GetOrder(string customerName)
        {
            Console.WriteLine("Received GetOrder request");
            Customer customer = customers.Find(p => p.Surname == customerName);
            return javaScriptSerializer.Serialize(customer.Orders);
        }
    }
}
