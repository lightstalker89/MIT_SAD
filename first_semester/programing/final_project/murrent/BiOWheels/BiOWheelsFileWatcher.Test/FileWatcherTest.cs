// *******************************************************
// * <copyright file="FileWatcherTest.cs" company="MDMCoWorks">
// * Copyright (c) Mario Murrent. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Mario Murrent</author>
// *******************************************************/
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
        private IQueueManager queueManager;

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
        /// Represents the test folder used for specific test methods
        /// </summary>
        private string testFolder;

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

            this.testFolder = @"C:\Users\Mario Murrent\Pictures";

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
            ThreadTestHelper.WaitForCondition(() => this.fileWatcher.IsWorkerInProgress == false, 60000, 1000);
        }

        [TestCase]
        public void TestGetAllFiles()
        {
            IEnumerable<string> fileList = this.fileWatcher.GetFilesForDirectory(testFolder);

            Assert.NotNull(fileList);
            Assert.IsNotEmpty(fileList);
        }

        /// <summary>
        /// </summary>
        /// <param name="sender">
        /// </param>
        /// <param name="data">
        /// </param>
        private void FileWatcherProgressUpdate(object sender, UpdateProgressEventArgs data)
        {
            using (StreamWriter streamWriter = new StreamWriter("output.txt", true))
            {
                streamWriter.WriteLine("--" + DateTime.Now.ToShortTimeString() + "-- " + data.Message);
            }
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