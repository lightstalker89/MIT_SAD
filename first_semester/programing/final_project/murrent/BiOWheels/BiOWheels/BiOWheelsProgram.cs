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
    using System.Linq;
    using System.Threading;

    using BiOWheels.BiOWheelsConfiguration;

    using BiOWheelsCommandLineArgsParser;

    using BiOWheelsConfigManager;

    using BiOWheelsFileWatcher;
    using BiOWheelsFileWatcher.CustomEventArgs;

    using BiOWheelsLogger;

    using BiOWheelsVisualizer;

    /// <summary>
    /// Represents the BiOWheelsProgram class which contains the entry point of the application.
    /// </summary>
    public class BiOWheelsProgram
    {
        #region Constants

        /// <summary>
        /// Accepted command line arguments
        /// </summary>
        private const string Options = "px";

        /// <summary>
        /// Default console foreground color
        /// </summary>
        private const ConsoleColor DefaultConsoleForegroundColor = ConsoleColor.Gray;

        #endregion

        #region Private Fields

        /// <summary>
        /// The configuration for the application
        /// </summary>
        private static Configuration configuration;

        /// <summary>
        /// Value indicating whether the sync is in progress
        /// </summary>
        private static bool isSyncing = true;

        /// <summary>
        /// Value indicating whether the program should listen to console key input
        /// </summary>
        private static bool isListeningToConsoleKeyInput = true;
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
        /// Event for pressing the CTRL+C keyboard combination in the console
        /// </summary>
        /// <param name="sender">
        /// Sender of the event
        /// </param>
        /// <param name="e">
        /// Parameter coming from the event
        /// </param>
        protected static void ConsoleCancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            CloseApplication(0);
        }

        /// <summary>
        /// Occurs when the progress of the file watcher updates
        /// </summary>
        /// <param name="sender">
        /// Sender of the event
        /// </param>
        /// <param name="data">
        /// Data from the event
        /// </param>
        private static void WatcherProgressUpdate(object sender, UpdateProgressEventArgs data)
        {
            Log(data.Message, MessageType.INFO);
        }

        /// <summary>
        /// Occurs when an exception is caught from the file watcher
        /// </summary>
        /// <param name="sender">
        /// Sender of the event
        /// </param>
        /// <param name="data">
        /// Data from the event
        /// </param>
        private static void WatcherCaughtException(object sender, CaughtExceptionEventArgs data)
        {
            Log(data.ExceptionType + " occurred:" + data.ExceptionMessage, MessageType.ERROR);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Register modules in the SimpleContainer
        /// </summary>
        /// <param name="loadConfig">
        /// Specifies if the configuration should be loaded
        /// </param>
        /// <param name="args">
        /// Command line arguments
        /// </param>
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

            AttachFileWatcherEvents();

            if (loadConfig)
            {
                LoadConfig();
            }
            else
            {
                Log("BiOWheels was started with commandline arguments...", MessageType.INFO);

                ICommandLineArgsParser parser = SimpleContainer.Instance.Resolve<CommandLineArgsParser>();
                HandleParams(parser.Parse(args, Options));
            }
        }

        /// <summary>
        /// Loads the configuration
        /// </summary>
        private static void LoadConfig()
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

                    WriteLineToConsole("Error while loading the configuration. Press x to exit the program");

                    ListenToConsoleKeyInput();
                }
            }
        }

        /// <summary>
        /// Listen to key inputs from console
        /// </summary>
        private static void ListenToConsoleKeyInput()
        {
            while (isListeningToConsoleKeyInput)
            {
                ConsoleKey key = Console.ReadKey(true).Key;

                if (key == ConsoleKey.X)
                {
                    CloseApplication(0);
                }
                else if (key == ConsoleKey.S)
                {
                    //TODO: implement easter egg
                }
                else if (key == ConsoleKey.P)
                {
                    //TODO: Activate or deactivate parallel sync
                }
                else if (key == ConsoleKey.L)
                {
                    //TODO: disable console logger
                }
                else if (key == ConsoleKey.F)
                {
                    //TODO: disable file logger
                }
                else if (key == ConsoleKey.B)
                {
                    //TODO: pause process
                }
            }
        }

        /// <summary>
        /// Start the sync process
        /// </summary>
        private static void StartSync()
        {
            ListenToConsoleKeyInput();
        }

        /// <summary>
        /// Attach events for the <see cref="IFileWatcher"/>
        /// </summary>
        private static void AttachFileWatcherEvents()
        {
            IFileWatcher watcher = SimpleContainer.Instance.Resolve<IFileWatcher>();
            watcher.ProgressUpdate += WatcherProgressUpdate;
            watcher.CaughtException += WatcherCaughtException;
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
        /// <param name="text">
        /// Test to write to the console
        /// </param>
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

            IList<DirectoryMapping> mappings =
                configuration.DirectoryMappingInfo.Select(
                    directoryMappingInfo =>
                    new DirectoryMapping
                        {
                            DestinationDirectories = directoryMappingInfo.DestinationDirectories,
                            SorceDirectory = directoryMappingInfo.SourceMappingInfo.SourceDirectory,
                            Recursive = directoryMappingInfo.SourceMappingInfo.Recursive
                        }).ToList();

            SimpleContainer.Instance.Resolve<IFileWatcher>().SetSourceDirectories(mappings);
            SimpleContainer.Instance.Resolve<IFileWatcher>().SetBlockSize(configuration.BlockCompareOptions.BlockSizeInKB);
            SimpleContainer.Instance.Resolve<IFileWatcher>().Init();

            Log("Configuration successfully loaded", MessageType.INFO);
        }

        /// <summary>
        /// Handles ever parameter coming from the command line
        /// </summary>
        /// <param name="parameter">
        /// List of parameter from the command line
        /// </param>
        private static void HandleParams(IEnumerable<char> parameter)
        {
            foreach (char c in parameter)
            {
                if (c == 'h')
                {
                    SimpleContainer.Instance.Resolve<IVisualizer>().GetHelp();
                }
                else
                {
                    if (c == 'p')
                    {
                        // parallel sync
                    }
                }
            }
        }

        /// <summary>
        /// Close the application
        /// </summary>
        private static void CloseApplication(int exitCode)
        {
            Environment.Exit(exitCode);
        }

        #endregion
    }
}