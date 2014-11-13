// /*
// ******************************************************************
// * Copyright (c) 2014, Mario Murrent
// * All Rights Reserved.
// ******************************************************************
// */

using System;
using DIIoCApplication.ExtensionMethods;
using DIIoCApplication.Interfaces;
using DIIoCApplication.Models;

namespace DIIoCApplication.Logger
{
    public class ConsoleLogger : ILogger
    {
        public void Log(string message, Enums.LogType logType)
        {
            Console.WriteLine(message.ToLogFileFormat(logType));
        }
    }
}
