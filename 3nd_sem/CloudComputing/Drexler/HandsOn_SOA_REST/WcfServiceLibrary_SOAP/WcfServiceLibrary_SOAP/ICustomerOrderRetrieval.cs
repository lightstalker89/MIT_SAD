//-----------------------------------------------------------------------
// <copyright file="ICustomerOrderRetrieval.cs" company="MD Development">
//     Copyright (c) MD Development. All rights reserved.
// </copyright>
// <author>Michael Drexler</author>
//-----------------------------------------------------------------------
namespace ServiceLibrary
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.ServiceModel;
    using System.ServiceModel.Web;
    using System.Text;
    using DataAccessLayer.Models;

    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface ICustomerOrderRetrieval
    {
        [OperationContract]
        [WebGet(UriTemplate = "/getTest/?value={value}", ResponseFormat = WebMessageFormat.Json)]
        string TestMessage(int value);

        [OperationContract]
        [WebGet(UriTemplate = "/getCustomers", ResponseFormat = WebMessageFormat.Json)]
        List<Customer> GetAllCustomers();

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/addCustomer", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        bool AddCustomer(Customer customer);

        [OperationContract]
        [WebInvoke(Method = "DELETE", UriTemplate = "/deleteCustomer/?customerId={customerId}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        bool DeleteCustomer(long customerId);

        [OperationContract]
        [WebGet(UriTemplate = "/getOrders/?customerId={customerId}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        List<Order> GetOrdersForCustomer(long customerId);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/addOrder/?customerId={customerId}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        bool AddOrder(long customerId, Order order);

        [OperationContract]
        [WebInvoke(Method = "DELETE", UriTemplate = "/deleteOrder/?customerId={customerId}&orderId={orderId}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        bool DeleteOrder(long customerId, long orderId);
    }
}
