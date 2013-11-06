// *******************************************************
// * <copyright file="DirectoryMapping.cs" company="MDMCoWorks">
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
    /// Class representing the directory mapping information
    /// </summary>
    public class DirectoryMapping
    {
        /// <summary>
        /// Gets or sets a value indicating whether a directory should be processed recursive or not
        /// </summary>
        public bool Recursive { get; set; }

        /// <summary>
        /// Gets or sets the source directory
        /// </summary>
        public string SourceDirectory { get; set; }

        /// <summary>
        /// Gets or sets the destination directories
        /// </summary>
        public List<string> DestinationDirectories { get; set; }

        /// <summary>
        /// Gets or sets the folders which should be excluded for the sync process
        /// </summary>
        public List<string> ExcludedDirectories { get; set; }
    }
}