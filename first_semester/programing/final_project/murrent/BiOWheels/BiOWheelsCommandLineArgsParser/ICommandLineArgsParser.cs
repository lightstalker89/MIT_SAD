namespace BiOWheelsCommandLineArgsParser
{
    public interface ICommandLineArgsParser
    {
        void Parse(string[] args, string options);
    }
}