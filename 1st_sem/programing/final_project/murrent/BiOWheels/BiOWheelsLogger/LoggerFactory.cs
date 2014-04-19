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
    ///  Class representing the <see cref="LoggerFactory"/>
    /// </summary>
    public class LoggerFactory
    {
        /// <summary>
        /// Creates the console logger.
        /// </summary>
        /// <returns>An instance of the <see cref="ConsoleLogger"/> class</returns>
        public static ILogger CreateConsoleLogger()
        {
            return new ConsoleLogger();
        }

        /// <summary>
        /// Creates the file logger.
        /// </summary>
        /// <returns>An instance of the <see cref="FileLogger"/> class</returns>
        public static ILogger CreateFileLogger()
        {
            return new FileLogger();
        }

        /// <summary>
        /// Creates the combined logger.
        /// </summary>
        /// <param name="consoleLogger">
        /// The console logger.
        /// </param>
        /// <param name="fileLogger">
        /// The file logger.
        /// </param>
        /// <returns>
        /// An instance of the <see cref="CombinedLogger"/> class
        /// </returns>
        public static ILogger CreateCombinedLogger(ILogger consoleLogger, ILogger fileLogger)
        {
            return new CombinedLogger(consoleLogger, fileLogger);
        }

        /// <summary>
        /// Creates the combined logger.
        /// </summary>
        /// <returns>An instance of the <see cref="CombinedLogger"/> class</returns>
        public static ILogger CreateCombinedLogger()
        {
            return new CombinedLogger(CreateConsoleLogger(), CreateFileLogger());
        }
    }
}