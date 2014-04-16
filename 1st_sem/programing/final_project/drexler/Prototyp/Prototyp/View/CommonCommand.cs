//-----------------------------------------------------------------------
// <copyright file="CommonCommand.cs" company="MD Development">
//     Copyright (c) MD Development. All rights reserved.
// </copyright>
// <author>Michael Drexler</author>
//-----------------------------------------------------------------------
namespace Prototyp.View
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Prototyp.Log;

    /// <summary>
    /// Common command
    /// </summary>
    public class CommonCommand : CommandInterpreter
    {
        /// <summary>
        /// Logger instance
        /// </summary>
        private ILogger logger = Logger.Instance;

        /// <summary>
        /// Configuration object to change
        /// </summary>
        private Config config;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommonCommand"/> class
        /// </summary>
        /// <param name="defaultConfig">Configuration of the program</param>
        public CommonCommand(Config defaultConfig)
        {
            this.config = defaultConfig;
        }

        /// <summary>
        /// Handle the incoming command
        /// </summary>
        /// <param name="input">Incoming command</param>
        public override void HandleCommand(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                this.logger.Logs.Enqueue(new LogEntry("CommonCommand: Argument syntax exception!", LoggingType.Error));
                return;
            }

            // "Common Settings: --com <SP> fsize:<size> > bsize:<size> > lsize:<size> > act:<true/false>
            string[] arguments = input.Split('>');

            if (arguments.Length >= 1)
            {
                for (int i = 0; i < arguments.Length; ++i)
                {
                    if (arguments[i].ToLower().StartsWith("fsize:"))
                    {
                        arguments[i] = arguments[i].Substring(6);
                        long fileSize = this.config.FileSizeForBlockCompare;
                        long.TryParse(arguments[i], out fileSize);
                        this.logger.Logs.Enqueue(new LogEntry(string.Format("CommonCommand: Changed file size for block compare from {0} to {1}!", this.config.FileSizeForBlockCompare, fileSize), LoggingType.Info));
                        this.config.FileSizeForBlockCompare = fileSize;
                    }
                    else if (arguments[i].ToLower().StartsWith("bsize:"))
                    {
                        arguments[i] = arguments[i].Substring(6);
                        long blockSize = this.config.BlockSize;
                        long.TryParse(arguments[i], out blockSize);
                        this.logger.Logs.Enqueue(new LogEntry(string.Format("CommonCommand: Changed block size for block compare from {0} to {1}!", this.config.BlockSize, blockSize), LoggingType.Info));
                        this.config.BlockSize = blockSize;
                    }
                    else if (arguments[i].ToLower().StartsWith("lsize:"))
                    {
                        arguments[i] = arguments[i].Substring(6);
                        long logSize = this.config.FileSizeLogFile;
                        long.TryParse(arguments[i], out logSize);
                        this.logger.Logs.Enqueue(new LogEntry(string.Format("CommonCommand: Changed log file size from {0} to {1}!", this.config.FileSizeLogFile, logSize), LoggingType.Info));
                        this.config.FileSizeLogFile = logSize;
                    }
                    else if (arguments[i].ToLower().StartsWith("act:"))
                    {
                        arguments[i] = arguments[i].Substring(4);
                        if (arguments[i].ToLower().StartsWith("true"))
                        {
                            this.config.ParallelSync = true;
                            this.logger.Logs.Enqueue(new LogEntry("CommonCommand: Parallel synchronization activated!", LoggingType.Info));
                        }
                        else if (arguments[i].ToLower().StartsWith("false"))
                        {
                            this.config.ParallelSync = false;
                            this.logger.Logs.Enqueue(new LogEntry("CommonCommand: Parallel synchronization deactivated!", LoggingType.Info));
                        }
                        else
                        {
                            this.logger.Logs.Enqueue(new LogEntry("CommonCommand: Argument syntax error!", LoggingType.Error));
                        }
                    }
                    else
                    {
                        this.logger.Logs.Enqueue(new LogEntry("CommonCommand: Argument syntax error!", LoggingType.Error));
                    }
                }
            }
            else
            {
                this.logger.Logs.Enqueue(new LogEntry("CommonCommand: Argument syntax error!", LoggingType.Error));
            }
        }
    }
}
