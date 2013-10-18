using System;
using System.IO;
using NUnit.Framework;

namespace BiOWheelsLogger.Test
{
    [TestFixture]
    public class FileLoggerTest
    {
        private FileLogger logger;

        [SetUp]
        public void Init()
        {
            logger = new FileLogger();
            logger.Init();
            logger.SetIsEnabled<FileLogger>(true);
            logger.SetFileSize<FileLogger>(2);
        }

        [TestCase]
        public void LogTest()
        {
            for (int i = 0; i < 50; i++)
            {
                logger.Log("test logging" + i, MessageType.DEBUG);
            }
        }

        [TestCase]
        public void StringHelperTest()
        {
            const string message = "test logging";
            string logMessage = message.ToLogFileString(MessageType.DEBUG);

            Assert.IsNotNullOrEmpty(logMessage);
            StringAssert.Contains("[DEBUG] - test logging", logMessage);
        }

        ///// <summary>
        ///// Not testing any class method, but method for calculating the file size
        ///// </summary>
        //[TestCase]
        //public void TestStreamLength()
        //{
        //    Stream stream = new FileStream("nunit.framework.xml",FileMode.OpenOrCreate);
        //    double length = Math.Round(((stream.Length/1024f)/1024f),5, MidpointRounding.ToEven);

        //    Assert.NotNull(length);
        //    Assert.Less(length, 0.600);
        //}
    }
}
