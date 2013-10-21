using System.Collections.Generic;

namespace BiOWheels.Configuration
{
    public class DirectoryMappingInfo
    {
        public List<SourceMappingInfo> SourceMappingInfos { get; set; }

        public List<string> DestinationDirectories { get; set; }

        public List<string> ExcludedFromSource { get; set; }
    }
}
