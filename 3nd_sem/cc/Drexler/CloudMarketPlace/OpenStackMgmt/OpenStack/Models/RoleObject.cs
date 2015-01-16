//-----------------------------------------------------------------------
// <copyright file="RoleObject.cs" company="MD Development">
//     Copyright (c) MD Development. All rights reserved.
// </copyright>
// <author>Michael Drexler</author>
//-----------------------------------------------------------------------
namespace DataLayer.OpenStack.Models
{
    using System.Runtime.Serialization;

    [DataContract]
    public class RoleObject
    {
        /// <summary>
        /// Gets or sets a role name
        /// </summary>
        [DataMember(Name="name")]
        public string Name { get; set; }
    }
}
