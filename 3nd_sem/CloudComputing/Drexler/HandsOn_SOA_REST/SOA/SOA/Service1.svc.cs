//-----------------------------------------------------------------------
// <copyright file="Service1.cs" company="MD Development">
//     Copyright (c) MD Development. All rights reserved.
// </copyright>
// <author>Michael Drexler</author>
//-----------------------------------------------------------------------
namespace SOA
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.ServiceModel;
    using System.ServiceModel.Web;
    using System.Text;
    using DataAccessLayer;
    using DataAccessLayer.Models;
    using System.Web.Configuration;

    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService1
    {
        private List<Customer> customers;
        private IDataAccessService datacontext;

        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        public List<Customer> GetCustomers()
        {
            bool isDevelopmentState = bool.Parse(WebConfigurationManager.AppSettings["IsDevelopmentState"]);

            if (isDevelopmentState)
            {
                datacontext = new MockDataService();
            }
            else
            {
                datacontext = new DataAccessService();
            }

            if (this.customers == null)
            {
                this.customers = datacontext.GetCustomers();
            }

            return this.customers;
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }
    }
}
