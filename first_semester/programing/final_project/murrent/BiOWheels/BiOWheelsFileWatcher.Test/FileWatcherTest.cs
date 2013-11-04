// *******************************************************
// * <copyright file="FileWatcherTest.cs" company="MDMCoWorks">
// * Copyright (c) Mario Murrent. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Mario Murrent</author>
// *******************************************************/

using System.Globalization;
using System.Text;
using System.Threading;

namespace BiOWheelsFileWatcher.Test
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    using BiOWheelsFileWatcher.CustomEventArgs;

    using BiOWheelsTestHelper;

    using NUnit.Framework;

    /// <summary>
    /// Class representing the test for the <see cref="FileWatcher"/>
    /// </summary>
    [TestFixture]
    public class FileWatcherTest
    {
        /// <summary>
        /// Represents the <see cref="IQueueManager"/> instance
        /// </summary>
        private QueueManager queueManager;

        /// <summary>
        /// Represents the <see cref="FileWatcher"/> instance
        /// </summary>
        private FileWatcher fileWatcher;

        /// <summary>
        /// Represents a list of <see cref="DirectoryMapping"/> objects
        /// </summary>
        private List<DirectoryMapping> mappings;

        /// <summary>
        /// Represents the <see cref="FileComparator"/> instance
        /// </summary>
        private FileComparator fileComparator;

        /// <summary>
        /// List of notifications from the <see cref="FileWatcher"/>
        /// </summary>
        private List<string> notifications;

        /// <summary>
        /// Set up test environment
        /// </summary>
        [SetUp]
        public void Init()
        {
            this.fileComparator = new FileComparator();

            this.queueManager = new QueueManager(this.fileComparator);
            this.fileWatcher = new FileWatcher();
            this.fileWatcher.SetBlockCompareFileSizeInMB(10);
            this.fileWatcher.SetBlockSize(4096);
            this.fileWatcher.ProgressUpdate += this.FileWatcherProgressUpdate;
            this.fileWatcher.CaughtException += this.FileWatcherCaughtException;
            this.notifications = new List<string>();

            this.CheckDirectories();

            this.mappings = new List<DirectoryMapping>
                {
                    new DirectoryMapping
                        {
                            SourceDirectory = "A", 
                            DestinationDirectories = new List<string> { "B", "C" }, 
                            Recursive = true
                        }, 
                    new DirectoryMapping
                        {
                            SourceDirectory = "D", 
                            DestinationDirectories = new List<string> { "E", "F" }, 
                            Recursive = false
                        }
                };
            this.fileWatcher.SetSourceDirectories(this.mappings);
        }

        /// <summary>
        /// Test the <see cref="FileWatcher"/>
        /// </summary>
        [TestCase]
        public void TestFileWatcher()
        {
            this.fileWatcher.Init();
            ThreadTestHelper.WaitForCondition(() => this.fileWatcher.IsWorkerInProgress == false, 100000, 1000);
        }

        /// <summary>
        /// Test the <see cref="QueueManager"/>
        /// </summary>
        [TestCase]
        public void TestQueueManager()
        {
            const int fileNumber = 50;

            Random random = new Random();

            this.CreateRandomFiles(fileNumber, "A", random.NextDouble().ToString(CultureInfo.CurrentCulture));

            this.queueManager.DoWork();

            string[] files = Directory.GetFiles("A");

            foreach (string file in files)
            {
                SyncItem item = new SyncItem(new List<string> { "B", "C" }, file, "A" + Path.DirectorySeparatorChar + file, FileAction.COPY);

                Thread.Sleep(random.Next(0, 1000));

                this.queueManager.Enqueue(item);
            }

            ThreadTestHelper.WaitForCondition(() => this.queueManager.IsWorkerInProgress == false, 100000, 1000);

            string[] syncedFiles = Directory.GetFiles("A");

            Assert.NotNull(syncedFiles);
            Assert.True(syncedFiles.Length.Equals(fileNumber));
        }

        /// <summary>
        /// Create random files with content
        /// </summary>
        /// <param name="count">Indicates how many files should be created</param>
        /// <param name="directory">Specifies the directory where the files should be saved</param>
        /// <param name="text">Text which is placed inside the file</param>
        private void CreateRandomFiles(int count, string directory, string text)
        {
            ASCIIEncoding enc = new ASCIIEncoding();
            byte[] content = enc.GetBytes(text);

            for (int i = 0; i < count; i++)
            {
                using (FileStream fs = File.Create(directory + Path.DirectorySeparatorChar + i + ".txt"))
                {
                    fs.Write(content, 0, content.Length);
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="sender">
        /// </param>
        /// <param name="data">
        /// </param>
        private void FileWatcherProgressUpdate(object sender, UpdateProgressEventArgs data)
        {
            string message = "--" + DateTime.Now.ToShortTimeString() + "-- " + data.Message;
            try
            {
                using (StreamWriter streamWriter = new StreamWriter("output.txt", true))
                {
                    streamWriter.WriteLine(message);
                }
            }
            catch (IOException)
            {
                this.notifications.Add(message);
            }

            this.notifications.Add(message);
        }

        /// <summary>
        /// </summary>
        /// <param name="sender">
        /// </param>
        /// <param name="data">
        /// </param>
        private void FileWatcherCaughtException(object sender, CaughtExceptionEventArgs data)
        {
        }

        /// <summary>
        /// Create directories if the don't exist
        /// </summary>
        private void CheckDirectories()
        {
            if (!Directory.Exists("A"))
            {
                Directory.CreateDirectory("A");
            }

            if (!Directory.Exists("D"))
            {
                Directory.CreateDirectory("D");
            }
        }
    }
}