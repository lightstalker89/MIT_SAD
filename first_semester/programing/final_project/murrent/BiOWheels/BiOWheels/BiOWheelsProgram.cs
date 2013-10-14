using BiOWheelsLogger;

namespace BiOWheels
{
    public class BiOWheelsProgram
    {
        public static void Main(string[] args)
        {
            ApplicationStartUp();
        }

        private static void ApplicationStartUp()
        {
            SimpleContainer.Instance.Register<ILogger, ConsoleLogger>(new ConsoleLogger());
            SimpleContainer.Instance.Register<ILogger, CombinedLogger>(new CombinedLogger());
            SimpleContainer.Instance.Register<ILogger, FileLogger>(new FileLogger());
        }
    }
}
