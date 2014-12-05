using System.ServiceProcess;

namespace WebServiceWindowsService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
            { 
                new Service()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
