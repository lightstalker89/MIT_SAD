using System.Collections.Generic;
using BiOWheelsFileWatcher.Interfaces;

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

        public static IFileWatcher CreateFileWatcher(IQueueManager queueManager)
        {
            return new FileWatcher(queueManager);
        }

        public static IFileComparator CreateFileComparator(long blockSize)
        {
            return new FileComparator(blockSize);
        }

        public static IFileSystemManager CreateFileSystemManager(IFileComparator fileComparator)
        {
            return new FileSystemManager(fileComparator);
        }

        public static IQueueManager CreateQueueManager(IFileSystemManager fileSystemManager)
        {
            return new QueueManager(fileSystemManager);
        }
    }
}
