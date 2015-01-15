//-----------------------------------------------------------------------
// <copyright file="AuthenticationRequest.cs" company="MD Development">
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
    public class AuthenticationRequest
    {
        //public AuthenticationRequest()
        //{ 
        //}

        //public AuthenticationRequest(AuthenticationObject authObject) : this()
        //{
        //    this.Auth = authObject;
        //}

        /// <summary>
        /// Gets or sets the authentication request object
        /// </summary>
        [DataMember(Name="auth")]
        public AuthenticationObject Auth { get; set; }
    }
}
