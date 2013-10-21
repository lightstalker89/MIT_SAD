using System.Collections.Generic;

namespace BiOWheels.Configuration
{
    public class Configuration
    {
        public Configuration() { }

        public List<DirectoryMappingInfo> DirectoryMappingInfo { get; set; }

        public BlockCompareOptions BlockCompareOptions { get; set; }

        public LogFileOptions LogFileOptions { get; set; }

        public bool ParallelSync { get; set; }
    }
}
