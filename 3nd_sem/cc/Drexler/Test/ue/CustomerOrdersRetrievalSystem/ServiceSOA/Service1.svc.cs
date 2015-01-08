using ServiceData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace ServiceSOA
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class Service1 : IService1
    {
        private List<Customer> customers = new List<Customer>();

        private Dictionary<String, List<Order>> orders = new Dictionary<string, List<Order>>();

        public List<Customer> GetCustomers()
        {
            return customers;
        }

        public List<Order> GetOrders(string customerName)
        {
            return orders[customerName];
        }

        public bool AddOrder(Order order)
        {
            if (orders[order.CustomerName].Any(x => x.Name == order.Name))
            {
                return false;
            }

            orders[order.CustomerName].Add(order);
            return true;
        }

        public bool AddCustomer(Customer customer)
        {
            if (customers.Any(x => x.Name == customer.Name))
            {
                return false;
            }

            customers.Add(customer);
            orders.Add(customer.Name, new List<Order>());
            return true;
        }

        public bool DeleteOrder(Order order)
        {
            orders[order.CustomerName].RemoveAll(x => x.Name == order.Name);
            return true;
        }

        public bool DeleteCustomer(Customer customer)
        {
            customers.RemoveAll(x => x.Name == customer.Name);
            orders.Remove(customer.Name);
            return true;
        }
    }
}
