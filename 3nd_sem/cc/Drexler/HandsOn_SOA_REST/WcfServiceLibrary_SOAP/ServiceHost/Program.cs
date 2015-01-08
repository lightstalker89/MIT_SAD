//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="MD Development">
//     Copyright (c) MD Development. All rights reserved.
// </copyright>
// <author>Michael Drexler</author>
//-----------------------------------------------------------------------
namespace ServiceHost
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.ServiceModel;
    using System.ServiceModel.Description;
    using System.ServiceModel.Web;
    using System.Text;
    using System.Threading.Tasks;
    using ServiceLibrary;

    /// <summary>
    /// Service hoster
    /// </summary>
    class Program
    {
        /// <summary>
        /// Create, starts and stops the service
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            using (ServiceHost host = new ServiceHost(typeof(CustomerOrderRetrieval)))
            {
                try
                {
                    // Open the service host to start listening for messages.
                    host.Open();
                    Console.WriteLine("Service started! Service base address");
                    Console.WriteLine("Press [enter] to stop the service!");
                    Console.ReadLine();

                    // Close the service host
                    host.Close();
                }
                catch (CommunicationException cex)
                {
                    Console.WriteLine("An exception occurred: {0}", cex.Message);
                }
            }

            //ServiceHost soapHost = new ServiceHost(typeof(CustomerOrderRetrieval), new Uri("http://localhost:9000/soap"));
            //WebServiceHost restHost = new WebServiceHost(typeof(CustomerOrderRetrieval),new Uri("http://localhost:9000/rest"));

            //soapHost.Open();
            //restHost.Open();

            //Console.WriteLine("Service gestarted!");
            //Console.WriteLine("Press [ENTER] to stop the service!");
            //Console.ReadLine();

            //soapHost.Close();
            //restHost.Close();
        }
    }
}
