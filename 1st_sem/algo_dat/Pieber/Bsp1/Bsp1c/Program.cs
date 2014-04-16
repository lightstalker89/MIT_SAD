using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bsp1c
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Faktorielle von n: n!
            // Rekursion
            // Tail Rekursion
            // ohne Rekursion

            ulong n = 8;

            ulong fac = FactorialStdRecursion(n);
            Console.WriteLine("{0}! (standard): {1}", n, fac);

            fac = FactorialTailRecursion(n);
            Console.WriteLine("{0}! (tail):     {1}", n, fac);

            fac = FactorialLoop(n);
            Console.WriteLine("{0}! (loop):     {1}", n, fac);

            Console.WriteLine();
            Console.WriteLine("A result of -1 for a factorial describes an invalid number.");

            Console.ReadKey();
        }

        public static ulong FactorialStdRecursion(ulong n)
        {
            if (n <= 1)
            {
                return Math.Max(n, 1);
            }

            // needed only for numerical types which allows negativ values
            //if (n < 0)
            //{
            //    return -1;
            //}

            return n * FactorialStdRecursion(n - 1);
        }

        public static ulong FactorialTailRecursion(ulong n, ulong fac = 1)
        {
            if (n <= 1)
            {
                return fac;
            }

            // needed only for numerical types which allows negativ values
            //if (n < 0)
            //{
            //    return -1;
            //}

            return FactorialTailRecursion(n - 1, n * fac);
        }

        public static ulong FactorialLoop(ulong n)
        {
            ulong fac = 1;

            for (ulong i = 2; i <= n; ++i)
            {
                fac *= i;
            }

            // needed only for numerical types which allows negativ values
            //if (n < 0)
            //{
            //    fac = -1;
            //}

            return fac;
        }
    }
}
