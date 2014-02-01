//-----------------------------------------------------------------------
// <copyright file="Job.cs" company="MD Development">
//     Copyright (c) MD Development. All rights reserved.
// </copyright>
// <author>Michael Drexler</author>
//-----------------------------------------------------------------------
namespace Prototyp
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Prototyp.Log;

    /// <summary>
    /// Job status
    /// </summary>
    public enum Jobstatus
    {
        /// <summary>
        /// Job is waiting
        /// </summary>
        waiting,

        /// <summary>
        /// Job is running
        /// </summary>
        running,

        /// <summary>
        /// Job failed
        /// </summary>
        failed,

        /// <summary>
        /// Job is successfully
        /// </summary>
        success
    }

    /// <summary>
    /// Event type occured 
    /// </summary>
    public enum EventType
    {
        /// <summary>
        /// Changed file or directory
        /// </summary>
        Changed,

        /// <summary>
        /// Created file or directory
        /// </summary>
        Created,

        /// <summary>
        /// Deleted file or directory
        /// </summary>
        Deleted,

        /// <summary>
        /// Renamed file or directory
        /// </summary>
        Renamed
    }

    /// <summary>
    /// Job class
    /// </summary>
    public class Job
    {
        // Fields

        /// <summary>
        /// Count the jobs
        /// </summary>
        private static long counter = 0;

        /// <summary>
        /// Current job id
        /// </summary>
        private long id;

        /// <summary>
        /// State of the job 
        /// </summary>
        private Jobstatus status;

        /// <summary>
        /// Start date of the job
        /// </summary>
        private DateTime start;

        /// <summary>
        /// End date of the job
        /// </summary>
        private DateTime end;

        /// <summary>
        /// The source directory 
        /// </summary>
        private string sourceDirectory;

        /// <summary>
        /// The excluded directories from the given source directory
        /// </summary>
        private List<Directory> exceptionDirectories;

        /// <summary>
        /// The target directory
        /// </summary>
        private string targetDirectory;

        /// <summary>
        /// File/directory path
        /// </summary>
        private string fileName;

        /// <summary>
        /// Old file/directory path
        /// </summary>
        private string oldName;

        /// <summary>
        /// Event type occurred for creating this job
        /// </summary>
        private EventType eventType;

        /// <summary>
        /// Include sub directories for syncing
        /// </summary>
        private bool includeSubdirectories = false;

        /// <summary>
        /// Logger instance
        /// </summary>
        private ILogger logger = Logger.Instance;

        /// <summary>
        /// Initializes a new instance of the <see cref="Job"/> class
        /// </summary>
        /// <param name="sourceDirectory">Source directory</param>
        /// <param name="targetDirectory">Target directory</param>
        /// <param name="path">Name of the file or directory</param>
        /// <param name="etype">Occurred event type</param>
        /// <param name="includeSubdirectories">Include sub directories</param>
        /// <param name="oldName">Old name of file or directory</param>
        /// <param name="exceptionDirectories">Directories excluded for the given source directory</param>
        public Job(string sourceDirectory, string targetDirectory, string path, EventType etype, bool includeSubdirectories, string oldName = "", List<Directory> exceptionDirectories = null)
        {
            this.id = counter++;
            this.start = DateTime.Now;
            this.status = Jobstatus.waiting;
            this.sourceDirectory = sourceDirectory;
            this.targetDirectory = targetDirectory;
            this.exceptionDirectories = exceptionDirectories;
            this.fileName = path;
            this.includeSubdirectories = includeSubdirectories;
            this.oldName = oldName;
            this.eventType = etype;
        }

        // Properties

        /// <summary>
        /// Gets the job id
        /// </summary>
        public long Id
        {
            get { return this.id; }
        }

        /// <summary>
        /// Gets a value indicating whether sub directories are included
        /// </summary>
        public bool IncludeSubdirectories
        {
            get { return this.includeSubdirectories; }
        }

        /// <summary>
        /// Gets the file name
        /// </summary>
        public string FileName
        {
            get { return this.fileName; }
        }

        /// <summary>
        /// Gets the name before the renaming
        /// </summary>
        public string OldName
        {
            get { return this.oldName; }
        }

        /// <summary>
        /// Gets the target directory
        /// </summary>
        public string TargetDirectory
        {
            get { return this.targetDirectory; }
        }

        /// <summary>
        /// Gets the source directory
        /// </summary>
        public string SourceDirectory
        {
            get { return this.sourceDirectory; }
        }

        /// <summary>
        /// Gets a list of exluded directories
        /// </summary>
        public List<Directory> ExceptionDirectories
        {
            get { return this.exceptionDirectories; }
        }

        /// <summary>
        /// Gets or sets the end datetime
        /// </summary>
        public DateTime End
        {
            get { return this.end; }
            set { this.end = value; }
        }

        /// <summary>
        /// Gets or sets the job status
        /// </summary>
        public Jobstatus Status
        {
            get { return this.status; }
            set { this.status = value; }
        }

        /// <summary>
        /// Gets the event type which created this job
        /// </summary>
        public EventType EventType
        {
            get { return this.eventType; }
        }

        // Methods
    }
}
