using System.IO;
using System.Linq;
using BiOWheelsFileWatcher.Interfaces;

namespace BiOWheelsFileWatcher
{
    internal class FileSystemManager : IFileSystemManager
    {
        #region Private Fields

        /// <summary>
        /// Represents an instance of the <see cref="FileComparator"/> class
        /// </summary>
        private IFileComparator fileComparator;

        #endregion

        internal FileSystemManager(IFileComparator fileComparator)
        {
            this.FileComparator = fileComparator;
        }

        #region Properties

        /// <summary>
        /// Gets or sets the <see cref="FileComparator"/> instance
        /// </summary>
        internal IFileComparator FileComparator
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

        /// <inheritdoc/>
        public void DiffFile(SyncItem item)
        {
            foreach (string destinationFile in
                item.Destinations.Select(
                    destination => destination + Path.DirectorySeparatorChar + Path.GetFileName(item.SourceFile)))
            {
            }
        }

        /// <inheritdoc/>
        public void CopyDirectory(SyncItem item)
        {
            foreach (string destination in item.Destinations)
            {
                this.CreateDirectoryIfNotExists(destination);

                string pathToCopy = destination + Path.DirectorySeparatorChar + item.SourceFile;

                this.CreateDirectoryIfNotExists(pathToCopy);
            }
        }

        /// <inheritdoc/>
        public void Delete(SyncItem item)
        {
            foreach (string destination in item.Destinations)
            {
                string pathToDelete = destination + Path.DirectorySeparatorChar + item.SourceFile;

                if (pathToDelete.IsDirectory())
                {
                    Directory.Delete(pathToDelete, true);
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
        public void Copy(SyncItem item)
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
