//-----------------------------------------------------------------------
// <copyright file="AccessObject.cs" company="MD Development">
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
    public class AccessObject
    {
        /// <summary>
        /// Gets or sets the token object
        /// </summary>
        [DataMember(Name="token")]
        public TokenObject Token { get; set; }

        /// <summary>
        /// Gets or sets the service catalog
        /// </summary>
        [DataMember(Name = "serviceCatalog")]
        public List<ServiceCatalogObject> ServiceCatalog { get; set; }

        /// <summary>
        /// Gets or sets the user object
        /// </summary>
        [DataMember(Name = "user")]
        public UserObject User { get; set; }

        /// <summary>
        /// Gets or sets the meta data
        /// </summary>
        [DataMember(Name = "metadata")]
        public MetaDataObject MetaData { get; set; }
    }
}
