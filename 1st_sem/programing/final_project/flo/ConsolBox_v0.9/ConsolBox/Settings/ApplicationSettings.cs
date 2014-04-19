// *******************************************************
// * <copyright file="ApplicationSettings.cs" company="FGrill">
// * Copyright (c) 2013 Florian Grill. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Florian Grill</author>
// *******************************************************/
namespace ConsoleBox.Settings
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;

    /// <summary>
    /// Class representing the configuration
    /// </summary>
    public class ApplicationSettings
    {
        /// <summary>
        /// Gets or sets the size of the log file
        /// </summary>
        [XmlElement("LogFileSizeInMB")]
        public int LogFileSize { get; set; }

        /// <summary>
        /// Gets or sets the size for the block compare
        /// </summary>
        [XmlElement("BlockCompareSizeInMb")]
        public int BlockCompareSizeInMb { get; set; }

        /// <summary>
        /// Gets or sets the size of the block size
        /// </summary>
        [XmlElement("BlockSizeInMb")]
        public int BlockSizeInMb { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the parallel synchronization
        /// feature is on or off
        /// </summary>
        [XmlElement("ParalellSync")]
        public bool ParalellSync { get; set; }
    }
}
