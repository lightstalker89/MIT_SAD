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
    /// </summary>
    public class DirectoryMappingInfo
    {
        /// <summary>
        /// </summary>
        public List<SourceMappingInfo> SourceMappingInfos { get; set; }

        /// <summary>
        /// </summary>
        public List<string> DestinationDirectories { get; set; }

        /// <summary>
        /// </summary>
        public List<string> ExcludedFromSource { get; set; }
    }
}