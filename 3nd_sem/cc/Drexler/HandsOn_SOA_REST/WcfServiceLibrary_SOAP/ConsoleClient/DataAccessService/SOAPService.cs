//-----------------------------------------------------------------------
// <copyright file="SOAPService.cs" company="MD Development">
//     Copyright (c) MD Development. All rights reserved.
// </copyright>
// <author>Michael Drexler</author>
//-----------------------------------------------------------------------
namespace ConsoleClient.DataAccessService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Collections.ObjectModel;
    using ServiceRef;

    public class SOAPService : IService
    {
        private CustomerOrderRetrievalClient clientProxy;

        public SOAPService()
        {
            clientProxy = new CustomerOrderRetrievalClient();
        }

        /// <summary>
        /// Send and get test message from SOAP service
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public string TestMessage(int value)
        {
            return this.clientProxy.TestMessage(value);
        }

        /// <summary>
        /// Get a list of customers from SOAP service
        /// </summary>
        /// <returns></returns>
        public Customer[] GetAllCustomers()
        {
            return clientProxy.GetAllCustomers();
        }

        /// <summary>
        /// Add a new customer via the SOAP service
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        public bool AddCustomer(Customer customer)
        {
            if (customer == null)
            {
                return false;
            }

            return this.clientProxy.AddCustomer(customer);
        }

        /// <summary>
        /// Delete a customer via the SOAP service
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        public bool DeleteCustomer(long customerId)
        {
            return this.clientProxy.DeleteCustomer(customerId);
        }

        public Order[] GetOrdersForCustomer(long customerId)
        {
            return this.clientProxy.GetOrdersForCustomer(customerId);
        }

        /// <summary>
        /// Add a new order to a customer
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public bool AddOrder(long customerId, Order order)
        {
            if (order == null)
            {
                return false;
            }

            return this.clientProxy.AddOrder(customerId, order);
        }

        /// <summary>
        /// Delete a order for a customer
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public bool DeleteOrder(long customerId, long orderId)
        {
            return this.clientProxy.DeleteOrder(customerId, orderId);
        }
    }
}
