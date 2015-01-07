//-----------------------------------------------------------------------
// <copyright file="IdentityObject.cs" company="MD Development">
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
    public class IdentityObject
    {
        /// <summary>
        /// Gets or sets the access object
        /// </summary>
        [DataMember]
        public AccessObject Access { get; set; }

        /// <summary>
        /// Gets or sets the service catalog
        /// </summary>
        [DataMember]
        public List<ServiceCatalogObject> ServiceCatalog { get; set; }

        /// <summary>
        /// Gets or sets the user object
        /// </summary>
        [DataMember]
        public UserObject User { get; set; }

        /// <summary>
        /// Gets or sets the meta data
        /// </summary>
        [DataMember]
        public MetaDataObject MetaData { get; set; }
    }
}
