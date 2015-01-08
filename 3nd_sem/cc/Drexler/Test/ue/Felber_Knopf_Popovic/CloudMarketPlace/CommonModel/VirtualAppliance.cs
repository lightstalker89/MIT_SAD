// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VirtualAppliance.cs" company="FHWN">
//   Felber, Knopf, Popovic
// </copyright>
// <summary>
//   Defines the VirtualAppliance type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace CommonModel
{
    using System.Runtime.Serialization;

    /// <summary>
    /// The virtual appliance.
    /// </summary>
    [DataContract]
    public class VirtualAppliance : VirtualMachine
    {
        /// <summary>
        /// Gets or sets the application type.
        /// </summary>
        [DataMember]
        public int ApplicationType { get; set; }

        /// <summary>
        /// Gets or sets the installed software.
        /// </summary>
        [DataMember]
        public int InstalledSoftware { get; set; }

        /// <summary>
        /// Gets or sets the supported programming languages.
        /// </summary>
        [DataMember]
        public int SupportedProgrammingLanguages { get; set; }
    }
}