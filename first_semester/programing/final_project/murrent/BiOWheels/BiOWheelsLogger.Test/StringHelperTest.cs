using NUnit.Framework;

namespace BiOWheelsLogger.Test
{
    public class StringHelperTest
    {
        [TestCase]
        public void StringHelperMethodTest()
        {
            const string Message = "test logging";
            string logMessage = Message.ToLogFileString(MessageType.DEBUG);

            Assert.IsNotNullOrEmpty(logMessage);
            StringAssert.Contains("[DEBUG] - test logging", logMessage);
        }
    }
}
