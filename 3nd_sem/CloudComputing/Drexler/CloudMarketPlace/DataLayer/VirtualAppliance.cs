//-----------------------------------------------------------------------
// <copyright file="VirtualAppliance.cs" company="MD Development">
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
    /// This class handles all informations about virtual appliances
    /// </summary>
    [DataContract]
    public class VirtualAppliance : VirtualMachine
    {
        /// <summary>
        /// Gets or sets the application type
        /// </summary>
        [DataMember]
        public int ApplicationType { get; set; }

        /// <summary>
        /// Gets or sets the installed software
        /// </summary>
        [DataMember]
        public int InstalledSoftware { get; set; }

        /// <summary>
        /// Gets or sets the supported programming languages
        /// </summary>
        [DataMember]
        public int SupportedProgrammingLanguages { get; set; }
    }
}
