using System.ServiceModel;

namespace RESTWCFWebService
{
    [ServiceContract]
    public interface IService
    {
        [OperationContract]
        string GetCustomers();

        [OperationContract]
        string GetOrder(string customerName);
    }
}
