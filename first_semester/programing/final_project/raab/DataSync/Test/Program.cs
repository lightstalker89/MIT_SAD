using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DataSync
{
    class Program
    {
        static void Main(string[] args)
        {
            FileSystemWatcher fsw = new FileSystemWatcher(@"D:\FH\Master\Programmieren\DataSync\WATCH1");
            fsw.Changed += fsw_Changed;
            fsw.Deleted += fsw_Deleted;
            fsw.Created += fsw_Created;
            fsw.Renamed += fsw_Renamed;


            fsw.EnableRaisingEvents = true;
            fsw.IncludeSubdirectories = true;

            Console.Read();
        }

        static void fsw_Renamed(object sender, RenamedEventArgs e)
        {
            Console.WriteLine("Renamed: {0}, to: {1}", e.FullPath, e.OldFullPath);
        }

        static void fsw_Created(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine("Created: {0}", e.FullPath);
        }

        static void fsw_Deleted(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine("Deleted: {0}", e.FullPath);
        }

        static void fsw_Changed(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine("Changed: {0}", e.FullPath);
        }
    }
}
