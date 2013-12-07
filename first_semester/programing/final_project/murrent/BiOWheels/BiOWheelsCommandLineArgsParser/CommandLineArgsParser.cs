// *******************************************************
// * <copyright file="CommandLineArgsParser.cs" company="MDMCoWorks">
// * Copyright (c) 2013 Mario Murrent. All rights reserved.
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
    /// Class representing a <see cref="CommandLineArgsParser"/>
    /// </summary>
    public class CommandLineArgsParser : ICommandLineArgsParser
    {
        #region Private Fields

        /// <summary>
        /// Field representing the option index
        /// </summary>
        private int optionIndex;

        /// <summary>
        /// Field representing the next argument
        /// </summary>
        private string nextArgument = string.Empty;

        /// <summary>
        /// Field representing the argument
        /// </summary>
        private string optarg = string.Empty;

        /// <summary>
        /// Field representing the mapping for parameter and value
        /// </summary>
        private readonly Dictionary<string, string> paramValueMapping;

        #endregion

        public CommandLineArgsParser()
        {
            this.paramValueMapping = new Dictionary<string, string>();
        }

        #region Properties

        /// <summary>
        /// Gets the option argument
        /// </summary>
        public string Optarg
        {
            get
            {
                return this.optarg;
            }
        }

        /// <summary>
        /// Gets the option index
        /// </summary>
        public int OptionIndex
        {
            get
            {
                return this.optionIndex;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Parses the command line args
        /// </summary>
        /// <param name="args">
        /// Command line args
        /// </param>
        /// <param name="options">
        /// Accepted command line arguments
        /// </param>
        /// <returns>
        /// The list of command line arguments
        /// </returns>
        public IList<char> Parse(string[] args, string options)
        {
            IList<char> includedArgs = new List<char>();

            char c;
            while ((c = this.Getopt(args.Length, args, options)) != '\0')
            {
                includedArgs.Add(c);
            }

            return includedArgs;
        }

        /// <summary>
        /// Gets the value for parameter.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>The value as string</returns>
        public string GetValueForParameter(string key)
        {
            if(this.paramValueMapping.ContainsKey(key))
            {
                return this.paramValueMapping[key];
            }

            return string.Empty;
        }

        /// <summary>
        /// Get an valid option
        /// </summary>
        /// <param name="argc">
        /// Count of the command line arguments
        /// </param>
        /// <param name="argv">
        /// All arguments coming from the command line
        /// </param>
        /// <param name="options">
        /// All allowed command line arguments a string
        /// </param>
        /// <returns>
        /// The valid command line argument as char
        /// </returns>
        protected char Getopt(int argc, string[] argv, string options)
        {
            this.optarg = string.Empty;

            if (argc < 0)
            {
                return '?';
            }

            if (this.optionIndex == 0)
            {
                this.nextArgument = string.Empty;
            }

            if (this.nextArgument.Length == 0)
            {
                if (this.optionIndex >= argc || argv[this.optionIndex][0] != '-' || argv[this.optionIndex].Length < 2)
                {
                    // no more options
                    this.optarg = string.Empty;
                    if (this.optionIndex < argc)
                    {
                        this.optarg = argv[this.optionIndex]; // return leftover arg
                    }

                    return '\0';
                }

                if (argv[this.optionIndex] == "--")
                {
                    // 'end of options' flag
                    this.optionIndex++;
                    this.optarg = string.Empty;
                    if (this.optionIndex < argc)
                    {
                        this.optarg = argv[this.optionIndex];
                    }

                    return '\0';
                }

                this.nextArgument = string.Empty;
                if (this.optionIndex < argc)
                {
                    this.nextArgument = argv[this.optionIndex];
                    this.nextArgument = this.nextArgument.Substring(1); // skip past -
                }

                this.optionIndex++;
            }

            char c = this.nextArgument[0]; // get option char
            this.nextArgument = this.nextArgument.Substring(1); // skip past option char
            int index = options.IndexOf(c); // check if this is valid option char

            if (index == -1 || c == ':')
            {
                return '?';
            }

            index++;
            if ((index < options.Length) && (options[index] == ':'))
            {
                // option takes an arg
                if (this.nextArgument.Length > 0)
                {
                    this.optarg = this.nextArgument;
                    this.nextArgument = string.Empty;
                }
                else if (this.optionIndex < argc)
                {
                    this.optarg = argv[this.optionIndex];
                    this.optionIndex++;
                }
                else
                {
                    return '?';
                }
            }

            return c;
        }

        #endregion
    }
}