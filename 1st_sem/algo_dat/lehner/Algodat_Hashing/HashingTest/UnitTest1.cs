using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Diagnostics;
using System.Collections;
using Algodat_Hashing;

namespace HashingTest
{
    [TestClass]
    public class UnitTest1
    {

        private string[] WordArray { get; set; }

        public UnitTest1()
        {
            string text = "Das Hashverfahren ist ein Algorithmus zum Suchen von Datenobjekten in großen Datenmengen. Es basiert auf der Idee, dass eine mathematische Funktion die Position eines Objektes in einer Tabelle berechnet. Dadurch erübrigt sich das Durchsuchen vieler Datenobjekte, bis das Zielobjekt gefunden wurde.";
            TextParser parser = new TextParser(text);
            this.WordArray = parser.ParseText();
        }

        [TestMethod]
        public void SearchHashlist()
        {
            Hashtable hashTable = new Hashtable();
            foreach (string item in this.WordArray)
            {
                try
                {
                    hashTable.Add(item.GetHashCode(), item);
                }
                catch (ArgumentException)
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
                test = this.WordArray[r.Next(this.WordArray.Length - 1)];

                sw.Start();

                result = hashTable[test.GetHashCode()];

                sw.Stop();

                Assert.AreEqual(test, result);

                averageTicks += sw.ElapsedTicks;
                averageTimeMS += sw.ElapsedMilliseconds;
            }

            Console.WriteLine("Time needed for searching word in hash table: {0} ms - Ticks: {1} ", averageTimeMS / 20, averageTicks / 20);
        }

        [TestMethod]
        public void SearchArray()
        {
            CArray array = new CArray();
            foreach (string item in this.WordArray)
            {
                if (!array.SearchForString(item))
                {
                    array.AddString(item);
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
                test = this.WordArray[r.Next(this.WordArray.Length - 1)];

                sw.Start();

                Assert.IsTrue(array.SearchForString(test));

                sw.Stop();

                averageTicks += sw.ElapsedTicks;
                averageTimeMS += sw.ElapsedMilliseconds;
            }

            Console.WriteLine("Time needed for searching word in Array: {0} ms - Ticks: {1} ", averageTimeMS / 20, averageTicks / 20);
        }
    }
}
