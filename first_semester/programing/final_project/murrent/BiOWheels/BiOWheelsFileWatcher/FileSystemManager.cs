// *******************************************************
// * <copyright file="FileSystemManager.cs" company="MDMCoWorks">
// * Copyright (c) 2013 Mario Murrent. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Mario Murrent</author>
// *******************************************************/
namespace BiOWheelsFileWatcher
{
    using System;
    using System.IO;
    using System.Linq;

    using BiOWheelsFileWatcher.Interfaces;

    /// <summary>
    /// </summary>
    internal class FileSystemManager : IFileSystemManager
    {
        #region Private Fields

        /// <summary>
        /// Represents an instance of the <see cref="FileComparator"/> class
        /// </summary>
        private IFileComparator fileComparator;

        /// <summary>
        /// Block compare size in MB
        /// </summary>
        private long blockCompareFileSizeInMB;

        #endregion

        /// <summary>
        /// </summary>
        /// <param name="fileComparator">
        /// </param>
        internal FileSystemManager(IFileComparator fileComparator)
        {
            this.FileComparator = fileComparator;
        }

        #region Properties

        /// <summary>
        /// Gets or sets the block size in MB
        /// </summary>
        public long BlockCompareFileSizeInMB
        {
            get
            {
                return this.blockCompareFileSizeInMB;
            }

            set
            {
                this.blockCompareFileSizeInMB = value;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="FileComparator"/> instance
        /// </summary>
        internal IFileComparator FileComparator
        {
            get
            {
                return this.fileComparator;
            }

            set
            {
                this.fileComparator = value;
            }
        }

        #endregion

        #region Methods

        /// <inheritdoc/>
        public void CopyDirectory(SyncItem item)
        {
            foreach (string destination in item.Destinations)
            {
                this.CreateDirectoryIfNotExists(destination, null);

                string pathToCopy = destination + Path.DirectorySeparatorChar + item.SourceFile;

                DirectoryInfo directoryInfo = new DirectoryInfo(item.SourceFile);

                this.CreateDirectoryIfNotExists(pathToCopy, directoryInfo);
            }
        }

        /// <inheritdoc/>
        public void Delete(SyncItem item)
        {
            foreach (
                string pathToDelete in
                    item.Destinations.Select(destination => destination + Path.DirectorySeparatorChar + item.SourceFile))
            {
                if (pathToDelete.IsDirectory())
                {
                    if (Directory.Exists(pathToDelete))
                    {
                        Directory.Delete(pathToDelete, true);
                    }
                }
                else
                {
                    if (File.Exists(pathToDelete))
                    {
                        File.Delete(pathToDelete);
                    }
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="item">
        /// </param>
        public void Copy(SyncItem item)
        {
            if (item.FullQualifiedSourceFileName.IsDirectory())
            {
                this.CopyDirectory(item);
            }
            else
            {
                this.CopyFile(item);
            }
        }

        /// <summary>
        /// Creates a directory if it does not exist
        /// </summary>
        /// <param name="directory">
        /// Path of the directory
        /// </param>
        /// <param name="directoryInfo">Directory information </param>
        internal void CreateDirectoryIfNotExists(string directory, DirectoryInfo directoryInfo)
        {
            if (directory != null && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }

        /// <summary>
        /// Copies a file to the given destinations
        /// </summary>
        /// <param name="item">
        /// Item from the queue
        /// </param>
        internal void CopyFile(SyncItem item)
        {
            // TODO: Parallel Sync implement
            if (this.MustCompareFileInBlocks(item.FullQualifiedSourceFileName))
            {
                this.DiffFile(item);
            }
            else
            {
                foreach (string destination in item.Destinations)
                {
                    this.CreateDirectoryIfNotExists(destination, null);

                    string pathToCopy =
                        Path.GetDirectoryName(destination + Path.DirectorySeparatorChar + item.SourceFile);

                    this.CreateDirectoryIfNotExists(pathToCopy, null);

                    string fileToCopy = pathToCopy + Path.DirectorySeparatorChar + Path.GetFileName(item.SourceFile);

                    File.Copy(item.FullQualifiedSourceFileName, fileToCopy, true);
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="item">
        /// </param>
        internal void DiffFile(SyncItem item)
        {
            foreach (string destinationFile in
                item.Destinations.Select(
                    destination => destination + Path.DirectorySeparatorChar + Path.GetFileName(item.SourceFile)))
            {
            }
        }

        /// <summary>
        /// Checks if the file must be compared in blocks
        /// </summary>
        /// <param name="file">
        /// Full qualified file name
        /// </param>
        /// <returns>
        /// A value whether the file must be compared in blocks or not
        /// </returns>
        internal bool MustCompareFileInBlocks(string file)
        {
            if (file.IsDirectory())
            {
                return false;
            }

            double length;

            using (Stream actualFileStream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                length = Math.Round((actualFileStream.Length / 1024f) / 1024f, 2, MidpointRounding.AwayFromZero);
                actualFileStream.Close();
            }

            GC.Collect();

            return length > this.BlockCompareFileSizeInMB;
        }

        #endregion
    }
}