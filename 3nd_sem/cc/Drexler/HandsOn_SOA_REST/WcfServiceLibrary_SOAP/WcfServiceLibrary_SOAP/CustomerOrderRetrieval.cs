//-----------------------------------------------------------------------
// <copyright file="Syncer.cs" company="MD Development">
//     Copyright (c) MD Development. All rights reserved.
// </copyright>
// <author>Michael Drexler</author>
//-----------------------------------------------------------------------
namespace ServiceLibrary
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.ServiceModel;
    using System.ServiceModel.Web;
    using System.Text;
    using DataAccessLayer.Models;
    using DataAccessLayer;

    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in both code and config file together.
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Single, InstanceContextMode = InstanceContextMode.Single)]
    public class CustomerOrderRetrieval : ICustomerOrderRetrieval
    {
        /// <summary>
        /// Data access context
        /// </summary>
        private IDataAccessService database = new MockDataService();

        /// <summary>
        /// Service holds mock data within a list
        /// </summary>
        private List<Customer> customers = null;

        /// <summary>
        /// Send a test message to check if service is running
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public string TestMessage(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        /// <summary>
        /// Get a list of all customers
        /// </summary>
        /// <returns></returns>
        public List<Customer> GetAllCustomers()
        {
           if(database == null)
           {
               throw new Exception("No available data provider!");
           }

           if(this.customers == null)
           {
               this.customers = database.GetCustomers();
           }

           return this.customers;
        }

        /// <summary>
        /// Add a new customer to the database
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        public bool AddCustomer(Customer customer)
        {
            if(customer == null)
            {
                throw new ArgumentNullException("customer");
            }

            if(customers != null)
            {
                customers.Add(customer);

                return true;
            }

            return false;
        }

        /// <summary>
        /// Delete a specific customer from the database
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        public bool DeleteCustomer(long customerId)
        {
            Customer c = this.customers.Where(m => m.Id == customerId).SingleOrDefault();

            if(c == null)
            {
                return false;
            }

            return this.customers.Remove(c);
        }

        /// <summary>
        /// Get a list of orders for a given customer
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public List<Order> GetOrdersForCustomer(long customerId)
        {
            List<Order> orders = database.GetCustomers().Where(m => m.Id == customerId).Select(m => m.CustomerOrders).FirstOrDefault();

            return orders;
        }

        /// <summary>
        /// Add a new Order from a customer to the database
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public bool AddOrder(long customerId, Order order)
        {
            if(order == null)
            {
                throw new ArgumentNullException("order");
            }

            var customer = database.GetCustomers().Where(m => m.Id == customerId).Select(m => m).FirstOrDefault();

            if(customer != null)
            {
                if (customer.CustomerOrders == null)
                {
                    customer.CustomerOrders = new List<Order>();
                }

                customer.CustomerOrders.Add(order);

                return true;
            }

            return false;
        }

        /// <summary>
        /// Delete an order of a specific customer from the database
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        public bool DeleteOrder(long customerId, long orderId)
        {
            //if (order == null)
            //{
            //    throw new ArgumentNullException("order");
            //}

            var customer = this.customers.Where(m => m.Id == customerId).SingleOrDefault();

            if (customer == null)
            {
                return false;
            }

            if (customer.CustomerOrders == null || customer.CustomerOrders.Count == 0)
            {
                return false;
            }

            var order = customer.CustomerOrders.Where(m => m.OrderId == orderId).SingleOrDefault();

            if (order == null)
            {
                return false;
            }

            return customer.CustomerOrders.Remove(order);


            //if(customers != null)
            //{
            //    foreach (var customer in customers)
            //    {
            //        if (customer.CustomerOrders != null)
            //        {
            //            foreach (var corder in customer.CustomerOrders)
            //            {
            //                if (corder.OrderId == order.OrderId)
            //                {
            //                    return customer.CustomerOrders.Remove(order);
            //                }
            //            }
                        
            //        }
            //    }
            //}

            //return false;
        }
    }
}
