//-----------------------------------------------------------------------
// <copyright file="AddCommand.cs" company="MD Development">
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
    /// Add new SourceDirectory command
    /// </summary>
    public class AddCommand : CommandInterpreter
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
        /// Logger instance
        /// </summary>
        private ILogger logger = Logger.Instance;

        /// <summary>
        /// Configuration for the program
        /// </summary>
        private Config config;

        /// <summary>
        /// Initializes a new instance of the <see cref="AddCommand"/> class
        /// </summary>
        /// <param name="defaultConfig">Configuration for the program</param>
        public AddCommand(Config defaultConfig)
        {
            this.config = defaultConfig;
        }

        /// <summary>
        /// Occurs, when a new source-directory was added to the collection.s
        /// </summary>
        public event EventHandler<SourceDirectoryAddedEventArgs> SourceDirectoryAdded;

        /// <summary>
        /// Handles an add new source directory command
        /// </summary>
        /// <param name="input">Input to handle</param>
        public override void HandleCommand(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                this.logger.Logs.Enqueue(new LogEntry("AddCommand: Argument syntax exception!", LoggingType.Error));
                return;
            }

            Prototyp.Directory source = null;
            string[] arguments = input.Split('>');
            if (arguments.Length >= 2)
            {
                for (int i = 0; i < arguments.Length; ++i)
                {
                    if (arguments[i].ToLower().StartsWith("src:"))
                    {
                        arguments[i] = arguments[i].Substring(4);

                        if (System.IO.Directory.Exists(arguments[i]))
                        {
                            List<Prototyp.Directory> targets = new List<Directory>();
                            List<Prototyp.Directory> exceptions = new List<Directory>();
                            source = new Directory(targetDirectories: targets, exceptionDirectories: exceptions);
                            source.IncludeSubdirectories = true; // Default

                            if (arguments[i].ToLower().Substring(0, 2).Equals("\\"))
                            {
                                source.DirectoryType = DirectoryType.Shared;
                            }
                            else
                            {
                                source.DirectoryType = DirectoryType.Local;
                            }

                            source.Path = arguments[i];
                        }
                    }
                    else if (arguments[i].ToLower().StartsWith("dst:"))
                    {
                        // Check target directories parameter
                        if (source == null)
                        {
                            // Bad sequence of commands
                            this.logger.Logs.Enqueue(new LogEntry("AddCommand: Bad sequence of commands!", LoggingType.Error));
                            return;
                        }

                        arguments[i] = arguments[i].Substring(4); // Cut (dst:)<path>;<path>;
                        string[] listOfTargets = arguments[i].Split(';'); // Get array from destinations if available

                        if (listOfTargets.Length > 1)
                        {
                            for (int a = 0; a < listOfTargets.Length; ++a)
                            {
                                // not sure if Directory.Exists is right for target path checking???
                                if (System.IO.Directory.Exists(listOfTargets[a]))
                                {
                                    Directory target = new Directory();

                                    if (listOfTargets[a].ToLower().Substring(0, 2).Equals("\\"))
                                    {
                                        target.DirectoryType = DirectoryType.Shared;
                                    }
                                    else
                                    {
                                        target.DirectoryType = DirectoryType.Local;
                                    }

                                    target.Path = listOfTargets[a];
                                    source.TargetDirectories.Add(target);
                                }
                                else
                                {
                                    this.logger.Logs.Enqueue(new LogEntry(string.Format("AddCommand: Target directory-{0} does not exist!", listOfTargets[a]), LoggingType.Error));
                                    return;
                                }
                            }
                        }
                        else
                        {
                            if (System.IO.Directory.Exists(listOfTargets[0]))
                            {
                                Directory target = new Directory();

                                if (listOfTargets[0].ToLower().Substring(0, 2).Equals("\\\\"))
                                {
                                    target.DirectoryType = DirectoryType.Shared;
                                }
                                else
                                {
                                    target.DirectoryType = DirectoryType.Local;
                                }

                                target.Path = listOfTargets[0];
                                source.TargetDirectories.Add(target);
                            }
                            else
                            {
                                this.logger.Logs.Enqueue(new LogEntry(string.Format("AddCommand: Target directory-{0} does not exist!", listOfTargets[0]), LoggingType.Error));
                                return;
                            }
                        }
                    }  
                    else if (arguments[i].ToLower().StartsWith("exc:"))
                    {
                        // Check exception diretories parameter
                        if (source == null)
                        {
                            // Bad sequence of commands
                            this.logger.Logs.Enqueue(new LogEntry("AddCommand: Bad sequence of commands!", LoggingType.Error));
                            return;
                        }

                        if (source.TargetDirectories == null && source.TargetDirectories.Count == 0)
                        {
                            // Bad sequence of commands
                            this.logger.Logs.Enqueue(new LogEntry("AddCommand: Bad sequence of commands!", LoggingType.Error));
                            return;
                        }

                        arguments[i] = arguments[i].Substring(4); // Cut (exc:)<path>;<path>;
                        string[] listOfExceptions = arguments[i].Split(';'); // Get array from exceptions if available

                        if (listOfExceptions.Length > 1)
                        {
                            for (int a = 0; a < listOfExceptions.Length; ++a)
                            {
                                // not sure if Directory.Exists is right for target path checking???
                                if (System.IO.Directory.Exists(listOfExceptions[a]))
                                {
                                    Directory exception = new Directory();

                                    if (listOfExceptions[a].ToLower().Substring(0, 4).Equals("\\\\"))
                                    {
                                        exception.DirectoryType = DirectoryType.Shared;
                                    }
                                    else
                                    {
                                        exception.DirectoryType = DirectoryType.Local;
                                    }

                                    exception.Path = listOfExceptions[a];
                                    source.ExceptionDirectories.Add(exception);
                                }
                                else
                                {
                                    this.logger.Logs.Enqueue(new LogEntry(string.Format("AddCommand: Target directory-{0} does not exist!", listOfExceptions[a]), LoggingType.Error));
                                    return;
                                }
                            }
                        }
                        else
                        {
                            if (System.IO.Directory.Exists(listOfExceptions[0]))
                            {
                                Directory exception = new Directory();

                                if (listOfExceptions[0].ToLower().Substring(0, 4).Equals("\\\\"))
                                {
                                    exception.DirectoryType = DirectoryType.Shared;
                                }
                                else
                                {
                                    exception.DirectoryType = DirectoryType.Local;
                                }

                                exception.Path = listOfExceptions[0];
                                source.ExceptionDirectories.Add(exception);
                            }
                            else
                            {
                                this.logger.Logs.Enqueue(new LogEntry(string.Format("AddCommand: Target directory: {0} does not exist!", listOfExceptions[0]), LoggingType.Error));
                                return;
                            }
                        }
                    }   
                    else if (arguments[i].ToLower().StartsWith("sub:"))
                    {
                        // Check include subdirectories within the source directory parameter
                        arguments[i] = arguments[i].Substring(4);

                        if (arguments[i].ToLower().Equals("false"))
                        {
                            source.IncludeSubdirectories = false;
                        }
                        else
                        {
                            source.IncludeSubdirectories = true; // Default
                        }
                    }
                    else
                    {
                        this.logger.Logs.Enqueue(new LogEntry(string.Format("AddCommand: Argument syntax error {0}!", arguments[i]), LoggingType.Error));
                        return;
                    }
                }

                if (source != null)
                {
                    this.config.SourceDirectories.Add(source);
                    if (this.SourceDirectoryAdded != null)
                    {
                        this.SourceDirectoryAdded(this, new SourceDirectoryAddedEventArgs(source));
                    }

                    this.logger.Logs.Enqueue(new LogEntry(string.Format("AddCommand: SourceDirectory {0} added to config!", source.Path), LoggingType.Success));
                }
            }
            else
            {
                this.logger.Logs.Enqueue(new LogEntry("AddCommand: Missing parameter(s)!", LoggingType.Error));
                return;
            }
        }
    }
}
