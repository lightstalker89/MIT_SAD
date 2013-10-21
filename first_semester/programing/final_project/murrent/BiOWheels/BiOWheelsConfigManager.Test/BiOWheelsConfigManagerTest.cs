using System.Collections.Generic;
using System.IO;
using BiOWheels.Configuration;
using NUnit.Framework;

namespace BiOWheelsConfigManager.Test
{
    [TestFixture]
    public class BiOWheelsConfigManagerTest
    {
        private IConfigurationManager configurationManager;
        private Configuration config;

        [SetUp]
        public void Init()
        {
            configurationManager = new ConfigurationManager();

            config = new Configuration
            {
                BlockCompareOptions =
                    new BlockCompareOptions
                    {
                        BlockCompareFileSizeInMB = 20,
                        BlockSizeInKB = 2048
                    },
                LogFileOptions = new LogFileOptions
                {
                    LogFileFolder = "log",
                    LogFileSizeInMB = 1
                },
                ParallelSync = false,
                DirectoryMappingInfo = new List<DirectoryMappingInfo>
                                                              {
                                                                 new DirectoryMappingInfo
                                                                 {
                                                                     DestinationDirectories = new List<string>{"A","B"},
                                                                     ExcludedFromSource = new List<string>(), 
                                                                     SourceMappingInfos = new List<SourceMappingInfo>
                                                                                          {
                                                                                              new SourceMappingInfo{Recursive = true, SourceDirectory = "C"},
                                                                                              new SourceMappingInfo{Recursive = false, SourceDirectory = "D"}
                                                                                          }
                                                                 }
                                                              }
            };
        }

        [TestCase]
        public void TestLoadWithWrongValue()
        {

        }

        [TestCase]
        public void TestWrite()
        {
            configurationManager.Write("config.xml", config);

            FileStream fs = File.Open("config.xml", FileMode.Open);

            Assert.NotNull(fs);

            fs.Close();
        }

        [TestCase]
        public void TestLoad()
        {
            Configuration loadedConfiguration = configurationManager.Load<Configuration>("config.xml");

            Assert.NotNull(config);
            Assert.AreEqual(loadedConfiguration.SerialilzeToXML(), config.SerialilzeToXML());
        }
    }
}
