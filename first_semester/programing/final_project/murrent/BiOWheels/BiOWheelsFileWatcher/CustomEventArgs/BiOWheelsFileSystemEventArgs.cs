// *******************************************************
// * <copyright file="BiOWheelsFileSystemEventArgs.cs" company="MDMCoWorks">
// * Copyright (c) Mario Murrent. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Mario Murrent</author>
// *******************************************************/
namespace BiOWheelsFileWatcher.CustomEventArgs
{
    using System.IO;

    /// <summary>
    /// </summary>
    public class BiOWheelsFileSystemEventArgs : FileSystemEventArgs
    {
        /// <summary>
        /// </summary>
        /// <param name="changeType">
        /// </param>
        /// <param name="directory">
        /// </param>
        /// <param name="name">
        /// </param>
        public BiOWheelsFileSystemEventArgs(WatcherChangeTypes changeType, string directory, string name)
            : base(changeType, directory, name)
        {
        }
    }
}