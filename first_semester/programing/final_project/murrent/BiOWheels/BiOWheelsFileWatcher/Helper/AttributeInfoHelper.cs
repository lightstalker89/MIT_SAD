// *******************************************************
// * <copyright file="AttributeInfoHelper.cs" company="MDMCoWorks">
// * Copyright (c) 2013 Mario Murrent. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Mario Murrent</author>
// *******************************************************/
namespace BiOWheelsFileWatcher.Helper
{
    using System;
    using System.IO;
    using System.Security.AccessControl;

    /// <summary>
    /// The <see ref="AttributeInfoHelper"/> class and its interaction logic 
    /// </summary>
    public static class AttributeInfoHelper
    {
        /// <summary>
        /// Copies the file attributes to.
        /// </summary>
        /// <param name="sourceFileName">
        /// Name of the source file.
        /// </param>
        /// <param name="destinationFileName">
        /// Name of the destination file.
        /// </param>
        public static void CopyFileAttributesTo(this string sourceFileName, string destinationFileName)
        {
            FileInfo sourceFileInfo = new FileInfo(sourceFileName);
            FileInfo destinationFileInfo = new FileInfo(destinationFileName)
                {
                    Attributes = sourceFileInfo.Attributes, 
                    CreationTime = sourceFileInfo.CreationTime, 
                    CreationTimeUtc = sourceFileInfo.CreationTimeUtc, 
                    IsReadOnly = sourceFileInfo.IsReadOnly, 
                    LastAccessTime = sourceFileInfo.LastAccessTime, 
                    LastAccessTimeUtc = sourceFileInfo.LastAccessTimeUtc, 
                    LastWriteTime = sourceFileInfo.LastWriteTime, 
                    LastWriteTimeUtc = sourceFileInfo.LastWriteTimeUtc, 
                };

            FileSecurity sourceFileSecurity = sourceFileInfo.GetAccessControl();
            destinationFileInfo.SetAccessControl(sourceFileSecurity);

            GC.Collect();
        }

        /// <summary>
        /// Copies the directory attributes to.
        /// </summary>
        /// <param name="sourceDirectoryName">
        /// Name of the source directory.
        /// </param>
        /// <param name="destinationDirectoryName">
        /// Name of the destionation directory.
        /// </param>
        public static void CopyDirectoryAttributesTo(this string sourceDirectoryName, string destinationDirectoryName)
        {
            DirectoryInfo sourceDirectoryInfo = new DirectoryInfo(sourceDirectoryName);
            DirectoryInfo destinationDirectoryInfo = new DirectoryInfo(destinationDirectoryName)
                {
                    Attributes = sourceDirectoryInfo.Attributes, 
                    CreationTime = sourceDirectoryInfo.CreationTime, 
                    CreationTimeUtc = sourceDirectoryInfo.CreationTimeUtc, 
                    LastAccessTime = sourceDirectoryInfo.LastAccessTime, 
                    LastAccessTimeUtc = sourceDirectoryInfo.LastAccessTimeUtc, 
                    LastWriteTime = sourceDirectoryInfo.LastWriteTime, 
                    LastWriteTimeUtc = sourceDirectoryInfo.LastWriteTimeUtc, 
                };

            DirectorySecurity sourceDirectorySecurity = sourceDirectoryInfo.GetAccessControl();
            destinationDirectoryInfo.SetAccessControl(sourceDirectorySecurity);

            GC.Collect();
        }
    }
}