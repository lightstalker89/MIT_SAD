using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Numerics;
using Beispiel1C;
using System.Collections.Generic;
using System.Diagnostics;

namespace UnitTestBeispiel1C
{
    [TestClass]
    public class UnitTestFakultaet
    {
        private Dictionary<long, long> factorial;
        private Stopwatch stopwatch = new Stopwatch();

        [TestInitialize]
        public void Init()
        {
            factorial = new Dictionary<long, long>
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
            BigInteger result = Fakultaet.BerechneStandardRekusion(5);
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

                foreach (long item in this.factorial.Keys)
                {
                    long value = 0;
                    this.factorial.TryGetValue(item, out value);
                    lastItem = item;
                    Debug.WriteLine("Fakturielle von {0} ist {1}", value, Fakultaet.BerechneStandardRekusion(value).ToString());
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

                foreach (long item in this.factorial.Keys)
                {
                    long value = 0;
                    this.factorial.TryGetValue(item, out value);
                    lastItem = item;
                    BigInteger result = 1;
                    Debug.WriteLine("Fakturielle von {0} ist {1}", value, Fakultaet.BerechneTailRekursion(value, result).ToString());
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

                foreach (long item in this.factorial.Keys)
                {
                    long value = 0;
                    this.factorial.TryGetValue(item, out value);
                    lastItem = item;
                    Debug.WriteLine("Fakturielle von {0} ist {1}", value, Fakultaet.BerechneOhneRekursion(value).ToString());
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
            BigInteger result = Fakultaet.BerechneTailRekursion(5, 1);
            Assert.AreEqual(120, result);
        }

        [TestMethod]
        public void BerechneOhneRekursion()
        {
            BigInteger result = Fakultaet.BerechneOhneRekursion(5);
            Assert.AreEqual(120, result);
        }
    }
}
