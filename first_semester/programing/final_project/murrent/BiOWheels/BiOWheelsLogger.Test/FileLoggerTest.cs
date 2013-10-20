using System;
using System.IO;
using NUnit.Framework;

namespace BiOWheelsLogger.Test
{
    using System.Threading;

    [TestFixture]
    public class FileLoggerTest
    {
        private FileLogger logger;

        [SetUp]
        public void Init()
        {
            logger = new FileLogger();
            logger.SetIsEnabled<FileLogger>(true);
            logger.SetFileSize<FileLogger>(1);
            logger.Init();
        }

        [TestCase]
        public void LogTest()
        {
            for (int i = 0; i < 15000; i++)
            {
                logger.Log("test logging" + i, MessageType.DEBUG);
            }
        }

        [TestCase]
        public void StringHelperTest()
        {
            const string Message = "test logging";
            string logMessage = Message.ToLogFileString(MessageType.DEBUG);

            Assert.IsNotNullOrEmpty(logMessage);
            StringAssert.Contains("[DEBUG] - test logging", logMessage);
        }
    }
}
