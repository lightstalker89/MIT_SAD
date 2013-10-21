// *******************************************************
// * <copyright file="ICommandLineArgsParser.cs" company="MDMCoWorks">
// * Copyright (c) Mario Murrent. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Mario Murrent</author>
// *******************************************************/
namespace BiOWheelsCommandLineArgsParser
{
    using System.Collections.Generic;

    /// <summary>
    /// Interface for the <see cref="CommandLineArgsParser"/> class
    /// </summary>
    public interface ICommandLineArgsParser
    {
        /// <summary>
        /// Parse the array of command line arguments
        /// </summary>
        /// <param name="args">
        /// The command line arguments
        /// </param>
        /// <param name="options">
        /// The supported command line parameter as string
        /// </param>
        /// <returns>
        /// A list of given command line parameter entered by the user
        /// </returns>
        IList<char> Parse(string[] args, string options);
    }
}