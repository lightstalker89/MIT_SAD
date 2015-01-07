//-----------------------------------------------------------------------
// <copyright file="Customer.cs" company="MD Development">
//     Copyright (c) MD Development. All rights reserved.
// </copyright>
// <author>Michael Drexler</author>
//-----------------------------------------------------------------------
namespace DataAccessLayer.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Runtime.Serialization;
    using System.Threading;

    /// <summary>
    /// Provides information about a customer
    /// </summary>
    [DataContract]
    public class Customer
    {
        /// <summary>
        /// Gets or sets an unique customer id
        /// </summary>
        [DataMember]
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets a value for the fore name of a customer
        /// </summary>
        [DataMember]
        public string Forename { get; set; }

        /// <summary>
        /// Gets or sets a value for the last name of a customer
        /// </summary>
        [DataMember]
        public string Lastname { get; set; }

        /// <summary>
        /// Gets or sets the orders for the customer (Navigation property - 1:n)
        /// </summary>
        [DataMember]
        public virtual List<Order> CustomerOrders { get; set; }
    }
}
