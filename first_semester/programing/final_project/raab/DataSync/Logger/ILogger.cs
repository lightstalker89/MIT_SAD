using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataSync
{
    public interface ILogger
    {
        void WriteLog(string msg, DataLogger.MessageType msgType);
    }
}
