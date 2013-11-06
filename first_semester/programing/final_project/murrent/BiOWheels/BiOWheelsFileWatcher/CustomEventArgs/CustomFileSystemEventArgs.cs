// *******************************************************
// * <copyright file="CustomFileSystemEventArgs.cs" company="MDMCoWorks">
// * Copyright (c) 2013 Mario Murrent. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Mario Murrent</author>
// *******************************************************/
namespace BiOWheelsFileWatcher.CustomEventArgs
{
    /// <summary>
    /// </summary>
    public class CustomFileSystemEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomFileSystemEventArgs"/> class
        /// </summary>
        /// <param name="fullQualifiedFileName">
        /// Full name of the qualified file.
        /// </param>
        /// <param name="fileName">
        /// Name of the file.
        /// </param>
        public CustomFileSystemEventArgs(string fullQualifiedFileName, string fileName)
        {
            this.FullQualifiedFileName = fullQualifiedFileName;
            this.FileName = fileName;
        }

        #region Properties

        /// <summary>
        /// Gets or sets the full qualified file name
        /// </summary>
        public string FullQualifiedFileName { get; set; }

        /// <summary>
        /// Gets or sets the file name
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the file must be compared in blocks or not
        /// </summary>
        public bool CompareInBlocks { get; set; }

        #endregion
    }
}