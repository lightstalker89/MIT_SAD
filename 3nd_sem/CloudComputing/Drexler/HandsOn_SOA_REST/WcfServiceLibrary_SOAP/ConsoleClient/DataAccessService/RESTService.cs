//-----------------------------------------------------------------------
// <copyright file="RESTService.cs" company="MD Development">
//     Copyright (c) MD Development. All rights reserved.
// </copyright>
// <author>Michael Drexler</author>
//-----------------------------------------------------------------------
namespace ConsoleClient.DataAccessService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Net;
    using System.Runtime.Serialization;
    using System.IO;
    using System.Runtime.Serialization.Json;
    using ServiceRef;

    public class RESTService : IService
    {
        private string serviceURL;
        private Stream stream;
        private StreamReader reader;
        private WebRequest request;
        private WebResponse response;

        public RESTService(string serviceURL)
        {
            this.serviceURL = serviceURL;
        }

        /// <summary>
        /// Send and get test message from REST Service
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public string TestMessage(int value)
        {
            string message = string.Empty;
            string functionURL = string.Format("{0}/getTest/?value={1}", this.serviceURL , value);
            request = WebRequest.Create(functionURL);
            request.Method = "GET";
            request.ContentType = "application/json; charset=utf-8";

            try
            {
                response = request.GetResponse();
                stream = response.GetResponseStream();
                reader = new StreamReader(stream);

                message = reader.ReadToEnd();
            }
            catch (Exception ex)
            {
                // Log to logfile
                return ex.Message;
            }

            return message;
        }

        /// <summary>
        /// Get a list of customer from REST Service
        /// </summary>
        /// <returns></returns>
        public Customer[] GetAllCustomers()
        {
            string functionURL = string.Format("{0}/getCustomers", this.serviceURL);

            this.request = WebRequest.Create(functionURL);
            this.request.Method = "GET";
            this.request.ContentType = "application/json; charset=utf-8";
            this.response = this.request.GetResponse();
            DataContractJsonSerializer collectionData = new DataContractJsonSerializer(typeof(Customer[]));

            this.stream = response.GetResponseStream();

            var customers = (Customer[])collectionData.ReadObject(this.stream);

            return customers;
        }

        /// <summary>
        /// Add a new customer
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        public bool AddCustomer(Customer customer)
        {
            if (customer == null)
            {
                return false;
            }

            string functionUrl = string.Format("{0}/addCustomer", this.serviceURL);

            this.request = (HttpWebRequest)WebRequest.Create(functionUrl);
            this.request.Method = "POST";
            this.request.ContentType = @"application/json; charset=utf-8";

            var serializer = new DataContractJsonSerializer(typeof(Customer));
            bool addedSuccessfully = false;
            string message = string.Empty;

            // Serializing input parameter
            using (var mstream = new MemoryStream())
            using (this.reader = new StreamReader(mstream))
            {
                serializer.WriteObject(mstream, customer);
                mstream.Position = 0; message = this.reader.ReadToEnd();
            }

            // Assigning the stream in the request
            using (var streamWriter = new StreamWriter(this.request.GetRequestStream()))
            {
                streamWriter.Write(message);
            }

            // Get the response from the request
            this.response = this.request.GetResponse();

            // Reading the result from the response
            this.stream = this.response.GetResponseStream();
            this.reader = new StreamReader(this.stream);
            message = reader.ReadToEnd();

            bool.TryParse(message, out addedSuccessfully);

            return addedSuccessfully;
        }

        /// <summary>
        /// Delete a customer form the data source
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public bool DeleteCustomer(long customerId)
        {
            string functionUrl = string.Format("{0}/deleteCustomer/?customerId={1}", this.serviceURL, customerId);

            this.request = (HttpWebRequest)WebRequest.Create(functionUrl);
            this.request.Method = "DELETE";
            this.request.ContentType = @"application/json; charset=utf-8";

            var serializer = new DataContractJsonSerializer(typeof(Customer));
            bool addedSuccessfully = false;
            string message = string.Empty;

            #region Old implementation if parameter should be an object of Customer
            //// Serializing input parameter
            //using (var mstream = new MemoryStream())
            //using (this.reader = new StreamReader(mstream))
            //{
            //    serializer.WriteObject(mstream, customer);
            //    mstream.Position = 0; message = this.reader.ReadToEnd();
            //}

            // Assigning the stream in the request
            //using (var streamWriter = new StreamWriter(this.request.GetRequestStream()))
            //{
            //    streamWriter.Write(message);
            //}
            #endregion

            // Get the response from the request
            this.response = this.request.GetResponse();

            // Reading the result from the response
            this.stream = this.response.GetResponseStream();
            this.reader = new StreamReader(this.stream);
            message = reader.ReadToEnd();

            bool.TryParse(message, out addedSuccessfully);

            return addedSuccessfully;
        }

        /// <summary>
        /// Get all orders for a given customer
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public Order[] GetOrdersForCustomer(long customerId)
        {
            string functionUrl = string.Format("{0}/getOrders/?customerId={1}", this.serviceURL, customerId);

            this.request = (HttpWebRequest)WebRequest.Create(functionUrl);
            this.request.Method = "GET";
            this.request.ContentType = @"application/json; charset=utf-8";

            var serializer = new DataContractJsonSerializer(typeof(Order[]));
            bool addedSuccessfully = false;
            string message = string.Empty;

            // Get the response from the request
            this.response = this.request.GetResponse();

            // Reading the result from the response
            this.stream = this.response.GetResponseStream();
            var orders = (Order[])serializer.ReadObject(this.stream);

            return orders;
        }

        /// <summary>
        /// Add a new order to a given customer 
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public bool AddOrder(long customerId, Order order)
        {
            if (order == null)
            {
                return false;
            }

            string functionUrl = string.Format("{0}/addOrder/?customerId={1}", this.serviceURL, customerId);

            this.request = (HttpWebRequest)WebRequest.Create(functionUrl);
            this.request.Method = "POST";
            this.request.ContentType = @"application/json; charset=utf-8";

            var serializer = new DataContractJsonSerializer(typeof(Order));
            bool addedSuccessfully = false;
            string message = string.Empty;

            // Serializing input parameter
            using (var mstream = new MemoryStream())
            using (this.reader = new StreamReader(mstream))
            {
                serializer.WriteObject(mstream, order);
                mstream.Position = 0; message = this.reader.ReadToEnd();
            }

            // Assigning the stream in the request
            using (var streamWriter = new StreamWriter(this.request.GetRequestStream()))
            {
                streamWriter.Write(message);
            }

            // Get the response from the request
            this.response = this.request.GetResponse();

            // Reading the result from the response
            this.stream = this.response.GetResponseStream();
            this.reader = new StreamReader(this.stream);
            message = reader.ReadToEnd();

            bool.TryParse(message, out addedSuccessfully);

            return addedSuccessfully;
        }

        /// <summary>
        /// Delete a order for a customer
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public bool DeleteOrder(long customerId, long orderId)
        {
            string functionUrl = string.Format("{0}/deleteOrder/?customerId={1}&orderId={2}", this.serviceURL, customerId, orderId);

            this.request = (HttpWebRequest)WebRequest.Create(functionUrl);
            this.request.Method = "DELETE";
            this.request.ContentType = @"application/json; charset=utf-8";

            var serializer = new DataContractJsonSerializer(typeof(Order));
            bool addedSuccessfully = false;
            string message = string.Empty;

            // Get the response from the request
            this.response = this.request.GetResponse();

            // Reading the result from the response
            this.stream = this.response.GetResponseStream();
            this.reader = new StreamReader(this.stream);
            message = reader.ReadToEnd();

            bool.TryParse(message, out addedSuccessfully);

            return addedSuccessfully;
        }
    }
}
