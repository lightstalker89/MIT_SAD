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
        /// Containing a list of customers
        /// </summary>
        private List<Customer> customers;

        /// <summary>
        /// Get all customers
        /// </summary>
        /// <returns>A list of customers</returns>
        public List<Customer> GetCustomers()
        {
            if (this.customers == null)
            {
                this.customers = new List<Customer>()
                {
                    new Customer(){ Id = 1, Forename = "Manfred", Lastname = "Muster"},
                    new Customer(){ Id = 2, Forename = "Markus", Lastname = "Lechner"},
                    new Customer(){ Id = 3, Forename = "Matthias", Lastname = "Raab"},
                    new Customer(){ Id = 4, Forename = "Alexander", Lastname = "Pilhar"},
                    new Customer(){ Id = 5, Forename = "Hannes", Lastname = "Knopf"},
                    new Customer(){ Id = 6, Forename = "Carina", Lastname = "Hafner"},
                };
            }           

            return this.customers;
        }
    }
}
