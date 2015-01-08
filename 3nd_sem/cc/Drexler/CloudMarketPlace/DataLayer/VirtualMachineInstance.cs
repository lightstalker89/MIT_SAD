//-----------------------------------------------------------------------
// <copyright file="VirtualMachineInstance.cs" company="MD Development">
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
    /// All informations about a virtual machine instance
    /// </summary>
    [DataContract]
    public class VirtualMachineInstance : VirtualMachine
    {
        /// <summary>
        /// Gets or sets the instance/server id
        /// </summary>
        [DataMember]
        public string InstanceID { get; set; }
    }
}
