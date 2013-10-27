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
        /// </summary>
        public List<string> Destinations { get; set; }

        /// <summary>
        /// </summary>
        /// <param name="path">
        /// </param>
        public BiOWheelsFileSystemWatcher(string path)
            : base(path)
        {
        }
    }
}