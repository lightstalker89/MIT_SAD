//-----------------------------------------------------------------------
// <copyright file="PasswordCredentialObject.cs" company="MD Development">
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
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class PasswordCredentialObject
    {
        /// <summary>
        /// Gets or sets the username 
        /// </summary>
        [DataMember(Name="username")]
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the password
        /// </summary>
        [DataMember(Name="password")]
        public string Password { get; set; }
    }
}
