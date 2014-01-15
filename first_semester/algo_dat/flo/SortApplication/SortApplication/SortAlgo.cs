using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SortApplication
{
    public class SortAlgo
    {
        /// <summary>
        /// finde das kleinste Element a[i] aus i=0..N und
        /// tausche es gegen das erste a[0]
        /// finde das kleinste Element aus a[i], i=1..N
        /// und tausche es gegen a[1] usw.
        /// </summary>
        /// <param name="array">Das zu sortierende array</param>
        /// <returns>Das sortierte array</returns>
        public int[] selectionSort(int[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                // Position min der kleinsten Zahl ab Position i suchen
                int min = i;
                for (int j = i + 1; j < array.Length; j++)
                    if (array[j] < array[min])
                        min = j;

                // Zahl an Position i mit der kleinsten Zahl vertauschen
                int tmp = array[min];
                array[min] = array[i];
                array[i] = tmp;
            }
            return array;
        }


        /// <summary>
        /// Durchlaufe wiederholt die Daten und
        /// vertausche jedesmal die Nachbarn, falls
        /// notwendig.
        /// Wenn ein kompletter Durchlauf ohne Tausch
        /// gelingt, sind die Daten sortiert.
        /// </summary>
        /// <param name="array">Das zu sortierende array</param>
        /// <returns>Das sortierte array</returns>
        public int[] bubbleSort(int[] array)
        {
            for (int i = 0; i < array.Length - 1; ++i)
            {
                for (int j = 0; j < array.Length - i - 1; ++j)
                {
                    if (array[j] > array[j + 1])
                    {
                        int tmp = array[j];
                        array[j] = array[j + 1];
                        array[j + 1] = tmp;
                    }
                }
            }
            return array;
        }

        /// <summary>
        /// Suche a[i], hinter welchem ein neues Element
        /// eingefügt werden muß
        /// Verschiebe a[i+1]..a[N-1] nach hinten
        /// kopiere neues Element nach a[i+1] 
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public int[] insertionSort(int[] array)
        {
            for (int j = 0; j < array.Length; j++)
            {
                int key = array[j];
                int i = j - 1;

                while (i >= 0 && array[i] > key)
                {
                    array[i + 1] = array[i];
                    i = i - 1;
                }
                array[i + 1] = key;
            }

            return array;
        }
    }
}
