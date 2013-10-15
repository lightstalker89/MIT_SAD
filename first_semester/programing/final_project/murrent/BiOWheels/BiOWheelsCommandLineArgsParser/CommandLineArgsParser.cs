namespace BiOWheelsCommandLineArgsParser
{
    public class CommandLineArgsParser : ICommandLineArgsParser
    {
        #region Private Fields
		private int optind;
		private string nextarg = string.Empty;
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
		public char Getopt(int argc, string[] argv, string optstring)
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
			int index = optstring.IndexOf(c);	// check if this is valid option char

			if (index == -1 || c == ':')
				return '?';

			index++;
			if ((index < optstring.Length) && (optstring[index] == ':'))
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
