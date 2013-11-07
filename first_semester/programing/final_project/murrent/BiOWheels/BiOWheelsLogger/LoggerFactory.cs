using System;

namespace BiOWheelsLogger
{
    public class LoggerFactory
    {
        public static ILogger CreateConsoleLogger()
        {
            return new ConsoleLogger();
        }

        public static ILogger CreateFileLogger()
        {
            return new FileLogger();
        }

        public static ILogger CreateCombinedLogger(ILogger consoleLogger, ILogger fileLogger)
        {
            return new CombinedLogger(consoleLogger, fileLogger);
        }

        public static ILogger CreateCombinedLogger()
        {
            return new CombinedLogger(CreateConsoleLogger(), CreateFileLogger());
        }
    }
}
