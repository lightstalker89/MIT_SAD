namespace CustomerServiceHost
{
    using System;
    using System.ServiceModel;
    using BasicCustomerService;

    public class Program
    {
        private static ServiceHost host = null;

        private static void Main(string[] args)
        {
            StartService();

            Console.WriteLine("SOAP Customer Service is running, press any key to exit....");
            Console.ReadKey();

            CloseService();
        }

        private static void StartService()
        {
            host = new ServiceHost(typeof(CustomerService));
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
