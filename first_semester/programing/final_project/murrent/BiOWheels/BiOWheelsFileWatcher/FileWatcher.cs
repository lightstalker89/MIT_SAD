// *******************************************************
// * <copyright file="FileWatcher.cs" company="MDMCoWorks">
// * Copyright (c) Mario Murrent. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Mario Murrent</author>
// *******************************************************/
namespace BiOWheelsFileWatcher
{
    using System;
    using System.Collections.Generic;
    using System.Threading;

    /// <summary>
    /// Class representing the <see cref="FileWatcher"/> and its interaction logic
    /// </summary>
    public class FileWatcher : IFileWatcher
    {
        /// <summary>
        /// </summary>
        private IEnumerable<DirectoryMapping> mappings;

        /// <summary>
        /// 
        /// </summary>
        internal IEnumerable<DirectoryMapping> Mappings
        {
            get
            {
                return this.mappings;
            }

            set
            {
                this.mappings = value;
            }
        }

        /// <inheritdoc/>
        public void Init()
        {
            this.Mappings = new ArraySegment<DirectoryMapping>();

            foreach (DirectoryMapping mapping in this.Mappings)
            {
                Thread backgroundWatcherThread = new Thread(this.WatchDirectory);
                backgroundWatcherThread.Start(mapping);
            }
        }

        /// <inheritdoc/>
        public void SetSourceDirectories(IEnumerable<DirectoryMapping> directoryMappings)
        {
            this.mappings = directoryMappings;
        }

        /// <summary>
        /// Method for watching a specific directory - will be executed in a new thread
        /// </summary>
        /// <param name="mappingInfo">
        /// Object containing the <see cref="DirectoryMapping"/> information
        /// </param>
        private void WatchDirectory(object mappingInfo)
        {
            if (mappingInfo.GetType() == typeof(DirectoryMapping))
            {
            }
            else
            {
                // TODO: Custom error event implementation
            }
        }
    }
}