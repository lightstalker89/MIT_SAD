//-----------------------------------------------------------------------
// <copyright file="IdentityService.cs" company="MD Development">
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
    using System.Text;
    using DataLayer.OpenStack.Models;
    using DataLayer.OpenStack.RequestHandling;
    using System.IO;
    using System.Net;
    using System.Runtime.Serialization.Json;

    /// <summary>
    /// 
    /// </summary>
    public class IdentityService : IIdentityService
    {
        /// <summary>
        /// 
        /// </summary>
        private Stream stream;

        /// <summary>
        /// 
        /// </summary>
        private StreamReader sReader;

        /// <summary>
        /// 
        /// </summary>
        private WebRequest request;

        /// <summary>
        /// 
        /// </summary>
        private WebResponse response;

        /// <summary>
        /// 
        /// </summary>
        private DataContractJsonSerializer serializer;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="identityServiceURL"></param>
        /// <param name="authInformations"></param>
        /// <returns></returns>
        public IdentityObject GetAuthentication(string identityServiceURL, AuthenticationToken authInformations = null)
        {
            string message = string.Empty;
            identityServiceURL = Path.Combine(identityServiceURL, "/v2.0/tokens");
            this.request = WebRequest.Create(identityServiceURL);
            this.request.Method = "POST";
            this.request.ContentType = "application/json; charset=utf-8";
            this.serializer = new DataContractJsonSerializer(typeof(AuthenticationToken));

            if (authInformations == null)
            {
                return null;
            }

            // Serializing input parameter
            using(var mStream = new MemoryStream())
            using(this.sReader = new StreamReader(mStream))
            {
                this.serializer.WriteObject(mStream, authInformations);
                mStream.Position = 0;
                message = this.sReader.ReadToEnd();
            }

            // Assigning the stream in the request
            using (var streamWriter = new StreamWriter(this.request.GetRequestStream()))
            {
                streamWriter.Write(message);
            }

            // Get the response from the request
            this.response = this.request.GetResponse();

            // Reading the result from the request
            this.stream = this.response.GetResponseStream();
            this.serializer = new DataContractJsonSerializer(typeof(IdentityObject));
            var identity = (IdentityObject)serializer.ReadObject(this.stream);
            
            return identity;
        }
    }
}
