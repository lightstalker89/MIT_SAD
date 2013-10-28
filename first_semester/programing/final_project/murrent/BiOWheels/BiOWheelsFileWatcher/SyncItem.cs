// *******************************************************
// * <copyright file="SyncItem.cs" company="MDMCoWorks">
// * Copyright (c) Mario Murrent. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Mario Murrent</author>
// *******************************************************/
namespace BiOWheelsFileWatcher
{
    /// <summary>
    /// </summary>
    internal class SyncItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SyncItems"/> class
        /// </summary>
        /// <param name="destinationFolder">
        /// </param>
        /// <param name="sourceFile">
        /// </param>
        /// <param name="fileAction">
        /// </param>
        public SyncItem(string destinationFolder, string sourceFile, FileAction fileAction)
        {
            this.DestinationFolder = destinationFolder;
            this.SourceFile = sourceFile;
            this.FileAction = fileAction;
        }

        /// <summary>
        /// Gets or sets the destination folder
        /// </summary>
        public string DestinationFolder { get; set; }

        /// <summary>
        /// Gets or sets the source file
        /// </summary>
        public string SourceFile { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="FileAction"/>
        /// </summary>
        public FileAction FileAction { get; set; }
    }
}