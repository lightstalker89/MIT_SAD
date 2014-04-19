using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections;
using System.Diagnostics;

namespace Algodat_Hashing
{
    class Program
    {
        static void Main(string[] args)
        {
            string text = "Das Hashverfahren ist ein Algorithmus zum Suchen von Datenobjekten in großen Datenmengen. Es basiert auf der Idee, dass eine mathematische Funktion die Position eines Objektes in einer Tabelle berechnet. Dadurch erübrigt sich das Durchsuchen vieler Datenobjekte, bis das Zielobjekt gefunden wurde.";
            TextParser parser = new TextParser(text);
            string[] stringArray = parser.ParseText();
            Hashtable hashTable = new Hashtable();
            foreach (string item in stringArray)
            {
                try
                {
                    hashTable.Add(item.GetHashCode(), item);
                }
                catch (ArgumentException e)
                {
                    Console.WriteLine("Key bereits vorhanden für Value: {0}", item);
                }
            }

            string test = String.Empty;
            Random r = new Random();
            Stopwatch sw = new Stopwatch();
            object result;

            long averageTimeMS = 0;
            long averageTicks = 0;

            for (int i = 0; i < 20; i++)
            {
                test = stringArray[r.Next(stringArray.Length - 1)];

                sw.Start();

                result = hashTable[test.GetHashCode()];

                sw.Stop();

                averageTicks += sw.ElapsedTicks;
                averageTimeMS += sw.ElapsedMilliseconds;
            }

            Console.WriteLine("Time needed for searching word in hash table: {0} ms - Ticks: {1} ", averageTimeMS/20, averageTicks/20);

            sw.Reset();

            List<string> list = new List<string>();
            foreach (string item in stringArray)
            {
                if (!list.Contains(item))
                {
                    list.Add(item);
                }
            }

            for (int i = 0; i < 20; i++)
            {
                test = stringArray[r.Next(stringArray.Length - 1)];

                sw.Start();

                list.Contains(test);

                sw.Stop();

                averageTicks += sw.ElapsedTicks;
                averageTimeMS += sw.ElapsedMilliseconds;
            }

            Console.WriteLine("Time needed for searching word in List: {0} ms - Ticks: {1} ", averageTimeMS / 20, averageTicks / 20);
        }
    }
}
