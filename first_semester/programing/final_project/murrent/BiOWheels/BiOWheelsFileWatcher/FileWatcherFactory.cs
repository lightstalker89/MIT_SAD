using System.Collections.Generic;

namespace BiOWheelsFileWatcher
{
    public class FileWatcherFactory
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

        public static IFileWatcher CreateFileWatcher()
        {
            return new FileWatcher();
        }

        internal static FileComparator CreateFileComparator(long blockSize)
        {
            return new FileComparator(blockSize);
        }

        internal static FileSystemManager CreateFileSystemManager(long blockSize)
        {
            return new FileSystemManager(CreateFileComparator(blockSize));
        }
    }
}
