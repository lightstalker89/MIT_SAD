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
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using BiOWheelsFileWatcher.Helper;
    using BiOWheelsFileWatcher.Interfaces;

    /// <summary>
    /// Class representing the <see cref="FileSystemManager" />
    /// </summary>
    internal class FileSystemManager : IFileSystemManager
    {
        #region Private Fields

        /// <summary>
        /// Value indicating whether parallel sync is activated or not
        /// </summary>
        private bool isParallelSyncActivated;

        /// <summary>
        /// Represents an instance of the <see cref="FileComparator"/> class
        /// </summary>
        private IFileComparator fileComparator;

        /// <summary>
        /// Block compare size in MB
        /// </summary>
        private long blockCompareFileSizeInMB;

        /// <summary>
        /// Holds an instance of the <see cref="DirectoryVolumenComparator"/>
        /// </summary>
        private DirectoryVolumenComparator directoryVolumenComparator;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="FileSystemManager"/> class
        /// </summary>
        /// <param name="fileComparator">
        /// The file comparator.
        /// </param>
        internal FileSystemManager(IFileComparator fileComparator)
        {
            this.directoryVolumenComparator = new DirectoryVolumenComparator();
            this.FileComparator = fileComparator;
        }

        #region Properties

        /// <inheritdoc/>
        public bool IsParallelSyncActivated
        {
            get
            {
                return this.isParallelSyncActivated;
            }

            set
            {
                this.isParallelSyncActivated = value;
            }
        }

        /// <inheritdoc/>
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
        public void Delete(SyncItem item)
        {
            if (item.Destinations.Any())
            {
                foreach (string pathToDelete in
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
            else
            {
                if (item.SourceFile.IsDirectory())
                {
                    Directory.Delete(item.SourceFile, true);
                }
                else
                {
                    File.Delete(item.SourceFile);
                }
            }
        }

        /// <inheritdoc/>
        public void Copy(SyncItem item)
        {
            if (item.FullQualifiedSourceFileName.IsDirectory())
            {
                this.CopyDirectory(item);
            }
            else
            {
                if (this.MustCompareFileInBlocks(item.FullQualifiedSourceFileName))
                {
                    if (this.IsParallelSyncActivated)
                    {
                        this.DiffParallel(item);
                    }
                    else
                    {
                        this.DiffFile(item);
                    }
                }
                else
                {
                    if (this.isParallelSyncActivated)
                    {
                        this.CopyFileParallel(item);
                    }
                    else
                    {
                        this.CopyFile(item);
                    }
                }
            }
        }

        /// <inheritdoc/>
        public void Rename(SyncItem item)
        {
            foreach (string pathToCopy in
                item.Destinations.Select(
                    destination => Path.GetDirectoryName(destination + Path.DirectorySeparatorChar + item.SourceFile)))
            {
                string newItemToRename = pathToCopy + Path.DirectorySeparatorChar + item.SourceFile;
                string oldItemRenamed = pathToCopy + Path.DirectorySeparatorChar + item.OldFileName;

                if (oldItemRenamed.IsDirectory())
                {
                    if (Directory.Exists(oldItemRenamed))
                    {
                        Directory.Move(oldItemRenamed, newItemToRename);
                    }
                }
                else
                {
                    if (File.Exists(oldItemRenamed))
                    {
                        File.Move(oldItemRenamed, newItemToRename);
                    }
                }
            }
        }

        /// <summary>
        /// Copies the directory.
        /// </summary>
        /// <param name="item">
        /// Item from the queue
        /// </param>
        internal void CopyDirectory(SyncItem item)
        {
            foreach (string destination in item.Destinations)
            {
                this.CreateDirectoryIfNotExists(destination);

                string pathToCopy = destination + Path.DirectorySeparatorChar + item.SourceFile;

                this.CreateDirectoryIfNotExists(pathToCopy);

                item.FullQualifiedSourceFileName.CopyDirectoryAttributesTo(pathToCopy);
            }
        }

        /// <summary>
        /// Creates a directory if it does not exist
        /// </summary>
        /// <param name="directory">
        /// Path of the directory
        /// </param>
        internal void CreateDirectoryIfNotExists(string directory)
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
            foreach (string destination in item.Destinations)
            {
                string pathToCopy =
                    Path.GetDirectoryName(destination + Path.DirectorySeparatorChar + item.SourceFile);

                this.CreateDirectoryIfNotExists(pathToCopy);

                string fileToCopy = pathToCopy + Path.DirectorySeparatorChar + Path.GetFileName(item.SourceFile);

                using (
                    FileStream fileStream = new FileStream(
                        item.FullQualifiedSourceFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite),
                               fileStreamOutPut = new FileStream(fileToCopy, FileMode.Create))
                {
                    this.CopyStreams(fileStream, fileStreamOutPut);
                }

                item.FullQualifiedSourceFileName.CopyFileAttributesTo(fileToCopy);
            }

        }

        /// <summary>
        /// Copies the files parallel.
        /// </summary>
        /// <param name="item">The item.</param>
        internal void CopyFileParallel(SyncItem item)
        {
            string sourceDirectory = Directory.GetDirectoryRoot(item.FullQualifiedSourceFileName);

            List<string> parallelDestinations = new List<string>();
            List<string> destinations = new List<string>();

            Parallel.ForEach(
                item.Destinations,
                destination =>
                {
                    if (this.directoryVolumenComparator.CompareDirectories(sourceDirectory, destination))
                    {
                        destinations.Add(destination);
                    }
                    else
                    {
                        parallelDestinations.Add(destination);
                    }

                });

            Parallel.ForEach(
                parallelDestinations,
                destination =>
                {
                    string pathToCopy =
                    Path.GetDirectoryName(destination + Path.DirectorySeparatorChar + item.SourceFile);

                    this.CreateDirectoryIfNotExists(pathToCopy);

                    string fileToCopy = pathToCopy + Path.DirectorySeparatorChar + Path.GetFileName(item.SourceFile);

                    using (
                   FileStream fileStream = new FileStream(
                       item.FullQualifiedSourceFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite),
                              fileStreamOutPut = new FileStream(fileToCopy, FileMode.Create))
                    {
                        this.CopyStreams(fileStream, fileStreamOutPut);
                    }

                    item.FullQualifiedSourceFileName.CopyFileAttributesTo(fileToCopy);
                });

            foreach (string destination in destinations)
            {
                string pathToCopy =
                  Path.GetDirectoryName(destination + Path.DirectorySeparatorChar + item.SourceFile);

                this.CreateDirectoryIfNotExists(pathToCopy);

                string fileToCopy = pathToCopy + Path.DirectorySeparatorChar + Path.GetFileName(item.SourceFile);

                using (
               FileStream fileStream = new FileStream(
                   item.FullQualifiedSourceFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite),
                          fileStreamOutPut = new FileStream(fileToCopy, FileMode.Create))
                {
                    this.CopyStreams(fileStream, fileStreamOutPut);
                }

                item.FullQualifiedSourceFileName.CopyFileAttributesTo(fileToCopy);
            }
        }

        /// <summary>
        /// Diff the file.
        /// </summary>
        /// <param name="item">
        /// The item.
        /// </param>
        internal void DiffFile(SyncItem item)
        {
            foreach (string destinationFile in
                item.Destinations.Select(
                    destination => destination + Path.DirectorySeparatorChar + Path.GetFileName(item.SourceFile)))
            {
                if (File.Exists(destinationFile))
                {
                    this.fileComparator.Compare(item.FullQualifiedSourceFileName, destinationFile);
                }
                else
                {
                    using (
                        FileStream fileStream = new FileStream(
                            item.FullQualifiedSourceFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite),
                                   fileStreamOutPut = new FileStream(destinationFile, FileMode.Create))
                    {
                        this.CopyStreams(fileStream, fileStreamOutPut);
                    }
                }
            }
        }

        /// <summary>
        /// Diff files parallel
        /// </summary>
        /// <param name="item">
        /// The item
        /// </param>
        internal void DiffParallel(SyncItem item)
        {
            string sourceDirectory = Directory.GetDirectoryRoot(item.FullQualifiedSourceFileName);

            List<string> parallelDestinations = new List<string>();
            List<string> destinations = new List<string>();

            Parallel.ForEach(
                item.Destinations,
                destination =>
                {
                    if (this.directoryVolumenComparator.CompareDirectories(sourceDirectory, destination))
                    {
                        destinations.Add(destination);
                    }
                    else
                    {
                        parallelDestinations.Add(destination);
                    }

                });
        }

        /// <summary>
        /// Copies the streams.
        /// </summary>
        /// <param name="inputFileStream">
        /// The input file stream.
        /// </param>
        /// <param name="outputFileStream">
        /// The output file stream.
        /// </param>
        internal void CopyStreams(FileStream inputFileStream, FileStream outputFileStream)
        {
            byte[] buffer = new byte[4096];

            int read;
            while ((read = inputFileStream.Read(buffer, 0, buffer.Length)) > 0)
            {
                outputFileStream.Write(buffer, 0, read);
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

            using (Stream actualFileStream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                length = Math.Round((actualFileStream.Length / 1024f) / 1024f, 2, MidpointRounding.AwayFromZero);
            }

            GC.Collect();

            return length > this.BlockCompareFileSizeInMB;
        }

        #endregion
    }
}