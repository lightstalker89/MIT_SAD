using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beispiel1C
{
    public class Fakultaet
    {
        /// <summary>
        /// Berechnung der Fakultät mit der Standard Rekursion
        /// Hier wird das Resultat immer als Return-Wert der Funktion
        /// übergeben => Am Stack zwischengespeichert
        /// </summary>
        /// <param name="n">Faktor</param>
        public static long BerechneStandardRekusion(long n)
        {
            if (n <= 1)
            {
                return 1;
            }

            return n * BerechneStandardRekusion(n - 1);
        }

        /// <summary>
        /// Berechnung der Fakultät mit der Tail Rekursion
        /// Hier wird das Resultat als Parameter der Funktion übergeben
        /// Der Unterschied => Return-Wert muss nicht am Stack gespeichert werden
        /// und es steht dem neuen rekursiven Funktionsaufruf wieder der ganze Stack zur Verfügung
        /// </summary>
        /// <param name="n">Faktor</param>
        public static long BerechneTailRekursion(int n, long result)
        {
            if (n <= 1)
            {
                return result;
            }

            return BerechneTailRekursion(n - 1, result * n);
        }

        /// <summary>
        /// Die aufgelöste Version ohne Rekursion zur Berechnung der Fakultät. 
        /// </summary>
        /// <param name="n">Faktor</param>
        /// <param name="a"></param>
        public static int BerechneOhneRekursion(int n)
        {
            int temp = 1;

            if (n < 0)
            {
                throw new ArgumentOutOfRangeException("Unvalid parameter");
            }
            else if (n == 0 || n == 1)
            {
                return 1;
            }

            for (int i = 1; i <= n; ++i)
            {
                temp *= i;
            }

            return temp;
        }
    }
}
