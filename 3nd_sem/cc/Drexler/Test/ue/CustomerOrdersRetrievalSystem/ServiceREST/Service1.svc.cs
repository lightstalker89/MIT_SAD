using ServiceData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace ServiceREST
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class Service1 : IService1
    {
        private List<Customer> customers = new List<Customer>();

        private Dictionary<String, List<Order>> orders = new Dictionary<string,List<Order>>();

        ///// <summary>
        ///// The <see cref="Customer"/> list.
        ///// </summary>
        //private readonly List<Customer> customerList = new List<Customer>();

        ///// <summary>
        ///// The <see cref="Order"/> list.
        ///// </summary>
        //private readonly List<Order> orderList = new List<Order>();

        ///// <summary>
        ///// The add customer.
        ///// </summary>
        ///// <param name="customerToAdd">
        ///// The customer.
        ///// </param>
        ///// <returns>
        ///// The <see cref="bool"/>.
        ///// </returns>
        //public bool AddCustomer(Customer customerToAdd)
        //{
        //    if (this.customerList.Any(customerTmp => customerTmp.Name == customerToAdd.Name))
        //    {
        //        return false;
        //    }

        //    this.customerList.Add(customerToAdd);
        //    return true;
        //}

        ///// <summary>
        ///// The delete customer.
        ///// </summary>
        ///// <param name="customerToDelete">
        ///// The customer.
        ///// </param>
        ///// <returns>
        ///// The <see cref="bool"/>.
        ///// </returns>
        //public bool DeleteCustomer(Customer customerToDelete)
        //{
        //    if (this.customerList.All(tmpCustomer => tmpCustomer.Name != customerToDelete.Name))
        //    {
        //        return false;
        //    }

        //    this.customerList.RemoveAll(tmpCustomer => tmpCustomer.Name == customerToDelete.Name);
        //    this.orderList.RemoveAll(order => order.CustomerName == customerToDelete.Name);
        //    return true;
        //}

        ///// <summary>
        ///// The get customers.
        ///// </summary>
        ///// <returns>
        ///// List of all <see cref="Customer"/>
        ///// </returns>
        //public List<Customer> GetCustomers()
        //{
        //    return this.customerList;
        //}

        ///// <summary>
        ///// The add order.
        ///// </summary>
        ///// <param name="orderToAdd">
        ///// The order.
        ///// </param>
        ///// <returns>
        ///// The <see cref="bool"/>.
        ///// </returns>
        //public bool AddOrder(Order orderToAdd)
        //{
        //    // If ordered twice from the same customer or the customer doesnt exists - return false.
        //    //if (this.orderList.Any(tmpOrder => tmpOrder.CustomerName == orderToAdd.CustomerName && tmpOrder.Name == orderToAdd.Name) ||
        //    //    this.customerList.All(tmpCustomer => tmpCustomer.Name != orderToAdd.CustomerName))
        //    //{
        //    //    return false;
        //    //}

        //    this.orderList.Add(orderToAdd);
        //    return true;
        //}

        ///// <summary>
        ///// The delete order.
        ///// </summary>
        ///// <param name="orderToDelete">
        ///// The order.
        ///// </param>
        ///// <returns>
        ///// The <see cref="bool"/>.
        ///// </returns>
        //public bool DeleteOrder(Order orderToDelete)
        //{
        //    if (this.orderList.All(tmpOrder => tmpOrder.Name != orderToDelete.Name))
        //    {
        //        return false;
        //    }

        //    this.orderList.RemoveAll(order => order.Name == orderToDelete.Name);
        //    return true;
        //}

        ///// <summary>
        ///// The get orders.
        ///// </summary>
        ///// <param name="customerName">
        ///// The customer name.
        ///// </param>
        ///// <returns>
        ///// List of all <see cref="Order"/> from the related customer name.
        ///// </returns>
        //public List<Order> GetOrders(string customerName)
        //{
        //    return this.orderList.FindAll(order => order.CustomerName == customerName);
        //}

        //public bool AddCustomer(Customer addCustomer)
        //{
        //    throw new NotImplementedException();
        //}

        //public bool DeleteCustomer(Customer deleteCustomer)
        //{
        //    throw new NotImplementedException();
        //}

        //public List<Customer> GetCustomers()
        //{
        //    throw new NotImplementedException();
        //}

        //public bool AddOrder(Order orderToAdd)
        //{
        //    throw new NotImplementedException();
        //}

        //public bool DeleteOrder(Order order)
        //{
        //    throw new NotImplementedException();
        //}

        //public List<Order> GetOrders(string customerName)
        //{
        //    throw new NotImplementedException();
        //}
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
            if(customers.Any(x => x.Name == customer.Name))
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
