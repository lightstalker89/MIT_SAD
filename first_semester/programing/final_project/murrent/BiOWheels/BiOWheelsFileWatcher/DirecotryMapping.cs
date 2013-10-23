
using System.Collections.Generic;

namespace BiOWheelsFileWatcher
{
    public class DirecotryMapping
    {
        public bool Recursive { get; set; }

        public string SorceDirectory { get; set; }

        public List<string> DestinationDirectories { get; set; }
    }
}
