using System.Collections.Generic;
using BiOWheelsFileWatcher;
using BiOWheelsVisualizer;

namespace BiOWheels
{
    using BiOWheelsCommandLineArgsParser;
    using BiOWheelsConfigManager;
    using BiOWheelsLogger;

    /// <summary>
    /// 
    /// </summary>
    public class BiOWheelsProgram
    {
        #region Private Fields
        /// <summary>
        /// Accepted commandline arguments
        /// </summary>
        private const string Options = "p";
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            ApplicationStartUp(args.Length > 0);

            if (args.Length > 0)
            {
                Log("BiOWheels was started with commandline arguments...", MessageType.INFO);

                ICommandLineArgsParser parser = SimpleContainer.Instance.Resolve<CommandLineArgsParser>();
                HandleParams(parser.Parse(args, Options));
            }
            else
            {
                Log("BiOWheels was started without any commandline arguments...", MessageType.INFO);
            }
        }

        /// <summary>
        /// Register Moduels in the SimpleContainer
        /// </summary>
        /// <param name="loadConfig"></param>
        private static void ApplicationStartUp(bool loadConfig)
        {
            SimpleContainer.Instance.Register<IConfigurationManager, ConfigurationManager>(new ConfigurationManager());
            SimpleContainer.Instance.Register<ILogger, CombinedLogger>(new CombinedLogger());
            SimpleContainer.Instance.Resolve<ILogger>().SetIsEnabled<FileLogger>(true);
            SimpleContainer.Instance.Resolve<ILogger>().SetIsEnabled<ConsoleLogger>(true);
            SimpleContainer.Instance.Resolve<ILogger>().SetFileSize<ConsoleLogger>(2);
            SimpleContainer.Instance.Register<ICommandLineArgsParser, CommandLineArgsParser>(new CommandLineArgsParser());
            SimpleContainer.Instance.Register<IVisualizer, Visualizer>(new Visualizer());
            SimpleContainer.Instance.Register<IFileWatcher, FileWatcher>(new FileWatcher());


            if (loadConfig)
            {
                IConfigurationManager configurationManager = SimpleContainer.Instance.Resolve<IConfigurationManager>();
            }
        }

        #region Methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="messageType"></param>
        private static void Log(string message, MessageType messageType)
        {
            SimpleContainer.Instance.Resolve<ILogger>().Log(message, messageType);
        }

        private static void HandleParams(IEnumerable<char> parameter)
        {
            foreach (char c in parameter)
            {
                if(c == 'p')
                {
                    // parallel sync
                }
            }
        }
        #endregion
    }
}
