namespace BasicCustomerRestServiceInterfaces
{
    using System.Collections.Generic;
    using System.ServiceModel;
    using System.ServiceModel.Web;
    using BasicCustomerRestServiceInterfaces.Models;

    [ServiceContract]
    public interface ICustomerService
    {
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "GetCustomers", ResponseFormat = WebMessageFormat.Json)]
        List<Customer> GetCustomers();

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "GetOrders/{customerName}", ResponseFormat = WebMessageFormat.Json)]
        List<List<string>> GetOrders(string customerName);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "AddOrder/{customerName}/order/{order}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        bool AddOrder(string customerName, string order);

        [OperationContract]
        [WebInvoke(Method = "DELETE", UriTemplate = "DeleteOrder/{customerName}/orderIndex/{orderIndex}", ResponseFormat = WebMessageFormat.Json)]
        bool DeleteOrder(string customerName, string orderIndex);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "AddCustomer/{customerName}", ResponseFormat = WebMessageFormat.Json)]
        bool AddCustomer(string customerName);

        [OperationContract]
        [WebInvoke(Method = "DELETE", UriTemplate = "DeleteCustomer/{customerName}", ResponseFormat = WebMessageFormat.Json)]
        bool DeleteCustomer(string customerName);
    }
}
