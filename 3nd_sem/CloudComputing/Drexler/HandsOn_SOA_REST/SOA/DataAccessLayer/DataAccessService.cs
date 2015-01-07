//-----------------------------------------------------------------------
// <copyright file="DataAccessService.cs" company="MD Development">
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
    /// Provides data from a data source
    /// </summary>
    public class DataAccessService : IDataAccessService
    {
        public List<Customer> GetCustomers()
        {
            throw new NotImplementedException();
        }

        public List<Order> GetOrders()
        {
            throw new NotImplementedException();
        }
    }
}
