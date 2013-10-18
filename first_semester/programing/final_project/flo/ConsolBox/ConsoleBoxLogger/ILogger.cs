using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleBoxLogger
{
    public interface ILogger
    {
        long FileSize { get; set; }

        void Log(string message, MessageType messageType);
    }
}
