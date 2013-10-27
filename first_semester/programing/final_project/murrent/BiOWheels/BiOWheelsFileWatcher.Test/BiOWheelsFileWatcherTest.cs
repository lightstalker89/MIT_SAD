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

    using NUnit.Framework;

    /// <summary>
    /// Class representing the test for the <see cref="FileWatcher"/>
    /// </summary>
    [TestFixture]
    public class BiOWheelsFileWatcherTest
    {
        /// <summary>
        /// </summary>
        private IFileWatcher fileWatcher;

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
            this.mappings = new List<DirectoryMapping> { };

        }

        /// <summary>
        /// Test the <see cref="FileWatcher"/>
        /// </summary>
        [TestCase]
        public void TestFileWatcher()
        {
            this.fileWatcher.SetSourceDirectories(this.mappings);
            this.fileWatcher.Init();
            //ThreadTestHelper.WaitForCondition(() => this.fileWatcher. == false, 30000, 1000);

        }
    }
}