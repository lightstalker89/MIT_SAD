// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VirtualMachineInstance.cs" company="FHWN">
//   Felber, Knopf, Popovic
// </copyright>
// <summary>
//   Defines the VirtualMachineInstance type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace CommonModel
{
    using System.Runtime.Serialization;

    /// <summary>
    /// The virtual machine instance.
    /// </summary>
    [DataContract]
    public class VirtualMachineInstance : VirtualMachine
    {
        /// <summary>
        /// Gets or sets the instance id.
        /// </summary>
        [DataMember]
        public string InstanceId { get; set; }
    }
}
