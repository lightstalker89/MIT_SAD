using System;
using BiOWheelsCommandLineArgsParser;
using BiOWheelsConfigManager;
using BiOWheelsLogger;

namespace BiOWheels
{
    public class BiOWheelsProgram
    {
        public static void Main(string[] args)
        {
            ApplicationStartUp(args.Length > 0);

            if (args.Length > 0)
            {
                Configuration config = SimpleContainer.Instance.Resolve<IConfigurationManager>().Configuration;
                ICommandLineArgsParser parser = SimpleContainer.Instance.Resolve<CommandLineArgsParser>();
            }
            else
            {
                Log("BiOWheels was started without any commandline arguments...", MessageType.INFO);
            }
        }

        private static void ApplicationStartUp(bool loadConfig)
        {
            SimpleContainer.Instance.Register<IConfigurationManager, IConfigurationManager>();
            SimpleContainer.Instance.Register<ILogger, CombinedLogger>(new CombinedLogger { IsEnabled = true });
            SimpleContainer.Instance.Register<ICommandLineArgsParser, CommandLineArgsParser>();

            if (loadConfig)
            {
                IConfigurationManager configurationManager = SimpleContainer.Instance.Resolve<IConfigurationManager>();
                configurationManager.ConfigurationLoadingFailed += configurationManager_OnConfigurationLoadingFailed;
                configurationManager.Load();
            }
        }

        private static void configurationManager_OnConfigurationLoadingFailed(object sender, EventArgs e)
        {
            Log("Error while loading the configuration", MessageType.ERROR);
        }

        private static void Log(string message, MessageType messageType)
        {
            SimpleContainer.Instance.Resolve<ILogger>().Log(message, messageType);
        }
    }
}
