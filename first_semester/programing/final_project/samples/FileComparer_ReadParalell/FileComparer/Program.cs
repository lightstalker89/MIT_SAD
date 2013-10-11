using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using System.Security.Cryptography;
using System.Diagnostics;

namespace FileComparer
{
    class Program
    {
        private static readonly string SourceDir = Environment.CurrentDirectory + Path.DirectorySeparatorChar + "SourceFiles"; // Quellverzeichnis
        private static readonly string DestinationDir = Environment.CurrentDirectory + Path.DirectorySeparatorChar + "DestinationFiles"; // Zielvereichnis
        private static readonly List<string> InputFiles = Directory.GetFiles(SourceDir, "*.*", SearchOption.AllDirectories).ToList();
        private static readonly List<string> OutputFiles = Directory.GetFiles(DestinationDir, "*.*", SearchOption.AllDirectories).ToList();

        static void Main(string[] args)
        {
            Console.WriteLine("Syncing files parallel:");
            compareFiles();
            Console.WriteLine("Thread is currently working...");

            Console.ReadKey();
        }

        public static void compareFiles()
        {
            Thread myThread = new Thread(() =>
            {
                Stopwatch watch = Stopwatch.StartNew();
                OutputFiles.AsParallel().ForAll(CheckFile);
                watch.Stop();
                Console.WriteLine(watch.ElapsedMilliseconds + "ms needed for sync");

            });
            myThread.Start();
        }

        private static void CheckFile(string filePathSource)
        {
            string filePathDestination = SourceDir + Path.DirectorySeparatorChar + Path.GetFileName(filePathSource);

            List<byte[]> checksumsSource = new List<byte[]>();
            List<byte[]> checksumsDestination = new List<byte[]>();

            if (File.Exists(SourceDir + Path.DirectorySeparatorChar + Path.GetFileName(filePathSource)))
            {
                Console.WriteLine("File already exists - Analyzing file...");

                using (FileStream strSource = new FileStream(filePathSource, FileMode.Open),
                    strDestination = new FileStream(filePathDestination, FileMode.Open))
                {
                    //ceck with tasks
                    Task[] tasks = { Task.Factory.StartNew(() => CheckBlock(strSource, ref checksumsSource)),
                                       Task.Factory.StartNew(() => CheckBlock(strDestination, ref checksumsDestination)) };
                    Task.WaitAll(tasks);

                    //Check serial
                    //CheckBlock(strSource, ref checksumsSource);
                    //CheckBlock(strDestination, ref checksumsDestination);

                    strSource.Close();
                    strDestination.Close();
                }

                CheckEqual(checksumsSource, checksumsDestination, Path.GetFileName(filePathSource));
            }
            else
            {
                Console.WriteLine("Syncing file " + Path.GetFileName(filePathSource));
                File.Copy(filePathSource, SourceDir + Path.DirectorySeparatorChar + Path.GetFileName(filePathSource), true);
            }
        }

        private static void CheckBlock(FileStream str, ref List<byte[]> checksums)
        {
            byte[] buffer = new byte[2048];
            while (str.Read(buffer, 0, buffer.Length) != 0)
            {
                using (var md5 = MD5.Create())
                {
                    checksums.Add(md5.ComputeHash(buffer));
                }
            }
        }

        //shit Method begin
        private static void CheckEqual(List<byte[]> checksumsSource, List<byte[]> checksumsDestination, string fileName)
        {
            bool equal = true;
            for (int i = 0; i < checksumsSource.Count; i++)
            {
                if (!checksumsSource[i].SequenceEqual(checksumsDestination[i]))
                {
                    equal = false;
                    break;
                }

            }

            if (equal)
            {
                Console.WriteLine("Equal " + fileName);
            }
            else
            {
                Console.WriteLine("Not equal " + fileName);
            }
        }
        //shit Method ends
    }
}
