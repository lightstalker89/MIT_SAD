// *******************************************************
// * <copyright file="Configuration.cs" company="MDMCoWorks">
// * Copyright (c) 2013 Mario Murrent. All rights reserved.
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
    /// Class representing the configuration
    /// </summary>
    public class Configuration
    {
        /// <summary>
        /// Gets or sets the list of <see cref="DirectoryMappingInfo"/> elements
        /// </summary>
        public List<DirectoryMappingInfo> DirectoryMappingInfo { get; set; }

        /// <summary>
        /// Gets or sets the options used for the block compare
        /// </summary>
        public BlockCompareOptions BlockCompareOptions { get; set; }

        /// <summary>
        /// Gets or sets the options for the log file
        /// </summary>
        public LogFileOptions LogFileOptions { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether ParallelSync is enabled or not
        /// </summary>
        public bool ParallelSync { get; set; }
    }
}