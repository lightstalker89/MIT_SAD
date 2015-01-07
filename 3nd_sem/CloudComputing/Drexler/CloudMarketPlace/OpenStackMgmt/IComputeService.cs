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
        string StartServer(string tenantId, string serverId, IdentityObject identity);

        /// <summary>
        /// Stops a running server and changes its status to STOPPED    
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="serverId"></param>
        /// <returns></returns>
        string StopServer(string tenantId, string serverId, IdentityObject identity);
    }
}
