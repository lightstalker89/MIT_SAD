// *******************************************************
// * <copyright file="IFileHandleWrapper.cs" company="MDMCoWorks">
// * Copyright (c) 2013 Mario Murrent. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Mario Murrent</author>
// *******************************************************/
namespace BiOWheelsFileHandleWrapper
{
    /// <summary>
    /// </summary>
    public interface IFileHandleWrapper
    {
        /// <summary>
        /// Occurs when all file handles have been found
        /// </summary>
        event FileHandleWrapper.FileHandlesFoundHandler FileHandlesFound;

        /// <summary>
        /// Finds the handles for file.
        /// </summary>
        /// <param name="searchPattern">
        /// The search pattern.
        /// </param>
        void FindHandlesForFile(string searchPattern);
    }
}