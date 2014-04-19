// *******************************************************
// * <copyright file="CustomFileRenamedEventArgs.cs" company="MDMCoWorks">
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
    ///  Class representing the <see cref="CustomFileRenamedEventArgs"/>
    /// </summary>
    public class CustomFileRenamedEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomFileRenamedEventArgs"/> class.
        /// </summary>
        /// <param name="fullQualifiedFileName">
        /// Full name of the qualified file.
        /// </param>
        /// <param name="fileName">
        /// Name of the file.
        /// </param>
        /// <param name="oldFileName">
        /// Old name of the file.
        /// </param>
        /// <param name="oldFullQualifiedFileName">
        /// Old name of the full qualified file.
        /// </param>
        public CustomFileRenamedEventArgs(
            string fullQualifiedFileName, string fileName, string oldFileName, string oldFullQualifiedFileName)
        {
            this.FullQualifiedFileName = fullQualifiedFileName;
            this.FileName = fileName;
            this.OldFileName = oldFileName;
            this.OldFullQualifiedFileName = oldFullQualifiedFileName;
        }

        #region Properties

        /// <summary>
        /// Gets or sets the old full qualified file name
        /// </summary>
        public string OldFullQualifiedFileName { get; set; }

        /// <summary>
        /// Gets or sets the old file name
        /// </summary>
        public string OldFileName { get; set; }

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