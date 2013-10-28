// *******************************************************
// * <copyright file="DirectoryMappingInfo.cs" company="MDMCoWorks">
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
    /// Class representing information for a directory mapping
    /// </summary>
    public class DirectoryMappingInfo
    {
        /// <summary>
        /// Gets or sets the info for a source mapping
        /// </summary>
        public SourceMappingInfo SourceMappingInfo { get; set; }

        /// <summary>
        /// Gets or sets the destination directories
        /// </summary>
        public List<string> DestinationDirectories { get; set; }

        /// <summary>
        /// Gets or sets the folders which should be excluded for the sync process
        /// </summary>
        public List<string> ExcludedFromSource { get; set; }
    }
}