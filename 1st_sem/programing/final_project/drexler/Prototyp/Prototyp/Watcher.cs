//-----------------------------------------------------------------------
// <copyright file="Watcher.cs" company="MD Development">
//     Copyright (c) MD Development. All rights reserved.
// </copyright>
// <author>Michael Drexler</author>
//-----------------------------------------------------------------------
namespace Prototyp
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Prototyp.Log;
    using Prototyp.View;

    /// <summary>
    /// Synchronisation Task
    /// </summary>
    public class Watcher 
    {
        /// <summary>
        /// A list of all active fileSystemWatcher, which are monitoring the configured source directories
        /// </summary>
        private List<FileSystemWatcher> directoryWatchers;

        /// <summary>
        /// Includes the whole config for this SyncProgram, even a list of all source directories to monitor and sync
        /// </summary>
        private Config config;

        /// <summary>
        /// The log window to display all log entries
        /// </summary>
        private IWindow logWindow;

        /// <summary>
        /// A queue with jobs to progress for synchronization
        /// </summary>
        private ConcurrentQueue<Job> jobQueue;

        /// <summary>
        /// The logger instance to log new log entries
        /// </summary>
        private ILogger logger = Logger.Instance;

        /// <summary>
        /// Initializes a new instance of the <see cref="Watcher"/> class
        /// </summary>
        /// <param name="queueOfJobs">A queue of jobs for the syncer to progress</param>
        /// <param name="logWindow">Window for logging</param>
        /// <param name="config">Configuration of the program</param>
        public Watcher(ConcurrentQueue<Job> queueOfJobs, IWindow logWindow, Config config)
        {
            this.logWindow = logWindow;
            this.config = config;
            this.jobQueue = queueOfJobs;
            this.directoryWatchers = new List<FileSystemWatcher>();
        }

        ///// <summary>
        ///// One or arbitrarily many source directories with one or arbitrarily many target directories
        ///// </summary>
        ////public List<Directory> SourceDirectories
        ////{
        ////    get { return this.sourceDirectories; }
        ////    set { this.sourceDirectories = value; }
        ////}

        /// <summary>
        /// Gets a list of fileSystemWatchers
        /// </summary>
        public List<FileSystemWatcher> DirectoryWatchers
        {
            get { return this.directoryWatchers; }
        }

        /// <summary>
        /// Start watching any changes within the source directories
        /// </summary>
        public void ActivateMonitoring()
        {
            this.logger.Logs.Enqueue(new Log.LogEntry("Watcher: Start watching source directories ... ", LoggingType.Info));

            // TODO Maybe multiple threads are accessing the config object?? Make it thread save with monitor.enter(o)?
            foreach (Directory directory in this.config.SourceDirectories)
            {
                try
                {
                    FileSystemWatcher watcher = new FileSystemWatcher(directory.Path);
                    watcher.IncludeSubdirectories = directory.IncludeSubdirectories;
                    watcher.Changed += this.Watcher_Changed;
                    watcher.Created += this.Watcher_Created;
                    watcher.Deleted += this.Watcher_Deleted;
                    watcher.Renamed += this.Watcher_Renamed;
                    watcher.Error += this.Watcher_Error;
                    watcher.EnableRaisingEvents = true;
                    this.directoryWatchers.Add(watcher);
                }
                catch (Exception e)
                {
                    this.logger.Logs.Enqueue(new LogEntry(string.Format("Watcher: Error on creating directory monitoring! Error: {0}", e.Message), LoggingType.Error));
                }
            }
        }

        //// TODO Write one handling function for equal event handlers
        ////private void HandleDetectedChanges(object sender, EventHandler e)
        ////{

        ////}

        /// <summary>
        /// Add a new source directory to the monitoring list
        /// </summary>
        /// <param name="watcher">New FileSystemWatcher</param>
        public void CreateFileWatcher(FileSystemWatcher watcher)
        {
            watcher.Changed += this.Watcher_Changed;
            watcher.Deleted += this.Watcher_Deleted;
            watcher.Created += this.Watcher_Created;
            watcher.Renamed += this.Watcher_Renamed;
            watcher.Error += this.Watcher_Error;
            watcher.EnableRaisingEvents = true;
            this.directoryWatchers.Add(watcher);
            this.logger.Logs.Enqueue(new LogEntry(string.Format("Watcher: Source Directory {0} will be monitored!", watcher.Path), LoggingType.Info));
        }

        /// <summary>
        /// Remove a FileSystemWatcher from the monitoring list
        /// </summary>
        /// <param name="removeDir">Remove monitoring from this directory</param>
        public void RemoveFileWatcher(Directory removeDir)
        {
            this.logger.Logs.Enqueue(new LogEntry(string.Format("Watcher: Try to remove monitoring for {0}!", removeDir.Path), LoggingType.Info));
            FileSystemWatcher fileSystemWatcher = this.directoryWatchers.Where(m => m.Path == removeDir.Path).Select(m => m).SingleOrDefault();
            if (fileSystemWatcher != null)
            {
                fileSystemWatcher.EnableRaisingEvents = false;
                fileSystemWatcher.Changed -= this.Watcher_Changed;
                fileSystemWatcher.Created -= this.Watcher_Created;
                fileSystemWatcher.Deleted -= this.Watcher_Deleted;
                fileSystemWatcher.Renamed -= this.Watcher_Renamed;
                fileSystemWatcher.Error -= this.Watcher_Error;
                this.directoryWatchers.Remove(fileSystemWatcher);
                this.logger.Logs.Enqueue(new LogEntry(string.Format("Watcher: {0} removed from monitoring!", fileSystemWatcher.Path), LoggingType.Info));
            }
            else
            {
                this.logger.Logs.Enqueue(new LogEntry(string.Format("Watcher: No monitoring found for {0}!", removeDir.Path), LoggingType.Warning));
            }
        }

        /// <summary>
        /// Error Event Handler
        /// </summary>
        /// <param name="sender">Caller of the events</param>
        /// <param name="e">Event arguments</param>
        public void Watcher_Error(object sender, ErrorEventArgs e)
        {
            this.logger.Logs.Enqueue(new LogEntry(string.Format("Watcher: Error-Event {0}", e.GetException()), LoggingType.Error));
        }

        /// <summary>
        /// Renamed Event Handler
        /// </summary>
        /// <param name="sender">Caller of the event</param>
        /// <param name="e">Event arguments</param>
        public void Watcher_Renamed(object sender, RenamedEventArgs e)
        {
            // string directory = Path.GetDirectoryName(e.Name);

            // Try to get the configured source directory containing this path
            Directory sourceDirectory = this.config.SourceDirectories.Where(m => m.Path.Equals(e.FullPath) || e.FullPath.StartsWith(m.Path)).Select(m => m).SingleOrDefault();

            if (sourceDirectory != null && sourceDirectory.TargetDirectories != null && sourceDirectory.TargetDirectories.Count > 0)
            {
                foreach (Directory targetDirectory in sourceDirectory.TargetDirectories)
                {
                    Job temp = new Job(sourceDirectory.Path, targetDirectory.Path, e.Name, EventType.Renamed, sourceDirectory.IncludeSubdirectories, e.OldName, sourceDirectory.ExceptionDirectories);
                    this.jobQueue.Enqueue(temp);
                }

                this.logger.Logs.Enqueue(new Log.LogEntry("Watcher: A file/directory was renamed! A new job was already created!", LoggingType.Info));
            }
            else
            {
                this.logger.Logs.Enqueue(new LogEntry(string.Format("Watcher: Renamed file/directory {0} not found within configured source directories!", e.FullPath), LoggingType.Warning));
            }
        }

        /// <summary>
        /// Deleted Event Handler
        /// </summary>
        /// <param name="sender">Caller of the event</param>
        /// <param name="e">Event arguments</param>
        public void Watcher_Deleted(object sender, FileSystemEventArgs e)
        {
            // Try to get the configured source directory containing this path
            Directory sourceDirectory = this.config.SourceDirectories.Where(m => m.Path.Equals(e.FullPath) || e.FullPath.StartsWith(m.Path)).Select(m => m).SingleOrDefault();

            if (sourceDirectory != null && sourceDirectory.TargetDirectories != null && sourceDirectory.TargetDirectories.Count > 0)
            {
                foreach (Directory dir in sourceDirectory.TargetDirectories)
                {
                    Job temp = new Job(sourceDirectory.Path, dir.Path, e.Name, EventType.Deleted, sourceDirectory.IncludeSubdirectories, string.Empty, sourceDirectory.ExceptionDirectories);
                    this.jobQueue.Enqueue(temp);
                }
            }
            else 
            {
                this.logger.Logs.Enqueue(new LogEntry(string.Format("Watcher: Deleted file/directory {0} not found within configured source directories!", e.FullPath), LoggingType.Warning));
            }
        }

        /// <summary>
        /// Created Event Handler
        /// This event is fired when a file is created in the directory that is being monitored. 
        /// If you are planning to use this event to move the file that was created, you must write some error 
        /// handling in your event handler that can handle situations where the file is currently in use by another process. 
        /// The reason for this is that the Created event can be fired before the process that created the file has released the file. 
        /// This will cause exceptions to be thrown if you have not prepared the code correctly.
        /// </summary>
        /// <param name="sender">Caller of the event</param>
        /// <param name="e">Event arguments</param>
        public void Watcher_Created(object sender, FileSystemEventArgs e)
        {
            // string directory = Path.GetDirectoryName(e.Name);

            // Try to get the configured source directory containing this path
            Directory sourceDirectory = this.config.SourceDirectories.Where(m => m.Path.Equals(e.FullPath) || e.FullPath.StartsWith(m.Path)).Select(m => m).SingleOrDefault();

            if (sourceDirectory != null && sourceDirectory.TargetDirectories != null && sourceDirectory.TargetDirectories.Count > 0)
            {
                foreach (Directory targetDirectory in sourceDirectory.TargetDirectories)
                {
                    Job temp = new Job(sourceDirectory.Path, targetDirectory.Path, e.Name, EventType.Created, sourceDirectory.IncludeSubdirectories, string.Empty, sourceDirectory.ExceptionDirectories);
                    this.jobQueue.Enqueue(temp);
                }

                this.logger.Logs.Enqueue(new Log.LogEntry("Watcher: A new file/directory was added! A new job was already created!", LoggingType.Info));
            }
            else
            {
                this.logger.Logs.Enqueue(new LogEntry(string.Format("Watcher: Created file/directory {0} not found within configured source directories!", e.FullPath), LoggingType.Warning));
            }
        }

        /// <summary>
        /// Changed Event Handler
        /// The change event is fired when a file has been modified in the directory that is being monitored. 
        /// It is important to note that this event may be fired multiple times, even when only one change to the content of 
        /// the file has occurred. This is due to other properties of the file changing as the file is saved.
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">Event arguments</param>
        public void Watcher_Changed(object sender, FileSystemEventArgs e)
        {
            // string directory = Path.GetDirectoryName(e.Name);

            // Try to get the configured source directory containing this path
            Directory sourceDirectory = this.config.SourceDirectories.Where(m => m.Path.Equals(e.FullPath) || e.FullPath.StartsWith(m.Path)).Select(m => m).SingleOrDefault();

            if (sourceDirectory != null && sourceDirectory.TargetDirectories != null && sourceDirectory.TargetDirectories.Count > 0)
            {
                foreach (Directory targetDirectory in sourceDirectory.TargetDirectories)
                {
                    Job temp = new Job(sourceDirectory.Path, targetDirectory.Path, e.Name, EventType.Changed, sourceDirectory.IncludeSubdirectories, string.Empty, sourceDirectory.ExceptionDirectories);
                    this.jobQueue.Enqueue(temp);
                }

                this.logger.Logs.Enqueue(new Log.LogEntry("Watcher: A file/directory was changed! A new job was already created!", LoggingType.Info));
            }
            else
            {
                this.logger.Logs.Enqueue(new LogEntry(string.Format("Watcher: Changed file/directory {0} not found within configured source directories!", e.FullPath), LoggingType.Warning));
            }
        }
    }
}
