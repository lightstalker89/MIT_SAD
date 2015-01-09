//-----------------------------------------------------------------------
// <copyright file="IComputeService.cs" company="MD Development">
//     Copyright (c) MD Development. All rights reserved.
// </copyright>
// <author>Michael Drexler</author>
//-----------------------------------------------------------------------
namespace OpenStackMgmt
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Text;
    using DataLayer.OpenStack.Models;
    using DataLayer.OpenStack.ResponseHandling;
    using DataLayer.OpenStack.RequestHandling;

    /// <summary>
    /// 
    /// </summary>
    public interface IComputeService
    {
        /// <summary>
        /// Starts a stopped server and changes its status to ACTIVE
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="serverId"></param>
        /// <returns></returns>
        string StartServer(string tenantId, string serverId, AccessObject identity);

        /// <summary>
        /// Stops a running server and changes its status to STOPPED    
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="serverId"></param>
        /// <returns></returns>
        string StopServer(string tenantId, string serverId, AccessObject identity);

        /// <summary>
        /// Lists IDs, names, and links for all servers.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        ListServersResponseObject ListServers(ListServersObject parameters, AccessObject identity);
    }
}
