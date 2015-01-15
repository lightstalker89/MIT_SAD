//-----------------------------------------------------------------------
// <copyright file="UserObject.cs" company="MD Development">
//     Copyright (c) MD Development. All rights reserved.
// </copyright>
// <author>Michael Drexler</author>
//-----------------------------------------------------------------------
namespace DataLayer.OpenStack.Models
{
    using System.Runtime.Serialization;


    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class UserObject
    {
        /// <summary>
        /// Gets or sets the username
        /// </summary>
        [DataMember(Name="username")]
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the roles links
        /// </summary>
        [DataMember(Name="roles_links")]
        public string[] Roles_links { get; set; }

        /// <summary>
        /// Gets or sets an user Id
        /// </summary>
        [DataMember(Name="id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the roles of the user
        /// </summary>
        [DataMember(Name="roles")]
        public RoleObject Roles { get; set; }

        /// <summary>
        /// Gets or sets a name
        /// </summary>
        [DataMember(Name="name")]
        public string Name { get; set; }
    }
}
