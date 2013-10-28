// *******************************************************
// * <copyright file="BiOWheelsFileSystemWatcher.cs" company="MDMCoWorks">
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
    using System.IO;

    /// <summary>
    /// </summary>
    public class BiOWheelsFileSystemWatcher : FileSystemWatcher
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BiOWheelsFileSystemWatcher"/> class
        /// </summary>
        /// <param name="path">
        /// </param>
        public BiOWheelsFileSystemWatcher(string path)
            : base(path)
        {
        }

        #region Properties

        /// <summary>
        /// Gets or sets the destination folder
        /// </summary>
        public List<string> Destinations { get; set; }

        #endregion
    }
}