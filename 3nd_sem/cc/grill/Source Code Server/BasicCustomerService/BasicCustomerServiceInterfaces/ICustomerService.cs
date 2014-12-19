namespace BasicCustomerServiceInterfaces
{
    using System.Collections.Generic;
    using System.ServiceModel;
    using BasicCustomerServiceInterfaces.Models;

    [ServiceContract(Namespace = "http://grill.com/webservices/", Name = "CustomerService")]
    public interface ICustomerService
    {
        [OperationContract]
        List<Customer> GetCustomers();

        [OperationContract]
        List<List<string>> GetOrders(string customerName);

        [OperationContract]
        bool AddOrder(string customerName, List<string> order);

        [OperationContract]
        bool DeleteOrder(string customerName, int orderIndex);

        [OperationContract]
        bool AddCustomer(string customerName);

        [OperationContract]
        bool DeleteCustomer(string customerName);
    }
}
