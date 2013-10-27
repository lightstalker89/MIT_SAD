
namespace BiOWheelsFileWatcher
{
    internal class SyncItem
    {
        public SyncItem(string destinationFolder, string sourceFile, FileAction fileAction)
        {
            this.DestinationFolder = destinationFolder;
            this.SourceFile = sourceFile;
            this.FileAction = fileAction;
        }

        /// <summary>
        /// 
        /// </summary>
        public string DestinationFolder { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public string SourceFile { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public FileAction FileAction { get; set; }
    }
}
