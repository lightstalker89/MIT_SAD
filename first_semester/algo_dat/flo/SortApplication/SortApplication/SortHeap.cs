using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SortApplication
{
    public class SortHeap
    {
        public int[] heapSort(int[] array)
        {
            generateMaxHeap(array);

            //hier wird sortiert
            for (int i = array.Length - 1; i >= 0; i += -1)
            {
                vertausche(array, i, 0);
                versickern(array, 0, i);
            }
            return array;
        }

        /// <summary>
        /// Erstellt einen MaxHeap Baum im Array
        /// </summary>
        /// <param name="array">Das array</param>
        /// <remarks></remarks>
        private void generateMaxHeap(int[] array)
        {
            //starte von der Mitte rückwärts.
            for (int i = (int)(array.Length / 2 - 1); i >= 1; i += -1)
            {
                versickern(array, i, array.Length);
            }
        }

        /// <summary>
        /// versenkt ein element im baum
        /// </summary>
        /// <param name="array">Das Array</param>
        /// <param name="i">Das zu versenkende Element</param>
        /// <param name="n">Die letzte Stelle im Baum die beachtet werden soll</param>
        /// <remarks></remarks>
        private void versickern(int[] array, int i, int n)
        {
            while (i <= (n / 2 - 1))
            {
                int kindIndex = (i + 1) * 2 - 1;
                //berechnet den Index des linken kind

                //bestimme ob ein rechtes Kind existiert
                if (kindIndex + 1 <= n - 1)
                {
                    //rechtes kind existiert
                    if (array[kindIndex] < array[kindIndex + 1])
                        kindIndex += 1;
                    //wenn rechtes kind größer ist nimm das 
                }

                //teste ob element sinken muss 
                if (array[i] < array[kindIndex])
                {
                    vertausche(array, i, kindIndex);
                    i = kindIndex;
                }
                else { break; }
            }
        }

        /// <summary>
        /// Vertauscht die arraypositionen von i und kindIndex
        /// </summary>
        /// <param name="a">a Das Array in dem getauscht wird</param>
        /// <param name="i">i der erste index</param>
        /// <param name="kindIndex">kindIndex der 2. index</param>
        /// <remarks></remarks>
        private void vertausche(int[] a, int i, int kindIndex)
        {
            int z = a[i];
            a[i] = a[kindIndex];
            a[kindIndex] = z;
        }
    }
}
