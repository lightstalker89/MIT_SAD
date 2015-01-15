//-----------------------------------------------------------------------
// <copyright file="TokenObject.cs" company="MD Development">
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
    public class TokenObject
    {
        /// <summary>
        /// Gets or sets a timestamp that indicates when the token was issued. 
        /// </summary>
        [DataMember(Name="issued_at")]
        public string Issued_at { get; set; }

        /// <summary>
        /// Gets or sets a timestamp that indicates when the token expires.
        /// </summary>
        [DataMember(Name="expires")]
        public string Expires { get; set; }

        /// <summary>
        /// Gets or sets the authentication token.
        /// </summary>
        [DataMember(Name="id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the tenant 
        /// </summary>
        [DataMember(Name="tenant")]
        public TenantObject Tenant { get; set; }
    }
}
