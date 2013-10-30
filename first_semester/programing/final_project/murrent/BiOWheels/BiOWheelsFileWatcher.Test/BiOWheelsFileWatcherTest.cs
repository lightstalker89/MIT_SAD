// *******************************************************
// * <copyright file="BiOWheelsFileWatcherTest.cs" company="MDMCoWorks">
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
    public class BiOWheelsFileWatcherTest
    {
        /// <summary>
        /// </summary>
        private IQueueManager queueManager;

        /// <summary>
        /// </summary>
        private FileWatcher fileWatcher;

        /// <summary>
        /// </summary>
        private List<DirectoryMapping> mappings;

        /// <summary>
        /// Set up test environment
        /// </summary>
        [SetUp]
        public void Init()
        {
            this.queueManager = new QueueManager();
            this.fileWatcher = new FileWatcher();
            this.fileWatcher.SetBlockCompareFileSizeInMB(10);
            this.fileWatcher.ProgressUpdate += this.FileWatcherProgressUpdate;
            this.fileWatcher.CaughtException += this.FileWatcherCaughtException;

            this.CheckDirectories();

            this.mappings = new List<DirectoryMapping>
                {
                    new DirectoryMapping
                        {
                            SorceDirectory = "A", 
                            DestinationDirectories = new List<string> { "B", "C" }, 
                            Recursive = true
                        }, 
                    new DirectoryMapping
                        {
                            SorceDirectory = "D", 
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