//-----------------------------------------------------------------------
// <copyright file="IIdentityService.cs" company="MD Development">
//     Copyright (c) MD Development. All rights reserved.
// </copyright>
// <author>Michael Drexler</author>
//-----------------------------------------------------------------------
namespace OpenStackManagement
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.ServiceModel;
    using System.ServiceModel.Web;
    using System.Text;
    using DataLayer.OpenStack.Models;
    using DataLayer.OpenStack.RequestHandling;
   
    /// <summary>
    /// 
    /// </summary>
    [ServiceContract]
    public interface IIdentityService
    {
        /// <summary>
        /// Calls the OpenStack Identity Service /v2.0/tokens operation to get an authenticated identity
        /// </summary>
        /// <param name="identityServiceURL"></param>
        /// <param name="authInformations"></param>
        /// <returns></returns>
        [OperationContract]  
        IdentityObject GetAuthentication(string identityServiceURL, AuthenticationToken authInformations = null);
    }
}
