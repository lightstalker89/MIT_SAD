//-----------------------------------------------------------------------
// <copyright file="ServiceCatalogObject.cs" company="MD Development">
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
    public class ServiceCatalogObject
    {
        /// <summary>
        /// Gets or sets the service catalog endpoints
        /// </summary>
        [DataMember]
        public List<EndpointObject> Endpoints { get; set; }

        /// <summary>
        /// Gets or sets the endpoint links
        /// </summary>
        [DataMember]
        public string[] Endpoints_links { get; set; }

        /// <summary>
        /// Gets or sets the endpoint type
        /// </summary>
        [DataMember]
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the endpoint name
        /// </summary>
        [DataMember]
        public string Name { get; set; }
    }
}
