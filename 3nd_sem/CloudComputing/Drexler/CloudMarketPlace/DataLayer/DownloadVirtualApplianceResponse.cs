//-----------------------------------------------------------------------
// <copyright file="DownloadVirtualApplianceResponse.cs" company="MD Development">
//     Copyright (c) MD Development. All rights reserved.
// </copyright>
// <author>Michael Drexler</author>
//-----------------------------------------------------------------------
namespace DataLayer
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
    public class DownloadVirtualApplianceResponse : MarketPlaceServiceResponse
    {
        /// <summary>
        /// Gets or sets the byte array.
        /// </summary>
        [DataMember]
        public byte[] ByteArray { get; set; }
    }
}
