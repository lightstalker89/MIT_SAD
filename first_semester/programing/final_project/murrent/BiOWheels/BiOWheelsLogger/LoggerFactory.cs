// *******************************************************
// * <copyright file="LoggerFactory.cs" company="MDMCoWorks">
// * Copyright (c) 2013 Mario Murrent. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Mario Murrent</author>
// *******************************************************/
namespace BiOWheelsLogger
{
    /// <summary>
    /// </summary>
    public class LoggerFactory
    {
        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public static ILogger CreateConsoleLogger()
        {
            return new ConsoleLogger();
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public static ILogger CreateFileLogger()
        {
            return new FileLogger();
        }

        /// <summary>
        /// </summary>
        /// <param name="consoleLogger">
        /// </param>
        /// <param name="fileLogger">
        /// </param>
        /// <returns>
        /// </returns>
        public static ILogger CreateCombinedLogger(ILogger consoleLogger, ILogger fileLogger)
        {
            return new CombinedLogger(consoleLogger, fileLogger);
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public static ILogger CreateCombinedLogger()
        {
            return new CombinedLogger(CreateConsoleLogger(), CreateFileLogger());
        }
    }
}