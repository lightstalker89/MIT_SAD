//-----------------------------------------------------------------------
// <copyright file="Config.cs" company="MD Development">
//     Copyright (c) MD Development. All rights reserved.
// </copyright>
// <author>Michael Drexler</author>
//-----------------------------------------------------------------------
namespace Prototyp
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml;
    using System.Xml.Linq;
    using Prototyp.Log;
    using Prototyp.View.EventArgs;

    /// <summary>
    /// Config class, 
    /// </summary>
    public class Config
    {
        // Fields 

        /// <summary>
        /// Activate parallel synchronization
        /// </summary>
        private bool parallelSync = false;

        /// <summary>
        /// File size to activate block compare
        /// </summary>
        private long fileSizeForBlockCompare;

        /// <summary>
        /// Block sizes for the block compare
        /// </summary>
        private long blockSize;

        /// <summary>
        /// File size for the log file
        /// </summary>
        private long fileSizeLogFile;

        /// <summary>
        /// Configured source directories
        /// </summary>
        private List<Directory> sourceDirectories; // List of Sourcedirectories; includes List of Targetdirectories, includes list of Exceptiondirectories
        
        /// <summary>
        /// Logger instance to write logs
        /// </summary>
        private ILogger logger = Logger.Instance;

        /// <summary>
        /// Initializes a new instance of the <see cref="Config"/> class 
        /// </summary>
        public Config()
        {
            this.sourceDirectories = new List<Directory>();
        }

        // Properties

        /// <summary>
        /// Gets the defined source directories
        /// </summary>
        public List<Directory> SourceDirectories
        {
            get { return this.sourceDirectories; }
        }

        /// <summary>
        /// Gets or sets the size of a file at which block comparing is prefered
        /// </summary>
        public long FileSizeForBlockCompare
        {
            get { return this.fileSizeForBlockCompare; }
            set { this.fileSizeForBlockCompare = value; }
        }

        /// <summary>
        /// Gets or sets the size of a block for comparing
        /// </summary>
        public long BlockSize
        {
            get { return this.blockSize; }
            set { this.blockSize = value; }
        }

        /// <summary>
        /// Gets or sets the size of the log file
        /// </summary>
        public long FileSizeLogFile
        {
            get { return this.fileSizeLogFile; }
            set { this.fileSizeLogFile = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the "ParallelSyn"-Feature is active or not
        /// </summary>
        public bool ParallelSync
        {
            get { return this.parallelSync; }
            set { this.parallelSync = value; }
        }

        // Methods

        /// <summary>
        /// Read the default config from xml file and fill the object
        /// </summary>
        /// <param name="configDirectory">Directory for the default configuration file</param>
        /// <param name="configFilename">File name for the default configuration file</param>
        /// <returns>Success state of reading</returns>
        public bool ReadConfig(string configDirectory, string configFilename)
        {
            try
            {
                XDocument doc = XDocument.Load(Path.Combine(configDirectory, configFilename));
                XNamespace ns = doc.Root.Name.Namespace;

                XElement sourceDirectoriesElement = (from d in doc.Root.Descendants(ns + "SourceDirectories")
                                                      select d).FirstOrDefault();

                long.TryParse(doc.Root.Element(ns + "FileSize").Value, out this.fileSizeForBlockCompare);
                long.TryParse(doc.Root.Element(ns + "BlockSize").Value, out this.blockSize);
                long.TryParse(doc.Root.Element(ns + "LogFileSize").Value, out this.fileSizeLogFile);
                bool.TryParse(doc.Root.Element(ns + "ParallelSync").Value, out this.parallelSync);

                IEnumerable<XElement> source = from d in sourceDirectoriesElement.Elements()
                                               select d;

                if (source != null && source.Count() > 0)
                {
                    // Loop through all source directories
                    foreach (XElement sourceDir in source)
                    {
                        List<Directory> tempTargetItems = new List<Directory>();
                        List<Directory> tempExceptionItems = new List<Directory>();

                        // Check if source directory has Path, TargetDirectories, ExceptionDirectories, IncludeSubdirectories etc. Options
                        if (sourceDir.HasElements)
                        {
                            // childchildElement = TargetDirectory
                            XElement targetElement = (from d in sourceDir.Elements(ns + "TargetDirectories")
                                                      select d).FirstOrDefault();

                            XElement exceptionElement = (from d in sourceDir.Elements(ns + "ExceptionDirectories")
                                                         select d).FirstOrDefault();

                            // Target Directories
                            // Check if target directories element exists 
                            if (targetElement != null)
                            {
                                // Loop through all target directories
                                foreach (XElement childchildElement in targetElement.Elements())
                                {
                                    Directory targetItem = new Directory();
                                    targetItem.Path = childchildElement.Element(ns + "Path").Value;
                                    if (childchildElement.HasAttributes)
                                    {
                                        // check if passed type is one of the three defined local,share,network.
                                        foreach (XAttribute attr in childchildElement.Attributes())
                                        {
                                            if (attr.Name.LocalName == "type")
                                            {
                                                switch (attr.Value.ToLower())
                                                {
                                                    case "shared": targetItem.DirectoryType = DirectoryType.Shared;
                                                        break;
                                                    case "local": targetItem.DirectoryType = DirectoryType.Local;
                                                        break;
                                                    case "network": targetItem.DirectoryType = DirectoryType.Network;
                                                        break;
                                                    default:
                                                        // ConsoleLogger.Write("Ungültiger Typ als Zielverzeichnis!", LogType.Error);
                                                        break;
                                                }
                                            }
                                            else if (attr.Name.LocalName == "ip")
                                            {
                                                IPAddress address;
                                                IPAddress.TryParse(attr.Value, out address);
                                                targetItem.RemoteHost = address;
                                            }
                                            else if (attr.Name.LocalName == "port")
                                            {
                                                int port = 0;
                                                int.TryParse(attr.Value, out port);
                                                targetItem.Port = port;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        throw new XmlException("None of the following attributes configured: type, ip, host!");
                                    }

                                    // Check if target path is equal with attribute value for directory type
                                    if (targetItem.Path.StartsWith("\\") && targetItem.DirectoryType != DirectoryType.Shared)
                                    {
                                        throw new XmlException("Directory type is not valid for this path!");
                                    }

                                    if (targetItem.DirectoryType == DirectoryType.Network && targetItem.RemoteHost == null && targetItem.Port == 0)
                                    {
                                        throw new XmlException("Directory type is not valid! Check the attributes in the config file and try it again!");
                                    }

                                    tempTargetItems.Add(targetItem);
                                }
                            }
                            else
                            {
                                throw new XmlException("No target directory configured!");
                            }

                            // End of target
                            // Exception Directory
                            // Check if exception directories exists
                            if (exceptionElement != null)
                            {
                                foreach (XElement childchildElement in exceptionElement.Elements())
                                {
                                    Directory exceptionItem = new Directory();
                                    exceptionItem.Path = childchildElement.Element(ns + "Path").Value;

                                    tempExceptionItems.Add(exceptionItem);
                                }
                            }

                            // End of exception 
                            Directory sourceDirItem = new Directory(tempTargetItems, tempExceptionItems);
                            sourceDirItem.Path = sourceDir.Element(ns + "Path").Value;
                            sourceDirItem.IncludeSubdirectories = Convert.ToBoolean(sourceDir.Element(ns + "IncludeSubdirectories").Value);
                            this.sourceDirectories.Add(sourceDirItem);
                        }
                        else
                        {
                            throw new XmlException("Please check your config file and its values!");
                        }
                    }

                    return true;
                }
                else
                {
                    throw new XmlException("No source directory configured!");
                }
            }
            catch (XmlException ex)
            {
                this.logger.Logs.Enqueue(new Log.LogEntry(ex.Message, LoggingType.Error));
            }

            return false;
        }
    }
}
