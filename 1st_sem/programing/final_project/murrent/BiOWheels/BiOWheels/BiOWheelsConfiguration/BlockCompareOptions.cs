// *******************************************************
// * <copyright file="BlockCompareOptions.cs" company="MDMCoWorks">
// * Copyright (c) 2013 Mario Murrent. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Mario Murrent</author>
// *******************************************************/
namespace BiOWheels.BiOWheelsConfiguration
{
    /// <summary>
    /// Class representing the options for block comparison
    /// </summary>
    public class BlockCompareOptions
    {
        /// <summary>
        /// Gets or sets the file size in MB for files which should be used for the block compare
        /// </summary>
        public long BlockCompareFileSizeInMB { get; set; }

        /// <summary>
        /// Gets or sets the Block size in KB for the blocks used to compare files
        /// </summary>
        public long BlockSizeInKB { get; set; }
    }
}