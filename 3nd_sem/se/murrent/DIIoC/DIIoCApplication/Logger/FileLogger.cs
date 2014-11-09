using System.IO;
using DIIoCApplication.ExtensionMethods;
using DIIoCApplication.Interfaces;

namespace DIIoCApplication.Logger
{
    public class FileLogger : ILogger
    {
        private readonly string fileName = "logFile.log";

        public FileLogger()
        {
           // this.CheckIfFileExists();
        }

        public void Log(string message, Models.Enums.LogType logType)
        {
            using (StreamWriter str = new StreamWriter(fileName, true))
            {
                str.WriteLine(message.ToLogFileFormat(logType));
            }
        }

        private void CheckIfFileExists()
        {
            if (!File.Exists(fileName))
            {
                File.Create(fileName);
            }

        }
    }
}
