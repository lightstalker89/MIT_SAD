namespace BiOWheelsCommandLineArgsParser
{
    public class CommandLineArgsParserFactory
    {
        public static ICommandLineArgsParser CreateCommandLineArgsParser()
        {
            return new CommandLineArgsParser();
        }
    }
}
