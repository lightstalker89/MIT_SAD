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
            logger.SetFileSize<FileLogger>(2);
            logger.Init();

        }

        [TestCase]
        public void LogTest()
        {
            for (int i = 0; i < 50; i++)
            {
                logger.Log("test logging" + i, MessageType.DEBUG);
                //WaitForCondition(() => logger.IsWorkerInProgress == false);
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

        internal delegate bool WaitCondition();
        internal static void WaitForCondition(WaitCondition condition)
        {
            const int TotalTimeout = 60000;
            const int Step = 1000;
            int currentTimeout = 0;
            while (condition() == false && currentTimeout < TotalTimeout)
            {
                Thread.Sleep(Step);
                currentTimeout += Step;
            }

            if (currentTimeout >= TotalTimeout)
            {
                Assert.Fail("Timeout reached.");
            }
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
