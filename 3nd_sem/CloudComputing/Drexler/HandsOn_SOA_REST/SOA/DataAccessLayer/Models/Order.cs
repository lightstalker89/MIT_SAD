//-----------------------------------------------------------------------
// <copyright file="Orders.cs" company="MD Development">
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
    /// Provides all information about an order
    /// </summary>
    [DataContract]
    public class Order
    {
        /// <summary>
        /// Gets or sets a value for the OrderID
        /// </summary>
        [DataMember]
        public long OrderId { get; set; }

        /// <summary>
        /// Gets or sets a value for the order name
        /// </summary>
        [DataMember]
        public string OrderName { get; set; }

        /// <summary>
        /// Gets or sets a value for the customer (Navigation Propertie - n:1)
        /// </summary>
        [IgnoreDataMember]
        public virtual Customer Customer { get; set; }
    }
}
