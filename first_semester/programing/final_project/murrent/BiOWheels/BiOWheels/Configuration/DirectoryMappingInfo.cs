using System.Collections.Generic;

namespace BiOWheels.Configuration
{
    public class DirectoryMappingInfo
    {
        public List<string> SourceDirectories { get; internal set; }

        public List<string> DestinationDirectory { get; internal set; }

        public List<string> ExcludedFromSource { get; internal set; }

        public bool Recursive { get; internal set; }
    }
}
