using System.IO;

namespace BiOWheelsFileWatcher.CustomEventArgs
{
    public class CustomFileSystemEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomFileSystemEventArgs"/> class
        /// </summary>
        /// <param name="fullQualifiedFileName"></param>
        /// <param name="fileName"></param>
        public CustomFileSystemEventArgs(string fullQualifiedFileName, string fileName)
        {
            this.FullQualifiedFileName = fullQualifiedFileName;
            this.FileName = fileName;
        }

        #region Properties

        /// <summary>
        /// Gets or sets the full qualified file name
        /// </summary>
        public string FullQualifiedFileName { get; set; }

        /// <summary>
        /// Gets or sets the file name
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets a value indication whether the file must be compared in blocks or not
        /// </summary>
        public bool CompareInBlocks { get; set; }

        #endregion
    }
}
