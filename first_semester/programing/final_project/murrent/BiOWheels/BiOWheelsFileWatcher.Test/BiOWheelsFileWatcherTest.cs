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
    using System.Collections.Generic;

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
            this.fileWatcher = new FileWatcher();
            this.mappings = new List<DirectoryMapping>
                {
                    new DirectoryMapping
                        {
                            SorceDirectory = "A", 
                            DestinationDirectories = new List<string> { "B", "C" }, 
                            Recursive = true
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
    }
}