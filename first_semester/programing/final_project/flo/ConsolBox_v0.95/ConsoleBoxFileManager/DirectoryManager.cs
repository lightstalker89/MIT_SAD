// *******************************************************
// * <copyright file="DirectoryManager.cs" company="FGrill">
// * Copyright (c) 2013 Florian Grill. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Florian Grill</author>
// *******************************************************/

namespace ConsoleBoxDataManager
{
    using System.IO;
    using System.Linq;

    /// <summary>
    /// Class representing the <see cref="DirectoryManager"/>
    /// </summary>
    public class DirectoryManager : IDirectoryManager
    {
        /// <inheritdoc/>
        public bool ParallelSync { get; set; }

        /// <inheritdoc/>
        public void HandleDirectories(QueueItem currentElement)
        {
            switch (currentElement.Action)
            {
                case "Copy":
                    if (this.ParallelSync)
                    {
                        currentElement.DestinationFolder.AsParallel().ForAll(f =>
                            f.ForEach(m =>
                                this.CreateDirectory(
                                    currentElement.SourceFolder, currentElement.SourcePath, Path.Combine(m, currentElement.Name), m, currentElement.Name)));
                    }
                    else
                    {
                        currentElement.DestinationFolder.ForEach(f =>
                            f.ForEach(m =>
                                this.CreateDirectory(
                                currentElement.SourceFolder, currentElement.SourcePath, Path.Combine(m, currentElement.Name), m, currentElement.Name)));
                    }

                    break;
                case "Delete":
                    if (this.ParallelSync)
                    {
                        currentElement.DestinationFolder.AsParallel().ForAll(f =>
                            f.ForEach(m =>
                                this.DeleteDirectory(
                                Path.Combine(m, currentElement.Name))));
                    }
                    else
                    {
                        currentElement.DestinationFolder.ForEach(f =>
                            f.ForEach(m =>
                                DeleteDirectory(
                                Path.Combine(m, currentElement.Name))));
                    }

                    break;
                case "Rename":
                    if (this.ParallelSync)
                    {
                        currentElement.DestinationFolder.AsParallel().ForAll(f =>
                            f.ForEach(m =>
                                this.RenameDirectory(
                                currentElement.SourcePath, m, currentElement.Name, currentElement.OldFileName)));
                    }
                    else
                    {
                        currentElement.DestinationFolder.AsParallel().ForAll(f =>
                            f.ForEach(m =>
                                RenameDirectory(
                                currentElement.SourcePath, m, currentElement.Name, currentElement.OldFileName)));
                    }

                    break;
            }
        }

        /// <summary>
        /// Deletes the directory.
        /// </summary>
        /// <param name="destPath">The destination path.</param>
        /// <returns>Return new object</returns>
        private object DeleteDirectory(string destPath)
        {
            if (Directory.Exists(destPath))
            {
                Directory.Delete(destPath, true);
            }

            return new object();
        }

        /// <summary>
        /// Creates the directory.
        /// </summary>
        /// <param name="sourceFolder">The source folder.</param>
        /// <param name="sourcePath">The source path.</param>
        /// <param name="destPath">The destination path.</param>
        /// <param name="destinationFolder">The destination folder.</param>
        /// <param name="relativePath">The relative path.</param>
        /// <returns>Return new object</returns>
        private object CreateDirectory(string sourceFolder, string sourcePath, string destPath, string destinationFolder, string relativePath)
        {
            if (!Directory.Exists(Path.GetDirectoryName(destPath)))
            {
                FileDirectoryClassHelper.CreateDirectoryPath(new DirectoryInfo(sourceFolder).GetDirectories(), destinationFolder, relativePath.Split(Path.DirectorySeparatorChar));
            }

            Directory.CreateDirectory(Path.Combine(sourceFolder, destPath), new DirectoryInfo(sourcePath).GetAccessControl());
            FileDirectoryClassHelper.CopyFolderSettings(new DirectoryInfo(destPath), new DirectoryInfo(sourcePath));
            return new object();
        }

        #region rename
        /// <summary>
        /// Renames the directory.
        /// </summary>
        /// <param name="sourcePath">The source path.</param>
        /// <param name="destinationFolder">The destination folder.</param>
        /// <param name="name">The name of the file.</param>
        /// <param name="oldName">The old name of the file.</param>
        /// <returns>Return new object</returns>
        private object RenameDirectory(string sourcePath, string destinationFolder, string name, string oldName)
        {
            Directory.Move(Path.Combine(destinationFolder, oldName), Path.Combine(destinationFolder, name));
            return new object();
        }
        #endregion
    }
}
