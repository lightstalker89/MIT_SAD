// *******************************************************
// * <copyright file="IFileWatcher.cs" company="MDMCoWorks">
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

    /// <summary>
    /// </summary>
    public interface IFileWatcher
    {
        /// <summary>
        /// </summary>
        /// <param name="mappings">
        /// </param>
        void SetSourceDirectories(IEnumerable<DirectoryMapping> mappings);

        /// <summary>
        /// Initialize and assign all needed properties and start a monitor thread for every directory
        /// </summary>
        void Init();
    }
}