using BiOWheelsFileWatcher;
using BiOWheelsVisualizer;

namespace BiOWheels
{
    using System;
    using BiOWheelsCommandLineArgsParser;
    using BiOWheelsConfigManager;
    using BiOWheelsLogger;

    /// <summary>
    /// 
    /// </summary>
    public class BiOWheelsProgram
    {
        #region Private Fields
        private const string Options = "";
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
                ICommandLineArgsParser parser = SimpleContainer.Instance.Resolve<CommandLineArgsParser>();
                parser.Parse(args, Options);
            }
            else
            {
                Configuration config = SimpleContainer.Instance.Resolve<IConfigurationManager>().Configuration;

                Log("BiOWheels was started without any commandline arguments...", MessageType.INFO);
            }
        }

        /// <summary>
        /// Register Moduels in the SimpleContainer
        /// </summary>
        /// <param name="loadConfig"></param>
        private static void ApplicationStartUp(bool loadConfig)
        {
            SimpleContainer.Instance.Register<IConfigurationManager, IConfigurationManager>();
            SimpleContainer.Instance.Register<ILogger, CombinedLogger>(new CombinedLogger { IsEnabled = true });
            SimpleContainer.Instance.Register<ICommandLineArgsParser, CommandLineArgsParser>();
            SimpleContainer.Instance.Register<IVisualizer, Visualizer>();
            SimpleContainer.Instance.Register<IFileWatcher, FileWatcher>();

            if (loadConfig)
            {
                IConfigurationManager configurationManager = SimpleContainer.Instance.Resolve<IConfigurationManager>();
                configurationManager.ConfigurationLoadingFailed += configurationManager_OnConfigurationLoadingFailed;
                configurationManager.Load();
            }
        }

        #region Events
        /// <summary>
        /// Event raised when the loading of the configuration failed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void configurationManager_OnConfigurationLoadingFailed(object sender, EventArgs e)
        {
            Log("Error while loading the configuration", MessageType.ERROR);
        }
        #endregion

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
        #endregion
    }
}
