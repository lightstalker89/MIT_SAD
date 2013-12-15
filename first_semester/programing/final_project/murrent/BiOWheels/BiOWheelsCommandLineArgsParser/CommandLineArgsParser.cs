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
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    /// <summary>
    /// Class representing a <see cref="CommandLineArgsParser"/>
    /// </summary>
    public class CommandLineArgsParser : ICommandLineArgsParser
    {
        #region Private Fields

        /// <summary>
        /// Field representing the mapping for parameter and value
        /// </summary>
        private readonly Dictionary<string, string> paramValueMapping;

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

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandLineArgsParser"/> class.
        /// </summary>
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

        /// <inheritdoc/>
        public IList<char> Parse(string[] args, string options)
        {
            List<char> includedArgs = new List<char>();

            char c;
            while ((c = this.Getopt(args.Length, args, options)) != '\0')
            {
                includedArgs.Add(c);

                if (c.Equals('f'))
                {
                    int index = Array.IndexOf(args, "-f");
                    index++;

                    if (index < args.Length)
                    {
                        this.paramValueMapping.Add(c.ToString(CultureInfo.CurrentCulture), args[index]);
                    }
                }
            }

            if (includedArgs.Contains('h'))
            {
                includedArgs.RemoveAll(p => p != 'h');
            }

            return includedArgs;
        }

        /// <inheritdoc/>
        public string GetValueForParameter(string key)
        {
            if (this.paramValueMapping.ContainsKey(key))
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