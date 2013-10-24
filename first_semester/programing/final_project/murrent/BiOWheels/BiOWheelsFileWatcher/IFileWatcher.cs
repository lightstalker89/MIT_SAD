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
        void SetSourceDirectories(IEnumerable<DirecotryMapping> mappings);
    }
}