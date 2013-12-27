// *******************************************************
// * <copyright file="ConsoleBoxFileWatcher.cs" company="FGrill">
// * Copyright (c) 2013 Florian Grill. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Florian Grill</author>
// *******************************************************/

namespace ConsoleBoxFileWatcher
{
    using System;
    using System.IO;

    /// <summary>
    /// Class representing the <see cref="CustomSystemWatcher"/>
    /// </summary>
    internal class CustomSystemWatcher : FileSystemWatcher
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomSystemWatcher"/> class.
        /// </summary>
        /// <param name="sourceFolderInfo">The source folder information.</param>
        public CustomSystemWatcher(SourceFolderInfo sourceFolderInfo)
            : base(sourceFolderInfo.Path)
        {
            this.SourceFolderInfo = sourceFolderInfo;
        }

        /// <summary>
        /// Gets or sets the source folder information.
        /// </summary>
        public SourceFolderInfo SourceFolderInfo { get; set; }
    }
}
