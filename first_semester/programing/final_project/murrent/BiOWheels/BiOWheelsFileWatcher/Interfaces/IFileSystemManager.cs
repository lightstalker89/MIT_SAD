namespace BiOWheelsFileWatcher.Interfaces
{
    public interface IFileSystemManager
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        void Delete(SyncItem item);

        /// <summary>
        /// Copies a file to the given destinations
        /// </summary>
        /// <param name="item">
        /// Item from the queue
        /// </param>
        void Copy(SyncItem item);

        /// <summary>
        /// Compare files from a destination with files in all given destinations
        /// </summary>
        /// <param name="item">
        /// Item from the queue
        /// </param>
        void DiffFile(SyncItem item);
    }
}