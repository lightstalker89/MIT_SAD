using System.IO;
using System.Linq;

namespace BiOWheelsFileWatcher
{
    internal class FileSystemManager
    {
        #region Private Fields

        /// <summary>
        /// Represents an instance of the <see cref="FileComparator"/> class
        /// </summary>
        private FileComparator fileComparator;

        #endregion

        internal FileSystemManager(FileComparator fileComparator)
        {
            this.FileComparator = fileComparator;
        }

        #region Properties

        /// <summary>
        /// Gets or sets the <see cref="FileComparator"/> instance
        /// </summary>
        internal FileComparator FileComparator
        {
            get
            {
                return this.fileComparator;
            }

            set
            {
                this.fileComparator = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Copies a file to the given destinations
        /// </summary>
        /// <param name="item">
        /// Item from the queue
        /// </param>
        internal void CopyFile(SyncItem item)
        {
            // TODO: Parallel Sync implement
            foreach (string destination in item.Destinations)
            {
                this.CreateDirectoryIfNotExists(destination);

                string pathToCopy = Path.GetDirectoryName(destination + Path.DirectorySeparatorChar + item.SourceFile);

                this.CreateDirectoryIfNotExists(pathToCopy);

                string fileToCopy = pathToCopy + Path.DirectorySeparatorChar + Path.GetFileName(item.SourceFile);

                File.Copy(item.FullQualifiedSourceFileName, fileToCopy, true);
            }
        }

        /// <summary>
        /// Compare files from a destination with files in all given destinations
        /// </summary>
        /// <param name="item">
        /// Item from the queue
        /// </param>
        internal void DiffFile(SyncItem item)
        {
            foreach (string destinationFile in
                item.Destinations.Select(
                    destination => destination + Path.DirectorySeparatorChar + Path.GetFileName(item.SourceFile)))
            {
            }
        }

        /// <summary>
        /// Copies a directory to the given destinations
        /// </summary>
        /// <param name="item">
        /// Item from the queue
        /// </param>
        internal void CopyDirectory(SyncItem item)
        {
            foreach (string destination in item.Destinations)
            {
                this.CreateDirectoryIfNotExists(destination);

                string pathToCopy = destination + Path.DirectorySeparatorChar + item.SourceFile;

                this.CreateDirectoryIfNotExists(pathToCopy);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        internal void Delete(SyncItem item)
        {
            foreach (string destination in item.Destinations)
            {
                string pathToDelete = destination + Path.DirectorySeparatorChar + item.SourceFile;

                if (pathToDelete.IsDirectory())
                {
                    Directory.Delete(pathToDelete,true);
                }
                else
                {
                    File.Delete(pathToDelete);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        internal void Copy(SyncItem item)
        {
            if (item.FullQualifiedSourceFileName.IsDirectory())
            {
                this.CopyDirectory(item);
            }
            else
            {
                this.CopyFile(item);
            }
        }

        /// <summary>
        /// Creates a directory if it does not exist
        /// </summary>
        /// <param name="directory">
        /// </param>
        internal void CreateDirectoryIfNotExists(string directory)
        {
            if (directory != null && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }

        #endregion
    }
}
