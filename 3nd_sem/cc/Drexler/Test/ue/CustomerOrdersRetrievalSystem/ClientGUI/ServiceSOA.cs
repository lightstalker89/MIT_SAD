using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientGUI
{
    class ServiceSOA : IService
    {
        private ServiceSOAReference.Service1Client service;

        public ServiceSOA()
        {
            service = new ServiceSOAReference.Service1Client();
        }

        public List<ServiceData.Customer> GetCustomers()
        {
            return service.GetCustomers().ToList();
        }

        public List<ServiceData.Order> GetOrders(string customerName)
        {
            return service.GetOrders(customerName).ToList();
        }

        public bool AddOrder(ServiceData.Order order)
        {
            return service.AddOrder(order);
        }

        public bool AddCustomer(ServiceData.Customer customer)
        {
            return service.AddCustomer(customer);
        }

        public bool DeleteOrder(ServiceData.Order order)
        {
            return service.DeleteOrder(order);
        }

        public bool DeleteCustomer(ServiceData.Customer customer)
        {
            return service.DeleteCustomer(customer);
        }
    }
}
