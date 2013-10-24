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
        private IEnumerable<DirecotryMapping> mappings;

        /// <summary>
        /// 
        /// </summary>
        internal IEnumerable<DirecotryMapping> Mappings
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

        /// <summary>
        /// Initialize and assign all needed properties and start a monitor thread for every directory
        /// </summary>
        public void Init()
        {
            this.Mappings = new ArraySegment<DirecotryMapping>();

            foreach (DirecotryMapping mapping in this.Mappings)
            {
                Thread backgroundWatcherThread = new Thread(this.WatchDirectory);
                backgroundWatcherThread.Start(mapping);
            }
        }

        /// <inheritdoc/>
        public void SetSourceDirectories(IEnumerable<DirecotryMapping> direcotryappings)
        {
            this.mappings = direcotryappings;
        }

        /// <summary>
        /// Method for watching a specific directory - will be executed in a new thread
        /// </summary>
        /// <param name="mappingInfo">
        /// Object containing the <see cref="DirecotryMapping"/> information
        /// </param>
        private void WatchDirectory(object mappingInfo)
        {
            if (mappingInfo.GetType() == typeof(DirecotryMapping))
            {
            }
            else
            {
                // TODO: Custom error event implementation
            }
        }
    }
}