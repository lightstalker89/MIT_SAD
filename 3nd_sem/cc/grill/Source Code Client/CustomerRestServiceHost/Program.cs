namespace CustomerRestServiceHost
{
    using System;
    using System.ServiceModel;
    using System.ServiceModel.Web;
    using BasicCustomerRestService;

    public class Program
    {
        private static WebServiceHost host = null;

        public static void Main(string[] args)
        {
            StartService();

            Console.WriteLine("REST Customer Service is running, press any key to exit....");
            Console.ReadKey();

            CloseService();
        }

        private static void StartService()
        {
            host = new WebServiceHost(typeof(CustomerService));
            host.Open();
        }

        private static void CloseService()
        {
            if (host.State != CommunicationState.Closed)
            {
                host.Close();
            }
        }
    }
}
