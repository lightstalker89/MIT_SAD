// *******************************************************
// * <copyright file="Configuration.cs" company="MDMCoWorks">
// * Copyright (c) Mario Murrent. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Mario Murrent</author>
// *******************************************************/
namespace BiOWheels.BiOWheelsConfiguration
{
    using System.Collections.Generic;

    /// <summary>
    /// </summary>
    public class Configuration
    {
        /// <summary>
        /// </summary>
        public List<DirectoryMappingInfo> DirectoryMappingInfo { get; set; }

        /// <summary>
        /// </summary>
        public BlockCompareOptions BlockCompareOptions { get; set; }

        /// <summary>
        /// </summary>
        public LogFileOptions LogFileOptions { get; set; }

        /// <summary>
        /// </summary>
        public bool ParallelSync { get; set; }
    }
}