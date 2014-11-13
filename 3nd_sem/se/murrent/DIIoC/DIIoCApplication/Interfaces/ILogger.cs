// /*
// ******************************************************************
// * Copyright (c) 2014, Mario Murrent
// * All Rights Reserved.
// ******************************************************************
// */

using DIIoCApplication.Models;

namespace DIIoCApplication.Interfaces
{
    public interface ILogger
    {
        void Log(string message, Enums.LogType logType);
    }
}
