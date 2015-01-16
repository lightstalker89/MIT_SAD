//-----------------------------------------------------------------------
// <copyright file="EndpointObject.cs" company="MD Development">
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
    public class EndpointObject
    {
        /// <summary>
        /// Gets or sets the admin URL
        /// </summary>
        [DataMember(Name="adminURL")]
        public string AdminURL { get; set; }

        /// <summary>
        /// Gets or sets the region
        /// </summary>
        [DataMember(Name="region")]
        public string Region { get; set; }

        /// <summary>
        /// Gets or sets the internal URL
        /// </summary>
        [DataMember(Name="internalURL")]
        public string InternalURL { get; set; }

        /// <summary>
        /// Gets or sets the endpoint Id
        /// </summary>
        [DataMember(Name="id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the public URL
        /// </summary>
        [DataMember(Name="publicURL")]
        public string PublicURL { get; set; }
    }
}
