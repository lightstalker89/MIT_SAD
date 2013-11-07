namespace BiOWheelsLogger
{
    public class LoggerFactory
    {
        public static ConsoleLogger CreateConsoleLogger()
        {
            return new ConsoleLogger();
        }

        public static FileLogger CreateFileLogger()
        {
            return new FileLogger();
        }

        public static CombinedLogger CreateCombinedLogger(ILogger consoleLogger, ILogger fileLogger)
        {
            return new CombinedLogger(consoleLogger, fileLogger);
        }

        public static CombinedLogger CreateCombinedLogger()
        {
            return new CombinedLogger(CreateConsoleLogger(), CreateFileLogger());
        }
    }
}
