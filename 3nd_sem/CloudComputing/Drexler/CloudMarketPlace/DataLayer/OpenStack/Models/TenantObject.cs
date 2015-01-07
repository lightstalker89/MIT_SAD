//-----------------------------------------------------------------------
// <copyright file="TenantObject.cs" company="MD Development">
//     Copyright (c) MD Development. All rights reserved.
// </copyright>
// <author>Michael Drexler</author>
//-----------------------------------------------------------------------
namespace DataLayer.OpenStack.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class TenantObject
    {
        /// <summary>
        /// Gets or sets a description for a tenant
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets a tenant enabled or disabled
        /// </summary>
        [DataMember]
        public bool Enabled { get; set; }

        /// <summary>
        /// Gets or sets a tenant id
        /// </summary>
        [DataMember]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets a name for a tenant
        /// </summary>
        [DataMember]
        public string Name { get; set; }
    }
}
