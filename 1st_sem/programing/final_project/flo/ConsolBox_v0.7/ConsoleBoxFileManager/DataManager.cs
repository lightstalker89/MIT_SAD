// *******************************************************
// * <copyright file="DataManager.cs" company="FGrill">
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
    using System.Collections.Concurrent;
    using System.IO;
    using System.Threading;
    using ConsoleBoxDataManager.Events;

    /// <summary>
    /// Class representing the <see cref="DataManager"/>
    /// </summary>
    public class DataManager : IDataManager
    {
        /// <summary>
        /// The file manager.
        /// Handles all file jobs.
        /// </summary>
        private readonly IFileManager fileManager = new FileManager();


        /// <summary>
        /// The directory manager
        /// Handles all directory jobs.
        /// </summary>
        private readonly IDirectoryManager directoryManager = new DirectoryManager();

        /// <summary>
        /// The worker thread
        /// </summary>
        private Thread workerThread;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataManager"/> class.
        /// </summary>
        public DataManager()
        {
            this.QueueItems = new ConcurrentQueue<QueueItem>();
            this.workerThread = new Thread(this.HandleQueueItems) { IsBackground = true };
            this.workerThread.Start();
        }

        #region events
        /// <inheritdoc/>
        public event EventHandler<ExceptionDataManagerEventArgs> ExceptionMessage;
        #endregion

        #region properties
        /// <inheritdoc/>
        public ConcurrentQueue<QueueItem> QueueItems { get; set; }
        #endregion


        /// <inheritdoc/>
        public void InitializeDataManager(long blockCompareSize, long blockSize, bool parallelSync)
        {
            this.fileManager.BlockCompareSizeInMb = blockCompareSize;
            this.fileManager.BlockSizeInMb = blockSize;

            this.fileManager.ParallelSync = parallelSync;
            this.directoryManager.ParallelSync = parallelSync;
        }

        /// <inheritdoc/>
        public void AddFileManagerJob(QueueItem item)
        {
            this.QueueItems.Enqueue(item);
        }

        /// <summary>
        /// Handles the queue items.
        /// Is running in a background Thread.
        /// </summary>
        private void HandleQueueItems()
        {
            do
            {
                if (this.QueueItems.Count > 0)
                {
                    QueueItem currentElement;
                    if (this.QueueItems.TryDequeue(out currentElement))
                    {
                        try
                        {
                            if (!currentElement.IsDirectory)
                            {
                                if (File.Exists(currentElement.SourcePath) || currentElement.Action == "Delete")
                                {
                                    this.fileManager.HandleFiles(currentElement);
                                }
                            }
                            else
                            {
                                if (Directory.Exists(currentElement.SourcePath) || currentElement.Action == "Delete")
                                {
                                    this.directoryManager.HandleDirectories(currentElement);
                                }
                            }
                        }
                        catch (UnauthorizedAccessException unauthorizedAccessException)
                        {
                            this.ExceptionMessage(this, new ExceptionDataManagerEventArgs(unauthorizedAccessException.GetType(), unauthorizedAccessException.Message));
                        }
                        catch (ArgumentException argumentException)
                        {
                            this.ExceptionMessage(this, new ExceptionDataManagerEventArgs(argumentException.GetType(), argumentException.Message));
                        }
                        catch (PathTooLongException pathTooLongException)
                        {
                            this.ExceptionMessage(this, new ExceptionDataManagerEventArgs(pathTooLongException.GetType(), pathTooLongException.Message));
                        }
                        catch (DirectoryNotFoundException directoryNotFoundException)
                        {
                            this.ExceptionMessage(this, new ExceptionDataManagerEventArgs(directoryNotFoundException.GetType(), directoryNotFoundException.Message));
                        }
                        catch (FileNotFoundException fileNotFoundException)
                        {
                            this.ExceptionMessage(this, new ExceptionDataManagerEventArgs(fileNotFoundException.GetType(), fileNotFoundException.Message));
                        }
                        catch (IOException inoutException)
                        {
                            if (this.CheckIfRepeat(currentElement))
                            {
                                this.QueueItems.Enqueue(currentElement);
                            }

                            this.ExceptionMessage(this, new ExceptionDataManagerEventArgs(inoutException.GetType(), inoutException.Message));
                        }
                        catch (NotSupportedException notSupportedException)
                        {
                            this.ExceptionMessage(this, new ExceptionDataManagerEventArgs(notSupportedException.GetType(), notSupportedException.Message));
                        }
                        catch (AggregateException aggregateException)
                        {
                            foreach (var exception in aggregateException.InnerExceptions)
                            {
                                this.ExceptionMessage(this, new ExceptionDataManagerEventArgs(exception.GetType(), "At action: "  + currentElement.Action + " - "  + exception.Message));
                            }
                        }
                    }
                }
                else
                {
                    Thread.Sleep(500);
                }
            }
            while (true);
        }

        #region helper methods
        /// <summary>
        /// Checks if the element should be
        /// put in queue again.
        /// </summary>
        /// <param name="currentElement">The current element.</param>
        /// <returns>
        /// Returns true if the element should be
        /// put in queue again
        /// </returns>
        private bool CheckIfRepeat(QueueItem currentElement)
        {
            if (currentElement.IsDirectory)
            {
                if (Directory.Exists(currentElement.SourcePath) || currentElement.Action == "DELETE")
                {
                    return true;
                }
                else
                {
                    return false;
                } 
            }
            else
            {
                if (File.Exists(currentElement.SourcePath) || currentElement.Action == "DELETE")
                {
                    if (this.CheckIfTempFile(Path.GetFileName(currentElement.SourcePath), currentElement.SourcePath))
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return false;
                } 
            }
        }

        /// <summary>
        /// Checks if is a temporary file.
        /// </summary>
        /// <param name="sourceName">Name of the source.</param>
        /// <param name="sourcePath">The source path.</param>
        /// <returns>
        /// Returns true if the element is a
        /// temporary file
        /// </returns>
        private bool CheckIfTempFile(string sourceName, string sourcePath)
        {
            if ((sourceName.Substring(0, 2) == "~$" && (new FileInfo(sourcePath)).Attributes.ToString().Contains(FileAttributes.Hidden.ToString())) ||
                ((Path.GetExtension(sourcePath) == ".tmp") && (new FileInfo(sourceName)).Attributes.ToString().Contains(FileAttributes.Hidden.ToString())))
            {
                return true;
            }
            else
            {
                return false;
            } 
        }
        #endregion
    }
}
