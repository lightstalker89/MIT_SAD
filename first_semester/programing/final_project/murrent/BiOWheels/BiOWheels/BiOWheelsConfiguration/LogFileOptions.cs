// *******************************************************
// * <copyright file="LogFileOptions.cs" company="MDMCoWorks">
// * Copyright (c) Mario Murrent. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Mario Murrent</author>
// *******************************************************/
namespace BiOWheels.BiOWheelsConfiguration
{
    /// <summary>
    /// Class representing the options for log files
    /// </summary>
    public class LogFileOptions
    {
        /// <summary>
        /// Gets or sets the file size in MB for a log file
        /// </summary>
        public long LogFileSizeInMB { get; set; }

        /// <summary>
        /// Gets or sets the folder for the log files
        /// </summary>
        public string LogFileFolder { get; set; }
    }
}