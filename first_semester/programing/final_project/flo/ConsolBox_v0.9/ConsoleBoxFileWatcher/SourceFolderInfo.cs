// *******************************************************
// * <copyright file="SourceFolderInfo.cs" company="FGrill">
// * Copyright (c) 2013 Florian Grill. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Florian Grill</author>
// *******************************************************/

namespace ConsoleBoxFileWatcher
{
    using System.Collections.Generic;

    /// <summary>
    /// Class representing the <see cref="SourceFolderInfo"/>
    /// </summary>
    public class SourceFolderInfo
    {
        /// <summary>
        /// Gets or sets the Path.
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [Recursion].
        /// </summary>
        public bool Recursion { get; set; }

        /// <summary>
        /// Gets or sets the destination folders.
        /// </summary>
        public List<string> DestinationFolders { get; set; }

        /// <summary>
        /// Gets or sets the distributed destination folders.
        /// </summary>
        public List<List<string>> SplittedDestinationFolders { get; set; }

        /// <summary>
        /// Gets or sets the exception folders.
        /// </summary>
        public List<string> ExceptionFolders { get; set; }
    }
}
