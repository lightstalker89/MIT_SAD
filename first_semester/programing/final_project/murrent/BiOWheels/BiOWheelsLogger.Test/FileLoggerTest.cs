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
            logger.SetIsEnabled<FileLogger>(true);
            logger.SetFileSize<FileLogger>(1);
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

            foreach(string file in Directory.GetFiles("log"))
            {
                Stream actualFileStream = new FileStream(file, FileMode.Open);
                double length = Math.Round(
                    (actualFileStream.Length / 1024f) / 1024f, 5, MidpointRounding.AwayFromZero);

                Assert.IsTrue(length <= 1.001);
            }
        }
    }
}
