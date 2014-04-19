using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleBoxConfigLoader
{
    public class SrcFolder
    {
        private string sourceFolder;
        public string SourceFolder
        {
            get { return sourceFolder; }
            set { sourceFolder = value; }
        }

        private List<string> destinationFolders;
        public List<string> DestinationFolders
        {
            get { return destinationFolders; }
            set { destinationFolders = value; }
        }

        private List<string> folderExceptions;
        public List<string> FolderExceptions
        {
            get { return folderExceptions; }
            set { folderExceptions = value; }
        }

        private bool recursion;
        public bool Recursion
        {
            get { return recursion; }
            set { recursion = value; }
        }

        public SrcFolder(string sourceFolders, List<string> destinationFolders, List<string> folderExceptions, bool recursion)
        {
            this.SourceFolder = sourceFolders;
            this.DestinationFolders = destinationFolders;
            this.FolderExceptions = folderExceptions;
            this.Recursion = recursion;
        }
    }
}
