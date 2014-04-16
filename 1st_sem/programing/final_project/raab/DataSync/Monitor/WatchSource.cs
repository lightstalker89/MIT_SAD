using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DataSync
{
    public class WatchSource
    {
        public SourceWatcher Source { get; set; }


        public delegate void NewJobDelegate(JobTask value);
        public event NewJobDelegate OnNewCommingJob;

        public WatchSource(SourceWatcher source)
        {
            this.Source = source;
            this.StartWatch();
        }

        public void DoSomething()
        {
        }

        private void StartWatch()
        {
            this.Source.FileSystemWatcher = new FileSystemWatcher(this.Source.Path);

            this.Source.FileSystemWatcher.Created += OnCreated;
            this.Source.FileSystemWatcher.Deleted += OnDeleted;
            this.Source.FileSystemWatcher.Changed += OnChanged;
            this.Source.FileSystemWatcher.Renamed += OnRenamed;

            this.Source.FileSystemWatcher.IncludeSubdirectories = this.Source.IncludeSubdirectories;

            this.Source.FileSystemWatcher.EnableRaisingEvents = true;
        }

        public void EndWatch()
        {
            this.Source.FileSystemWatcher.Created -= OnCreated;
            this.Source.FileSystemWatcher.Deleted -= OnDeleted;
            this.Source.FileSystemWatcher.Changed -= OnChanged;
            this.Source.FileSystemWatcher.Renamed -= OnRenamed;

            this.Source.FileSystemWatcher.EnableRaisingEvents = false;

            this.Source.FileSystemWatcher = null;
        }

        public bool isDirectory(string path)
        {
            FileAttributes attr;

            if (File.Exists(path) || Directory.Exists(path))
            {
                try
                {
                    attr = File.GetAttributes(path);

                    if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
                        return true;
                }
                catch (FileNotFoundException)
                {
                    Console.WriteLine("Datei wurde nicht gefunden");
                }

            }

            return false;
        }

        void OnRenamed(object sender, RenamedEventArgs e)
        {
            //e.ChangeType

            string fullSourcePath = this.Source.Path;
            string fullNewFileSourcePath = e.FullPath;
            string relativeNewFilePath = fullNewFileSourcePath.Replace(fullSourcePath, "");
            relativeNewFilePath = relativeNewFilePath.Trim('\\');

            string fullOldFileSourcePath = e.OldFullPath;
            string relativeOldFilePath = fullOldFileSourcePath.Replace(fullSourcePath, "");
            relativeOldFilePath = relativeOldFilePath.Trim('\\');

            foreach (string destinationPath in this.Source.DestinationPaths)
            {
                string fullNewFileDestinationPath = Path.Combine(destinationPath, relativeNewFilePath);
                string fullOldFileDestinationPath = Path.Combine(destinationPath, relativeOldFilePath);

                if (isDirectory(e.FullPath))
                {
                    OnNewCommingJob(
                        new JobTaskOldValues(
                                fullNewFileSourcePath, fullNewFileDestinationPath,
                                fullOldFileSourcePath, fullOldFileDestinationPath,
                                JobType.RenameDirectory));
                }
                else if (!isDirectory(e.FullPath))
                {
                    OnNewCommingJob(
                        new JobTaskOldValues(
                                fullNewFileSourcePath, fullNewFileDestinationPath,
                                fullOldFileSourcePath, fullOldFileDestinationPath,
                                JobType.Rename));
                }
            }
        }

        void OnChanged(object sender, FileSystemEventArgs e)
        {
            string fullSourcePath = this.Source.Path;
            string fullFileSourcePath = e.FullPath;
            string relativeFilePath = fullFileSourcePath.Replace(fullSourcePath, "");
            relativeFilePath = relativeFilePath.Trim('\\');

            foreach (string destinationPath in this.Source.DestinationPaths)
            {
                string fullFileDestinationPath = Path.Combine(destinationPath, relativeFilePath);

                if (isDirectory(e.FullPath))
                {
                    OnNewCommingJob(new JobTask(fullFileSourcePath, fullFileDestinationPath, JobType.CreateDirectory));
                } 
                else if (!isDirectory(e.FullPath))
                {
                    OnNewCommingJob(new JobTask(fullFileSourcePath, fullFileDestinationPath, JobType.Copy));
                }
            }
            
        }

        void OnDeleted(object sender, FileSystemEventArgs e)
        {

            string fullSourcePath = this.Source.Path;
            string fullFileSourcePath = e.FullPath;
            string relativeFilePath = fullFileSourcePath.Replace(fullSourcePath, "");
            relativeFilePath = relativeFilePath.Trim('\\');

            foreach (string destinationPath in this.Source.DestinationPaths)
            {
                string fullFileDestinationPath = Path.Combine(destinationPath, relativeFilePath);

                //ist Datei oder wurde gelöscht
                if (File.Exists(fullFileDestinationPath) || Directory.Exists(fullFileDestinationPath))
                {
                    if(isDirectory(fullFileDestinationPath))
                    {
                        OnNewCommingJob(new JobTask(fullFileSourcePath, fullFileDestinationPath, JobType.DeleteDirectory));
                    }
                    else if (!isDirectory(fullFileDestinationPath))
                    {
                        OnNewCommingJob(new JobTask(fullFileSourcePath, fullFileDestinationPath, JobType.Delete));
                    }
                }
            }
            
        }

        void OnCreated(object sender, FileSystemEventArgs e)
        {
            string fullSourcePath = this.Source.Path;
            string fullFileSourcePath = e.FullPath;
            string relativeFilePath = fullFileSourcePath.Replace(fullSourcePath, "");
            relativeFilePath = relativeFilePath.Trim('\\');

            foreach (string destinationPath in this.Source.DestinationPaths)
            {
                string fullFileDestinationPath = Path.Combine(destinationPath, relativeFilePath);
                if (isDirectory(e.FullPath))
                {
                    OnNewCommingJob(new JobTask(fullFileSourcePath, fullFileDestinationPath, JobType.CreateDirectory));
                }
                else if (!isDirectory(e.FullPath))
                {
                    OnNewCommingJob(new JobTask(fullFileSourcePath, fullFileDestinationPath, JobType.Create));
                }
            }
        }

        private void PrintMsg(string msg)
        {
            Console.WriteLine(msg);
        }
    }
}
