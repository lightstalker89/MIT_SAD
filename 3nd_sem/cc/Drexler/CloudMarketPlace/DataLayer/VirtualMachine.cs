//-----------------------------------------------------------------------
// <copyright file="VirtualMachine.cs" company="MD Development">
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
    /// This class handles all informations about a virtual machine
    /// </summary>
    [DataContract]
    public class VirtualMachine
    {
        /// <summary>
        /// Gets or sets the type of the operating system
        /// </summary>
        [DataMember]
        public string OperatingSystemType { get; set; }

        /// <summary>
        /// Gets or sets the name of the operating system
        /// </summary>
        [DataMember]
        public string OperatingSystemName { get; set; }

        /// <summary>
        /// Gets or sets the version of the operating system
        /// </summary>
        [DataMember]
        public string OperatingSystemVersion { get; set; }

        /// <summary>
        /// Gets or sets the size of the virtual machine
        /// </summary>
        [DataMember]
        public int Size { get; set; }

        /// <summary>
        /// Gets or sets the supported virtualization platforms
        /// </summary>
        [DataMember]
        public string SupportedVirtualizationPlatforms { get; set; }

        /// <summary>
        /// Gets or sets the number of the recommended CPUs
        /// </summary>
        [DataMember]
        public int RecommendedCPUCount { get; set; }

        /// <summary>
        /// Gets or sets the recommended memory
        /// </summary>
        [DataMember]
        public double RecommendedMemory { get; set; }

        /// <summary>
        /// Gets or sets the comment
        /// </summary>
        [DataMember]
        public string Comment { get; set; }

        /// <summary>
        /// Gets or sets the image id
        /// </summary>
        [DataMember]
        public string ImageID { get; set; }

        /// <summary>
        /// Gets or sets the rate
        /// </summary>
        [DataMember]
        public byte Rate { get; set; }
    }
}
