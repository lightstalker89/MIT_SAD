using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace FileCompare
{
    class Program
    {
        private static string inputDir;
        private static string outputDir;
        private static List<string> inputFiles;
        private static List<string> outputFiles;

        static void Main(string[] args)
        {
            inputDir = Environment.CurrentDirectory + Path.DirectorySeparatorChar + "SourceFiles";
            outputDir = Environment.CurrentDirectory + Path.DirectorySeparatorChar + "DestinationFiles";

            if (Directory.Exists(inputDir) && Directory.Exists(outputDir))
            {
                inputFiles = Directory.GetFiles(inputDir, "*.*", SearchOption.AllDirectories).ToList();
                outputFiles = Directory.GetFiles(outputDir, "*.*", SearchOption.AllDirectories).ToList();
                //If u need a few files :)
                //CreateDummyFiles();

                //CompareWithAllFiles();
                //Console.WriteLine("===============================");
                //CompareWithFewerFiles(8000);
                //Console.WriteLine("===============================");
                //CompareWithFewerFiles(5000);
                //Console.WriteLine("===============================");
                //CompareWithFewerFiles(2500);
                //Console.WriteLine("===============================");
                //CompareWithFewerFiles(1000);
                //Console.WriteLine("===============================");
                //CompareWithFewerFiles(500);
                //Console.WriteLine("===============================");
                Console.WriteLine("Syncing files serial:");
                SyncFilesSerial();
                Console.WriteLine("Syncing files finished");
                Console.ReadLine();
            }
            else
            {
                Console.WriteLine("Directory does not exsist");
                Console.ReadLine();
            }
        }

        private static void SyncFilesSerial()
        {
            Stopwatch watch = Stopwatch.StartNew();

            outputFiles.ForEach(CheckFile);

            watch.Stop();
            Console.WriteLine(watch.ElapsedMilliseconds + "ms needed for sync");

        }

        private static void CheckFile(string filePath)
        {

            if (File.Exists(inputDir + Path.DirectorySeparatorChar + Path.GetFileName(filePath)))
            {
                HashAlgorithm hash = HashAlgorithm.Create();

                FileInfo fileInfoFirstFile = new FileInfo(filePath);
                FileInfo fileInfoSeoncdFile =
                    new FileInfo(inputDir + Path.DirectorySeparatorChar + Path.GetFileName(filePath));

                if (fileInfoFirstFile.Length != fileInfoSeoncdFile.Length)
                {
                    byte[] fileHash1;
                    byte[] fileHash2;

                    using (FileStream fileStream1 = new FileStream(filePath, FileMode.Open),
                                      fileStream2 =
                                          new FileStream(
                                              inputDir + Path.DirectorySeparatorChar + Path.GetFileName(filePath),
                                              FileMode.Open))
                    {
                        fileHash1 = hash.ComputeHash(fileStream1);
                        fileHash2 = hash.ComputeHash(fileStream2);
                    }

                    if (BitConverter.ToString(fileHash1) != BitConverter.ToString(fileHash2))
                    {
                        Console.WriteLine("File is different: " + inputDir + Path.DirectorySeparatorChar +
                                          Path.GetFileName(filePath));
                    }
                }
            }
            else
            {
                Console.WriteLine("Syncing file " + Path.GetFileName(filePath));
                File.Copy(filePath, inputDir + Path.DirectorySeparatorChar + Path.GetFileName(filePath), true);
            }

        }

        private static void CreateDummyFiles()
        {
            for (int i = 0; i < 12000; i++)
            {
                File.WriteAllText(Environment.CurrentDirectory + Path.DirectorySeparatorChar + "SourceFiles" +
                            Path.DirectorySeparatorChar + i + ".txt", "Inquietude simplicity terminated she compliment remarkably few her nay. The weeks are ham asked jokes. Neglected perceived shy nay concluded. Not mile draw plan snug next all. Houses latter an valley be indeed wished merely in my. Money doubt oh drawn every or an china. Visited out friends for expense message set eat.Ought these are balls place mrs their times add she. Taken no great widow spoke of it small. Genius use except son esteem merely her limits. Sons park by do make on. It do oh cottage offered cottage in written. Especially of dissimilar up attachment themselves by interested boisterous. Linen mrs seems men table. Jennings dashwood to quitting marriage bachelor in. On as conviction in of appearance apartments boisterous. ");
            }
        }

        private static void CompareWithFewerFiles(int count)
        {
            List<string> tmpFiles = outputFiles.GetRange(count, outputFiles.Count - count);

            Console.WriteLine("Compare with files: " + tmpFiles.Count + " files");

            Stopwatch watch = Stopwatch.StartNew();
            tmpFiles.AsParallel().ForAll(f => File.ReadAllBytes(f).GetHashCode());
            watch.Stop();

            Console.WriteLine(watch.ElapsedMilliseconds + " ms needed");

            watch.Restart();
            outputFiles.ForEach(f => File.ReadAllBytes(f).GetHashCode());
            watch.Stop();

            Console.WriteLine(watch.ElapsedMilliseconds + " ms needed");

        }

        private static void CompareWithAllFiles()
        {

            Console.WriteLine("Compare with all files: " + outputFiles.Count + " files");

            Stopwatch watch = Stopwatch.StartNew();
            outputFiles.AsParallel().ForAll(f => File.ReadAllBytes(f).GetHashCode());
            watch.Stop();

            Console.WriteLine(watch.ElapsedMilliseconds + " ms needed");

            watch.Restart();
            outputFiles.ForEach(f => File.ReadAllBytes(f).GetHashCode());
            watch.Stop();

            Console.WriteLine(watch.ElapsedMilliseconds + " ms needed");
        }
    }
}
