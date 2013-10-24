// *******************************************************
// * <copyright file="BiOWheelsProgram.cs" company="MDMCoWorks">
// * Copyright (c) Mario Murrent. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Mario Murrent</author>
// *******************************************************/
namespace BiOWheels
{
    using System;
    using System.Collections.Generic;

    using BiOWheels.BiOWheelsConfiguration;

    using BiOWheelsCommandLineArgsParser;

    using BiOWheelsConfigManager;

    using BiOWheelsFileWatcher;

    using BiOWheelsLogger;

    using BiOWheelsVisualizer;

    /// <summary>
    /// Represents the BiOWheelsProgram class which contains the entry point of the application.
    /// </summary>
    public class BiOWheelsProgram
    {
        #region Private Fields

        /// <summary>
        /// The configuration for the application
        /// </summary>
        private static Configuration configuration;

        /// <summary>
        /// Accepted command line arguments
        /// </summary>
        private const string Options = "px";

        /// <summary>
        /// 
        /// </summary>
        private static bool isSyncing = true;

        #endregion

        /// <summary>
        /// The entry point of the application
        /// </summary>
        /// <param name="args">
        /// Specified command line arguments
        /// </param>
        public static void Main(string[] args)
        {
            SetUpConsoleEvents();

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

            StartSync();
        }

        #region Events

        /// <summary>
        /// </summary>
        /// <param name="sender">
        /// </param>
        /// <param name="e">
        /// </param>
        protected static void ConsoleCancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            CloseApplication();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Register modules in the SimpleContainer
        /// </summary>
        /// <param name="loadConfig">
        /// Specifies if the configuration should be loaded
        /// </param>
        private static void ApplicationStartUp(bool loadConfig)
        {
            SimpleContainer.Instance.Register<IConfigurationManager, ConfigurationManager>(new ConfigurationManager());
            SimpleContainer.Instance.Register<ILogger, CombinedLogger>(new CombinedLogger());
            SimpleContainer.Instance.Resolve<ILogger>().SetIsEnabled<FileLogger>(true);
            SimpleContainer.Instance.Resolve<ILogger>().SetIsEnabled<ConsoleLogger>(true);
            SimpleContainer.Instance.Register<ICommandLineArgsParser, CommandLineArgsParser>(
                new CommandLineArgsParser());
            SimpleContainer.Instance.Register<IVisualizer, Visualizer>(new Visualizer());
            SimpleContainer.Instance.Register<IFileWatcher, FileWatcher>(new FileWatcher());

            if (loadConfig)
            {
                IConfigurationManager configurationManager = SimpleContainer.Instance.Resolve<IConfigurationManager>();
                object config = configurationManager.Load<Configuration>("BiOWheelsConfig.xml");

                if (config.GetType() == typeof(Configuration))
                {
                    configuration = config as Configuration;

                    if (configuration == null)
                    {
                        Log("Error while loading the configuration for BiOWheels", MessageType.ERROR);
                    }
                    else
                    {
                        DistributeConfigurationValues();
                    }
                }
                else
                {
                    LoaderException loaderException = config as LoaderException;

                    if (loaderException != null)
                    {
                        Log(
                            "Error while loading the configuration for BiOWheels - " + loaderException.ExceptionType
                            + " occured: " + loaderException.Message, 
                            MessageType.ERROR);
                    }
                }
            }
        }

        /// <summary>
        /// Start the sync process
        /// </summary>
        private static void StartSync()
        {
            while (isSyncing)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
            }
        }

        /// <summary>
        /// Set up events for the console window
        /// </summary>
        private static void SetUpConsoleEvents()
        {
            Console.CancelKeyPress += ConsoleCancelKeyPress;
        }

        /// <summary>
        /// Logs a message with the given parameters
        /// </summary>
        /// <param name="message">
        /// The message to log
        /// </param>
        /// <param name="messageType">
        /// The type of log message
        /// </param>
        private static void Log(string message, MessageType messageType)
        {
            SimpleContainer.Instance.Resolve<ILogger>().Log(message, messageType);
        }

        /// <summary>
        /// Distribute configuration to the modules
        /// </summary>
        private static void DistributeConfigurationValues()
        {
            SimpleContainer.Instance.Resolve<ILogger>().SetFileSize<ConsoleLogger>(
                configuration.LogFileOptions.LogFileSizeInMB);

            foreach (DirectoryMappingInfo directoryMappingInfo in configuration.DirectoryMappingInfo)
            {
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="parameter">
        /// </param>
        private static void HandleParams(IEnumerable<char> parameter)
        {
            foreach (char c in parameter)
            {
                if (c == 'p')
                {
                    // parallel sync
                }
                else if (c == 'x')
                {
                    CloseApplication();
                }
            }
        }

        /// <summary>
        /// Close the application
        /// </summary>
        private static void CloseApplication()
        {
        }

        #endregion
    }
}