using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DataSync
{
    public class Source
    {
        public string Path { get; private set; }
        public List<string> DestinationPaths { get; set; }
        public List<string> ExcludedDestinationPaths { get; set; }
        public bool IncludeSubdirectories { get; private set; }

        public Source(string path, List<string> destinationPaths, List<string> excludedDestinationPaths)
        {
            this.Path = path;
            this.DestinationPaths = destinationPaths;
            this.ExcludedDestinationPaths = excludedDestinationPaths;
            this.IncludeSubdirectories = true;
        }
        public Source(string path, List<string> destinationPaths, List<string> excludedDestinationPaths, bool includeSubdirectories)
        {
            this.Path = path;
            this.DestinationPaths = destinationPaths;
            this.ExcludedDestinationPaths = excludedDestinationPaths;
            this.IncludeSubdirectories = includeSubdirectories;
        }

        public Source(Source source)
        {
            this.Path = source.Path;
            this.DestinationPaths = source.DestinationPaths;
            this.ExcludedDestinationPaths = source.ExcludedDestinationPaths;
            this.IncludeSubdirectories = source.IncludeSubdirectories;
        }
    }

    public class SourceWatcher  : Source
    {
        public FileSystemWatcher FileSystemWatcher { get; set; }

        public SourceWatcher(string path, List<string> destinationPaths, List<string> excludedDestinationPaths, bool includeSubdirectories)
            : base(path, destinationPaths, excludedDestinationPaths, includeSubdirectories)
        {
        }

        public SourceWatcher(Source source)
            : base(source)
        {
        }
    }
}
