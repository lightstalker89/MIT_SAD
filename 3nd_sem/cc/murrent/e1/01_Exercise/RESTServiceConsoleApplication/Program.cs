using System;
using System.ServiceModel;
using RESTWCFWebService;

namespace RESTServiceConsoleApplication
{
    class Program
    {
        static ServiceHost host = null;

        static void StartService()
        {
            host = new ServiceHost(typeof(RestWcfService));
            /***********
             * if you don't want to use App.Config for the web service host, 
                 * just uncomment below:
             ***********
                 host.AddServiceEndpoint(new ServiceEndpoint(
                 ContractDescription.GetContract(typeof(IStudentEnrollmentService)),
                 new WSHttpBinding(), 
                 new EndpointAddress("http://localhost:8733/RestWcfWebService"))); 
             **********/
            host.Open();
        }

        static void CloseService()
        {
            if (host.State != CommunicationState.Closed)
            {
                host.Close();
            }
        }

        static void Main(string[] args)
        {
            StartService();

            Console.WriteLine("Student Enrollment Service is running....");
            Console.ReadKey();

            CloseService();
        }
    }
}
