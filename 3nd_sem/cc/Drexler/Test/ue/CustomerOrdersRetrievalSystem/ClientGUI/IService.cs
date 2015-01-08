using ServiceData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientGUI
{
    interface IService
    {
        List<Customer> GetCustomers();

        List<Order> GetOrders(string customerName);

        bool AddOrder(Order order);

        bool AddCustomer(Customer customer);

        bool DeleteOrder(Order order);

        bool DeleteCustomer(Customer customer);
    }
}
