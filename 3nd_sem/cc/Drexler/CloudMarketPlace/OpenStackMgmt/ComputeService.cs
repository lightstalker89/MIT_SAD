//-----------------------------------------------------------------------
// <copyright file="ComputeService.cs" company="MD Development">
//     Copyright (c) MD Development. All rights reserved.
// </copyright>
// <author>Michael Drexler</author>
//-----------------------------------------------------------------------
namespace OpenStackMgmt
{
    using DataLayer.OpenStack.Models;
    using DataLayer.OpenStack.RequestHandling;
    using DataLayer.OpenStack.ResponseHandling;
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
        private Uri computeServiceURL;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="computeServiceURL"></param>
        public ComputeService(string computeServiceURL)
        {
            this.computeServiceURL = new Uri(computeServiceURL);
        }

        /// <summary>
        /// Starts a stopped server and changes its status to ACTIVE
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="serverId"></param>
        /// <returns></returns>
        public string StartServer(string tenantId, string serverId, AccessObject identity)
        {
            try
            {
                if (identity == null)
                {
                    return "Invalid identity!";
                }

                string message = string.Empty;
                string relativeStartServerURL = string.Format("{0}/servers/{1}/action", tenantId, serverId);
                // string relativeStartServerURL = string.Format("ae14d396034245cb93cbaa778a70d733/servers/{0}/action", serverId);
                string charSign = this.computeServiceURL.PathAndQuery == "/" ? string.Empty : "/";
                this.computeServiceURL = new Uri(this.computeServiceURL, this.computeServiceURL.PathAndQuery + charSign + relativeStartServerURL);

                this.request = (HttpWebRequest)WebRequest.Create(this.computeServiceURL);
                this.request.Method = "POST";
                this.request.ContentType = "application/json";
                ((HttpWebRequest)this.request).Accept = "application/json";
                //this.request.Headers.Add("accept", "application/json");
                this.request.Headers.Add("x-auth-token", identity.Token.Id);

                StartServerRequest requestBody = new StartServerRequest();
                requestBody.OSStart = null;

                this.serializer = new DataContractJsonSerializer(typeof(StartServerRequest));
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

        /// <summary>
        /// Stops a running server and changes its status to STOPPED
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="serverId"></param>
        /// <returns></returns>
        public string StopServer(string tenantId, string serverId, AccessObject identity)
        {
            try
            {
                string message = string.Empty;
                string relativeStartServerURL = string.Format("{0}/servers/{1}/action", tenantId, serverId);
                string charSign = this.computeServiceURL.PathAndQuery == "/" ? string.Empty : "/";
                this.computeServiceURL = new Uri(this.computeServiceURL, this.computeServiceURL.PathAndQuery + charSign + relativeStartServerURL);

                this.request = (HttpWebRequest)WebRequest.Create(this.computeServiceURL);
                this.request.Method = "POST";
                this.request.ContentType = "application/json";
                ((HttpWebRequest)this.request).Accept = "application/json";
                this.request.Headers.Add("x-auth-token", identity.Token.Id);

                StopServerRequest requestBody = new StopServerRequest();
                requestBody.OSStop = null;

                this.serializer = new DataContractJsonSerializer(typeof(StopServerRequest));
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

        /// <summary>
        /// Lists IDs, names, and links for all servers.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public ListServersResponseObject ListServers(ListServersObject parameters, AccessObject identity)
        {
            if (identity == null)
            {
                return null;
            }

            try
            {
                string relativeListServerPath = string.Format("{0}/servers", identity.Token.Tenant.Id);
                string charSign = this.computeServiceURL.PathAndQuery == "/" ? string.Empty : "/";
                this.computeServiceURL = new Uri(this.computeServiceURL, this.computeServiceURL.PathAndQuery + charSign + relativeListServerPath);
                string message = string.Empty;

                this.request = (HttpWebRequest)WebRequest.Create(this.computeServiceURL);
                this.request.Method = "GET";
                this.request.ContentType = "application/json; charset=utf-8";
                ((HttpWebRequest)this.request).Accept = "application/json";
                this.request.Headers.Add("x-auth-token", identity.Token.Id);

                this.response = this.request.GetResponse();
                this.stream = this.response.GetResponseStream();

                this.serializer = new DataContractJsonSerializer(typeof(ListServersResponseObject));
                var servers = (ListServersResponseObject)this.serializer.ReadObject(stream);

                return servers;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private T PerformRequest<T>(string method, string requestUri, string contentType, string accept, string xAuthToken)
        {
            T responseContent;
            HttpWebRequest request = WebRequest.Create(requestUri) as HttpWebRequest;
            request.Accept = accept;
            request.Method = method;
            request.ContentType = contentType;
            request.Headers.Add("x-auth-token", xAuthToken);
            WebResponse response = request.GetResponse();
            using (Stream stream = response.GetResponseStream())
            {
                var serializer = new DataContractJsonSerializer(typeof(T));
                responseContent = (T)serializer.ReadObject(stream);
            }

            return responseContent;
        }

        //private T PerformRequest<T, U>(string method, string requestUri, string contentType, string accept, string xAuthToken, U requestContent)
        //{
        //    T responseContent;
        //    HttpWebRequest request = WebRequest.Create(requestUri) as HttpWebRequest;
        //    request.Accept = accept;
        //    request.Method = method;
        //    request.ContentType = contentType;
        //    request.Headers.Add("x-auth-token", xAuthToken);

        //    string message;

        //    serializer = new DataContractJsonSerializer(typeof(U));
        //    using (var mStream = new MemoryStream())
        //    {
        //        using (StreamReader sReader = new StreamReader(mStream))
        //        {
        //            serializer.WriteObject(mStream, requestContent);
        //            mStream.Position = 0;
        //            message = sReader.ReadToEnd();
        //        }
        //    }

        //    using (var streamwriter = new StreamWriter(request.GetRequestStream()))
        //    {
        //        streamwriter.Write(message);
        //    }

        //    WebResponse response = request.GetResponse();
        //    using (Stream stream = response.GetResponseStream())
        //    {
        //        var serializer = new DataContractJsonSerializer(typeof(T));
        //        responseContent = (T)serializer.ReadObject(stream);
        //    }

        //    return responseContent;
        //}

        //private T GetResponse<T>()
        //{
        //}
    }
}
