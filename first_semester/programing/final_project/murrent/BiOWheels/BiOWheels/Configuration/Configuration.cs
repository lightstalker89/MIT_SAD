using System.Collections.Generic;
using BiOWheelsConfigManager;

namespace BiOWheels.Configuration
{
    public class Configuration
    {
        public Configuration() { }

        public List<DirectoryMappingInfo> DirectoryMappingInfo { get; internal set; }

        public BlockCompareOptions BlockCompareOptions { get; internal set; }

        public long LogFileSize { get; internal set; }

        public bool ParallelSync { get; internal set; }
    }
}
