// *******************************************************
// * <copyright file="SyncItem.cs" company="MDMCoWorks">
// * Copyright (c) 2013 Mario Murrent. All rights reserved.
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
    /// Class representing a <see cref="SyncItem"/> and its interaction logic
    /// </summary>
    public class SyncItem
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
        /// <param name="fullQualifiedFileName">
        /// The full qualified file name of the file
        /// </param>
        /// <param name="oldFilename">
        /// The name for the old filename
        /// </param>
        /// <param name="fileAction">
        /// Action for the file
        /// </param>
        public SyncItem(
            IEnumerable<string> destinationFolder, 
            string sourceFile, 
            string fullQualifiedFileName, 
            string oldFilename, 
            FileAction fileAction)
        {
            this.Destinations = destinationFolder;
            this.SourceFile = sourceFile;
            this.FullQualifiedSourceFileName = fullQualifiedFileName;
            this.OldFileName = oldFilename;
            this.FileAction = fileAction;
        }

        /// <summary>
        /// Gets or sets the full qualified name for the file
        /// </summary>
        public string FullQualifiedSourceFileName { get; set; }

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

        /// <summary>
        /// Gets or sets the old file name
        /// </summary>
        public string OldFileName { get; set; }
    }
}