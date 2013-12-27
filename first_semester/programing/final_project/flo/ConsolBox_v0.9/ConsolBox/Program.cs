// *******************************************************
// * <copyright file="Program.cs" company="FGrill">
// * Copyright (c) 2013 Florian Grill. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Florian Grill</author>
// *******************************************************/

using ConsoleBoxVisualizer;

namespace ConsoleBox
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using ConsoleBox.Settings;
    using ConsoleBoxCommandLineArgsParser;
    using ConsoleBoxConfigLoader;
    using ConsoleBoxDataManager;
    using ConsoleBoxDataManager.Events;
    using ConsoleBoxFileWatcher;
    using ConsoleBoxFileWatcher.Events;
    using ConsoleBoxLogger;
    using ConsoleBoxVolumeSplitter;

    /// <summary>
    /// Represents the Program class which contains the entry point of the application.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Initializes static members of the <see cref="Program"/> class.
        /// </summary>
        static Program()
        {
            Container = new Container();
        }

        #region propterties
        /// <summary>
        /// Gets or sets the instance of the container object
        /// </summary>
        public static IContainer Container { get; set; }

        /// <summary>
        /// Gets or sets the command arguments
        /// </summary>
        public static char CommandArg { get; set; }

        /// <summary>
        /// Gets or sets the config Settings
        /// </summary>
        public static ConfigFile Config { get; set; }
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
            Console.WriteLine("Initialize application...");
            InitializeApplicationParts();
            RegisterEvents();
            StartApplicationParts(args);
            HandleKeyInput();
        }

        /// <summary>
        /// Handles the user key input.
        /// </summary>
        private static void HandleKeyInput()
        {
            bool run = true;
            while (run)
            {
                ConsoleKey key = Console.ReadKey(true).Key;

                switch (key)
                {
                    case ConsoleKey.H:
                        Container.GetService<IVisualizer>().ShowHelp();
                        break;
                    case ConsoleKey.P:
                        bool state = Container.GetService<IDataManager>().GetParallelSync();
                        Container.GetService<IDataManager>().SetParallelSync(!state);
                        Console.WriteLine(state
                            ? "Parallel synchronisation has been activated!"
                            : "Parallel synchronisation has been deactivated!");
                        break;
                    case ConsoleKey.C:
                        string compSize = Container.GetService<IVisualizer>().GetUserInput(
                            "Please enter the block compare size in mb: ");
                        long compareSize;

                        if (long.TryParse(compSize, out compareSize))
                        {
                            if (compareSize > 50)
                            {
                                Container.GetService<IDataManager>().SetBlockCompareSizeInMb(compareSize);
                                Container.GetService<ILogger>().Log(
                                "Block compare size successfully changed!", MessageType.Info);
                            }
                            else
                            {
                                Container.GetService<ILogger>().Log(
                                    "Could not set block compare size, value is too small!", MessageType.Info);
                                Container.GetService<IVisualizer>().WriteLine(
                                    "Please enter a valid block compare size bigger than 50 mb!");
                            }
                        }
                        else
                        {
                            Container.GetService<ILogger>().Log(
                                "Could not parse user input, to set block compare size!", MessageType.Info);
                        }

                        break;
                    case ConsoleKey.B:
                        string bloSize = Container.GetService<IVisualizer>().GetUserInput(
                            "Please enter the block size in mb: ");
                        long blockSize;

                        if (long.TryParse(bloSize, out blockSize))
                        {
                            if (blockSize > 1 && blockSize <= 20)
                            {
                                Container.GetService<IDataManager>().SetBlockSizeInMb(blockSize);
                                Container.GetService<ILogger>().Log(
                                "Block size successfully changed!", MessageType.Info);
                            }
                            else
                            {
                                Container.GetService<ILogger>().Log(
                                    "Could not set block size, value is too small or too big!", MessageType.Info);
                                Container.GetService<IVisualizer>().WriteLine(
                                    "Please enter a valid block compare size between 1 and 20 mb!");
                            }
                        }
                        else
                        {
                            Container.GetService<ILogger>().Log(
                                "Could not parse user input, to set block size!", MessageType.Info);
                        }

                        break;
                    case ConsoleKey.L:
                        string logFiSize = Container.GetService<IVisualizer>().GetUserInput(
                            "Please enter the log file size in mb: ");
                        long logFileSize;

                        if (long.TryParse(logFiSize, out logFileSize))
                        {
                            if (logFileSize >= 1)
                            {
                                Container.GetService<ILogger>().SetFileSize<FileLogger>(logFileSize);
                                Container.GetService<ILogger>().Log(
                                "Log file size successfully changed!", MessageType.Info);
                            }
                            else
                            {
                                Container.GetService<ILogger>().Log(
                                    "Could not set log file size, value is too small or too big!", MessageType.Info);
                                Container.GetService<IVisualizer>().WriteLine(
                                    "Please enter a valid log file size bigger than 1 mb!");
                            }
                        }
                        else
                        {
                            Container.GetService<ILogger>().Log(
                                "Could not parse user input, to set log file size!", MessageType.Info);
                        }

                        break;
                    case ConsoleKey.X:
                        if (Container.GetService<IDataManager>().IsSyncing)
                        {
                            string input = Container.GetService<IVisualizer>().GetUserInput(
                                "Application is currently snycing, would you really like to exit? y/n : ");
                            switch (input)
                            {
                                case "y":
                                    run = false;
                                    Container.GetService<ILogger>().Log(
                                       "Application close, while syncing was not complete!", MessageType.Info);
                                    break;
                                case "n":
                                    Container.GetService<IVisualizer>().WriteLine(
                                       "Application is not closing!");
                                    break;
                                default:
                                    Container.GetService<IVisualizer>().WriteLine(
                                       "Unexpected input, please try again!");
                                    break;
                            }
                        }
                        else
                        {
                            run = false;
                            Container.GetService<ILogger>().Log(
                               "Application close!", MessageType.Info);
                        }

                        break;
                    default:
                        Container.GetService<IVisualizer>().ShowHelp();
                        break;
                }
            }
        }

        /// <summary>
        /// Register all essential parts for the application
        /// in the container
        /// </summary>
        private static void InitializeApplicationParts()
        {
            Container.Register<IVisualizer, Visualizer>();
            Container.Register<ICommandLineArgsParser, CommandLineArgsParser>();
            Container.Register<IConfigLoader, ConfigLoader>();
            Container.Register<ILogger, Logger>();
            Container.Register<IVolumeSplitter, VolumeSplitter>();
            Container.Register<IFileWatcher, FileWatcher>();
            Container.Register<IDataManager, DataManager>();
        }

        /// <summary>
        /// Register all necessary events
        /// </summary>
        private static void RegisterEvents()
        {
            Container.GetService<IFileWatcher>().ExceptionMessage += LogFileWatcherException;
            Container.GetService<IFileWatcher>().FileWatcherJob += HandleFileWatcherJob;
            Container.GetService<IDataManager>().ExceptionMessage += LogDataManagerException;
        }

        #region Event hanlder
        /// <summary>
        /// Handle the file watcher events
        /// </summary>
        /// <param name="sender">
        /// Sender of the event
        /// </param>
        /// <param name="data">
        /// Data from the event
        /// </param>
        private static void HandleFileWatcherJob(object sender, FileWatcherJobEventArgs data)
        {
            Container.GetService<IDataManager>().AddFileManagerJob(new QueueItem(
                data.IsDirectory,
                data.SourceFolder,
                data.SourcePath,
                data.Name,
                data.Destinations,
                data.Action,
                data.OldFileName));

            Container.GetService<ILogger>().Log(data.Message, MessageType.Info);
        }

        /// <summary>
        /// Handle the file watcher exception events
        /// </summary>
        /// <param name="sender">
        /// Sender of the event
        /// </param>
        /// <param name="data">
        /// Data from the event
        /// </param>
        private static void LogFileWatcherException(object sender, ExceptionFileWatcherEventArgs data)
        {
            Container.GetService<ILogger>().Log(data.ExceptionType + "occured: " + data.ExceptionMessage, MessageType.Error);
        }

        /// <summary>
        /// Handle the data manager exception events
        /// </summary>
        /// <param name="sender">
        /// Sender of the event
        /// </param>
        /// <param name="data">
        /// Data from the event
        /// </param>
        private static void LogDataManagerException(object sender, ExceptionDataManagerEventArgs data)
        {
            Container.GetService<ILogger>().Log(data.ExceptionType + "occured: " + data.ExceptionMessage, MessageType.Error);
        }
        #endregion

        /// <summary>
        /// Start up the application with all necessary components
        /// </summary>
        /// <param name="args">
        /// Command line arguments
        /// </param>
        private static void StartApplicationParts(string[] args)
        {
            Container.GetService<IVisualizer>().MaximizeConsoleWindow();
            CommandArg = Container.GetService<ICommandLineArgsParser>().ParseArgs(args);
            if (CommandArg == 'p' || CommandArg == 'z')
            {
                if ((Config = Container.GetService<IConfigLoader>().LoadConfig<ConfigFile>(args.Length > 0 ? args[0] : "ConfigFile.xml")) != null)
                {
                    Container.GetService<ILogger>().SetFileSize<FileLogger>(Config.Settings.LogFileSize);
                    Container.GetService<IDataManager>().InitializeDataManager(Config.Settings.BlockCompareSizeInMb, Config.Settings.BlockSizeInMb, Config.Settings.ParalellSync);
                    List<SourceFolderInfo> sourceFolderInfo = Config.Folders.FolderMapping.Select(
                         directoryMappingInfo => new SourceFolderInfo
                         {
                             Path = directoryMappingInfo.SourceFolders.Path,
                             Recursion = directoryMappingInfo.SourceFolders.Recursion,
                             DestinationFolders = directoryMappingInfo.SourceFolders.DestinationFolder,
                             SplittedDestinationFolders = Container.GetService<IVolumeSplitter>().SplitFoldersByVolumeSerial(
                             directoryMappingInfo.SourceFolders.DestinationFolder),
                             ExceptionFolders = directoryMappingInfo.SourceFolders.ExceptionFolder
                         }).ToList();
                    
                    // Check folders
                    Container.GetService<IFileWatcher>().InitializeFileWatcher(sourceFolderInfo);
                    Container.GetService<IFileWatcher>().ScanFoldersForSyncState();
                    Container.GetService<IFileWatcher>().SetFileSystemWatcher();

                    // Start UI
                }

                // Visualizer give hint Erronous XML File
                // Logger
            }

            // Visualizer give hint '?' || Logger
        }
    }
}
