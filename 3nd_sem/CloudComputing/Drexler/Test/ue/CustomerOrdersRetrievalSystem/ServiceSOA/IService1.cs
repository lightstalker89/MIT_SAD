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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IService1
    {
        [OperationContract]
        List<Customer> GetCustomers();

        [OperationContract]
        List<Order> GetOrders(string customerName);

        [OperationContract]
        bool AddOrder(Order order);

        [OperationContract]
        bool AddCustomer(Customer customer);
        
        [OperationContract]
        bool DeleteOrder(Order order);

        [OperationContract]
        bool DeleteCustomer(Customer customer);

    }
}
