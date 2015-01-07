//-----------------------------------------------------------------------
// <copyright file="IDataAccessService.cs" company="MD Development">
//     Copyright (c) MD Development. All rights reserved.
// </copyright>
// <author>Michael Drexler</author>
//-----------------------------------------------------------------------
namespace DataAccessLayer
{
    using DataAccessLayer.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Provides an interface for data access
    /// </summary>
    public interface IDataAccessService
    {
        /// <summary>
        /// Get all customers
        /// </summary>
        /// <returns>A list of customers</returns>
        List<Customer> GetCustomers();
    }
}
