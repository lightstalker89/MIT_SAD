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
        [WebInvoke(Method = "GET", UriTemplate = "Customers", ResponseFormat = WebMessageFormat.Json)]
        List<Customer> GetCustomers();

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "Orders/{customerName}", ResponseFormat = WebMessageFormat.Json)]
        List<List<string>> GetOrders(string customerName);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "Order/{customerName}/order/{order}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        bool AddOrder(string customerName, string order);

        [OperationContract]
        [WebInvoke(Method = "DELETE", UriTemplate = "Order/{customerName}/orderIndex/{orderIndex}", ResponseFormat = WebMessageFormat.Json)]
        bool DeleteOrder(string customerName, string orderIndex);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "Customer/{customerName}", ResponseFormat = WebMessageFormat.Json)]
        bool AddCustomer(string customerName);

        [OperationContract]
        [WebInvoke(Method = "DELETE", UriTemplate = "Customer/{customerName}", ResponseFormat = WebMessageFormat.Json)]
        bool DeleteCustomer(string customerName);
    }
}
