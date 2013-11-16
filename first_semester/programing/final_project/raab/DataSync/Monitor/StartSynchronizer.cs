using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DataSync
{
    public class StartSynchronizer
    {
        public Source Source { get; set; }
        private List<string> AvailableDirectoriesInSourceList { get; set; }
        private List<string> AvailableFilesInSourceList { get; set; }
        public delegate void NewJobDelegate(JobTask value);
        public event NewJobDelegate OnNewCommingJob;
        public QueueWorker QueueWorker { get; set; }

        public StartSynchronizer(Source source, QueueWorker queue)
        {
            this.Source = source;
            this.QueueWorker = queue;
            this.AvailableDirectoriesInSourceList = new List<string>();
            this.AvailableFilesInSourceList = new List<string>();
            this.OnNewCommingJob += new NewJobDelegate(sync_OnNewCommingInJob);

        }

        void sync_OnNewCommingInJob(JobTask newJob)
        {
            this.QueueWorker.AddJob(newJob);
        }

        public void StartSynchronizingDirectories()
        {
            this.AvailableDirectoriesInSourceList = GetAllDirectories();

            foreach (string dir in this.AvailableDirectoriesInSourceList)
            {
                string fullSourcePath = this.Source.Path;
                string relativePath = dir.Replace(fullSourcePath, "");
                relativePath = relativePath.Trim('\\');

                foreach (string destinationPath in this.Source.DestinationPaths)
                {
                    string fullDestinationPath = Path.Combine(destinationPath, relativePath);

                    if (!Directory.Exists(fullDestinationPath))
                    {
                        OnNewCommingJob(new JobTask(fullSourcePath, fullDestinationPath, JobType.CreateDirectory));
                    }
                }
            }
        }

        public void StartSynchronizingFiles()
        {
            this.AvailableFilesInSourceList = GetAllFiles();

            foreach (string file in this.AvailableFilesInSourceList)
            {
                string fullSourcePath = this.Source.Path;
                string relativePath = file.Replace(fullSourcePath, "");
                relativePath = relativePath.Trim('\\');

                foreach (string destinationPath in this.Source.DestinationPaths)
                {
                    string fullDestinationPath = Path.Combine(destinationPath, relativePath);

                    if (!File.Exists(fullDestinationPath))
                    {
                        OnNewCommingJob(new JobTask(file, fullDestinationPath, JobType.Create));
                    }
                }
            }
        }


        private List<string> GetAllDirectories()
        {
            List<string> directoryList = new List<string>();
            string[] directoryArray = Directory.GetDirectories(this.Source.Path, "*", SearchOption.AllDirectories);

            foreach (string directory in directoryArray)
            {
                directoryList.Add(directory);
            }

            return directoryList;
        }

        private List<string> GetAllFiles()
        {
            List<string> fileList = new List<string>();
            string[] fileArray = Directory.GetFiles(this.Source.Path, "*", SearchOption.AllDirectories);

            foreach (string file in fileArray)
            {
                fileList.Add(file);
            }

            return fileList;
        }
    }
}
