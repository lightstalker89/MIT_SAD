// *******************************************************
// * <copyright file="IFileManager.cs" company="FGrill">
// * Copyright (c) 2013 Florian Grill. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Florian Grill</author>
// *******************************************************/

namespace ConsoleBoxDataManager
{
    /// <summary>
    /// Interface representing methods and properties of the FileManager
    /// </summary>
    public interface IFileManager
    {
        /// <summary>
        /// Gets or sets the block size in mb.
        /// </summary>
        /// <value>
        /// The block size in mb.
        /// </value>
        long BlockSizeInMb { get; set; }

        /// <summary>
        /// Gets or sets the block compare size in mb.
        /// </summary>
        /// <value>
        /// The block compare size in mb.
        /// </value>
        long BlockCompareSizeInMb { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether parallel
        /// synchronization is on or off.
        /// </summary>
        /// <value>
        ///   <c>true</c> if parallel synchronization is on; otherwise, <c>false</c>.
        /// </value>
        bool ParallelSync { get; set; }

        /// <summary>
        /// Handles the files.
        /// </summary>
        /// <param name="currentElement">The current element.</param>
        void HandleFiles(QueueItem currentElement);
    }
}
