// *******************************************************
// * <copyright file="FileWatcher.cs" company="FGrill">
// * Copyright (c) 2013 Florian Grill. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Florian Grill</author>
// *******************************************************/

namespace ConsoleBoxFileWatcher
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using ConsoleBoxFileWatcher.Events;

    /// <summary>
    /// Class representing the <see cref="FileWatcher"/>
    /// </summary>
    public class FileWatcher : IFileWatcher
    {
        #region fields
        /// <summary>
        /// The filters for the file watcher
        /// </summary>
        private readonly NotifyFilters[] filters = 
        { 
            NotifyFilters.Attributes |
            NotifyFilters.FileName |
            NotifyFilters.Size |
            NotifyFilters.CreationTime |
            NotifyFilters.Security,
            NotifyFilters.DirectoryName
        };
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="FileWatcher"/> class.
        /// </summary>
        public FileWatcher()
        {
            this.SystemWatcherEventList = new List<FileSystemWatcher>();
        }

        #region events
        /// <inheritdoc/>
        public event EventHandler<ExceptionFileWatcherEventArgs> ExceptionMessage;

        /// <inheritdoc/>
        public event EventHandler<FileWatcherJobEventArgs> FileWatcherJob;
        #endregion

        #region properties
        /// <summary>
        /// Gets or sets the source folders.
        /// </summary>
        /// <value>
        /// The source folders.
        /// </value>
        public IList<SourceFolderInfo> SourceFolders { get; set; }

        /// <summary>
        /// Gets the system watcher event list.
        /// </summary>
        /// <value>
        /// The system watcher event list.
        /// </value>
        public List<FileSystemWatcher> SystemWatcherEventList { get; private set; }
        #endregion

        #region initialize file system watcher and events
        /// <inheritdoc/>
        public void InitializeFileWatcher(IList<SourceFolderInfo> sourceFolderInfo)
        {
            this.SourceFolders = sourceFolderInfo;
        }

        /// <inheritdoc/>
        public void SetFileSystemWatcher()
        {
            foreach (var sourceFolderInfo in this.SourceFolders)
            {
                try
                {
                    foreach (var filter in this.filters)
                    {
                        CustomSystemWatcher fsw = new CustomSystemWatcher(sourceFolderInfo);
                        fsw.EnableRaisingEvents = true;
                        fsw.NotifyFilter = filter;
                        fsw.IncludeSubdirectories = sourceFolderInfo.Recursion;
                        fsw.Changed += this.Fsw_Changed;
                        fsw.Deleted += this.Fsw_Deleted;
                        fsw.Created += this.Fsw_Created;
                        fsw.Renamed += this.Fsw_Renamed;
                        this.SystemWatcherEventList.Add(fsw);
                    }
                }
                catch (ArgumentNullException argumentNullException)
                {
                    this.ExceptionMessage(this, new ExceptionFileWatcherEventArgs(argumentNullException.GetType(), "A null reference Exception occured when initialise File watcher"));
                }
                catch (PathTooLongException pathTooLongException)
                {
                    this.ExceptionMessage(this, new ExceptionFileWatcherEventArgs(pathTooLongException.GetType(), "Path too long when initialise File watcher"));
                }
                catch (ArgumentException argumentException)
                {
                    this.ExceptionMessage(this, new ExceptionFileWatcherEventArgs(argumentException.GetType(), "Arguement was invalide when initialise File watcher"));
                }
            }
        }
        #endregion

        #region create synchron state
        /// <inheritdoc/>
        public void ScanFoldersForSyncState()
        {
            try
            {
                foreach (var mapping in this.SourceFolders)
                {
                    List<string> sourceFiles =
                        Directory.GetFiles(mapping.Path, "*", SearchOption.AllDirectories).ToList();
                    List<string> sourceDirectories =
                        Directory.GetDirectories(mapping.Path, "*", SearchOption.AllDirectories).ToList();

                    // For creating needed files or directories
                    this.HandleSourceItems(
                        mapping.Path, mapping.DestinationFolders, mapping.SplittedDestinationFolders, false);
                    this.HandleSourceItems(
                        mapping.Path, mapping.DestinationFolders, mapping.SplittedDestinationFolders, true);

                    // For deleting unnecessary files or directories
                    this.HandleDestinationItems(
                        mapping.Path, mapping.DestinationFolders, sourceDirectories, mapping.SplittedDestinationFolders, true);
                    this.HandleDestinationItems(
                        mapping.Path, mapping.DestinationFolders, sourceFiles, mapping.SplittedDestinationFolders, false);
                }
            }
            catch (DirectoryNotFoundException directoryNotFoundException)
            {
                this.ExceptionMessage(this, new ExceptionFileWatcherEventArgs(directoryNotFoundException.GetType(), " Directory was not found, please check config file!"));
            }
            catch (FileNotFoundException fileNotFoundException)
            {
                this.ExceptionMessage(this, new ExceptionFileWatcherEventArgs(fileNotFoundException.GetType(), " File was not found, please check config file!"));
            }
        }

        /// <summary>
        /// Adds the source items to queue.
        /// </summary>
        /// <param name="sourceDirectory">The source directory.</param>
        /// <param name="destinationFolders">The destination folders.</param>
        /// <param name="splittedDestinationFolders">The distributed destination folders.</param>
        /// <param name="isDirectory">if set to <c>true</c> it is directory.</param>
        private void HandleSourceItems(
            string sourceDirectory,
            List<string> destinationFolders,
            List<List<string>> splittedDestinationFolders,
            bool isDirectory)
        {
            IEnumerable<string> items;

            if (!isDirectory)
            {
                items = Directory.GetFiles(sourceDirectory + "\\", "*", SearchOption.AllDirectories);
            }
            else
            {
                items = Directory.GetDirectories(sourceDirectory, "*.*", SearchOption.AllDirectories);
            }

            foreach (string fullPath in items)
            {
                string name = fullPath.Replace(sourceDirectory + "\\", string.Empty);

                if (!this.CeckExceptionFolders(fullPath))
                {
                    this.FileWatcherJob(
                    this,
                    new FileWatcherJobEventArgs(
                        isDirectory,
                        sourceDirectory,
                        fullPath,
                        name,
                        splittedDestinationFolders,
                        EnumActions.Copy.ToString(),
                        string.Concat("Synchronise, element: ", fullPath),
                        string.Empty));
                }
            }
        }

        /// <summary>
        /// Adds the destination items to queue.
        /// </summary>
        /// <param name="sourceDirectory">The source directory.</param>
        /// <param name="destinationItems">The destination items.</param>
        /// <param name="sourceItems">The source items.</param>
        /// <param name="splittedDestinationFolders">The distributed destination folders.</param>
        /// <param name="isDirectory">if set to <c>true</c> it is directory.</param>
        private void HandleDestinationItems(
            string sourceDirectory,
            IEnumerable<string> destinationItems, 
            List<string> sourceItems,
            List<List<string>> splittedDestinationFolders,
            bool isDirectory)
        {
            for (int i = 0; i < sourceItems.Count; i++)
            {
                sourceItems[i] = sourceItems[i].Replace(sourceDirectory + "\\", string.Empty);
            }

            foreach (string destinationItem in destinationItems)
            {
                IEnumerable<string> allDestinationItems = null;

                if (!isDirectory)
                {
                    allDestinationItems = Directory.GetFiles(destinationItem, "*", SearchOption.AllDirectories);
                }
                else
                {
                    allDestinationItems = Directory.GetDirectories(
                        destinationItem + "\\", "*", SearchOption.AllDirectories);
                }

                foreach (string actualDestinationItem in allDestinationItems)
                {
                    string newDestinationItem = actualDestinationItem.Replace(destinationItem + "\\", string.Empty);

                    if (!sourceItems.Contains(newDestinationItem))
                    {
                        if (!this.CeckExceptionFolders(actualDestinationItem))
                        {
                            this.FileWatcherJob(
                            this,
                            new FileWatcherJobEventArgs(
                                isDirectory,
                                sourceDirectory,
                                actualDestinationItem,
                                newDestinationItem,
                                splittedDestinationFolders,
                                EnumActions.Delete.ToString(),
                                string.Concat("Synchronise, element: ", actualDestinationItem),
                                string.Empty));
                        }
                    }
                }
            }
        }
        #endregion

        #region event handler
        /// <summary>
        /// Handles the Renamed event of the file watcher.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RenamedEventArgs"/> instance containing the event data.</param>
        private void Fsw_Renamed(object sender, RenamedEventArgs e)
        {
            this.AddItemToQueue(
                ((CustomSystemWatcher)sender).SourceFolderInfo, 
                e.FullPath,
                e.Name, 
                EnumActions.Rename, 
                this.CheckFileOrDirectory(e.FullPath, EnumActions.Rename, sender, e.OldName, e.Name), 
                e.OldName);
        }

        /// <summary>
        /// Handles the Created event of the file watcher.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="FileSystemEventArgs"/> instance containing the event data.</param>
        private void Fsw_Created(object sender, FileSystemEventArgs e)
        {
            this.AddItemToQueue(
                ((CustomSystemWatcher)sender).SourceFolderInfo, 
                e.FullPath,
                e.Name, 
                EnumActions.Copy,
                this.CheckFileOrDirectory(e.FullPath, EnumActions.Copy, sender, string.Empty, string.Empty), 
                string.Empty);
        }

        /// <summary>
        /// Handles the Deleted event of the file watcher.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="FileSystemEventArgs"/> instance containing the event data.</param>
        private void Fsw_Deleted(object sender, FileSystemEventArgs e)
        {
            this.AddItemToQueue(
                ((CustomSystemWatcher)sender).SourceFolderInfo, 
                e.FullPath,
                e.Name, 
                EnumActions.Delete,
                this.CheckFileOrDirectory(e.FullPath, EnumActions.Delete, sender, string.Empty, string.Empty), 
                string.Empty);
        }

        /// <summary>
        /// Handles the Changed event of the file watcher.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="FileSystemEventArgs"/> instance containing the event data.</param>
        private void Fsw_Changed(object sender, FileSystemEventArgs e)
        {
            this.AddItemToQueue(
                ((CustomSystemWatcher)sender).SourceFolderInfo, 
                e.FullPath,
                e.Name, 
                EnumActions.Copy,
                this.CheckFileOrDirectory(e.FullPath, EnumActions.Copy, sender, string.Empty, string.Empty), 
                string.Empty);
        }
        #endregion

        #region file or directory checker
        /// <summary>
        /// Checks if it is file or directory.
        /// </summary>
        /// <param name="sourcePath">The source Path.</param>
        /// <param name="action">The action.</param>
        /// <param name="sender">The sender.</param>
        /// <param name="oldName">The old name.</param>
        /// <param name="name">The name.</param>
        /// <returns>returns true if it is directory.</returns>
        private bool CheckFileOrDirectory(string sourcePath, EnumActions action, object sender, string oldName, string name)
        {
            if (action == EnumActions.Delete)
            {
                if (((FileSystemWatcher)sender).NotifyFilter.ToString().Contains(NotifyFilters.DirectoryName.ToString()))
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
                if (Directory.Exists(sourcePath))
                {
                    return true;
                }
                else
                {
                    if (File.Exists(sourcePath))
                    {
                        if (action == EnumActions.Rename)
                        {
                            return false;
                        }
                        else
                        {
                            return false;
                        }
                    }

                    return false;
                }
            }
        }
        #endregion

        #region obtain job event
        /// <summary>
        /// Adds the item to queue.
        /// </summary>
        /// <param name="sourceFolderInfo">The source folder information.</param>
        /// <param name="fullPath">The full Path.</param>
        /// <param name="name">The name.</param>
        /// <param name="action">The action.</param>
        /// <param name="isDirectory">if set to <c>true</c> it is directory.</param>
        /// <param name="oldFileName">Old name of the file.</param>
        private void AddItemToQueue(
            SourceFolderInfo sourceFolderInfo,
            string fullPath,
            string name,
            EnumActions action,
            bool isDirectory,
            string oldFileName)
        {
            // Filter folders which are in exception folders
            if (!this.CeckExceptionFolders(fullPath))
            {
                string message = string.Concat(action.ToString(), " occured, element: ", fullPath);
                this.FileWatcherJob(
                    this,
                    new FileWatcherJobEventArgs(
                        isDirectory,
                        sourceFolderInfo.Path,
                        fullPath,
                        name,
                        sourceFolderInfo.SplittedDestinationFolders,
                        action.ToString(),
                        message,
                        oldFileName));
            }
        }
        #endregion

        /// <summary>
        /// Checks the file or directory is in or is exception folder.
        /// </summary>
        /// <param name="fullPath">The full Path.</param>
        /// <returns>Returns true if file or directory is in,
        /// or is exception folder</returns>
        private bool CeckExceptionFolders(string fullPath)
        {
            var es = from e in this.SourceFolders
                     where e.ExceptionFolders.Any(fullPath.Contains)
                     select e;

            return es.Any();
        }
    }
}
