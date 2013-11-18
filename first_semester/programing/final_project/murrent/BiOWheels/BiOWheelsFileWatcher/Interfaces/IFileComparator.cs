// *******************************************************
// * <copyright file="IFileComparator.cs" company="MDMCoWorks">
// * Copyright (c) 2013 Mario Murrent. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Mario Murrent</author>
// *******************************************************/
namespace BiOWheelsFileWatcher.Interfaces
{
    /// <summary>
    ///  Interface representing the <see cref="IFileComparator"/>
    /// </summary>
    public interface IFileComparator
    {
        /// <summary>
        /// Compares to files in blocks
        /// </summary>
        /// <param name="sourceFile">
        /// Destination file
        /// </param>
        /// <param name="destinationFile">
        /// Source file
        /// </param>
        void Compare(string sourceFile, string destinationFile);
    }
}