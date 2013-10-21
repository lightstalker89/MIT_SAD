using System.Collections.Generic;

namespace BiOWheelsCommandLineArgsParser
{
    /// <summary>
    /// 
    /// </summary>
    public class CommandLineArgsParser : ICommandLineArgsParser
    {
        #region Private Fields
        /// <summary>
        /// 
        /// </summary>
		private int optind;

        /// <summary>
        /// 
        /// </summary>
		private string nextarg = string.Empty;

        /// <summary>
        /// 
        /// </summary>
		private string optarg = string.Empty;
        #endregion

		#region Properties
		public string Optarg
		{
			get
			{
				return optarg;
			}
		}

		public int Optind
		{
			get
			{
				return optind;
			}
		}
		#endregion

		#region Methods
        /// <summary>
        /// Parses the commandline args
        /// </summary>
        /// <param name="args">commandline args</param>
        /// <param name="options">accepted commandline arguments</param>
        public IList<char> Parse(string[] args, string options)
        {
            IList<char> includedArgs = new List<char>();

            char c;
            while ((c = Getopt(args.Length, args, options)) != '\0')
            {
                includedArgs.Add(c);
            }

            return includedArgs;
        }

        /// <summary>
        /// Get an valid option
        /// </summary>
        /// <param name="argc"></param>
        /// <param name="argv"></param>
        /// <param name="options"></param>
        /// <returns></returns>
		protected char Getopt(int argc, string[] argv, string options)
		{
			optarg = string.Empty;

			if (argc < 0)
				return '?';

			if (optind == 0)
				nextarg = string.Empty;

			if (nextarg.Length == 0)
			{
				if (optind >= argc || argv[optind][0] != '-' || argv[optind].Length < 2)
				{
					// no more options
					optarg = string.Empty;
					if (optind < argc)
						optarg = argv[optind];	// return leftover arg
					return '\0';
				}

				if (argv[optind] == "--")
				{
					// 'end of options' flag
					optind++;
					optarg = string.Empty;
					if (optind < argc)
						optarg = argv[optind];
					return '\0';
				}

				nextarg = string.Empty;
				if (optind < argc)
				{
					nextarg = argv[optind];
					nextarg = nextarg.Substring(1);		// skip past -
				}
				optind++;
			}

			char c = nextarg[0];				// get option char
			nextarg = nextarg.Substring(1);		// skip past option char
            int index = options.IndexOf(c);	// check if this is valid option char

			if (index == -1 || c == ':')
				return '?';

			index++;
            if ((index < options.Length) && (options[index] == ':'))
			{
				// option takes an arg
				if (nextarg.Length > 0)
				{
					optarg = nextarg;
					nextarg = string.Empty;
				}
				else if (optind < argc)
				{
					optarg = argv[optind];
					optind++;
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
