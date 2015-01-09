//-----------------------------------------------------------------------
// <copyright file="AuthenticationResponse.cs" company="MD Development">
//     Copyright (c) MD Development. All rights reserved.
// </copyright>
// <author>Michael Drexler</author>
//-----------------------------------------------------------------------
namespace DataLayer.OpenStack.ResponseHandling
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Text;
    using System.Threading.Tasks;
    using DataLayer.OpenStack.Models;

    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class AuthenticationResponse
    {
        /// <summary>
        /// Gets or sets the access authentication object
        /// </summary>
        [DataMember(Name="access")]
        public AccessObject Access { get; set; }
    }
}
