using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DataSync
{
    class Program
    {
        public static DataLogger dataLogger;

        /// <summary>
        /// 
        /// 
        /// XSD über XML erstellen: FreeFormater.com
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            dataLogger = new DataLogger(
                Path.Combine(Properties.Settings.Default.LogPfad, Properties.Settings.Default.LogFile),
                Path.Combine(Properties.Settings.Default.LogPfad, Properties.Settings.Default.LogFileBak),
                Int32.Parse(Properties.Settings.Default.MaxSize),
                true, true
                );


            //List<string> destinationDirectory1 = new List<string>();
            //destinationDirectory1.Add(@"D:\FH\Master\Programmieren\DataSync\DEST1");
            //destinationDirectory1.Add(@"D:\FH\Master\Programmieren\DataSync\DEST2");

            //Source source = new Source(@"D:\FH\Master\Programmieren\DataSync\WATCH1", destinationDirectory1, null);

            Configuration sourceConfig = new Configuration("SourceConfig.xml", "SourceConfig.xsd");
            dataLogger.WriteLog("Configuration loaded", DataLogger.MessageType.Standard);

            QueueWorker neueQueue = new QueueWorker();

            foreach (Source source in sourceConfig.SourceList)
            {
                StartSynchronizer startSynchronizer = new StartSynchronizer(source, neueQueue);
                startSynchronizer.StartSynchronizingDirectories();
                startSynchronizer.StartSynchronizingFiles();
            }
            dataLogger.WriteLog("Initialized sourcepaths for watch", DataLogger.MessageType.Standard);


            Watch w = new Watch(dataLogger, neueQueue);
            foreach (Source source in sourceConfig.SourceList)
            {
                w.AddNewWatchSource(source);
            }
            dataLogger.WriteLog("Watching started", DataLogger.MessageType.Standard);

            Thread queueWorkerThread;
            queueWorkerThread = new Thread(new ParameterizedThreadStart(queueWorkerThreadProcedure));
            queueWorkerThread.Start(neueQueue);
            dataLogger.WriteLog("Job Queue started", DataLogger.MessageType.Standard);


            ConsoleKeyInfo key;
            bool exit = false;

            do
            {
                key = Console.ReadKey();

                if (key.Key == ConsoleKey.Enter)
                {
                    Console.Clear();
                    foreach (JobTask item in neueQueue.QueueList)
                    {
                        Console.WriteLine(item);
                    }
                }
                else if (key.Key == ConsoleKey.Escape)
                {
                    if (neueQueue.QueueList.Count > 0)
                    {
                        queueWorkerThread.Suspend();
                        ConsoleKeyInfo key2;
                        do
                        {
                            Console.WriteLine("There are currently jobs in the queue. Do you really want to exit? (y / n)");
                            key2 = Console.ReadKey();
                            System.Threading.Thread.Sleep(500);

                        } while (key2.Key != ConsoleKey.Y && key2.Key != ConsoleKey.N);

                        if (key2.Key == ConsoleKey.Y)
                            exit = true;
                        else
                        {
                            exit = false;
                            queueWorkerThread.Resume();
                        }
                    }
                    else
                    {
                        exit = true;
                    }
                }
                System.Threading.Thread.Sleep(500);
            } while (!exit);
            
            w.EndAllToWatch();
            dataLogger.WriteLog("Watching stopped", DataLogger.MessageType.Standard);
            if (queueWorkerThread.ThreadState == ThreadState.Suspended || queueWorkerThread.ThreadState == ThreadState.Stopped)
            {
                queueWorkerThread.Resume();
                queueWorkerThread.Abort();
            }
            else
            {
                queueWorkerThread.Abort();
            }

            dataLogger.WriteLog("Job Queue stopped", DataLogger.MessageType.Standard);
            dataLogger = null;

            Console.WriteLine("Exit DataSync");
            System.Threading.Thread.Sleep(500);
        }

        /// <summary>
        /// Arbeitet die Jobs in einem eigenen Thread ab
        /// </summary>
        /// <param name="object">QueueWorker-Objekt</param>
        private static void queueWorkerThreadProcedure(object @object)
        {

            while (true)
            {
                if (@object is QueueWorker)
                {
                    QueueWorker jobQueue = ((QueueWorker)@object);
                    if (jobQueue.QueueList.Count > 0)
                    {
                        JobTask job = jobQueue.GetNextJob();

                        FileManagement fileManager = new FileManagement(job);

                        //Console.WriteLine(job);

                        switch (job.Type)
                        {
                            case JobType.Create:
                                if (fileManager.CopyFile())
                                {
                                    dataLogger.WriteLog(
                                        string.Format("Create-Job succeeded - File was created: {0}", job.DestinationFilePath), 
                                        DataLogger.MessageType.Standard);
                                }
                                else
                                {
                                    dataLogger.WriteLog(
                                        string.Format("Error by Create-Job - File wasn't created: {0}", job.DestinationFilePath),
                                        DataLogger.MessageType.Error);
                                }
                                break;
                            case JobType.Copy:
                                if (fileManager.CopyFile())
                                {
                                    dataLogger.WriteLog(
                                        string.Format("Copy-Job succeeded - File was copied: {0}", job.DestinationFilePath), 
                                        DataLogger.MessageType.Standard);
                                }
                                else
                                {
                                    dataLogger.WriteLog(
                                        string.Format("Error by Copy-Job - File wasn't copied: {0}", job.DestinationFilePath),
                                        DataLogger.MessageType.Error);
                                }
                                break;
                            case JobType.Delete:
                                if (fileManager.DeleteFile())
                                {
                                    dataLogger.WriteLog(
                                        string.Format("Delete-Job succeeded - File was deleted: {0}", 
                                            job.DestinationFilePath),
                                        DataLogger.MessageType.Standard);
                                }
                                else
                                {
                                    dataLogger.WriteLog(
                                        string.Format("Error by Delete-Job - File wasn't deleted: {0}",
                                            job.DestinationFilePath),
                                        DataLogger.MessageType.Error);
                                }                             
                                break;
                            case JobType.Rename:
                                if (fileManager.RenameFile())
                                {
                                    dataLogger.WriteLog(string.Format("Rename-Job succeeded - File was renamed from: {0} to: {1}",
                                        ((JobTaskOldValues)job).OldDestinationFilePath,
                                        ((JobTaskOldValues)job).DestinationFilePath),
                                        DataLogger.MessageType.Standard);
                                }
                                else
                                {
                                    dataLogger.WriteLog(string.Format("Error by Rename-Job - File wasn't renamed from: {0} to: {1}",
                                        ((JobTaskOldValues)job).OldDestinationFilePath,
                                        ((JobTaskOldValues)job).DestinationFilePath),
                                        DataLogger.MessageType.Error);
                                }
                                break;

                            case JobType.Move:
                                if (fileManager.MoveFile())
                                {
                                    dataLogger.WriteLog(string.Format("Move-Job succeeded - File was moved from: {0} to: {1}", 
                                        ((JobTaskOldValues)job).OldDestinationFilePath,
                                        ((JobTaskOldValues)job).DestinationFilePath),
                                        DataLogger.MessageType.Standard);
                                }
                                else
                                {
                                    dataLogger.WriteLog(string.Format("Error by Move-Job - File wasn't moved from: {0} to: {1}",
                                        ((JobTaskOldValues)job).OldDestinationFilePath,
                                        ((JobTaskOldValues)job).DestinationFilePath),
                                        DataLogger.MessageType.Error);
                                }
                                break;

                            case JobType.CreateDirectory:
                                if (fileManager.CreateDirectory())
                                {
                                    dataLogger.WriteLog(
                                        string.Format("Create-Job succeeded - Directory was created: {0}", job.DestinationFilePath), 
                                        DataLogger.MessageType.Standard);
                                }
                                else
                                {
                                    dataLogger.WriteLog(
                                        string.Format("Error by Create-Job - Directory wasn't created: {0}", job.DestinationFilePath),
                                        DataLogger.MessageType.Error);
                                }
                                break;

                            case JobType.RenameDirectory:
                                if (fileManager.RenameDirectory())
                                {
                                    dataLogger.WriteLog(string.Format("Rename-Job succeeded - Directory was renamed from: {0} to: {1}",
                                        ((JobTaskOldValues)job).OldDestinationFilePath,
                                        ((JobTaskOldValues)job).DestinationFilePath),
                                        DataLogger.MessageType.Standard);
                                }
                                else
                                {
                                    dataLogger.WriteLog(string.Format("Error by Rename-Job - Directory wasn't renamed from: {0} to: {1}",
                                        ((JobTaskOldValues)job).OldDestinationFilePath,
                                        ((JobTaskOldValues)job).DestinationFilePath),
                                        DataLogger.MessageType.Error);
                                }
                                break;

                            case JobType.MoveDirectory:
                                if (fileManager.MoveDirectory())
                                {
                                    dataLogger.WriteLog(string.Format("Move-Job succeeded - Directory was moved from: {0} to: {1}",
                                        ((JobTaskOldValues)job).OldDestinationFilePath,
                                        ((JobTaskOldValues)job).DestinationFilePath),
                                        DataLogger.MessageType.Standard);
                                }
                                else
                                {
                                    dataLogger.WriteLog(string.Format("Error by Move-Job - Directory wasn't moved from: {0} to: {1}",
                                        ((JobTaskOldValues)job).OldDestinationFilePath,
                                        ((JobTaskOldValues)job).DestinationFilePath),
                                        DataLogger.MessageType.Error);
                                }
                                break;

                            case JobType.DeleteDirectory:
                                if (fileManager.DeleteDirectory())
                                {
                                    dataLogger.WriteLog(
                                        string.Format("Delete-Job succeeded - Directory was deleted: {0}",
                                            job.DestinationFilePath),
                                        DataLogger.MessageType.Standard);
                                }
                                else
                                {
                                    dataLogger.WriteLog(
                                        string.Format("Error by Delete-Job - Directory wasn't deleted: {0}",
                                            job.DestinationFilePath),
                                        DataLogger.MessageType.Error);
                                }
                                break;
                            default:
                                break;
                        }

                        System.Threading.Thread.Sleep(500);
                        //Console.Clear();
                    }
                }
            }
        }







    }
}
