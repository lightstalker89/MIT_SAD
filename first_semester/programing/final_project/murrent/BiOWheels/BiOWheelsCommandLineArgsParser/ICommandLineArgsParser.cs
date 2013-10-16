using System.Collections.Generic;

namespace BiOWheelsCommandLineArgsParser
{
    public interface ICommandLineArgsParser
    {
        IList<char> Parse(string[] args, string options);
    }
}