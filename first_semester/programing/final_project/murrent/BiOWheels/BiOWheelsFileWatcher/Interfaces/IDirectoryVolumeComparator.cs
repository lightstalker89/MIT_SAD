// *******************************************************
// * <copyright file="IDirectoryVolumeComparator.cs" company="MDMCoWorks">
// * Copyright (c) 2013 Mario Murrent. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Mario Murrent</author>
// *******************************************************/
namespace BiOWheelsFileWatcher.Interfaces
{
    /// <summary>
    ///  Interface representing the <see cref="IDirectoryVolumeComparator"/>
    /// </summary>
    public interface IDirectoryVolumeComparator
    {
        /// <summary>
        /// Compares two directories
        /// </summary>
        /// <param name="directory1">
        /// First directory
        /// </param>
        /// <param name="directory2">
        /// Second directory
        /// </param>
        /// <returns>
        /// A value indicating whether the directories are on the same physical disc or not
        /// </returns>
        bool CompareDirectories(string directory1, string directory2);
    }
}