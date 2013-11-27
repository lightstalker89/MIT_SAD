// *******************************************************
// * <copyright file="BiOWheelsProgram.cs" company="MDMCoWorks">
// * Copyright (c) 2013 Mario Murrent. All rights reserved.
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
    using BiOWheelsFileWatcher.Interfaces;

    using BiOWheelsLogger;

    using BiOWheelsTextToSpeechService;

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

        #endregion

        #region Private Fields

        /// <summary>
        /// Holds some messages
        /// </summary>
        private static readonly List<string> EggMessageList = new List<string>();

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

        /// <summary>
        /// Holds some click count
        /// </summary>
        private static int eggClickCount;

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
            Log(data.GetFormattedException(), MessageType.ERROR);
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
            FillEgMessageList();

            SimpleContainer.Instance.Register<ITextToSpeechService, ITextToSpeechService>(
                TextToSpeechServiceFactory.CreateTextToSpeechService());
            SimpleContainer.Instance.Register<IConfigurationManager, IConfigurationManager>(
                ConfigurationManagerFactory.CreateConfigurationManager());
            SimpleContainer.Instance.Register<ILogger, ILogger>(LoggerFactory.CreateCombinedLogger());

            SimpleContainer.Instance.Resolve<ILogger>().SetIsEnabled<FileLogger>(true);
            SimpleContainer.Instance.Resolve<ILogger>().SetIsEnabled<ConsoleLogger>(true);

            SimpleContainer.Instance.Register<ICommandLineArgsParser, ICommandLineArgsParser>(
                CommandLineArgsParserFactory.CreateCommandLineArgsParser());
            SimpleContainer.Instance.Register<IVisualizer, IVisualizer>(VisualizerFactory.CreateVisualizer());

            if (loadConfig)
            {
                LoadConfig();
            }
            else
            {
                Log("BiOWheels was started with commandline arguments...", MessageType.INFO);

                ICommandLineArgsParser parser = SimpleContainer.Instance.Resolve<CommandLineArgsParser>();
                HandleParams(parser.Parse(args, Options));

                // ToDo resolve commandline args
                // CreateFileWatcher();
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
                    CreateFileWatcher();
                    DistributeConfigurationValues();
                    StartSync();
                }
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
        /// Creates a new <see cref="IFileWatcher"/> object
        /// </summary>
        private static void CreateFileWatcher()
        {
            IFileComparator fileComparator =
                FileWatcherFactory.CreateFileComparator(configuration.BlockCompareOptions.BlockSizeInKB);
            IFileSystemManager fileSystemManager = FileWatcherFactory.CreateFileSystemManager(fileComparator);
            fileSystemManager.BlockCompareFileSizeInMB = configuration.BlockCompareOptions.BlockCompareFileSizeInMB;
            IQueueManager queueManager = FileWatcherFactory.CreateQueueManager(fileSystemManager);
            IFileWatcher fileWatcher = FileWatcherFactory.CreateFileWatcher(queueManager);

            // SimpleContainer.Instance.Register<IFileWatcher, IFileWatcher>(
            // FileWatcherFactory.CreateFileWatcher(
            // FileWatcherFactory.CreateQueueManager(
            // FileWatcherFactory.CreateFileSystemManager(
            // FileWatcherFactory.CreateFileComparator(configuration.BlockCompareOptions.BlockSizeInKB),
            // configuration.BlockCompareOptions.BlockCompareFileSizeInMB))));
            SimpleContainer.Instance.Register<IFileWatcher, IFileWatcher>(fileWatcher);

            AttachFileWatcherEvents();
        }

        /// <summary>
        /// Listen to key inputs from console
        /// </summary>
        private static void ListenToConsoleKeyInput()
        {
            while (isListeningToConsoleKeyInput)
            {
                ConsoleKey key = Console.ReadKey(true).Key;

                SimpleContainer.Instance.Resolve<ILogger>().SetIsEnabled<ConsoleLogger>(false);

                switch (key)
                {
                    case ConsoleKey.X:
                        Log("Closing BiOWheels", MessageType.INFO);
                        CloseApplication(0);
                        break;
                    case ConsoleKey.S:

                        // Log(GetEasterEgg(), MessageType.INFO);
                        string message = GetEasterEgg();
                        SimpleContainer.Instance.Resolve<ITextToSpeechService>().Speack(message);

                        break;
                    case ConsoleKey.P:

                        bool activated = SimpleContainer.Instance.Resolve<IFileSystemManager>().IsParallelSyncActivated;

                        SimpleContainer.Instance.Resolve<IFileSystemManager>().IsParallelSyncActivated = !activated;

                        Log("Parallel sync has been " + !activated, MessageType.INFO);

                        break;
                    case ConsoleKey.L:
                        break;
                    case ConsoleKey.F:
                        break;
                    case ConsoleKey.B:
                        break;

                    default:
                        SimpleContainer.Instance.Resolve<ILogger>().SetIsEnabled<ConsoleLogger>(true);
                        break;
                }
            }
        }

        /// <summary>
        /// Start the sync process
        /// </summary>
        private static void StartSync()
        {
            Log("Starting to monitor files.", MessageType.INFO);

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
            watcher.InitialScan();
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
                            SourceDirectory = directoryMappingInfo.SourceMappingInfo.SourceDirectory, 
                            Recursive = directoryMappingInfo.SourceMappingInfo.Recursive, 
                            ExcludedDirectories = directoryMappingInfo.ExcludedFromSource
                        }).ToList();

            SimpleContainer.Instance.Resolve<IFileWatcher>().SetSourceDirectories(mappings);

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
        /// Get the easter egg
        /// </summary>
        /// <returns>A message</returns>
        private static string GetEasterEgg()
        {
            eggClickCount++;

            Random random = new Random();

            if (eggClickCount % 3 == 0)
            {
                return "Really? Do something useful :)";
            }

            return EggMessageList[random.Next(0, EggMessageList.Count)];
        }

        /// <summary>
        /// Fills the message list.
        /// </summary>
        private static void FillEgMessageList()
        {
            EggMessageList.Add("There are exactly 10 types of people: These who know binary and these who do not.");
            EggMessageList.Add(
                "Programming is similar to sex. If you make a mistake, you have to support it for the rest of your life.");
            EggMessageList.Add("Always borrow money from a pessimist. He won’t expect it back.");
            EggMessageList.Add("Artificial Intelligence usually beats natural stupidity.");
            EggMessageList.Add("If Python is executable pseudocode, then perl is executable line noise.");
            EggMessageList.Add("If at first you don't succeed; call it version 1.0");
            EggMessageList.Add("My software never has bugs. It just develops random features.");
            EggMessageList.Add("Microsoft: You've got questions. We've got dancing paperclips.");
            EggMessageList.Add("Those who can't write programs, write help files.");
        }

        /// <summary>
        /// Close the application
        /// </summary>
        /// <param name="exitCode">
        /// The exit code.
        /// </param>
        private static void CloseApplication(int exitCode)
        {
            Environment.Exit(exitCode);
        }

        #endregion
    }
}