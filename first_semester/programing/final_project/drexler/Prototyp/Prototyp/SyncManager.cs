//-----------------------------------------------------------------------
// <copyright file="SyncManager.cs" company="MD Development">
//     Copyright (c) MD Development. All rights reserved.
// </copyright>
// <author>Michael Drexler</author>
//-----------------------------------------------------------------------
namespace Prototyp
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Prototyp.Log;
    using Prototyp.Network;
    using Prototyp.View;
    using Prototyp.View.EventArgs;

    /// <summary>
    /// SyncManager manages the synchronisation tasks
    /// </summary>
    public class SyncManager
    {
        /// <summary>
        /// List of watchers for the configured source directories
        /// </summary>
        private Watcher watcher;

        /// <summary>
        /// Syncer object to do the synchronization work
        /// </summary>
        private Syncer syncer;

        /// <summary>
        /// Configuration for the synchronization management
        /// </summary>
        private Config defaultConfig;

        /// <summary>
        /// Command window on the console
        /// </summary>
        private IWindow userWindow;

        /// <summary>
        /// Log window on the console
        /// </summary>
        private IWindow logWindow;

        /// <summary>
        /// Info window on the console
        /// </summary>
        private IWindow infoWindow;

        /// <summary>
        /// Window manager to manage the windows
        /// </summary>
        private WindowManager windowManager;

        /// <summary>
        /// Network interface
        /// </summary>
        ////private INetwork network;

        /// <summary>
        /// A queue for the synchronization worker to progress
        /// </summary>
        private ConcurrentQueue<Job> queueOfJobs;

        /// <summary>
        /// Main thread of the program
        /// </summary>
        private Thread mainThread;

        /// <summary>
        /// Synchronization worker runs within an own thread
        /// </summary>
        private Thread syncWorker;

        /// <summary>
        /// Logger runs within an own thread
        /// </summary>
        private Thread loggerThread;

        /// <summary>
        /// The logger instance
        /// </summary>
        private ILogger logger = Logger.Instance;

        /// <summary>
        /// Initializes a new instance of the <see cref="SyncManager"/> class 
        /// </summary>
        /// <param name="winManager">Manages the windows</param>
        public SyncManager(WindowManager winManager)
        {
            this.queueOfJobs = new ConcurrentQueue<Job>();
            this.windowManager = winManager;
            this.mainThread = Thread.CurrentThread;
            this.syncer = new Syncer(this.queueOfJobs);
        }

        /// <summary>
        /// Manages the work
        /// </summary>
        public void DoWork()
        {
            if (this.defaultConfig == null)
            {
                return;
            }

            // Create a new worker thread for synchronizing
            this.syncWorker = new Thread(new ThreadStart(this.syncer.StartSynchronising));
            this.syncWorker.IsBackground = true;
            this.syncWorker.Start();

            while (true)          
            {
                if (Console.KeyAvailable)
                {
                    if (this.loggerThread != null)
                    {
                        this.loggerThread.Suspend();

                        if (this.windowManager.GetActiveWindow().Equals(this.logWindow))
                        {
                            this.windowManager.MoveWindowToBackGround(this.windowManager.Windows.IndexOf(this.logWindow));
                            if (this.userWindow is ConsoleWindow)
                            {
                                ((ConsoleWindow)this.userWindow).SetCursorToTop();
                            }
                        }

                        string command = Console.ReadLine();
                        this.logger.Logs.Enqueue(new Log.LogEntry(command, LoggingType.Command));

                        // Handle the command with the right command interpreter
                        if (this.userWindow is ConsoleWindow)
                        {
                            if (!string.IsNullOrEmpty(command))
                            {
                                if (command.Length >= 6)
                                {
                                    switch (command.ToLower().Substring(0, 6))
                                    {
                                        case "--add ":
                                            ((ConsoleWindow)this.userWindow).CommandInterpreter = new AddCommand(this.defaultConfig);

                                            // Subscribe to the SourceDirectoryAdded event
                                            AddCommand addCommand = ((ConsoleWindow)this.userWindow).CommandInterpreter as AddCommand;
                                            addCommand.SourceDirectoryAdded += this.AddCommand_SourceDirectoryAdded;
                                            ((ConsoleWindow)this.userWindow).CommandInterpreter.HandleCommand(command.Substring(6));
                                            break;
                                        case "--rem ":
                                            ((ConsoleWindow)this.userWindow).CommandInterpreter = new RemoveCommand(this.defaultConfig);

                                            // Subscribe to the SourceDirectoryRemoved event
                                            RemoveCommand removeCommand = ((ConsoleWindow)this.userWindow).CommandInterpreter as RemoveCommand;
                                            removeCommand.SourceDirectoryRemoved += this.RemoveCommand_SourceDirectoryRemoved;
                                            ((ConsoleWindow)this.userWindow).CommandInterpreter.HandleCommand(command.Substring(6));
                                            break;
                                        case "--cha ":
                                            ((ConsoleWindow)this.userWindow).CommandInterpreter = new ChangeCommand(this.defaultConfig);
                                            ((ConsoleWindow)this.userWindow).CommandInterpreter.HandleCommand(command.Substring(6));
                                            break;
                                        case "--prin":
                                            ((ConsoleWindow)this.userWindow).CommandInterpreter = new PrintConfigCommand(this.defaultConfig);
                                            ((ConsoleWindow)this.userWindow).CommandInterpreter.HandleCommand(string.Empty);
                                            break;
                                        case "--com ":
                                            ((ConsoleWindow)this.userWindow).CommandInterpreter = new CommonCommand(this.defaultConfig);
                                            ((ConsoleWindow)this.userWindow).CommandInterpreter.HandleCommand(command.Substring(6));
                                            break;
                                        case "--quit":
                                            // check if syncher is progressing jobs, ask user if he wants to cancel this jobs
                                            if (this.queueOfJobs.Count > 0)
                                            {
                                                ((ConsoleWindow)this.userWindow).Write("Are you sure, you want to stop the synchronization? Y/N", ConsoleColor.White);
                                                command = Console.ReadLine();
                                                if (command.ToLower().Equals("y"))
                                                {
                                                    command = "exit";
                                                }
                                            }
                                            else
                                            {
                                                command = "exit";
                                            }

                                            break;
                                        default:
                                            this.logger.Logs.Enqueue(new LogEntry("SyncManager: Unknown command!", LoggingType.Error));
                                            break;
                                    }
                                }
                                else
                                {
                                    this.logger.Logs.Enqueue(new LogEntry("SyncManager: Invalid input!", LoggingType.Error));
                                }
                            }
                        }

                        if (command.ToLower().Equals("exit"))
                        {
                            break;
                        }

                        ((ConsoleWindow)this.userWindow).ClearWindow();
                        ((ConsoleWindow)this.logWindow).ClearWindow();
                        ((ConsoleWindow)this.logWindow).SetCursorToTop();

                        // Continue logging
                        this.loggerThread.Resume(); 
                    }
                    else
                    {
                        this.logger.Logs.Enqueue(new Log.LogEntry("SyncManager: Logger not running!", LoggingType.Warning));
                    }
                }
                else
                {
                    if (this.windowManager.GetActiveWindow().Equals(this.userWindow))
                    {
                        this.windowManager.MoveWindowToBackGround(this.windowManager.Windows.IndexOf(this.userWindow));
                        if (this.logWindow is ConsoleWindow)
                        {
                            ((ConsoleWindow)this.logWindow).SetCursorPosition();
                        }
                    }
                }
            }

            // Close Synchronization Program
            this.syncWorker.Abort();
            if (this.loggerThread.ThreadState == ThreadState.Suspended)
            {
                this.loggerThread.Resume();
            }

            this.loggerThread.Abort();
            this.syncWorker.Join();
            this.loggerThread.Join();
            this.mainThread.Abort();
        }

        /// <summary>
        /// Initializes the SyncManger. Load Config, create watchers for soucre directory.
        /// Start logger thread.
        /// </summary>
        public void Init()
        {
            this.logger.Logs.Enqueue(new Log.LogEntry("SyncManager: Start initializing ... ", LoggingType.Info));

            // Window for info
            Position topLeftInfo = new Position(2, 2);
            this.windowManager.CreateWindow(topLeftInfo, 145, 15, ConsoleColor.Green, "Info-Box");
            this.infoWindow = this.windowManager.GetActiveWindow();
            this.WriteInfosToInfoBox(); // Display help infos

            // Window for user input
            Position topLeft = new Position(2, 20);
            this.windowManager.CreateWindow(topLeft, 145, 5, ConsoleColor.Black, "Command-Box");
            this.userWindow = this.windowManager.GetActiveWindow();

            // Window for logging 
            Position topLeftLeft = new Position(topLeft.X, topLeft.Y + this.userWindow.WindowHeight + 4);
            this.windowManager.CreateWindow(topLeftLeft, 145, 50, ConsoleColor.Black, "Logger-Box");
            this.logWindow = this.windowManager.GetActiveWindow();            

            bool isSuccessfullyLoaded = this.LoadConfig();
            ((Logger)this.logger).Config = isSuccessfullyLoaded ? this.defaultConfig : null;

            // Start logger thread and start logging
            this.loggerThread = new Thread(new ParameterizedThreadStart(this.StartLogging));
            this.loggerThread.Start(this.logger);

            if (isSuccessfullyLoaded)
            {
                this.logger.Logs.Enqueue(new LogEntry("SyncManager: Default configuration file successfully loaded!", LoggingType.Success));
                this.logger.Logs.Enqueue(new LogEntry("SyncManager: Create new watcher to monitor the source directories ...", LoggingType.Info));
                this.watcher = new Watcher(this.queueOfJobs, this.logWindow, this.defaultConfig);
                this.watcher.ActivateMonitoring();
                this.logger.Logs.Enqueue(new LogEntry("SyncManager: Watcher created and ready for monitoring!", LoggingType.Success));

                // Check initially the sync between source and directory (Blocks the main thread until finished --> no config change)
                this.syncer.Init(this.defaultConfig);
            }
            else
            {
                this.logger.Logs.Enqueue(new LogEntry("SyncManager: Default configuration file couldn´t be loaded!",  LoggingType.Error));
                this.logger.Logs.Enqueue(new LogEntry("SYncManager: Config not loaded - Monitoring couldn´t be activated!", LoggingType.Warning));
            }
        }

        /// <summary>
        /// Loads the default config from an XML file
        /// </summary>
        /// <returns>If config file is successfully loaded</returns>
        public bool LoadConfig()
        {
            this.logger.Logs.Enqueue(new Log.LogEntry("SyncManager: Start loading the default configuration file ...", LoggingType.Info));

            string defaultConfigDirectory = ConfigurationManager.AppSettings["DefaultConfigDirectory"];
            string defaultConfigFilename = ConfigurationManager.AppSettings["DefaultConfigFileName"];

            try
            {
                defaultConfigDirectory = System.IO.Directory.Exists(defaultConfigDirectory) ? defaultConfigDirectory : null;
                defaultConfigFilename = File.Exists(Path.Combine(defaultConfigDirectory, defaultConfigFilename)) ? defaultConfigFilename : null;
                if (defaultConfigDirectory == null)
                {
                    this.logger.Logs.Enqueue(new Log.LogEntry("SyncManager: Default config directory doesn´t exist! Cannot load the default config file!", LoggingType.Error));
                }
                else
                {
                    if (defaultConfigFilename == null)
                    {
                        this.logger.Logs.Enqueue(new Log.LogEntry("SyncManager: No config file exists! Check your default config settings!", LoggingType.Error));
                    }
                    else
                    {
                        // Apply default Configuration
                        this.defaultConfig = new Config();
                        this.logger.Logs.Enqueue(new Log.LogEntry("SyncManager: Start reading default config file ...", LoggingType.Info));
                        return this.defaultConfig.ReadConfig(defaultConfigDirectory, defaultConfigFilename);
                    }
                }        
            }
            catch (Exception ex)
            {
                this.logger.Logs.Enqueue(new LogEntry(string.Format("SyncManager: [Exception] {0}", ex.Message), LoggingType.Error));
            }

            return false;
        }

        /// <summary>
        /// Take all log entries from a list and log it.
        /// Runs within an own thread.
        /// </summary>
        /// <param name="obj">Logger object</param>
        private void StartLogging(object obj)
        {
            Logger logger = (Logger)obj;

            while (true)
            {
                if (logger.Logs.Count > 0)
                {
                    while (logger.Logs.Count > 0)
                    {
                        Log.LogEntry temp = null;
                        this.logger.Logs.TryDequeue(out temp);

                        if (temp != null)
                        {
                            try
                            {
                                logger.WriteToConsole(temp.Message, temp.LogType, this.logWindow);
                                logger.WriteToFile(temp.Message, temp.LogType);
                            }
                            catch (Exception ex)
                            {
                                this.logger.Logs.Enqueue(new LogEntry(string.Format("Logger: Error on logging! {0}", ex.Message), LoggingType.Error));
                                break;
                            }
                        }
                    }
                }
                else
                {
                    try
                    {
                        Thread.Sleep(2000);
                    }
                    catch (ThreadInterruptedException e)
                    {
                        this.logger.Logs.Enqueue(new Log.LogEntry("Syncer:[Exception] Interrupted Exception" + e.Message, LoggingType.Error));
                    } 
                }
            }
        }

        /// <summary>
        /// Handles the source diretory removed event
        /// </summary>
        /// <param name="sender">Add Command</param>
        /// <param name="e">Source Directory Informations</param>
        private void RemoveCommand_SourceDirectoryRemoved(object sender, SourceDirectoryRemovedEventArgs e)
        {
            Directory removeDirectory = e.SourceDirectory;
            this.watcher.RemoveFileWatcher(removeDirectory);
        }

        /// <summary>
        /// Handles the source directory added event 
        /// </summary>
        /// <param name="sender">AddCommand fires the event</param>
        /// <param name="e">Source Directory Informations</param>
        private void AddCommand_SourceDirectoryAdded(object sender, SourceDirectoryAddedEventArgs e)
        {
            SourceDirectoryAddedEventArgs newSource = e;
            FileSystemWatcher watcher = new FileSystemWatcher(newSource.SourceDirectory.Path);
            watcher.IncludeSubdirectories = e.SourceDirectory.IncludeSubdirectories;
            this.watcher.CreateFileWatcher(watcher);
        }

        /// <summary>
        /// Write help information to the info box.
        /// </summary>
        private void WriteInfosToInfoBox()
        {
            if (this.infoWindow is ConsoleWindow)
            {
                ((ConsoleWindow)this.infoWindow).SetCursorToTop();
                string str =
         "Commands and Syntax to use this program " + Environment.NewLine +
         "Quit    Program: --quit" + Environment.NewLine +
         "Print    config: --prin                                                                     (Print current configuration)" + Environment.NewLine +
         "Common Settings: --com <SP> fsize:<size> > bsize:<size> > lsize:<size> > act:<true/false>  (Define FileSize for block compare," + Environment.NewLine +
         "                                                                      define block size, define log size, activate parallel sync, )" + Environment.NewLine +
         "Change Target D: --cha <SP> src:<path> > dst:<path>                                      (if exist, change it, else add it)" + Environment.NewLine +
         "Change Exceptio: --cha <SP> src:<path> > exc:<path>                                      (if exist, change it, else add it)" + Environment.NewLine +
         "Remove Targ_Exc: --rem <SP> src:<path> > dst:<path>;<path> > exc:<path>;<path>        (remove target or exception directory optionally)" + Environment.NewLine +
         "Remove SourceDi: --rem <SP> src:<path>                                                      (if only 'src:' argument is passed, delete source" + Environment.NewLine +
         "Add new  Source: --add <SP> src:<path> > dst:<path>;<path> > exc:<path>;<path> > sub:true/false  (Greater than '>' between arguments" + Environment.NewLine +
         "(Add a new source directory with target and optionally exception directory and include subdirectories for sync)";

                ((ConsoleWindow)this.infoWindow).Write(str, ((ConsoleWindow)this.infoWindow).ForegroundColor);
            }
        }
    }
}
