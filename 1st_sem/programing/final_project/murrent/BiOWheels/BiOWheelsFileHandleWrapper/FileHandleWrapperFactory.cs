// *******************************************************
// * <copyright file="FileHandleWrapperFactory.cs" company="MDMCoWorks">
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
    /// The <see ref="FileHandleWrapperFactory"/> class and its interaction logic 
    /// </summary>
    public class FileHandleWrapperFactory
    {
        /// <summary>
        /// Creates the file handle wrapper.
        /// </summary>
        /// <returns>An instance of the <see cref="FileHandleWrapper"/> class</returns>
        public static IFileHandleWrapper CreateFileHandleWrapper()
        {
            return new FileHandleWrapper();
        }
    }
}