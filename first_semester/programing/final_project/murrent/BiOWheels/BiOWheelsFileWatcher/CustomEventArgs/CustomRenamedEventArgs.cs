namespace BiOWheelsFileWatcher.CustomEventArgs
{
    public class CustomRenamedEventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fullQualifiedFileName"></param>
        /// <param name="fileName"></param>
        /// <param name="oldFileName"></param>
        public CustomRenamedEventArgs(string fullQualifiedFileName, string fileName, string oldFileName, string oldFullQualifiedFileName) 
        {
            this.FullQualifiedFileName = fullQualifiedFileName;
            this.FileName = fileName;
            this.OldFileName = oldFileName;
            this.OldFullQualifiedFileName = oldFullQualifiedFileName;
        }

        #region Properties

        /// <summary>
        /// Gets or sets the old full qualified file name
        /// </summary>
        public string OldFullQualifiedFileName { get; set; }

        /// <summary>
        /// Gets or sets the old file name
        /// </summary>
        public string OldFileName { get; set; }

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
