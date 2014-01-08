// *******************************************************
// * <copyright file="FileWatcherJobEventArgs.cs" company="FGrill">
// * Copyright (c) 2013 Florian Grill. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Florian Grill</author>
// *******************************************************/

namespace ConsoleBoxFileWatcher.Events
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Class representing the <see cref="FileWatcherJobEventArgs"/>
    /// </summary>
    public class FileWatcherJobEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileWatcherJobEventArgs"/> class.
        /// </summary>
        /// <param name="isDirectory">if set to <c>true</c> it is directory.</param>
        /// <param name="sourceFolder">The source folder.</param>
        /// <param name="sourcePath">The source Path to the element.</param>
        /// <param name="name">The name of the element.</param>
        /// <param name="destinations">The destinations.</param>
        /// <param name="action">The action.</param>
        /// <param name="message">The message.</param>
        /// <param name="oldFileName">Old name of the file.</param>
        public FileWatcherJobEventArgs(bool isDirectory, string sourceFolder, string sourcePath, string name, List<List<string>> destinations, string action, string message, string oldFileName)
        {
            this.IsDirectory = isDirectory;
            this.SourceFolder = sourceFolder;
            this.SourcePath = sourcePath;
            this.Name = name;
            this.Destinations = destinations;
            this.Action = action;
            this.Message = message;
            this.OldFileName = oldFileName;
        }

        /// <summary>
        /// Gets a value indicating whether is directory or not.
        /// </summary>
        /// <value>
        ///   <c>true</c> if it is directory; otherwise, <c>false</c>.
        /// </value>
        public bool IsDirectory { get; private set; }

        /// <summary>
        /// Gets the source folder Path.
        /// </summary>
        public string SourceFolder { get; private set; }

        /// <summary>
        /// Gets the Path to the element.
        /// </summary>
        public string SourcePath { get; private set; }

        /// <summary>
        /// Gets the name of the source element.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the destination list.
        /// </summary>
        public List<List<string>> Destinations { get; private set; }

        /// <summary>
        /// Gets or the action type
        /// </summary>
        public string Action { get; private set; }

        /// <summary>
        /// Gets the message for the logger.
        /// </summary>
        public string Message { get; private set; }

        /// <summary>
        /// Gets the old name of the file in case of rename event.
        /// </summary>
        public string OldFileName { get; private set; }
    }
}
