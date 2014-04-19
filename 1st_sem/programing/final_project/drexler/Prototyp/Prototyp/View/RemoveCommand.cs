//-----------------------------------------------------------------------
// <copyright file="RemoveCommand.cs" company="MD Development">
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
    using Prototyp.View.EventArgs;

    /// <summary>
    /// Remove command
    /// </summary>
    public class RemoveCommand : CommandInterpreter
    {
        /*
        * Commands: Add SourceDirectory, Add TargetDirectory, Add ExceptionDirectory
        *              Remove SourceDirectory, Remove TargetDirectory, Remove ExceptionDirectory
        *              Change LogFileSize, Change BlockSize, Change FileSize, Change ParallelSync
        *           
        * Syntax: 
        *              Add    Source: --Add <SP> src:<path> <SP> dst:<path>;<path> <SP> exc:<path>;<path> <SP> sub:true/false
        *              Change Target: --Cha <SP> src:<path> <SP> dst:<path>                (if exist, change it, else add it)
        *              Change Excep.: --Cha <SP> src:<path> <SP> exc:<path>                (if exist, change it, else add it)
        *              Remove Target: --Rem <SP> src:<path> <SP> dst:<path>;<path>         (if exist, remove it, else do nothing)
        *              Remove Excep.: --Rem <SP> src:<path> <SP> exc:<path>;<path>         (if exist, remove it, else do nothing)
        */

        /// <summary>
        /// Loger instance
        /// </summary>
        private ILogger logger = Logger.Instance;

        /// <summary>
        /// Configuration of the program
        /// </summary>
        private Config config;

        /// <summary>
        /// Initializes a new instance of the <see cref="RemoveCommand"/> class
        /// </summary>
        /// <param name="defaultConfig">Configuration of the program</param>
        public RemoveCommand(Config defaultConfig)
        {
            this.config = defaultConfig;
        }

        /// <summary>
        /// Occurs, when a new source-directory was removed from the collection
        /// </summary>
        public event EventHandler<SourceDirectoryRemovedEventArgs> SourceDirectoryRemoved;

        /// <summary>
        /// Handle the command
        /// </summary>
        /// <param name="input">Incoming command</param>
        public override void HandleCommand(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                this.logger.Logs.Enqueue(new LogEntry("RemoveCommand: Argument syntax exception!", LoggingType.Error));
                return;
            }

            Prototyp.Directory source = null;
            string[] arguments = input.Split('>');

            if (arguments.Length >= 2)
            {
                // Delete destination directory or/and exception directory for the given source directory
                for (int i = 0; i < arguments.Length; ++i)
                {
                    if (arguments[i].ToLower().StartsWith("src:"))
                    {
                        arguments[i] = arguments[i].Substring(4);

                        if (this.config.SourceDirectories.Exists(m => m.Path.Equals(arguments[i])))
                        {
                            source = this.config.SourceDirectories.Where(m => m.Path.Equals(arguments[i])).Select(m => m).FirstOrDefault();
                        }

                        if (source == null)
                        {
                            this.logger.Logs.Enqueue(new LogEntry(string.Format("RemoveCommand: Passed source directory {0} does not exist in config!", arguments[i]), LoggingType.Error));
                            return;
                        }
                    }
                    else if (arguments[i].ToLower().StartsWith("dst:"))
                    {
                        if (source == null)
                        {
                            this.logger.Logs.Enqueue(new LogEntry("RemoveCommand: Bad sequence of commands!", LoggingType.Error));
                            return;
                        }

                        if (source.TargetDirectories == null || source.TargetDirectories.Count == 0)
                        {
                            this.logger.Logs.Enqueue(new LogEntry("RemoveCommand: No target directories available for removing!", LoggingType.Warning));
                            return;
                        }

                        arguments[i] = arguments[i].Substring(4);
                        string[] listOfTargets = arguments[i].Split(';'); // Get array from destinations if available

                        if (listOfTargets.Length > 1)
                        {
                            for (int a = 0; a < listOfTargets.Length; ++a)
                            {
                                // Before trying to removing the targets, check if targets are available, else remove source directory too
                                if (source.TargetDirectories.Count == 0)
                                {
                                    break;
                                }

                                Directory targetToRemove = source.TargetDirectories.Where(m => m.Path.Equals(listOfTargets[a])).Select(m => m).FirstOrDefault();

                                if (targetToRemove != null)
                                {
                                    source.TargetDirectories.Remove(targetToRemove);
                                    this.logger.Logs.Enqueue(new LogEntry(string.Format("RemoveCommand: Target directory {0} removed for source directory {1}!", listOfTargets[a], source.Path), LoggingType.Success));
                                }
                                else
                                {
                                    this.logger.Logs.Enqueue(new LogEntry(string.Format("RemoveCommand: Passed target directory {0} not available in the list of targets for this source directory!", listOfTargets[a]), LoggingType.Warning));
                                }
                            }
                        }
                        else
                        {
                            Directory targetToRemove = source.TargetDirectories.Where(m => m.Path.Equals(listOfTargets[0])).Select(m => m).FirstOrDefault();

                            if (targetToRemove != null)
                            {
                                source.TargetDirectories.Remove(targetToRemove);
                                this.logger.Logs.Enqueue(new LogEntry(string.Format("RemoveCommand: Target directory {0} removed for source directory {1}!", listOfTargets[0], source.Path), LoggingType.Success));
                            }
                            else
                            {
                                this.logger.Logs.Enqueue(new LogEntry(string.Format("RemoveCommand: Passed target directory {0} not available in the list of targets for this source directory!", listOfTargets[0]), LoggingType.Warning));
                            }
                        }
                    }
                    else if (arguments[i].ToLower().StartsWith("exc:"))
                    {
                        if (source == null)
                        {
                            this.logger.Logs.Enqueue(new LogEntry("RemoveCommand: Bad sequence of commands!", LoggingType.Error));
                            return;
                        }

                        if (source.ExceptionDirectories == null || source.ExceptionDirectories.Count == 0)
                        {
                            this.logger.Logs.Enqueue(new LogEntry("RemoveCommand: No exception directories available for removing!", LoggingType.Warning));
                            return;
                        }

                        arguments[i] = arguments[i].Substring(4);
                        string[] listOfEXceptions = arguments[i].Split(';'); // Get array from destinations if available

                        if (listOfEXceptions.Length > 1)
                        {
                            for (int a = 0; a < listOfEXceptions.Length; ++a)
                            {
                                if (source.ExceptionDirectories.Count == 0)
                                {
                                    this.logger.Logs.Enqueue(new LogEntry(string.Format("RemoveCommand: Cannot remove {0} from the exception diretory list because the list is empty!", listOfEXceptions[0]), LoggingType.Warning));
                                    break;
                                }

                                Directory exceptionToRemove = source.ExceptionDirectories.Where(m => m.Path.Equals(listOfEXceptions[a])).Select(m => m).FirstOrDefault();

                                if (exceptionToRemove != null)
                                {
                                    source.ExceptionDirectories.Remove(exceptionToRemove);
                                    this.logger.Logs.Enqueue(new LogEntry(string.Format("RemoveCommand: Exception directory {0} removed for source directory {1}!", listOfEXceptions[a], source.Path), LoggingType.Success));
                                }
                                else
                                {
                                    this.logger.Logs.Enqueue(new LogEntry(string.Format("RemoveCommand: Passed exception directory {0} not available in the list of exceptions for this source directory!", listOfEXceptions[a]), LoggingType.Warning));
                                }
                            }
                        }
                        else
                        {
                            Directory exceptionToRemove = source.ExceptionDirectories.Where(m => m.Path.Equals(listOfEXceptions[0])).Select(m => m).FirstOrDefault();

                            if (exceptionToRemove != null)
                            {
                                source.ExceptionDirectories.Remove(exceptionToRemove);
                                this.logger.Logs.Enqueue(new LogEntry(string.Format("RemoveCommand: Exception directory {0} removed for this source directory {1}!", listOfEXceptions[0], source.Path), LoggingType.Success));
                            }
                            else
                            {
                                this.logger.Logs.Enqueue(new LogEntry(string.Format("RemoveCommand: Passed exception directory {0} not available in the list of exceptions for this source directory!", listOfEXceptions[0]), LoggingType.Warning));
                            }
                        }
                    }
                    else
                    {
                        this.logger.Logs.Enqueue(new LogEntry(string.Format("RemoveCommand: Argument syntax error {0}!", arguments[i]), LoggingType.Error));
                        return;
                    }
                }

                // Before trying to removing the targets, check if targets are available, else remove source directory too
                if (source.TargetDirectories.Count == 0)
                {
                    this.logger.Logs.Enqueue(new LogEntry("RemoveCommand: No targets available! Source directory will be removed!", LoggingType.Warning));
                    this.config.SourceDirectories.Remove(source);

                    if (this.SourceDirectoryRemoved != null)
                    {
                        // Fire source directory removed event
                        this.SourceDirectoryRemoved(this, new SourceDirectoryRemovedEventArgs(source));
                    }

                    this.logger.Logs.Enqueue(new LogEntry(string.Format("RemoveCommand: Source directory removed {0}!", source.Path), LoggingType.Success));
                }
            }
            else if (arguments.Length == 1)
            {
                // Delete source directory
                if (arguments[0].ToLower().StartsWith("src:"))
                {
                    arguments[0] = arguments[0].Substring(4);

                    if (this.config.SourceDirectories.Exists(m => m.Path.Equals(arguments[0])))
                    {
                        source = this.config.SourceDirectories.Where(m => m.Path.Equals(arguments[0])).Select(m => m).FirstOrDefault();
                    }

                    if (source == null)
                    {
                        this.logger.Logs.Enqueue(new LogEntry(string.Format("RemoveCommand: Passed source directory {0} does not exist in config!", arguments[0]), LoggingType.Warning));
                        return;
                    }

                    this.config.SourceDirectories.Remove(source);
                    if (this.SourceDirectoryRemoved != null)
                    {
                        // Fire source directory removed event
                        this.SourceDirectoryRemoved(this, new SourceDirectoryRemovedEventArgs(source));
                    }

                    this.logger.Logs.Enqueue(new LogEntry(string.Format("RemoveCommand: Source directory removed {0}!", source.Path), LoggingType.Success));
                }
                else
                {
                    this.logger.Logs.Enqueue(new LogEntry("RemoveCommand: Argument syntax error!", LoggingType.Error));
                }
            }
            else
            {
                this.logger.Logs.Enqueue(new LogEntry("RemoveCommand: Command syntax error!", LoggingType.Error));
                return;
            }
        }
    }
}
