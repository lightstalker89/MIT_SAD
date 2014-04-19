// *******************************************************
// * <copyright file="QueueItem.cs" company="FGrill">
// * Copyright (c) 2013 Florian Grill. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Florian Grill</author>
// *******************************************************/

namespace ConsoleBoxDataManager
{
    using System.Collections.Generic;

    /// <summary>
    /// Class representing the <see cref="QueueItem"/>
    /// </summary>
    public class QueueItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QueueItem"/> class.
        /// </summary>
        /// <param name="isDirectory">if set to <c>true</c> it is a directory.</param>
        /// <param name="sourceFolder">The source folder.</param>
        /// <param name="sourcePath">The source path.</param>
        /// <param name="name">The name.</param>
        /// <param name="destinationFolder">The destination folder.</param>
        /// <param name="action">The action which has to be performed.</param>
        /// <param name="oldFileName">Old name of the file.</param>
        public QueueItem(
            bool isDirectory, string sourceFolder, string sourcePath, string name, List<List<string>> destinationFolder, string action, string oldFileName)
        {
            this.IsDirectory = isDirectory;
            this.SourceFolder = sourceFolder;
            this.SourcePath = sourcePath;
            this.Name = name;
            this.Action = action;
            this.DestinationFolder = destinationFolder;
            this.OldFileName = oldFileName;
        }

        /// <summary>
        /// Gets a value indicating whether it is a directory.
        /// </summary>
        /// <value>
        ///   <c>true</c> if it is directory; otherwise, <c>false</c>.
        /// </value>
        public bool IsDirectory { get; private set; }

        /// <summary>
        /// Gets the source folder.
        /// </summary>
        /// <value>
        /// The source folder.
        /// </value>
        public string SourceFolder { get; private set; }

        /// <summary>
        /// Gets the source path.
        /// </summary>
        /// <value>
        /// The source path.
        /// </value>
        public string SourcePath { get; private set; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name of the file or directory.
        /// </value>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the action.
        /// </summary>
        /// <value>
        /// The action which has to be performed.
        /// </value>
        public string Action { get; private set; }

        /// <summary>
        /// Gets the destination folder.
        /// </summary>
        /// <value>
        /// The destination folders.
        /// </value>
        public List<List<string>> DestinationFolder { get; private set; }

        /// <summary>
        /// Gets the old name of the file.
        /// </summary>
        /// <value>
        /// The old name of the file.
        /// </value>
        public string OldFileName { get; private set; }
    }
}
