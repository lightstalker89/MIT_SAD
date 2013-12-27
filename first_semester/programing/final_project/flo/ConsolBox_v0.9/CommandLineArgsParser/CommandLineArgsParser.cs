// *******************************************************
// * <copyright file="CommandLineArgsParser.cs" company="FGrill">
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
    /// Represents the Command <see cref="CommandLineArgsParser"/> which checks the argument parameters.
    /// </summary>
    public class CommandLineArgsParser : ICommandLineArgsParser
    {
        /// <inheritdoc/>
        public char ParseArgs(string[] args)
        {
            switch (args.Length)
            {
                case 0:
                    return 'z';
                case 1:
                    return 'p';
                default:    
                    return '?';
            }
        }
    }
}
