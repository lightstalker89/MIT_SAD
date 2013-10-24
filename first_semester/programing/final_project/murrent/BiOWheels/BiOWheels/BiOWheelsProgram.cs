// *******************************************************
// * <copyright file="BiOWheelsProgram.cs" company="MDMCoWorks">
// * Copyright (c) Mario Murrent. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Mario Murrent</author>
// *******************************************************/

using System.Linq;
using System.Threading;

namespace BiOWheels
{
    using System;
    using System.Collections.Generic;

    using BiOWheelsConfiguration;

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
        [STAThread]
        public static void Main(string[] args)
        {
            Mutex mutex = new Mutex(false, "BiOWheelsProgramMutex");
            try
            {
                if (mutex.WaitOne(0, false))
                {
                    SetUpConsoleEvents();

                    ApplicationStartUp(args.Length <= 0, args);
                }
                else
                {
                    Console.WriteLine("Another instance of BioWheels is already running. Press x to close BiOWheels");

                    if (Console.ReadKey(true).Key == ConsoleKey.X)
                    {
                        Environment.Exit(89);
                    }
                }
            }
            finally
            {
                mutex.Close();
            }
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
        /// <param name="args">Commandline arguments</param>
        private static void ApplicationStartUp(bool loadConfig, string[] args)
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
                Log("BiOWheels was started without any commandline arguments...", MessageType.INFO);
                Log("Loading configuration", MessageType.INFO);

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

                    StartSync();
                }
                else
                {
                    LoaderException loaderException = config as LoaderException;

                    if (loaderException != null)
                    {
                        Log(
                            "Error while loading the configuration for BiOWheels - " + loaderException.ExceptionType
                            + " occurred: " + loaderException.Message,
                            MessageType.ERROR);

                        WriteLineToConsole("Error while loading the configuration. Exit program?");
                        Console.ReadKey(true);
                    }
                }
            }
            else
            {
                Log("BiOWheels was started with commandline arguments...", MessageType.INFO);

                ICommandLineArgsParser parser = SimpleContainer.Instance.Resolve<CommandLineArgsParser>();
                HandleParams(parser.Parse(args, Options));
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
        /// Writes the message to the console
        /// </summary>
        /// <param name="text">Test to write to the console</param>
        private static void WriteLineToConsole(string text)
        {
            SimpleContainer.Instance.Resolve<IVisualizer>().WriteLine(text);
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

            IList<DirectoryMapping> mappings = configuration.DirectoryMappingInfo.Select(
                 directoryMappingInfo => new DirectoryMapping
                                              {
                                                  DestinationDirectories = directoryMappingInfo.DestinationDirectories,
                                                  SorceDirectory = directoryMappingInfo.SourceMappingInfos.SourceDirectory,
                                                  Recursive = directoryMappingInfo.SourceMappingInfos.Recursive
                                              }).ToList();

            SimpleContainer.Instance.Resolve<IFileWatcher>().SetSourceDirectories(mappings);
            SimpleContainer.Instance.Resolve<IFileWatcher>().Init();

            Log("Configuration successfully loaded", MessageType.INFO);
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