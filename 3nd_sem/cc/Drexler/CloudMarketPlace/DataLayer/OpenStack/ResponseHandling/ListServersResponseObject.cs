//-----------------------------------------------------------------------
// <copyright file="ListServersResponseObject.cs" company="MD Development">
//     Copyright (c) MD Development. All rights reserved.
// </copyright>
// <author>Michael Drexler</author>
//-----------------------------------------------------------------------
namespace DataLayer.OpenStack.ResponseHandling
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class ListServersResponseObject
    {
        /// <summary>
        /// Gets or sets the X-Compute-Request-ID in the header of a response.
        /// Returns a unique identifier to provide tracking for the request. 
        /// The request-id associated with the request appears in the log lines for that request. 
        /// By default, the middleware configuration ensures the request_id appears in the log files. 
        /// </summary>
        [DataMember]
        public string Header { get; set; }

        /// <summary>
        /// Gets or sets a list of servers
        /// </summary>
        [DataMember]
        public List<Server> Servers { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class Server
    {
        /// <summary>
        /// Gets or sets a server id
        /// </summary>
        [DataMember]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets links
        /// </summary>
        [DataMember]
        public List<Link> Links { get; set; }

        /// <summary>
        /// Gets or sets a server name
        /// </summary>
        [DataMember]
        public string Name { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class Link
    {
        /// <summary>
        /// Gets or sets a hyperlink 
        /// </summary>
        [DataMember]
        public string Href { get; set; }

        /// <summary>
        /// Gets or sets self
        /// </summary>
        [DataMember]
        public string Self { get; set; }
    }
}
