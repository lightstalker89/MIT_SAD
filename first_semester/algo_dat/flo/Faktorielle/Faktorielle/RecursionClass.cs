using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Faktorielle
{
    public class RecursionClass
    {
        /// <summary>
        /// Calculate the faculty of n with the standard recursion.
        /// The problem with this method is, that we could get an
        /// stack overflow exception. The last operation  in the
        /// method is actually the multiplication, which can’t be executed
        /// until we know the result of FacultyRecursionStandard(n-1).
        /// </summary>
        /// <param name="n">
        /// The faculty
        /// </param>
        public double FacultyRecursionStandard(double n)
        {
            if (n == 0)
            {
                return 1;
            }
            else
            {
                return n * FacultyRecursionStandard(n - 1);
            }
        }

        /// <summary>
        /// Calculate the faculty of n with the tail recursion.
        /// Calculating only in the winding phase so that
        /// no Stack overflow exception can occure.
        /// </summary>
        /// <param name="n">
        /// The faculty
        /// </param>
        /// /// <param name="na">
        /// The result.
        /// </param>
        public double FacultyRecursionTail(double n, double na)
        {
            if (n < 2)
            {
                return na;
            }
            return FacultyRecursionTail(n - 1, n * na); // Calculation in winding phase
        }

        /// <summary>
        /// This solution calculates the faculty
        /// with a loop instead of using a recursive function.
        /// </summary>
        /// <param name="n">
        /// The faculty
        /// </param>
        public double FacultyLoop(double n)
        {
            if (n < 0)
            {
                throw new ArgumentOutOfRangeException("Illegal operation");
            }
            else if (n == 0 || n == 1)
            {
                return 1;
            }
            double faculty = 1;
            for (double i = 1; i <= n; i++)
            {
                faculty *= i;
            }

            return faculty;
        }
    }
}
