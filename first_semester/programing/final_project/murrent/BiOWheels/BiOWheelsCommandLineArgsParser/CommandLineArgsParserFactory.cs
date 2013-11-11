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
    /// </summary>
    public class CommandLineArgsParserFactory
    {
        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public static ICommandLineArgsParser CreateCommandLineArgsParser()
        {
            return new CommandLineArgsParser();
        }
    }
}