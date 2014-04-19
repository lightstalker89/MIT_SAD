using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSync
{
    public class FileManagement
    {
        public JobTask CurrentJob { get; set; }


        public FileManagement(JobTask currentJob)
        {
            this.CurrentJob = currentJob;
        }

        public bool DeleteFile()
        {
            if (File.Exists(this.CurrentJob.DestinationFilePath))
            {
                File.Delete(this.CurrentJob.DestinationFilePath);
                Console.WriteLine("File wurde gelöscht: {0}", this.CurrentJob.DestinationFilePath);
                return true;
            }
            return false;
        }

        public bool CopyFile()
        {
            if (File.Exists(this.CurrentJob.SourceFilePath))
            {
                //

                //try
                //{
                    File.Copy(this.CurrentJob.SourceFilePath, this.CurrentJob.DestinationFilePath, true);
                    //CopyFileOverStream(this.CurrentJob.SourceFilePath, this.CurrentJob.DestinationFilePath);
                //}
                //catch (FileNotFoundException)
                //{
                //    Console.WriteLine("File not found Exception");
                //}
                //catch (IOException)
                //{
                //    Console.WriteLine("IO Exception");
                //}

                return true;
            }
            return false;
        }

        private void CopyFileOverStream(string source, string destination)
        {
            using (var inputFile = new FileStream(source ,FileMode.Open,FileAccess.Read,FileShare.ReadWrite))
            {
                using (var outputFile = new FileStream(destination, FileMode.Create))
                {
                    var buffer = new byte[0x10000];
                    int bytes;

                    while ((bytes = inputFile.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        outputFile.Write(buffer, 0, bytes);
                    }
                }
            }
        }

        public bool RenameFile()
        {

            if (File.Exists(((JobTaskOldValues)this.CurrentJob).OldDestinationFilePath))
            {
                //try
                //{
                    File.Move(
                        ((JobTaskOldValues)this.CurrentJob).OldDestinationFilePath,
                        ((JobTaskOldValues)this.CurrentJob).DestinationFilePath);


                    //File.Delete(((JobTaskOldValues)this.CurrentJob).OldDestinationFilePath);

                //}
                //catch (Exception e)
                //{
                //    Console.WriteLine("Error: " + e);
                //    return false;
                //}


                Console.WriteLine("Datei wurde unbenannt von: {0} in {1}",
                    ((JobTaskOldValues)this.CurrentJob).OldDestinationFilePath,
                    ((JobTaskOldValues)this.CurrentJob).DestinationFilePath);
                return true;
            }
            return false;
        }

        public bool MoveFile()
        {

            if (File.Exists(((JobTaskOldValues)this.CurrentJob).OldDestinationFilePath))
            {
                File.Move(
                    ((JobTaskOldValues)this.CurrentJob).OldDestinationFilePath,
                    ((JobTaskOldValues)this.CurrentJob).DestinationFilePath);

                Console.WriteLine("Datei wurde unbenannt von: {0} in {1}",
                    ((JobTaskOldValues)this.CurrentJob).OldDestinationFilePath,
                    ((JobTaskOldValues)this.CurrentJob).DestinationFilePath);
                return true;
            }
            return false;
        }

        public bool CreateDirectory()
        {
            if (Directory.Exists(this.CurrentJob.SourceFilePath))
            {
                Directory.CreateDirectory( this.CurrentJob.DestinationFilePath);

                return true;
            }
            return false;
        }

        public bool DeleteDirectory()
        {
            if (Directory.Exists(this.CurrentJob.DestinationFilePath))
            {
                Directory.Delete(this.CurrentJob.DestinationFilePath);

                return true;
            }
            return false;
        }

        public bool RenameDirectory()
        {
            if (Directory.Exists(this.CurrentJob.SourceFilePath))
            {
                Directory.Move(
                    ((JobTaskOldValues)this.CurrentJob).OldDestinationFilePath,
                    ((JobTaskOldValues)this.CurrentJob).DestinationFilePath);

                return true;
            }
            return false;
        }

        public bool MoveDirectory()
        {
            if (Directory.Exists(this.CurrentJob.SourceFilePath))
            {
                Directory.Move(
                    ((JobTaskOldValues)this.CurrentJob).OldDestinationFilePath,
                    ((JobTaskOldValues)this.CurrentJob).DestinationFilePath);

                return true;
            }
            return false;
        }
        
    }
}
