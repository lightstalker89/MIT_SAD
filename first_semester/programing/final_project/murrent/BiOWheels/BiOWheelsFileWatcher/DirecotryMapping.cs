// *******************************************************
// * <copyright file="DirecotryMapping.cs" company="MDMCoWorks">
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
    public class DirecotryMapping
    {
        /// <summary>
        /// </summary>
        public bool Recursive { get; set; }

        /// <summary>
        /// </summary>
        public string SorceDirectory { get; set; }

        /// <summary>
        /// </summary>
        public List<string> DestinationDirectories { get; set; }
    }
}