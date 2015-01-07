//-----------------------------------------------------------------------
// <copyright file="MarketPlaceServiceResponse.cs" company="MD Development">
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

    [DataContract]
    public class MarketPlaceServiceResponse
    {
        /// <summary>
        /// Gets or sets a value indicating whether error.
        /// </summary>
        [DataMember]
        public bool Error { get; set; }

        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        [DataMember]
        public string ErrorMessage { get; set; }
    }
}
