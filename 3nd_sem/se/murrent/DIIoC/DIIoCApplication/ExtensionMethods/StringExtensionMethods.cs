// /*
// ******************************************************************
// * Copyright (c) 2014, Mario Murrent
// * All Rights Reserved.
// ******************************************************************
// */

using System;
using DIIoCApplication.Models;

namespace DIIoCApplication.ExtensionMethods
{
    public static class StringExtensionMethods
    {
        public static string ToLogFileFormat(this String str, Enums.LogType logType)
        {
            return string.Format("{0}\t{1}\t{2}", DateTime.Now, logType, str);
        }
    }
}
