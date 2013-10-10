using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace FileCompare
{
    class Program
    {
        private readonly static List<string> InputFiles = Directory.GetFiles(Environment.CurrentDirectory + Path.DirectorySeparatorChar + "SourceFiles", "*.*", SearchOption.AllDirectories).ToList();
        private static readonly List<string> OutputFiles = Directory.GetFiles(Environment.CurrentDirectory + Path.DirectorySeparatorChar + "DestinationFiles", "*.*", SearchOption.AllDirectories).ToList();

        static void Main(string[] args)
        {
            //If u need a few files :)
            //CreateDummyFiles();

            CompareWithAllFiles();
            Console.WriteLine("===============================");
            CompareWithFewerFiles(8000);
            Console.WriteLine("===============================");
            CompareWithFewerFiles(5000);
            Console.WriteLine("===============================");
            CompareWithFewerFiles(2500);
            Console.WriteLine("===============================");
            CompareWithFewerFiles(1000);
            Console.WriteLine("===============================");
            CompareWithFewerFiles(500);

            Console.ReadLine();
        }

        private static void CreateDummyFiles()
        {
            for (int i = 0; i < 12000; i++)
            {
                File.WriteAllText(Environment.CurrentDirectory + Path.DirectorySeparatorChar + "SourceFiles" +
                            Path.DirectorySeparatorChar + i + ".txt","Inquietude simplicity terminated she compliment remarkably few her nay. The weeks are ham asked jokes. Neglected perceived shy nay concluded. Not mile draw plan snug next all. Houses latter an valley be indeed wished merely in my. Money doubt oh drawn every or an china. Visited out friends for expense message set eat.Ought these are balls place mrs their times add she. Taken no great widow spoke of it small. Genius use except son esteem merely her limits. Sons park by do make on. It do oh cottage offered cottage in written. Especially of dissimilar up attachment themselves by interested boisterous. Linen mrs seems men table. Jennings dashwood to quitting marriage bachelor in. On as conviction in of appearance apartments boisterous. ");
            }
        }

        private static void CompareWithFewerFiles(int count)
        {
            List<string> tmpFiles = OutputFiles.GetRange(count, OutputFiles .Count- count);

            Console.WriteLine("Compare with files: " + tmpFiles.Count + " files");

            Stopwatch watch = Stopwatch.StartNew();
            tmpFiles.AsParallel().ForAll(f => File.ReadAllBytes(f).GetHashCode());
            watch.Stop();

            Console.WriteLine(watch.ElapsedMilliseconds + " ms needed");

            watch.Restart();
            OutputFiles.ForEach(f => File.ReadAllBytes(f).GetHashCode());
            watch.Stop();

            Console.WriteLine(watch.ElapsedMilliseconds + " ms needed");

        }

        private static void CompareWithAllFiles()
        {

            Console.WriteLine("Compare with all files: " + OutputFiles.Count + " files");

            Stopwatch watch = Stopwatch.StartNew();
            OutputFiles.AsParallel().ForAll(f => File.ReadAllBytes(f).GetHashCode());
            watch.Stop();

            Console.WriteLine(watch.ElapsedMilliseconds + " ms needed");

            watch.Restart();
            OutputFiles.ForEach(f => File.ReadAllBytes(f).GetHashCode());
            watch.Stop();

            Console.WriteLine(watch.ElapsedMilliseconds + " ms needed");
        }
    }
}
