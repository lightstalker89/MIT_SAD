//-----------------------------------------------------------------------
// <copyright file="ComputeService.cs" company="MD Development">
//     Copyright (c) MD Development. All rights reserved.
// </copyright>
// <author>Michael Drexler</author>
//-----------------------------------------------------------------------
namespace OpenStackMgmt
{
    using DataLayer.OpenStack.Models;
    using OpenStackMgmt;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Json;
    using System.Text;

    /// <summary>
    /// 
    /// </summary>
    public class ComputeService : IComputeService
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
        private string computeServiceURL;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="computeServiceURL"></param>
        public ComputeService(string computeServiceURL)
        {
            this.computeServiceURL = computeServiceURL;
        }

        /// <summary>
        /// Starts a stopped server and changes its status to ACTIVE
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="serverId"></param>
        /// <returns></returns>
        public string StartServer(string tenantId, string serverId, IdentityObject identity)
        {
            try
            {
                if (identity == null)
                {
                    return "Invalid identity!";
                }

                string message = string.Empty;
                string relativeStartServerURL = string.Format("/{0}/servers/{1}/start/", tenantId, serverId);
                this.computeServiceURL = Path.Combine(this.computeServiceURL, relativeStartServerURL);

                this.request = (HttpWebRequest)WebRequest.Create(this.computeServiceURL);
                this.request.Method = "POST";
                this.request.ContentType = @"application/json; charset=utf-8";
                this.request.Headers.Add(string.Format("x-auth-token:{0}",identity.Access.Token.Id));

                KeyValuePair<string, string> requestBody = new KeyValuePair<string,string>("os-stop",null);

                this.serializer = new DataContractJsonSerializer(typeof(KeyValuePair<string,string>));
                using (var mStream = new MemoryStream())
                using (this.sReader = new StreamReader(mStream))
                {
                    this.serializer.WriteObject(mStream, requestBody);
                    mStream.Position = 0;
                    message = this.sReader.ReadToEnd();
                }

                using (var streamwriter = new StreamWriter(this.request.GetRequestStream()))
                {
                    streamwriter.Write(message);
                }

                this.response = this.request.GetResponse();
                this.stream = this.response.GetResponseStream();
                this.sReader = new StreamReader(this.stream);

                message = this.sReader.ReadToEnd();

                return message;
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Stops a running server and changes its status to STOPPED
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="serverId"></param>
        /// <returns></returns>
        public string StopServer(string tenantId, string serverId, IdentityObject identity)
        {
            try
            {
                string message = string.Empty;
                string relativeStartServerURL = string.Format("/{0}/servers/{1}/stop/", tenantId, serverId);
                this.computeServiceURL = Path.Combine(this.computeServiceURL, relativeStartServerURL);

                this.request = (HttpWebRequest)WebRequest.Create(this.computeServiceURL);
                this.request.Method = "POST";
                this.request.ContentType = @"application/json; charset=utf-8";
                this.request.Headers.Add(string.Format("x-auth-token:{0}", identity.Access.Token.Id));

                KeyValuePair<string, string> requestBody = new KeyValuePair<string, string>("os-stop", null);

                this.serializer = new DataContractJsonSerializer(typeof(KeyValuePair<string, string>));
                using (var mStream = new MemoryStream())
                using (this.sReader = new StreamReader(mStream))
                {
                    this.serializer.WriteObject(mStream, requestBody);
                    mStream.Position = 0;
                    message = this.sReader.ReadToEnd();
                }

                using (var streamwriter = new StreamWriter(this.request.GetRequestStream()))
                {
                    streamwriter.Write(message);
                }

                this.response = this.request.GetResponse();
                this.stream = this.response.GetResponseStream();
                this.sReader = new StreamReader(this.stream);

                message = this.sReader.ReadToEnd();

                return message;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
