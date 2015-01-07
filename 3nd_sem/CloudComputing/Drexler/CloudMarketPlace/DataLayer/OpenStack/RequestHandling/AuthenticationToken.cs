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
    using System.Web;

    /// <summary>
    /// This class <see cref="AuthenticationToken"/> provides information to authenticate against the
    /// OpenStack Identity service
    /// </summary>
    public class AuthenticationToken
    {
        /// <summary>
        /// Gets or sets the username
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the tenantId
        /// </summary>
        public string TenantId { get; set; }

        /// <summary>
        /// Gets or sets the region
        /// </summary>
        public string Region { get; set; }
    }
}