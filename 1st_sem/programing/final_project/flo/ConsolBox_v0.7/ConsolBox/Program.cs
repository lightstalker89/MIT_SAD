// *******************************************************
// * <copyright file="Program.cs" company="FGrill">
// * Copyright (c) 2013 Florian Grill. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Florian Grill</author>
// *******************************************************/

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
        public static void Main(string[] args)
        {
            InitializeApplicationParts();
            RegisterEvents();
            StartApplicationParts(args);
            Console.ReadKey();
        }

        /// <summary>
        /// Register all essential parts for the application
        /// in the container
        /// </summary>
        private static void InitializeApplicationParts()
        {
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
