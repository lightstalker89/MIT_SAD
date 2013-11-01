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
    using System.Collections.Generic;

    /// <summary>
    /// </summary>
    internal class SyncItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SyncItem"/> class
        /// </summary>
        /// <param name="destinationFolder">
        /// Destination folder where the file should be copied to
        /// </param>
        /// <param name="sourceFile">
        /// Source file
        /// </param>
        /// <param name="fileAction">
        /// Action for the file
        /// </param>
        public SyncItem(IEnumerable<string> destinationFolder, string sourceFile, FileAction fileAction)
        {
            this.Destinations = destinationFolder;
            this.SourceFile = sourceFile;
            this.FileAction = fileAction;
        }

        /// <summary>
        /// Gets or sets the source file
        /// </summary>
        public string SourceFile { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="FileAction"/>
        /// </summary>
        public FileAction FileAction { get; set; }

        /// <summary>
        /// Gets or sets the destination directories for parallel copying
        /// </summary>
        public IEnumerable<string> Destinations { get; set; }

        /// <summary>
        /// Gets or sets the retries
        /// </summary>
        public int Retries { get; set; }
    }
}