using System.Collections.Generic;

namespace BiOWheelsFileWatcher
{
    public class FileSystemWatcherFactory
    {
        public static BiOWheelsFileSystemWatcher CreateFileSystemWatcher(string path)
        {
            return new BiOWheelsFileSystemWatcher(path);
        }

        public static BiOWheelsFileSystemWatcher CreateFileSystemWatcher(string path, bool recursive, List<string> destinationDirectories, long blockCompareFileSizeInMB, List<string> excludedDirectories)
        {
            return new BiOWheelsFileSystemWatcher(path)
                   {
                       IncludeSubdirectories = recursive,
                       Destinations = destinationDirectories,
                       BlockCompareFileSizeInMB = blockCompareFileSizeInMB,
                       ExcludedDirectories = excludedDirectories
                   };
        }

        public static FileWatcher CreateFileWatcher()
        {
            return new FileWatcher();
        }
    }
}
