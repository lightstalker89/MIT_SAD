// *******************************************************
// * <copyright file="FileManager.cs" company="FGrill">
// * Copyright (c) 2013 Florian Grill. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Florian Grill</author>
// *******************************************************/

namespace ConsoleBoxDataManager
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Security.Cryptography;

    /// <summary>
    /// Class representing the <see cref="FileManager"/>
    /// </summary>
    public class FileManager : IFileManager
    {
        /// <inheritdoc/>
        public long BlockSizeInMb { get; set; }

        /// <inheritdoc/>
        public long BlockCompareSizeInMb { get; set; }

        /// <inheritdoc/>
        public bool ParallelSync { get; set; }

        /// <inheritdoc/>
        public void HandleFiles(QueueItem currentElement)
        {
            switch (currentElement.Action)
            {
                case "Copy":
                    if (this.ParallelSync)
                    {
                        if (this.HaveToBlockCompare(currentElement.SourcePath))
                        {
                            currentElement.DestinationFolder.AsParallel().ForAll(f =>
                                    f.ForEach(m =>
                                        this.CompareFile(
                                        currentElement.SourcePath, currentElement.SourceFolder, Path.Combine(m, currentElement.Name), m, currentElement.Name)));
                        }
                        else
                        {
                            currentElement.DestinationFolder.AsParallel().ForAll(f =>
                                f.ForEach(m =>
                                    this.CopyFile(
                                    currentElement.SourceFolder, currentElement.SourcePath, Path.Combine(m, currentElement.Name), m, currentElement.Name)));
                        }
                    }
                    else
                    {
                        if (HaveToBlockCompare(currentElement.SourcePath))
                        {
                            currentElement.DestinationFolder.ForEach(f =>
                                    f.ForEach(m =>
                                        this.CompareFile(
                                        currentElement.SourcePath, currentElement.SourceFolder, Path.Combine(m, currentElement.Name), m, currentElement.Name)));
                        }
                        else
                        {
                            currentElement.DestinationFolder.ForEach(f =>
                                f.ForEach(m =>
                                    this.CopyFile(
                                    currentElement.SourceFolder, currentElement.SourcePath, Path.Combine(m, currentElement.Name), m, currentElement.Name)));
                        }
                    }

                    break;
                case "Delete":
                    if (this.ParallelSync)
                    {
                        currentElement.DestinationFolder.AsParallel().ForAll(f =>
                            f.ForEach(m =>
                                this.DeleteFile(Path.Combine(m, currentElement.Name))));
                    }
                    else
                    {
                        currentElement.DestinationFolder.ForEach(f =>
                            f.ForEach(m =>
                                this.DeleteFile(Path.Combine(m, currentElement.Name))));
                    }

                    break;
                case "Rename":
                    if (this.ParallelSync)
                    {
                        currentElement.DestinationFolder.AsParallel().ForAll(f =>
                                f.ForEach(m =>
                                    this.RenameFile(
                                    currentElement.SourcePath, m, currentElement.Name, currentElement.OldFileName)));
                    }
                    else
                    {
                        currentElement.DestinationFolder.AsParallel().ForAll(f =>
                                f.ForEach(m =>
                                    RenameFile(
                                    currentElement.SourcePath, m, currentElement.Name, currentElement.OldFileName)));
                    }

                    break;
            }
        }

        #region Methods

        #region delete
        /// <summary>
        /// Deletes the file.
        /// </summary>
        /// <param name="destPath">The destination path.</param>
        /// <returns>Return new object</returns>
        private object DeleteFile(string destPath)
        {
            if (Directory.Exists(Path.GetDirectoryName(destPath)))
            {
                File.Delete(destPath);
            }

            return new object();
        }
        #endregion

        #region copy

        /// <summary>
        /// Check if file should be block compared.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns>
        /// Returns true if the file should
        /// be block compared
        /// </returns>
        private bool HaveToBlockCompare(string file)
        {
            using (FileStream fileStream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                if (Math.Round((fileStream.Length / 1024f) / 1024f, 2, MidpointRounding.AwayFromZero) > this.BlockCompareSizeInMb)
                {
                    fileStream.Dispose();
                    fileStream.Close();
                    return true;
                }

                fileStream.Dispose();
                fileStream.Close();
            }

            return false;
        }
        #endregion

        #region copy

        /// <summary>
        /// Copies the file.
        /// </summary>
        /// <param name="sourceFolder">The source folder.</param>
        /// <param name="sourceFile">The source file.</param>
        /// <param name="destFile">The destination file.</param>
        /// <param name="destinationFolder">The destination folder.</param>
        /// <param name="name">The name of the file.</param>
        /// <returns>Return new object</returns>
        private object CopyFile(string sourceFolder, string sourceFile, string destFile, string destinationFolder, string name)
        {
            if (!Directory.Exists(Path.GetDirectoryName(destFile)))
            {
                FileDirectoryClassHelper.CreateDirectoryPath(new DirectoryInfo(sourceFolder).GetDirectories(), destinationFolder, name.Split(Path.DirectorySeparatorChar));
            }
            
            File.Copy(sourceFile, destFile, true);
            FileDirectoryClassHelper.CopyFileSettings(new FileInfo(destFile), new FileInfo(sourceFile));
            return new object();
        }

        /// <summary>
        /// Compares the file.
        /// </summary>
        /// <param name="srcFile">The path of the source file.</param>
        /// <param name="sourceFolder">The path to the source folder.</param>
        /// <param name="destFile">The path to the destination file.</param>
        /// <param name="destinationFolder">The path to the destination folder.</param>
        /// <param name="name">The file name.</param>
        /// <returns>Return new object</returns>
        private object CompareFile(string srcFile, string sourceFolder, string destFile, string destinationFolder, string name)
        {
            if (!Directory.Exists(Path.GetDirectoryName(destFile)))
            {
                FileDirectoryClassHelper.CreateDirectoryPath(new DirectoryInfo(sourceFolder).GetDirectories(), destinationFolder, name.Split(Path.DirectorySeparatorChar));
            }

            // To get bytes form MB (Max value is 4294967296 bytes)
            int blockSizeInByte = Convert.ToInt32(this.BlockSizeInMb * 1024 * 1024);
            int iteration = 0;
            if (File.Exists(destFile))
            {
                using (FileStream strDest = new FileStream(destFile, FileMode.Open, FileAccess.ReadWrite, FileShare.None),
                    strSrc = new FileStream(srcFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    BinaryWriter br = new BinaryWriter(strDest);
                    if (File.Exists(strDest.Name))
                    {
                        long arraySize;
                        byte[] byteHashSource = new byte[16];
                        byte[] byteHashDest = new byte[16];
                        for (long i = 0; i < strSrc.Length; i += arraySize)
                        {
                            arraySize = (i + blockSizeInByte) < strSrc.Length ? blockSizeInByte : (strSrc.Length - i);
                            byte[] buffSource = new byte[arraySize];
                            byte[] buffDestination = new byte[arraySize];

                            this.CheckBlock(strSrc, ref buffSource, ref byteHashSource);
                            this.CheckBlock(strDest, ref buffDestination, ref byteHashDest);

                            /*Task[] tasks = { Task.Factory.StartNew(() => CheckBlock(strSrc, ref buffSource, ref byteHashSource)),
                                           Task.Factory.StartNew(() => CheckBlock(strDest, ref buffDestination, ref byteHashDest)) };
                            Task.WaitAll(tasks);*/

                            this.CheckEquality(br, buffSource, byteHashSource, byteHashDest, iteration, blockSizeInByte);
                            ++iteration;
                        }

                        if (strDest.Length > strSrc.Length)
                        {
                            strDest.SetLength(strSrc.Length);
                        }
                    }

                    strDest.Dispose();
                    strDest.Close();
                }
            }
            else
            {
                File.Copy(srcFile, destFile, true);
            }

            FileDirectoryClassHelper.CopyFileSettings(new FileInfo(destFile), new FileInfo(srcFile));
            return new object();
        }

        /// <summary>
        /// Checks the block.
        /// </summary>
        /// <param name="str">The file stream.</param>
        /// <param name="buffer">The buffer.</param>
        /// <param name="byteHash">The byte hash.</param>
        /// <returns>Return new object</returns>
        private object CheckBlock(FileStream str, ref byte[] buffer, ref byte[] byteHash)
        {
            int count = 0;
            while ((count = str.Read(buffer, 0, buffer.Length)) <= buffer.Length)
            {
                using (var md5 = MD5.Create())
                {
                    byteHash = md5.ComputeHash(buffer);
                    break;
                }
            }

            return new object();
        }

        /// <summary>
        /// Checks the equality of the blocks.
        /// </summary>
        /// <param name="br">The binary writer.</param>
        /// <param name="buffSource">The buffer from the source.</param>
        /// <param name="byteHashSource">The byte hash from source.</param>
        /// <param name="byteHashDest">The byte hash from destination.</param>
        /// <param name="iteration">The iteration.</param>
        /// <param name="blockSizeInByte">The block size in byte.</param>
        private void CheckEquality(BinaryWriter br, byte[] buffSource, IEnumerable<byte> byteHashSource, IEnumerable<byte> byteHashDest, int iteration, int blockSizeInByte)
        {
            if (!byteHashSource.SequenceEqual(byteHashDest))
            {
                br.Seek(blockSizeInByte * iteration, SeekOrigin.Begin);
                br.Write(buffSource, 0, buffSource.Length);
                br.Flush();
            }
        }

        #endregion

        #region rename
        /// <summary>
        /// Renames the file.
        /// </summary>
        /// <param name="sourcePath">The source path.</param>
        /// <param name="destinationFolder">The destination folder.</param>
        /// <param name="name">The name of the file.</param>
        /// <param name="oldName">The old name of the file.</param>
        /// <returns>Return new object</returns>
        private object RenameFile(string sourcePath, string destinationFolder, string name, string oldName)
        {
            File.Copy(sourcePath, Path.Combine(destinationFolder, name), true);
            FileDirectoryClassHelper.CopyFileSettings(new FileInfo(Path.Combine(destinationFolder, name)), new FileInfo(sourcePath));
            File.Delete(Path.Combine(destinationFolder, oldName));

            return new object();
        }
        #endregion

        #endregion
    }
}
