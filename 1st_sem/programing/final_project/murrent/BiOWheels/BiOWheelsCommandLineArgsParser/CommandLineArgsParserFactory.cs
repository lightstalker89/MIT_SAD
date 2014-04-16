// *******************************************************
// * <copyright file="CommandLineArgsParserFactory.cs" company="MDMCoWorks">
// * Copyright (c) 2013 Mario Murrent. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Mario Murrent</author>
// *******************************************************/
namespace BiOWheelsCommandLineArgsParser
{
    /// <summary>
    ///  Class representing the <see cref="CommandLineArgsParserFactory"/>
    /// </summary>
    public class CommandLineArgsParserFactory
    {
        /// <summary>
        /// Creates the command line arguments parser.
        /// </summary>
        /// <returns>An instance of the <see cref="CommandLineArgsParser"/> class</returns>
        public static ICommandLineArgsParser CreateCommandLineArgsParser()
        {
            return new CommandLineArgsParser();
        }
    }
}