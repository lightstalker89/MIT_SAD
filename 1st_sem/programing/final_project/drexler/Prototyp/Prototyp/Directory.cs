//-----------------------------------------------------------------------
// <copyright file="Directory.cs" company="MD Development">
//     Copyright (c) MD Development. All rights reserved.
// </copyright>
// <author>Michael Drexler</author>
//-----------------------------------------------------------------------
namespace Prototyp
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Directory type
    /// </summary>
    public enum DirectoryType
    {
        /// <summary>
        /// Local directory
        /// </summary>
        Local,

        /// <summary>
        /// Directory on a share
        /// </summary>
        Shared,

        /// <summary>
        /// Directory on a network
        /// </summary>
        Network
    }

    /// <summary>
    /// Content Class
    /// </summary>
    public class Directory
    {
        // Fields

        /// <summary>
        /// Directory path
        /// </summary>
        private string path;

        /// <summary>
        /// Directory type, local, shared, network
        /// </summary>
        private DirectoryType directoryType = DirectoryType.Local;

        /// <summary>
        /// Include subdirectories for this directory
        /// </summary>
        private bool includeSubdirectories = false;

        /// <summary>
        /// Targets directories for synchronization
        /// </summary>
        private List<Directory> targetDirectories;

        /// <summary>
        /// Exception directories for synchronization
        /// </summary>
        private List<Directory> exceptionDirectories;

        /// <summary>
        /// IP address for the remote host to connect
        /// </summary>
        private IPAddress remoteHost;

        /// <summary>
        /// Port for the remote host to connect
        /// </summary>
        private int port;

        /// <summary>
        /// Initializes a new instance of the <see cref="Directory" /> class
        /// </summary>
        public Directory()
        {
            // Check type of path
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Directory"/> class
        /// </summary>
        /// <param name="targetDirectories">List of target directories for this source directory</param>
        public Directory(List<Directory> targetDirectories) : this()
        {
            this.targetDirectories = targetDirectories;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Directory"/> class
        /// </summary>
        /// <param name="targetDirectories">List of target directories for this source directory</param>
        /// <param name="exceptionDirectories">List of exception directories for this source directory</param>
        public Directory(List<Directory> targetDirectories, List<Directory> exceptionDirectories) : this()
        {
            this.targetDirectories = targetDirectories;
            this.exceptionDirectories = exceptionDirectories;
        }

        // Properties

        /// <summary>
        /// Gets the target directory or directories for one source directory
        /// </summary>
        public List<Directory> TargetDirectories
        {
            get { return this.targetDirectories; }
        }

        /// <summary>
        /// Gets the exception directory or directories for one source directory
        /// </summary>
        public List<Directory> ExceptionDirectories
        {
            get { return this.exceptionDirectories; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether including subdirectories or not
        /// </summary>
        public bool IncludeSubdirectories
        {
            get { return this.includeSubdirectories; }
            set { this.includeSubdirectories = value; }
        }

        /// <summary>
        /// Gets or sets the path for the source directory
        /// </summary>
        public string Path
        {
            get 
            { 
                return this.path;
            }

            set 
            { 
                this.path = value;
                if (this.path.ToLower().StartsWith("\\\\"))
                {
                    this.directoryType = Prototyp.DirectoryType.Shared;
                }
                else
                {
                    this.directoryType = Prototyp.DirectoryType.Local;
                }
            }
        }

        /// <summary>
        /// Gets or sets the typ of the directory
        /// </summary>
        public DirectoryType DirectoryType
        {
            get { return this.directoryType; }
            set { this.directoryType = value; }
        }

        /// <summary>
        /// Gets or sets the ip address for the remote host
        /// </summary>
        public IPAddress RemoteHost
        {
            get { return this.remoteHost; }
            set { this.remoteHost = value; }
        }

        /// <summary>
        /// Gets or sets the port for the remote host
        /// </summary>
        public int Port
        {
            get { return this.port; }
            set { this.port = value; }
        }

        // Functions
    }
}
