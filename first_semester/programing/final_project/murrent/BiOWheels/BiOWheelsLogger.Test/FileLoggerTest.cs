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
            for (int i = 0; i < 20; i++)
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

        [TestCase]
        public void TestStreamLength()
        {
            Stream stream = new FileStream("BiOWheelsLogger.dll",FileMode.OpenOrCreate);
            double length = stream.Length/1024/1024f;
            Assert.NotNull(length);

        }
    }
}
