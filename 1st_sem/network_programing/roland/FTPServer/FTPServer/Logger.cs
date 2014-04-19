using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTPServer
{
    class Logger
    {
        public void Log(string message)
        {
            string msg = string.Format("{0} {1}: {2}", DateTime.Now.ToShortDateString(), DateTime.Now.ToShortTimeString(), message);
            Console.WriteLine(msg);
        }
    }
}
