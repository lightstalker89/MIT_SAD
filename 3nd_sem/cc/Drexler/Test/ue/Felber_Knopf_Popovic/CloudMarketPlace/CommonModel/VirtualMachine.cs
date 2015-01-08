// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VirtualMachine.cs" company="FHWN">
//   Felber, Knopf, Popovic
// </copyright>
// <summary>
//   Defines the VirtualMachine type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace CommonModel
{
    using System.Runtime.Serialization;

    /// <summary>
    /// The virtual machine.
    /// </summary>
    [DataContract]
    public class VirtualMachine
    {
        /// <summary>
        /// Gets or sets the operating system type.
        /// </summary>
        [DataMember]
        public string OperatingSystemType { get; set; }

        /// <summary>
        /// Gets or sets the operating system name.
        /// </summary>
        [DataMember]
        public string OperatingSystemName { get; set; }

        /// <summary>
        /// Gets or sets the operating system version.
        /// </summary>
        [DataMember]
        public string OperatingSystemVersion { get; set; }

        /// <summary>
        /// Gets or sets the size.
        /// </summary>
        [DataMember]
        public int Size { get; set; }

        /// <summary>
        /// Gets or sets the supported virtualization platforms.
        /// </summary>
        [DataMember]
        public string SupportedVirtualizationPlatforms { get; set; }

        /// <summary>
        /// Gets or sets the recommended.
        /// </summary>
        [DataMember]
        public int RecommendedCpu { get; set; }

        /// <summary>
        /// Gets or sets the recommended memory.
        /// </summary>
        [DataMember]
        public double RecommendedMemory { get; set; }

        /// <summary>
        /// Gets or sets the rate.
        /// </summary>
        [DataMember]
        public byte Rate { get; set; }

        /// <summary>
        /// Gets or sets the comment.
        /// </summary>
        [DataMember]
        public string Comment { get; set; }

        /// <summary>
        /// Gets or sets the image id.
        /// </summary>
        [DataMember]
        public string ImageId { get; set; }
    }
}
