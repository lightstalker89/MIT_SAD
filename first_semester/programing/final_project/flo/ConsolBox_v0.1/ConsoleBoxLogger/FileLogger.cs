using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleBoxLogger
{
    public class FileLogger : ILogger
    {

        private readonly string logName = "Log.txt";
        private readonly string logBackupName = "Log.bak";

        private long fileSize;
        public long FileSize
        {
            get { return fileSize; }
            set { fileSize = value; }
        }

        public FileLogger()
        {
            CheckFileSize();
        }

        public void Log(string message, MessageType messageType)
        {
            using (StreamWriter streamWriter = new StreamWriter(logName, true))
            {
                streamWriter.WriteLine(message.ToLogFileString(messageType));
                streamWriter.Flush();
                streamWriter.Close();
            }
            CheckFileSize();
        }

        private void CheckFileSize()
        {
            /*FileInfo fi = new FileInfo(logName);
            double size = fi.Length * Math.Sqrt(1024);
            if (size > fileSize)
            {
                System.IO.File.Move(logName, logBackupName);
            }*/
        }
    }
}
