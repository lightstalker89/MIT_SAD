// *******************************************************
// * <copyright file="StringHelperTest.cs" company="MDMCoWorks">
// * Copyright (c) Mario Murrent. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Mario Murrent</author>
// *******************************************************/
namespace BiOWheelsLogger.Test
{
    using NUnit.Framework;

    /// <summary>
    /// Test class for the <see cref="StringHelper"/> class of the BiOWheelsConfigurationManager module
    /// </summary>
    public class StringHelperTest
    {
        /// <summary>
        /// Test method for testing the ToLogFileString method of the <see cref="StringHelper"/> class
        /// </summary>
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