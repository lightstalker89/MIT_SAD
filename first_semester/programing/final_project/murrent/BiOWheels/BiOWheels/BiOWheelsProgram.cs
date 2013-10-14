namespace BiOWheels
{
    using BiOWheelsLogger;
    using BiOWheelsConfigManager;

    public class BiOWheelsProgram
    {
        public static void Main(string[] args)
        {
            ApplicationStartUp();

            Configuration config = SimpleContainer.Instance.Resolve<IConfigurationManager>().Configuration;
        }

        private static void ApplicationStartUp()
        {
            SimpleContainer.Instance.Register<IConfigurationManager, IConfigurationManager>(new ConfigurationManager());
            SimpleContainer.Instance.Register<ILogger, CombinedLogger>(new CombinedLogger());
        }

        private void Log(string message, MessageType messageType)
        {
            ILogger logger = SimpleContainer.Instance.Resolve<CombinedLogger>();
            logger.Log(message, messageType);
        }
    }
}
