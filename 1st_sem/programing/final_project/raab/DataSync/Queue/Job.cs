using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSync
{
    public enum JobType
    {
        Create,
        Copy,
        Delete,
        Rename,
        Move,
        CreateDirectory,
        DeleteDirectory,
        RenameDirectory,
        MoveDirectory
    }

    public class JobTaskOldValues : JobTask
    {
        public string OldSourceFilePath { get; set; }
        public string OldDestinationFilePath { get; set; }


        public JobTaskOldValues(string sourceFilePath, string destinationFilePath,
            string oldSourceFilePath, string oldDestinationFilePath, JobType type) 
            : base(sourceFilePath, destinationFilePath, type) 
        {
            this.OldSourceFilePath = oldSourceFilePath;
            this.OldDestinationFilePath = oldDestinationFilePath;
        }
    }

    public class JobTask
    {
        public static int JobCounter { get; set; }
        public int ID { get; set; }
        public string SourceFilePath { get; set; }
        public string DestinationFilePath { get; set; }
        public JobType Type { get; set; }
        public FileInfo FileInfo { get; set; }

        public JobTask(string sourceFilePath, string destinationFilePath, JobType type)
        {
            JobCounter++;
            ID = JobCounter;
            this.SourceFilePath = sourceFilePath;
            this.DestinationFilePath = destinationFilePath;
            this.Type = type;
            FileInfo = new FileInfo(this.SourceFilePath);
        }

        public void DoJob()
        {
            switch (this.Type)
            {
                case JobType.Create:
                    Console.WriteLine("Create from: {0} to: ", this.SourceFilePath, this.DestinationFilePath);
                    break;
                case JobType.Copy:
                    Console.WriteLine("Copy from: {0} to: ", this.SourceFilePath, this.DestinationFilePath);
                    break;
                case JobType.Delete:
                    Console.WriteLine("Delete: {0}", this.DestinationFilePath);
                    break;
                case JobType.Rename:
                    Console.WriteLine("Rename to: {0}", this.SourceFilePath);
                    break;
                case JobType.Move:
                    Console.WriteLine("Move from: {0} to {1}", this.SourceFilePath, this.DestinationFilePath);
                    break;
                default:
                    break;
            }
        }

        public override string ToString()
        {
            long size = 0;
            if (File.Exists(this.SourceFilePath))
                size = this.FileInfo.Length;

            return string.Format("{3}-Job: {0}{4}from: {1}{4}to {2}{4}File-Size: {5}", this.ID, this.SourceFilePath, this.DestinationFilePath, this.Type, Environment.NewLine, size);
        }
    }

}
