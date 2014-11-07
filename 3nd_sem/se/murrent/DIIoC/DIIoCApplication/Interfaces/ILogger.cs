using DIIoCApplication.Models;

namespace DIIoCApplication.Interfaces
{
    public interface ILogger
    {
        void Log(string message, Enums.LogType logType);
    }
}
