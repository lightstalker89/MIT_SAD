//-----------------------------------------------------------------------
// <copyright file="ChangeCommand.cs" company="MD Development">
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
    /// Change command
    /// </summary>
    public class ChangeCommand : CommandInterpreter
    {
        /// <summary>
        /// Logger instance
        /// </summary>
        private ILogger logger = Logger.Instance;

        /// <summary>
        /// Configuration of the program
        /// </summary>
        private Config config;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChangeCommand"/> class
        /// </summary>
        /// <param name="defaultConfig">Configuration of the program</param>
        public ChangeCommand(Config defaultConfig)
        {
            this.config = defaultConfig;
        }

        /// <summary>
        /// Handle incoming command
        /// </summary>
        /// <param name="input">Incoming command</param>
        public override void HandleCommand(string input)
        {
            ////          "Change Target D: --cha <SP> src:<path> > dst:<path>                                      (if exist, change it, else add it)"                   + Environment.NewLine +
            //// "Change Exceptio: --cha <SP> src:<path> > exc:<path>                                      (if exist, change it, else add it)" 
            if (string.IsNullOrEmpty(input))
            {
                this.logger.Logs.Enqueue(new LogEntry("ChangeCommand: Argument syntax exception!", LoggingType.Error));
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

                        if (this.config.SourceDirectories.Exists(m => m.Path.Equals(arguments[i])))
                        {
                            source = this.config.SourceDirectories.Where(m => m.Path.Equals(arguments[i])).Select(m => m).FirstOrDefault();
                        }

                        if (source == null)
                        {
                            this.logger.Logs.Enqueue(new LogEntry(string.Format("ChangeCommand: Passed source directory {0} does not exist in config!", arguments[i]), LoggingType.Error));
                            return;
                        }
                    }
                    else if (arguments[i].ToLower().StartsWith("dst:"))
                    {
                        // Check target directories parameter
                        if (source == null)
                        {
                            // Bad sequence of commands
                            this.logger.Logs.Enqueue(new LogEntry("ChangeCommand: Bad sequence of commands!", LoggingType.Error));
                            return;
                        }

                        arguments[i] = arguments[i].Substring(4); // Cut (dst:)<path>;<path>;
                        string[] listOfTargets = arguments[i].Split(';'); // Get array from destinations if available

                        if (listOfTargets.Length > 1)
                        {
                            for (int a = 0; a < listOfTargets.Length; ++a)
                            {
                                Directory temp = new Directory();
                                temp.Path = listOfTargets[a];
                                source.TargetDirectories.Add(temp);
                                this.logger.Logs.Enqueue(new LogEntry(string.Format("ChangeCommand: New target directory {0} added for synching {1}", temp.Path, source.Path), LoggingType.Success));

                                // this._config.SourceDirectories.Where(m => m.Path.Equals(source.Path)).Select(m => m).SingleOrDefault().TargetDirectories.Add(temp);
                            }
                        }
                        else
                        {
                            Directory temp = new Directory();
                            temp.Path = listOfTargets[0];
                            source.TargetDirectories.Add(temp);
                            this.logger.Logs.Enqueue(new LogEntry(string.Format("ChangeCommand: New target directory {0} added for synching {1}", temp.Path, source.Path), LoggingType.Success));
                        }
                    }
                    else if (arguments[i].ToLower().StartsWith("exc:"))
                    {
                        if (source == null)
                        {
                            this.logger.Logs.Enqueue(new LogEntry("ChangeCommand: Bad sequence of commands!", LoggingType.Error));
                            return;
                        }

                        arguments[i] = arguments[i].Substring(4);
                        string[] listOfExceptions = arguments[i].Split(';');

                        if (listOfExceptions.Length > 1)
                        {
                            for (int a = 0; a < listOfExceptions.Length; ++a)
                            {
                                Directory temp = new Directory();
                                temp.Path = listOfExceptions[a];
                                source.ExceptionDirectories.Add(temp);
                                this.logger.Logs.Enqueue(new LogEntry(string.Format("ChangeCommand: New exception directory {0} added for synching {1}", temp.Path, source.Path), LoggingType.Success));
                            }
                        }
                        else
                        {
                            Directory temp = new Directory();
                            temp.Path = listOfExceptions[0];
                            source.ExceptionDirectories.Add(temp);
                            this.logger.Logs.Enqueue(new LogEntry(string.Format("ChangeCommand: New exception directory {0} added for synching {1}", temp.Path, source.Path), LoggingType.Success));
                        }
                    }
                    else
                    {
                        this.logger.Logs.Enqueue(new LogEntry("ChangeCommand: Argument syntax exception!", LoggingType.Error));
                    }
                }
            }
            else
            {
                this.logger.Logs.Enqueue(new LogEntry("ChangeCommand: Argument syntax exception!", LoggingType.Error));
            }
        }
    }
}
