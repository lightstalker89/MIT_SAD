using System;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace BiOWheelsLogger.Test
{
    [TestFixture]
    public class FileLoggerTest
    {
        private FileLogger logger;
        private const double FileSize = 0.5;

        [SetUp]
        public void Init()
        {
            logger = new FileLogger();
            logger.SetIsEnabled<FileLogger>(true);
            logger.SetFileSize<FileLogger>(FileSize);
            logger.Init();
        }

        [TestCase]
        public void LogTest()
        {
            for (int i = 0; i < 25000; i++)
            {
                logger.Log("test logging" + i, MessageType.DEBUG);
            }

            Assert.IsTrue(Directory.Exists("log"));
            Assert.IsNotEmpty(Directory.GetFiles("log"));

            string[] files = Directory.GetFiles("log");

            for (int i = 0; i < files.Count(); i++)
            {
                Stream actualFileStream = new FileStream(files[i], FileMode.Open);
                double length = Math.Round(
                    (actualFileStream.Length / 1024f) / 1024f, 1, MidpointRounding.AwayFromZero);

                Assert.IsTrue(length <= FileSize);
            }
        }
    }
}
