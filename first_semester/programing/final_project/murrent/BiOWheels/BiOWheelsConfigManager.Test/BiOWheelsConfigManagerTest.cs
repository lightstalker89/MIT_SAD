// *******************************************************
// * <copyright file="BiOWheelsConfigManagerTest.cs" company="MDMCoWorks">
// * Copyright (c) 2013 Mario Murrent. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Mario Murrent</author>
// *******************************************************/
namespace BiOWheelsConfigManager.Test
{
    using System.Collections.Generic;
    using System.IO;

    using BiOWheels.BiOWheelsConfiguration;

    using NUnit.Framework;

    /// <summary>
    /// The test class for the  <see cref="ConfigurationManager"/> class
    /// </summary>
    [TestFixture]
    public class BiOWheelsConfigManagerTest
    {
        /// <summary>
        /// The configuration  manager
        /// </summary>
        private IConfigurationManager configurationManager;

        /// <summary>
        /// The sample configuration
        /// </summary>
        private Configuration config;

        /// <summary>
        /// Set up test environment
        /// </summary>
        [SetUp]
        public void Init()
        {
            this.configurationManager = ConfigurationManagerFactory.CreateConfigurationManager();

            this.config = new Configuration
                {
                    BlockCompareOptions = new BlockCompareOptions { BlockCompareFileSizeInMB = 20, BlockSizeInKB = 2048 }, 
                    LogFileOptions = new LogFileOptions { LogFileFolder = "log", LogFileSizeInMB = 1 }, 
                    ParallelSync = false, 
                    DirectoryMappingInfo =
                        new List<DirectoryMappingInfo>
                            {
                                new DirectoryMappingInfo
                                    {
                                        DestinationDirectories =
                                            new List<string>
                                                {
                                                    @"C:\Users\Mario\Documents\GitHub\MIT_SAD\first_semester\programing\final_project\murrent\BiOWheels\BiOWheels\bin\Debug\A", 
                                                    @"C:\Users\Mario\Documents\GitHub\MIT_SAD\first_semester\programing\final_project\murrent\BiOWheels\BiOWheels\bin\Debug\B"
                                                }, 
                                        ExcludedFromSource =
                                            new List<string>
                                                {
                                                    @"C:\Users\Mario\Documents\GitHub\MIT_SAD\first_semester\programing\final_project\murrent\BiOWheels\BiOWheels\bin\Debug\S"
                                                }, 
                                        SourceMappingInfo =
                                            new SourceMappingInfo
                                                {
                                                    Recursive = false, 
                                                    SourceDirectory =
                                                        @"C:\Users\Mario\Documents\GitHub\MIT_SAD\first_semester\programing\final_project\murrent\BiOWheels\BiOWheels\bin\Debug\D"
                                                }
                                    }
                            }
                };
        }

        /// <summary>
        ///  Test method for testing the write method of the <see cref="ConfigurationManager"/> class with invalid parameter
        /// </summary>
        [TestCase]
        public void TestLoadWithWrongValue()
        {
        }

        /// <summary>
        /// Test method for testing the write method of the <see cref="ConfigurationManager"/> class
        /// </summary>
        [TestCase]
        public void TestWrite()
        {
            this.configurationManager.Write("BiOWheelsConfig.xml", this.config);

            FileStream fs = File.Open("BiOWheelsConfig.xml", FileMode.Open);

            Assert.NotNull(fs);

            fs.Close();
        }

        /// <summary>
        /// Test method for testing the load method of the <see cref="ConfigurationManager"/> class
        /// </summary>
        [TestCase]
        public void TestLoad()
        {
            object loadedConfiguration = this.configurationManager.Load<Configuration>("BiOWheelsConfig.xml");

            Assert.NotNull(this.config);

            if (loadedConfiguration.GetType() == typeof(Configuration))
            {
                Assert.AreEqual(loadedConfiguration.SerialilzeToXML(), this.config.SerialilzeToXML());
            }
        }
    }
}