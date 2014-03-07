//-----------------------------------------------------------------------
// <copyright file="PrintConfigCommand.cs" company="MD Development">
//     Copyright (c) MD Development. All rights reserved.
// </copyright>
// <author>Michael Drexler</author>
//-----------------------------------------------------------------------
namespace Prototyp.View
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Prototyp.Log;

    /// <summary>
    /// Print config command
    /// </summary>
    public class PrintConfigCommand : CommandInterpreter
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
        /// Initializes a new instance of the <see cref="PrintConfigCommand"/> class
        /// </summary>
        /// <param name="defaultConfig">Configuration of the program</param>
        public PrintConfigCommand(Config defaultConfig)
        {
            this.config = defaultConfig;
        }

        /// <summary>
        /// Handles the print config command.
        /// Print the config in a file.
        /// </summary>
        /// <param name="input">Incoming command</param>
        public override void HandleCommand(string input)
        {
            try
            {
                string filePath = Path.Combine(Environment.CurrentDirectory, DateTime.Now.ToString("yyyy-dd-M") + ".txt");
                using (StreamWriter sw = new StreamWriter(filePath, false))
                {
                    sw.WriteLine("------------------------------------------------------------------");
                    sw.WriteLine(string.Format("{0}- Current Configuration", DateTime.Now.ToString()));
                    sw.WriteLine("------------------------------------------------------------------");

                    if (this.config != null && this.config.SourceDirectories != null && this.config.SourceDirectories.Count > 0)
                    {
                        sw.WriteLine("  FileSize BlockCompare: {0}", this.config.FileSizeForBlockCompare);
                        sw.WriteLine("  BlockSize BlockCompare: {0}", this.config.BlockSize);
                        sw.WriteLine("  FileSize LogFile: {0}", this.config.FileSizeLogFile);
                        sw.WriteLine("  Parallel Sync activated: {0}", this.config.ParallelSync);
                        sw.WriteLine("  Count of source directories: {0}", this.config.SourceDirectories.Count);
                        sw.WriteLine();

                        foreach (Prototyp.Directory sourceDir in this.config.SourceDirectories)
                        {
                            sw.WriteLine("------------------------------------------------------------------");
                            sw.WriteLine("  Source Directory: {0}", sourceDir.Path);
                            sw.WriteLine("  Include subdirectores: {0}", sourceDir.IncludeSubdirectories);
                            sw.WriteLine("  Directory Type: {0}", sourceDir.DirectoryType.ToString());
                            
                            if (sourceDir.TargetDirectories != null && sourceDir.TargetDirectories.Count > 0)
                            {
                                sw.WriteLine("  Count of target direcoties for this source: {0}", sourceDir.TargetDirectories.Count.ToString());
                                sw.WriteLine();

                                foreach (Prototyp.Directory targetDir in sourceDir.TargetDirectories)
                                {
                                    sw.WriteLine("      Target Directory: {0}", targetDir.Path);
                                    sw.WriteLine("      Directory Type: {0}", targetDir.DirectoryType.ToString());
                                    sw.WriteLine();
                                }
                            }

                            if (sourceDir.ExceptionDirectories != null && sourceDir.ExceptionDirectories.Count > 0)
                            {
                                sw.WriteLine("  Count of exception directories for this source: {0}", sourceDir.ExceptionDirectories.Count.ToString());
                                sw.WriteLine();

                                foreach (Prototyp.Directory exceptDir in sourceDir.ExceptionDirectories)
                                {
                                    sw.WriteLine("      Exception Directory: {0}", exceptDir.Path);
                                    sw.WriteLine("      Diretory Type: {0}", exceptDir.Path);
                                    sw.WriteLine();
                                }
                            }

                            sw.WriteLine("------------------------------------------------------------------");
                        }
                    }
                    else
                    {
                        this.logger.Logs.Enqueue(new LogEntry("PrintConfigCommand: Config is empty!", LoggingType.Warning));
                    }
                }

                // Open file
                Process.Start(filePath);
            }
            catch (Exception ex)
            {
                this.logger.Logs.Enqueue(new LogEntry(string.Format("PrintConfigCommand: {0}", ex.Message), LoggingType.Error));
            }
        }
    }
}
