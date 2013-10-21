// *******************************************************
// * <copyright file="IFileWatcher.cs" company="MDMCoWorks">
// * Copyright (c) Mario Murrent. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Mario Murrent</author>
// *******************************************************/

using System.Collections.Generic;

namespace BiOWheelsFileWatcher
{
    /// <summary>
    /// </summary>
    public interface IFileWatcher
    {
        void SetSourceDirectories(IEnumerable<DirecotryMapping> mappings);
    }
}