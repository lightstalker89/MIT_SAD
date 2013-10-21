// *******************************************************
// * <copyright file="FileWatcher.cs" company="MDMCoWorks">
// * Copyright (c) Mario Murrent. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Mario Murrent</author>
// *******************************************************/

using System;
using System.Collections.Generic;

namespace BiOWheelsFileWatcher
{
    /// <summary>
    /// </summary>
    public class FileWatcher : IFileWatcher
    {
        private IEnumerable<DirecotryMapping> mappings;
 
        public FileWatcher()
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        public void Init()
        {
            this.mappings = new ArraySegment<DirecotryMapping>();
        }

        /// <inheritdoc/>
        public void SetSourceDirectories(IEnumerable<DirecotryMapping> direcotryappings)
        {
            this.mappings = direcotryappings;
        }
    }
}