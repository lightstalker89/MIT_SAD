// *******************************************************
// * <copyright file="IDirectoryManager.cs" company="FGrill">
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
    /// Interface representing methods and properties of the DirectoryManager
    /// </summary>
    public interface IDirectoryManager
    {
        /// <summary>
        /// Gets or sets a value indicating whether parallel
        /// synchronization is on or off.
        /// </summary>
        /// <value>
        ///   <c>true</c> if parallel synchronization is on; otherwise, <c>false</c>.
        /// </value>
        bool ParallelSync { get; set; }

        /// <summary>
        /// Handles the directories.
        /// </summary>
        /// <param name="currentElement">The current element.</param>
        void HandleDirectories(QueueItem currentElement);
    }
}
