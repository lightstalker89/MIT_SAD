//-----------------------------------------------------------------------
// <copyright file="Syncer.cs" company="MD Development">
//     Copyright (c) MD Development. All rights reserved.
// </copyright>
// <author>Michael Drexler</author>
//-----------------------------------------------------------------------
namespace DataLayer.OpenStack.RequestHandling
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class ListServersObject
    {
        /// <summary>
        /// Gets or sets the tenant id in a multi-tenancy cloud
        /// </summary>
        [DataMember(Name="tenant_id")]
        public string Tenant_Id { get; set; }

        /// <summary>
        /// Gets or sets a time/date stamp for when the server last changed status
        /// </summary>
        [DataMember(Name="changes_since")]
        public DateTime Changes_since { get; set; }

        /// <summary>
        /// Gest or sets the name of the image in URL format
        /// </summary>
        [DataMember(Name="image")]
        public Uri Image { get; set; }

        /// <summary>
        /// Gets or sets the name of the flavor in URL format
        /// </summary>
        [DataMember(Name="flavor")]
        public Uri Flavor { get; set; }

        /// <summary>
        /// Gets or sets the name of the server as a string; can be queried with regular expressions. Realize that
        /// ?name=bob returns both bob and bobb. If you need to match bob only, you can use a regular expression matching the
        /// syntax of the underlying database server implemented for Compute (such as MySQL or PostgreSQL)
        /// </summary>
        [DataMember(Name="name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a UUID of the server at which you want to set a marker
        /// </summary>
        [DataMember]
        public string Marker { get; set; }

        /// <summary>
        /// Gets or sets the integer value for the limit of values to return
        /// </summary>
        [DataMember]
        public int Limit { get; set; }

        /// <summary>
        /// Gets or sets the value of the status of the server so that you can filter on "ACTIVE" for example
        /// </summary>
        [DataMember]
        public ServerStatus Status { get; set; }

        /// <summary>
        /// Gets or sets the name of the host as a string
        /// </summary>
        [DataMember]
        public string Host { get; set; }

    }

    /// <summary>
    /// 
    /// </summary>
    public enum ServerStatus
    {
        ACTIVE = 0,
        BUILDING = 1,
        DELETED = 2,
        ERROR = 3,
        HARD_REBOOT = 4,
        PASSWORD = 5,
        PAUSED = 6,
        REBOOT = 7,
        REBUILD = 8,
        RESCUED = 9,
        RESIZED = 10,
        REVERT_RESIZE = 11,
        SHUTOFF = 12,
        SOFT_DELETED = 13,
        STOPPED = 14,
        SUSPENDED = 15,
        UNKOWN = 16,
        VERIFY_RESIZE = 17
    }
}
