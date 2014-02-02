//-----------------------------------------------------------------------
// <copyright file="Logger.cs" company="MD Development">
//     Copyright (c) MD Development. All rights reserved.
// </copyright>
// <author>Michael Drexler</author>
//-----------------------------------------------------------------------
namespace Prototyp.Log
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Prototyp.Log.Exceptions;
    using Prototyp.View;

    /// <summary>
    /// Global logger
    /// </summary>
    public class Logger : ILogger
    {
        /// <summary>
        /// File name of the log file
        /// </summary>
        private const string LOGFILENAME = "log.txt";

        /// <summary>
        /// File name of the overflow log file
        /// </summary>
        private const string OVERFLOWLOGFILENAME = "log.txt.bak";

        /// <summary>
        /// Object used for locking resources
        /// </summary>
        private static object mLock = new object();

        /// <summary>
        /// Object used for locking ressources
        /// </summary>
        private static object o = new object();

        /// <summary>
        /// Instance of the logger
        /// </summary>
        private static Logger logger = null;

        /// <summary>
        /// Configuration of the program
        /// </summary>
        private Config config;

        /// <summary>
        /// Log entries for the logger
        /// </summary>
        private ConcurrentQueue<LogEntry> logs;

        /// <summary>
        /// Prevents a default instance of the <see cref="Logger"/> class from being created
        /// the constructor. usually public, this time it is private to ensure 
        /// no one except this class can use it.
        /// </summary>
        private Logger()
        {
            this.logs = new ConcurrentQueue<LogEntry>();

            // mLock = new object();
        }

        /// <summary>
        /// Gets an instance of the Logger
        /// the public Instance property everyone uses to access the Logger
        /// </summary>
        public static Logger Instance
        {
            get
            {
                // If this is the first time we're referring to the
                // singleton object, the private variable will be null.
                if (logger == null)
                {
                    // for thread safety, lock an object when
                    // instantiating the new Logger object. This prevents
                    // other threads from performing the same block at the
                    // same time.
                    lock (mLock)
                    {
                        // Two or more threads might have found a null
                        // mLogger and are therefore trying to create a 
                        // new one. One thread will get to lock first, and
                        // the other one will wait until mLock is released.
                        // Once the second thread can get through, mLogger
                        // will have already been instantiated by the first
                        // thread so test the variable again. 
                        if (logger == null)
                        {
                            logger = new Logger();
                        }
                    }
                }

                return logger;
            }
        }

        /// <summary>
        /// Gets or sets new log entries
        /// </summary>
        public ConcurrentQueue<LogEntry> Logs
        {
            get { return this.logs; }
            set { this.logs = value; }
        }

        /// <summary>
        /// Gets or sets the configuration object
        /// </summary>
        internal Config Config
        {
            get { return this.config; }
            set { this.config = value; }
        }

        /// <summary>
        /// Write the logs to the window console
        /// </summary>
        /// <param name="logEntry">New log entry</param>
        /// <param name="logType">Type of the new log</param>
        /// <param name="window">Window within the console where to log</param>
        public void WriteToConsole(string logEntry, LoggingType logType, IWindow window)
        {
            Monitor.Enter(o);

            if (window is ConsoleWindow)
            {
                ConsoleWindow win = (ConsoleWindow)window;
                string output = string.Format("{0}-[{1}]: {2}", DateTime.Now, logType.ToString(), logEntry);

                // ConsoleColor oldColor = Console.ForegroundColor;
                // Console.ForegroundColor = ConsoleColor.White;
                ConsoleColor newFontColor = ConsoleColor.White;
                switch (logType)
                {
                    case LoggingType.Command:
                        newFontColor = ConsoleColor.Cyan;
                        break;
                    case LoggingType.Info:
                        newFontColor = ConsoleColor.White;
                        break;
                    case LoggingType.Warning:
                        newFontColor = ConsoleColor.Yellow;
                        break;
                    case LoggingType.Success:
                        newFontColor = ConsoleColor.Green;
                        break;
                    case LoggingType.Error:
                        newFontColor = ConsoleColor.Red;
                        break;
                }

                // Write to a window within the console
                win.Write(output, newFontColor);
            }

            Monitor.Exit(o);
        }

        /// <summary>
        /// Write the logs to the file
        /// </summary>
        /// <param name="logEntry">New log entry</param>
        /// <param name="logType">The type of the new log entry</param>
        public void WriteToFile(string logEntry, LoggingType logType)
        {
            string output = string.Format("{0}-[{1}]: {2}", DateTime.Now, logType.ToString(), logEntry);
            string path = Path.Combine(Environment.CurrentDirectory, LOGFILENAME);
            string overflowPath = Path.Combine(Environment.CurrentDirectory, OVERFLOWLOGFILENAME);

            if (!File.Exists(path))
            {
                File.Create(path).Dispose();
            }

            FileInfo info = new FileInfo(path);
            long fileSize = info.Length;

            // calculate new size of the log file
            long newFileSize = fileSize + Encoding.ASCII.GetBytes(output).Length;

            if (this.Config == null)
            {
                throw new LoggerException("Config file null reference exception! Check the path to the config file!");
            }

            if (newFileSize > this.Config.FileSizeLogFile)
            {
                if (File.Exists(overflowPath))
                {
                    File.Delete(overflowPath);
                }

                File.Move(path, overflowPath);

                // Dispose makes the file accessable for other processes
                File.Create(path).Dispose();
            }

            // Append the data to the log file 
            using (StreamWriter sw = new StreamWriter(path, true))
            {
                sw.WriteLine(output);
            }
            
        }

        /// <summary>
        /// Split strings which are too long for the window
        /// </summary>
        /// <param name="str">String to split</param>
        /// <param name="chunkSize">Chunk size</param>
        /// <returns>Split strings</returns>
        private IEnumerable<string> SplitStringAtLenght(string str, int chunkSize)
        {
            for (int i = 0; i < str.Length; i += chunkSize)
            {
                yield return str.Substring(i, Math.Min(chunkSize, str.Length - i));
            }
        }
    }
}
