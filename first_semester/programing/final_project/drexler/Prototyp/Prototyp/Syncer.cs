//-----------------------------------------------------------------------
// <copyright file="Syncer.cs" company="MD Development">
//     Copyright (c) MD Development. All rights reserved.
// </copyright>
// <author>Michael Drexler</author>
//-----------------------------------------------------------------------
namespace Prototyp
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Prototyp.Log;
    using Prototyp.View;

    /// <summary>
    /// Executes synchronisations
    /// </summary>
    public class Syncer
    {
        /// <summary>
        /// Lock resources for progressing
        /// </summary>
        private object mlock;

        /// <summary>
        /// Jobs for progressing
        /// </summary>
        private ConcurrentQueue<Job> jobs;

        /// <summary>
        /// Logger instance to write logs
        /// </summary>
        private ILogger logger = Logger.Instance;

        /// <summary>
        /// Configuration for this progam
        /// </summary>
        private Config config;

        /// <summary>
        /// Initializes a new instance of the <see cref="Syncer"/> class
        /// </summary>
        /// <param name="jobs">Queue containing jobs to progress</param>
        public Syncer(ConcurrentQueue<Job> jobs)
        {
            this.jobs = jobs;
            this.mlock = new object();
        }

        // Properties
        // Methods

        /// <summary>
        /// Initial check and synchronisation between source and target
        /// Will be progressed when the program starts.
        /// When the user starts the program, an initial check for changes between the defined source directories
        /// and the defined target directories should be executed. If differences between the directories are 
        /// available a synchronisation should be started. This should be happened within an own thread. That´s because
        /// the user shouldn´t have to wait for that process to be finished.
        /// </summary>
        /// <param name="config">Configuration of the program</param>
        public void Init(Config config)
        {
            this.config = config;
            this.logger.Logs.Enqueue(new LogEntry(string.Format("Syncer: Initial check of the sync state between source- and target directories"), LoggingType.Info));

            if (config != null && config.SourceDirectories != null && config.SourceDirectories.Count > 0)
            {
                foreach (Directory dir in config.SourceDirectories)
                {
                    if (dir.TargetDirectories != null && dir.TargetDirectories.Count > 0)
                    {
                        // DirectoryInfo dirInfo = new DirectoryInfo(dir.Path);
                        foreach (Directory item in dir.TargetDirectories)
                        {
                            try
                            {
                                this.CheckForChanges(dir.Path, item.Path, dir.ExceptionDirectories, dir.IncludeSubdirectories);
                            }
                            catch (DirectoryNotFoundException ex)
                            {
                                this.logger.Logs.Enqueue(new Log.LogEntry(string.Format("Syncer:[Exception] {0}", ex.Message), LoggingType.Error));
                            }
                            catch (IOException ex)
                            {
                                this.logger.Logs.Enqueue(new Log.LogEntry(string.Format("Syncer:[Exception] {0}", ex.Message), LoggingType.Error));
                            }
                        }              
                    }
                    else
                    {
                        this.logger.Logs.Enqueue(new LogEntry(string.Format("Syncer: No initial check for {0}, cause no targets are configured!", dir.Path), LoggingType.Warning));
                    }
                }
            }
            else
            {
                this.logger.Logs.Enqueue(new LogEntry(string.Format("Syncer: Initial check couldn´t be done, because no source directories configured!"), LoggingType.Warning));
            }
        }

        /// <summary>
        /// Start synchronising
        /// If new jobs are available, they will be progressed
        /// </summary>
        public void StartSynchronising()
        {
            this.logger.Logs.Enqueue(new Log.LogEntry("Syncer: Start synchronising ... ", LoggingType.Info));

            while (true)
            {   
                while (this.jobs != null && this.jobs.Count != 0)
                {
                    Job item;
                    while (!this.jobs.TryDequeue(out item))
                    {
                        Thread.SpinWait(4000);
                    }

                    item.Status = Jobstatus.running;
                    this.logger.Logs.Enqueue(new Log.LogEntry(string.Format("Syncer: Syncing {0} from-{1} to-{2} ... !", item.FileName, item.SourceDirectory, item.TargetDirectory), LoggingType.Info));

                    try
                    {
                        switch (item.EventType)
                        {
                            case EventType.Changed:
                                try
                                {
                                    Monitor.Enter(this.mlock);
                                    string filePath = Path.Combine(item.SourceDirectory, item.FileName);

                                    // FileInfo info = new FileInfo(filePath);
                                    if (!System.IO.Directory.Exists(filePath))
                                    {
                                        FileStream sourceInput = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                                        FileStream targetInput = new FileStream(Path.Combine(item.TargetDirectory, item.FileName), FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);

                                        if ((sourceInput.Length > this.config.FileSizeForBlockCompare) && (this.config.FileSizeForBlockCompare > this.config.BlockSize))
                                        {
                                            this.CopyViaBlockCompare(sourceInput, targetInput);
                                        }
                                        else
                                        {
                                            this.CopyFile(sourceInput, targetInput);
                                        }
                                        
                                        // info.CopyTo(Path.Combine(item.TargetDirectory, item.FileName), true);
                                        item.Status = Jobstatus.success;
                                        item.End = DateTime.Now;
                                    }

                                    Monitor.Exit(this.mlock);
                                }
                                catch (Exception e)
                                {
                                    this.logger.Logs.Enqueue(new LogEntry(string.Format("Syncer: Error on changing! Error- {0}", e.Message), LoggingType.Error));
                                }

                                break;
                            case EventType.Created:
                                try
                                {
                                    Monitor.Enter(this.mlock);

                                    string filePath = Path.Combine(item.SourceDirectory, item.FileName);
                                    FileInfo info = new FileInfo(filePath);

                                    // if (!System.IO.Directory.Exists(filePath))
                                    if ((info.Attributes & FileAttributes.Directory) == FileAttributes.Directory) 
                                    {
                                        info.CopyTo(Path.Combine(item.TargetDirectory, item.FileName), true);
                                        item.Status = Jobstatus.success;
                                        item.End = DateTime.Now;
                                    }
                                    else
                                    {
                                        this.DirectoryCopy(item.SourceDirectory, item.TargetDirectory, item.ExceptionDirectories, item.IncludeSubdirectories);
                                    }

                                    item.Status = Jobstatus.success;
                                    item.End = DateTime.Now;

                                    Monitor.Exit(this.mlock);
                                }
                                catch (Exception e)
                                {
                                    this.logger.Logs.Enqueue(new LogEntry(string.Format("Syncer: Error on changing! Error- {0}", e.Message), LoggingType.Error));
                                }

                                break;
                            case EventType.Deleted:
                                try
                                {
                                    Monitor.Enter(this.mlock);

                                    string deletePath = Path.Combine(item.TargetDirectory, item.FileName);
                                    if (File.Exists(deletePath))
                                    {
                                        File.Delete(deletePath);
                                        this.logger.Logs.Enqueue(new LogEntry(string.Format("Syncer: File/Directory {0} deleted!", deletePath), LoggingType.Success));
                                    }
                                    else if (System.IO.Directory.Exists(deletePath))
                                    {
                                        System.IO.Directory.Delete(deletePath, true);
                                        this.logger.Logs.Enqueue(new LogEntry(string.Format("Syncer: File/Directory {0} deleted!", deletePath), LoggingType.Success));
                                    }
                                    else
                                    {
                                        this.logger.Logs.Enqueue(new LogEntry(string.Format("Syncer: File/Directory {0} ´couldn´t be deleted, because it is not available within target dir!", deletePath), LoggingType.Warning));
                                    }

                                    Monitor.Exit(this.mlock);
                                }
                                catch (Exception e)
                                {
                                    this.logger.Logs.Enqueue(new LogEntry(string.Format("Syncer: Error on deleting! Error- {0}", e.Message), LoggingType.Error));
                                }

                                break;
                            case EventType.Renamed:
                                try
                                {
                                    Monitor.Enter(this.mlock);

                                    string oldRenamePath = Path.Combine(item.TargetDirectory, item.OldName);
                                    string newRenamePath = Path.Combine(item.TargetDirectory, item.FileName);

                                    if (File.Exists(oldRenamePath))
                                    {
                                        File.Move(oldRenamePath, newRenamePath);
                                    }
                                    else if (System.IO.Directory.Exists(oldRenamePath))
                                    {
                                        System.IO.Directory.Move(oldRenamePath, newRenamePath);
                                    }
                                    else
                                    {
                                        this.logger.Logs.Enqueue(new LogEntry(string.Format("Syncer: File/Directory {0} ´couldn´t be renamed, because it is not available within target dir!", oldRenamePath), LoggingType.Warning));
                                    }

                                    Monitor.Exit(this.mlock);
                                }
                                catch (Exception ex)
                                {
                                    this.logger.Logs.Enqueue(new LogEntry(string.Format("Syncer: Error on renaming! Error- {0}", ex.Message), LoggingType.Error));
                                }

                                break;
                            default:
                                break;
                        }
                    }
                    catch (DirectoryNotFoundException ex)
                    {
                        this.logger.Logs.Enqueue(new Log.LogEntry(string.Format("Syncer:[Exception] {0}", ex.Message), LoggingType.Error));
                    }
                    catch (Exception e)
                    {
                        this.logger.Logs.Enqueue(new Log.LogEntry(string.Format("Syncer:[Exception] {0}!", e.Message), LoggingType.Error));
                    }
                }

                try
                {
                    Thread.Sleep(1000);
                }
                catch (ThreadInterruptedException e)
                {
                    this.logger.Logs.Enqueue(new Log.LogEntry("Syncer:[Exception] Interrupted Exception" + e.Message, LoggingType.Error));
                }        
            }
        }

        /// <summary>
        /// Check every file and directory and progress only changed items
        /// </summary>
        /// <param name="sourceDir">Source directory</param>
        /// <param name="destDir">Targert directory</param>
        /// <param name="exeptionDirs">Excluded directories from the source directory</param>
        /// <param name="includeSubDir">Include sub directories</param>
        private void CheckForChanges(string sourceDir, string destDir, List<Directory> exeptionDirs, bool includeSubDir)
        {
            Monitor.Enter(this.mlock);
            DirectoryInfo dir = new DirectoryInfo(sourceDir);
            DirectoryInfo[] dirs = dir.GetDirectories();

            // If the source directory does not exist, throw an exception.
            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException("Syncer: Source directory does not exist or could not be found: " + sourceDir);
            }

            DirectoryInfo targetDir = null;
            DirectoryInfo[] targetDirs = null;
            FileInfo[] destFiles = null;
            bool excludedDir = false;

            // If the destination directory does not exist, create it.
            if (!System.IO.Directory.Exists(destDir))
            {
                foreach (Directory item in exeptionDirs)
                {
                    if (!this.CheckPathContainsGivenPath(item.Path, sourceDir))
                    {
                        System.IO.Directory.CreateDirectory(destDir);
                        excludedDir = false;
                        this.logger.Logs.Enqueue(new LogEntry(string.Format("Syncer: Target directory {0} doesn´t exist. -> Directory created", destDir), LoggingType.Info));
                    }
                    else
                    {
                        excludedDir = true;
                    }
                }
            }

            if (!excludedDir)
            {
                // Get all directories and files for this level in the tree
                targetDir = new DirectoryInfo(destDir);
                targetDirs = targetDir.GetDirectories();
                destFiles = targetDir.GetFiles();

                // Get the file contents of the directory to copy.
                FileInfo[] sourceFiles = dir.GetFiles();
                foreach (FileInfo sfile in sourceFiles)
                {
                    if (destFiles != null && destFiles.Length > 0)
                    {
                        FileInfo dfile = destFiles.Where(m => m.Name == sfile.Name).Select(m => m).SingleOrDefault();
                        if (dfile != null && dfile.Exists)
                        {
                            if (sfile.Attributes != dfile.Attributes || sfile.LastWriteTime != dfile.LastWriteTime ||
                                sfile.Length != dfile.Length || sfile.IsReadOnly != dfile.IsReadOnly)
                            {
                                Job temp = new Job(sourceDir, destDir, sfile.Name, EventType.Changed, includeSubDir, string.Empty, exeptionDirs);
                                this.jobs.Enqueue(temp);
                                this.logger.Logs.Enqueue(new LogEntry(string.Format("Syncer: File {0} not equal to file in target {1}. New job created", sfile.FullName, dfile.FullName), LoggingType.Info));
                            }
                        }
                        else
                        {
                            Job temp = new Job(sourceDir, destDir, sfile.Name, EventType.Created, includeSubDir, string.Empty, exeptionDirs);
                            this.jobs.Enqueue(temp);
                            this.logger.Logs.Enqueue(new LogEntry(string.Format("Syncer: File {0} not exist in target {1}. New job created", sfile.Name, destDir), LoggingType.Info));
                        }
                    }
                    else
                    {
                        Job temp = new Job(sourceDir, destDir, sfile.Name, EventType.Created, includeSubDir, string.Empty, exeptionDirs);
                        this.jobs.Enqueue(temp);
                        this.logger.Logs.Enqueue(new LogEntry(string.Format("Syncer: File {0} not exist in target {1}. New job created", sfile.Name, destDir), LoggingType.Info));
                    }
                }

                if (includeSubDir)
                {
                    // loop through all subdirectories within the source directory and compare to the target directories
                    foreach (DirectoryInfo subdir in dirs)
                    {
                        // Create the subdirectory.
                        if (targetDirs != null && targetDirs.Length > 0)
                        {
                            DirectoryInfo item = targetDirs.Where(m => m.Name == subdir.Name).Select(m => m).SingleOrDefault();
                            if (item != null && item.Exists)
                            {
                                if (subdir.Attributes != item.Attributes)
                                {
                                    Job temp = new Job(subdir.FullName, item.Name, subdir.FullName, EventType.Changed, includeSubDir, string.Empty, exeptionDirs);
                                    this.jobs.Enqueue(temp);
                                    this.logger.Logs.Enqueue(new LogEntry(string.Format("Syncer: Directory {0} not equal to directory in target {1}. New job created", subdir.FullName, item.FullName), LoggingType.Info));
                                }
                            }
                        }

                        string temppath = Path.Combine(destDir, subdir.Name);
                        this.CheckForChanges(subdir.FullName, temppath, exeptionDirs, includeSubDir);
                    }
                }
            }

            Monitor.Exit(this.mlock);
        }

        /// <summary>
        /// Copy only differences between files.
        /// BlockCompare will be activated if configured file size is reached.
        /// BlockSize for comparing should be configured within config file, else 4096 will be used.
        /// </summary>
        /// <param name="sourceInput">FileStream from source file</param>
        /// <param name="targetInput">FileStream from target file</param>
        private void CopyViaBlockCompare(FileStream sourceInput, FileStream targetInput)
        {
            //// TODO Compare blocks and copy differnt blocks
            try
            {
                int i, j = 0;
                byte[] bufferSource;
                byte[] bufferTarget;

                // Check if block size is configured else take frame size (4k)
                if (this.config != null && this.config.BlockSize > 0)
                {
                    bufferSource = new byte[this.config.BlockSize];
                    bufferTarget = new byte[this.config.BlockSize];
                }
                else
                {
                    bufferSource = new byte[4096];
                    bufferTarget = new byte[4096];
                }

                do
                {
                    i = sourceInput.Read(bufferSource, 0, bufferSource.Length);
                    j = targetInput.Read(bufferTarget, 0, bufferTarget.Length);

                    // buffers are not equal so write changed bytes to target
                    if (!bufferSource.Take(i).SequenceEqual(bufferTarget.Take(j)))
                    {
                        targetInput.Position = targetInput.Position - j;
                        targetInput.Write(bufferSource, 0, i);
                    }
                } 
                while (i != 0);

                sourceInput.Close();
                sourceInput.Dispose();
                targetInput.Close();
                targetInput.Dispose();
            }
            catch (Exception ex)
            {
                this.logger.Logs.Enqueue(new LogEntry(string.Format("Syncer: [Exception] Error- {0}", ex.Message), LoggingType.Error));
            }
        }

        /// <summary>
        /// Copy source file to the target, create if not exist
        /// </summary>
        /// <param name="sourceInput">FileStream of source file</param>
        /// <param name="targetInput">FileStream of target file</param>
        private void CopyFile(FileStream sourceInput, FileStream targetInput)
        {
            byte[] buffer = new byte[4096];

            int read;
            while ((read = sourceInput.Read(buffer, 0, buffer.Length)) > 0)
            {
                targetInput.Write(buffer, 0, read);
            }

            sourceInput.Dispose();
            targetInput.Dispose();
        }

        /// <summary>
        /// Copies Diretories and Files from a source location to the target location.
        /// </summary>
        /// <param name="sourceDirName">Source directory</param>
        /// <param name="destDirName">Target directory</param>
        /// <param name="exceptionDirs">Excluded directories from the source directory</param>
        /// <param name="copySubDirs">Recursively copy</param>
        private void DirectoryCopy(string sourceDirName, string destDirName, List<Directory> exceptionDirs, bool copySubDirs)
        {
            // Algorithmus der aktuelle Ebene (Directories und Files) synchronisiert, rekursiv
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);
            DirectoryInfo[] dirs = dir.GetDirectories();

            // If the source directory does not exist, throw an exception.
            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Syncer: Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            try
            {
                // If the destination directory does not exist, create it.
                if (!System.IO.Directory.Exists(destDirName))
                {
                    System.IO.Directory.CreateDirectory(destDirName);
                }
            }
            catch (IOException)
            {
                throw;
            }
            
            // Get the file contents of the directory to copy.
            FileInfo[] files = dir.GetFiles();

            foreach (FileInfo file in files)
            {
                // Create the path to the new copy of the file.
                string temppath = Path.Combine(destDirName, file.Name);
                bool allowCopy = true;
                if (exceptionDirs != null && exceptionDirs.Count > 0)
                {
                    foreach (Directory exeptionDirectory in exceptionDirs)
                    {
                        if (this.CheckPathContainsGivenPath(exeptionDirectory.Path, file.DirectoryName))
                        {
                            allowCopy = false;
                        }
                    }
                }

                if (allowCopy)
                {
                    // Copy the file.
                    file.CopyTo(temppath, true);
                }
            }

            // If copySubDirs is true, copy the subdirectories.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    // Create the subdirectory.
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    bool allowCopy = true;

                    if (exceptionDirs != null && exceptionDirs.Count > 0)
                    {
                        foreach (Directory exeptionDirectory in exceptionDirs)
                        {
                            if (this.CheckPathContainsGivenPath(exeptionDirectory.Path, subdir.FullName))
                            {
                                allowCopy = false;
                            }
                        }
                    }

                    if (allowCopy)
                    {
                        // Copy the subdirectories.
                        this.DirectoryCopy(subdir.FullName, temppath, exceptionDirs, copySubDirs);
                    }
                }
            }
        }

        /// <summary>
        /// Check if path contains the exception path
        /// </summary>
        /// <param name="exceptionPath">Excluded directory for the given source directory</param>
        /// <param name="path">Directory within the source directory</param>
        /// <returns>If path is excluded from the given path</returns>
        private bool CheckPathContainsGivenPath(string exceptionPath, string path)
        {
            if (exceptionPath.Length >= path.Length && path.StartsWith(exceptionPath))
            {
                return true;
            }

            return false;
        }

        ///// <summary>
        ///// TODO
        ///// </summary>
        ///// <param name="filename"></param>
        ///// <returns></returns>
        //// private byte[] BlockCompare(string sourceFile, string destFile)
        //// {
        ////    using (var md5 = MD5.Create())
        ////    {
        ////        using (var stream = File.OpenRead(sourceFile))
        ////        {
        ////            return md5.ComputeHash(stream);
        ////        }

        ////        using (var stream = File.OpenRead(destFile))
        ////        {

        ////        }
        ////    }
        //// }
    }
}
