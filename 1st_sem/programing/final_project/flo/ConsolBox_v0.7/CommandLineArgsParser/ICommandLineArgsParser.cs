// *******************************************************
// * <copyright file="ICommandLineArgsParser.cs" company="FGrill">
// * Copyright (c) 2013 Florian Grill. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Florian Grill</author>
// *******************************************************/

namespace ConsoleBoxCommandLineArgsParser
{
    /// <summary>
    /// Interface representing methods of the <see cref="ICommandLineArgsParser"/>
    /// </summary>
    public interface ICommandLineArgsParser
    {
        /// <summary>
        /// Analyze the command line parameter
        /// </summary>
        /// <param name="args">
        /// Specified command line arguments
        /// </param>
        /// <returns>
        /// The result code.
        /// p for one argument.
        /// z for no argument, search config file
        /// in current folder.
        /// ? for erroneous arguments, for example more than one. 
        /// </returns>
        char ParseArgs(string[] args);
    }
}
