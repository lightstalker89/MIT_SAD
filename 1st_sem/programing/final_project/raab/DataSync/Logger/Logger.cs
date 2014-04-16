using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DataSync
{
    public class DataLogger : ILogger
    {
        public enum MessageType 
        { 
            Standard,
            Error,
            Warning,
        }

        public string LogFile { get; private set; }
        public string LogFileBak { get; private set; }
        public int MaxSize { get; private set; }
        public bool PrintToConsole { get; private set; }
        public bool PrintToFile { get; private set; }

        public DataLogger(string logFile, string logFileBak, int maxSize)
        {
            this.LogFile = logFile;
            this.LogFileBak = logFileBak;
            this.MaxSize = maxSize;
            this.PrintToConsole = true;
            this.PrintToFile = true;
        }

        public DataLogger(string logFile, string logFileBak, int maxSize, bool printToConsole, bool printToFile)
        {
            this.LogFile = logFile;
            this.LogFileBak = logFileBak;
            this.MaxSize = maxSize;
            this.PrintToConsole = printToConsole;
            this.PrintToFile = printToFile;
        }

        public void WriteLog(string msg, MessageType msgType)
        {
            string logMsg;
            logMsg = string.Format("{0} - {1} - {2}{3}",
                String.Format("{0:dd.MM.yyyy HH:mm:ss:fff}", DateTime.Now),
                msgType, msg, Environment.NewLine);

            FileInfo fi = new FileInfo(this.LogFile);

            if (fi.Exists)
            {
                if (fi.Length >= this.MaxSize)
                {
                    File.Delete(this.LogFileBak);
                    File.Move(this.LogFile,  this.LogFileBak);
                }
            }

            if(this.PrintToFile)
                File.AppendAllText(this.LogFile, logMsg);

            if(this.PrintToConsole)
                Console.WriteLine(logMsg);
        }
        
    }
}
