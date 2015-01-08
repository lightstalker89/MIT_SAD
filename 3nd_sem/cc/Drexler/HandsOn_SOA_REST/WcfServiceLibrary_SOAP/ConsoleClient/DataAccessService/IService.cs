using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleClient.ServiceRef;
//using DataAccessLayer.Models;

namespace ConsoleClient.DataAccessService
{
    public interface IService
    {
        
        string TestMessage(int value);

        Customer[] GetAllCustomers();

        bool AddCustomer(Customer customer);

        bool DeleteCustomer(long customerId);

        Order[] GetOrdersForCustomer(long customerId);

        bool AddOrder(long customerId, Order order);

        bool DeleteOrder(long customerId, long orderId);
    }
}
