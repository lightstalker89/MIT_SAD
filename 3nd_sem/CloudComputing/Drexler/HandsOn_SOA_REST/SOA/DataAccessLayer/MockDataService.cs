//-----------------------------------------------------------------------
// <copyright file="MockDataService.cs" company="MD Development">
//     Copyright (c) MD Development. All rights reserved.
// </copyright>
// <author>Michael Drexler</author>
//-----------------------------------------------------------------------
namespace DataAccessLayer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using DataAccessLayer.Models;

    /// <summary>
    /// Provides mock data for development
    /// </summary>
    public class MockDataService : IDataAccessService
    {
        /// <summary>
        /// Get all customers
        /// </summary>
        /// <returns>A list of customers</returns>
        public List<Customer> GetCustomers()
        {
            return new List<Customer>()
            {
                new Customer(){ Forename = "Manfred", Lastname = "Muster"},
                new Customer(){ Forename = "Markus", Lastname = "Lechner"},
                new Customer(){ Forename = "Matthias", Lastname = "Raab"},
                new Customer(){ Forename = "Alexander", Lastname = "Pilhar"},
                new Customer(){ Forename = "Hannes", Lastname = "Knopf"},
                new Customer(){ Forename = "Carina", Lastname = "Hafner"},
            };
        }

        /// <summary>
        /// Gett all orders
        /// </summary>
        /// <returns>A list of orders</returns>
        public List<Order> GetOrders()
        {
            return new List<Order>()
            {
                new Order(){ OrderId = 1, OrderName = "Headphones"},
                new Order(){ OrderId = 2, OrderName = "Laptop"},
                new Order(){ OrderId = 3, OrderName = "Book"},
                new Order(){ OrderId = 4, OrderName = "Tablet"},
                new Order(){ OrderId = 5, OrderName = "Notebook"},
                new Order(){ OrderId = 6, OrderName = "USB cable"}
            };
        }
    }
}
