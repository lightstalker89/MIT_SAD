using System;
using System.Collections.Generic;
using System.Web.Services;
using ServiceModels;
using System.Web.Script.Serialization;
using System.Linq;

namespace SOAPWebService
{
    /// <summary>
    /// Summary description for SOAPWebService
    /// </summary>
    [WebService(Namespace = "http://localhost/", Description = "Basic Webservice to get customers and orders", Name = "SOAPWebService")]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class SOAPWebService : WebService
    {
        private List<Customer> customers = new List<Customer>();
        private List<Order> orders = new List<Order>();
        private JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();

        [WebMethod]
        public string GetCustomers()
        {
            Console.WriteLine("Received GetCustomers request");
            return javaScriptSerializer.Serialize(customers);
        }

        [WebMethod]
        public string GetOrder(string customerName)
        {
            Console.WriteLine("Received GetOrder request");
            Customer customer = customers.Find(p => p.Surname == customerName);
            return javaScriptSerializer.Serialize(customer.Orders);
        }
    }
}
