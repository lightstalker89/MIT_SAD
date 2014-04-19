// *******************************************************
// * <copyright file="IFileWatcher.cs" company="MDMCoWorks">
// * Copyright (c) 2013 Mario Murrent. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Mario Murrent</author>
// *******************************************************/
namespace BiOWheelsFileWatcher.Interfaces
{
    using System.Collections.Generic;

    /// <summary>
    /// Interface representing the must implement methods
    /// </summary>
    public interface IFileWatcher
    {
        /// <summary>
        /// Event for updating the progress
        /// </summary>
        event FileWatcher.ProgressUpdateHandler ProgressUpdate;

        /// <summary>
        /// Event for catching an exception
        /// </summary>
        event FileWatcher.CaughtExceptionHandler CaughtException;

        /// <summary>
        /// Sets source directories
        /// </summary>
        /// <param name="mappings">
        /// List of <see cref="DirectoryMapping"/> objects
        /// </param>
        void SetSourceDirectories(IEnumerable<DirectoryMapping> mappings);

        /// <summary>
        /// Initialize and assign all needed properties and start a monitor thread for every directory
        /// </summary>
        void Init();

        /// <summary>
        /// Scan directories and add jobs to the queue when application starts
        /// </summary>
        void InitialScan();
    }
}