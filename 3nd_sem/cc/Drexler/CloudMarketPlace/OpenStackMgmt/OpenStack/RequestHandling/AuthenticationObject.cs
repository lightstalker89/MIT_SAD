//-----------------------------------------------------------------------
// <copyright file="AuthenticationToken.cs" company="MD Development">
//     Copyright (c) MD Development. All rights reserved.
// </copyright>
// <author>Michael Drexler</author>
//-----------------------------------------------------------------------
namespace DataLayer.OpenStack.RequestHandling
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Web;

    /// <summary>
    /// This class <see cref="AuthenticationObject"/> provides information to authenticate against the
    /// OpenStack Identity service
    /// </summary>
    [DataContract]
    public class AuthenticationObject
    {
        /// <summary>
        /// Gets or sets the tenant name
        /// </summary>
        [DataMember(Name="tenantName")]
        public string TenantName { get; set; }

        /// <summary>
        /// Gets or sets the password credentials
        /// </summary>
        [DataMember(Name="passwordCredentials")]
        public PasswordCredentialObject PasswordCredentials { get; set; }
    }
}