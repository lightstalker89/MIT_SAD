using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Beispiel1C;
using System.Collections.Generic;
using System.Diagnostics;

namespace UnitTestBeispiel1C
{
    [TestClass]
    public class UnitTestFakultaet
    {
        private Dictionary<int, int> factorial;
        private Stopwatch stopwatch = new Stopwatch();

        [TestInitialize]
        public void Init()
        {
            factorial = new Dictionary<int, int>
            {
                {0, 1},
                {1, 1},
                {2, 2},
                {3, 6},
                {4, 140},
                {5, 550},
                {6, 800},
                {7, 1000},
                {8, 2000},
                {9, 4000},
                {10, 8000},
                {11, 16000},
                {12, 32000},
                {13, 64000},
                {14, 128000},
                {15, 256000},
                {16, 512000},
                {17, 1000000},
                {18, 3000000},
                {19, 1000000000},
            };

            this.stopwatch = new Stopwatch();
        }

        [TestMethod]
        public void BerechneStandardReksuion()
        {
            long result = Fakultaet.BerechneStandardRekusion(5);
            Assert.AreEqual(120, result);
        }

        [TestMethod]
        public void BerechneLimitStandardRekusion()
        {
            long lastItem = 0;
            // Untersuche die Grenzen der Funktionen

            try
            {
                stopwatch.Reset();
                stopwatch.Start();

                foreach (int item in this.factorial.Keys)
                {
                    int value = 0;
                    this.factorial.TryGetValue(item, out value);
                    lastItem = item;
                    long result = Fakultaet.BerechneStandardRekusion(value);
                }
            }
            catch (StackOverflowException ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                stopwatch.Stop();
                Debug.WriteLine("Time for standard recursion {0}ms - Last item: {1}", stopwatch.ElapsedMilliseconds, lastItem);  
            }      
        }

        [TestMethod]
        public void BerechneLimitTailRekusion()
        {
            long lastItem = 0;
            // Untersuche die Grenzen der Funktionen

            try
            {
                stopwatch.Reset();
                stopwatch.Start();

                foreach (int item in this.factorial.Keys)
                {
                    int value = 0;
                    this.factorial.TryGetValue(item, out value);
                    lastItem = item;
                    long result = 0;
                    long returnValue = Fakultaet.BerechneTailRekursion(value, result);
                }
            }
            catch (StackOverflowException ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                stopwatch.Stop();
                Debug.WriteLine("Time for tail recursion {0}ms - Last item: {1}", stopwatch.ElapsedMilliseconds, lastItem);
            }
        }

        [TestMethod]
        public void BerechneLimitOhneRekusion()
        {
            long lastItem = 0;
            // Untersuche die Grenzen der Funktionen

            try
            {
                stopwatch.Reset();
                stopwatch.Start();

                foreach (int item in this.factorial.Keys)
                {
                    int value = 0;
                    this.factorial.TryGetValue(item, out value);
                    lastItem = item;
                    long returnValue = Fakultaet.BerechneOhneRekursion(value);
                }
            }
            catch (StackOverflowException ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                stopwatch.Stop();
                Debug.WriteLine("Time for tail recursion {0}ms - Last item: {1}", stopwatch.ElapsedMilliseconds, lastItem);
            }
        }

        [TestMethod]
        public void BerechneTailRekursion()
        {
            long result = Fakultaet.BerechneTailRekursion(5, 1);
            Assert.AreEqual(120, result);
        }

        [TestMethod]
        public void BerechneOhneRekursion()
        {
            int result = Fakultaet.BerechneOhneRekursion(5);
            Assert.AreEqual(120, result);
        }
    }
}
