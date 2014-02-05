using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heap
{
    class HeapSort
    {
        public List<int> Sort(List<int> unsortedList)
        {
            generateMaxHeap(unsortedList);

            //hier wird sortiert
            for (int i = unsortedList.Count - 1; i >= 0; i += -1)
            {
                Vertausche(unsortedList, i, 0);
                Versickere(unsortedList, 0, i);
            }

            return unsortedList;
        }

        private void generateMaxHeap(List<int> unsortedList)
        {
            //starte von der Mitte rückwärts.
            for (int i = (int)(unsortedList.Count / 2 - 1); i >= 1; i += -1)
            {
                Versickere(unsortedList, i, unsortedList.Count);
            }
        }

        public void Versickere(List<int> unsortedList, int i, int n)
        {
            while (i <= (n / 2 - 1))
            {
                int kindIndex = (i + 1) * 2 - 1;
                //berechnet den Index des linken kind

                //bestimme ob ein rechtes Kind existiert
                if (kindIndex + 1 <= n - 1)
                {
                    //rechtes kind existiert
                    if (unsortedList[kindIndex] < unsortedList[kindIndex + 1])
                        kindIndex += 1;
                    //wenn rechtes kind größer ist nimm das 

                }

                //teste ob element sinken muss 
                if (unsortedList[i] < unsortedList[kindIndex])
                {
                    Vertausche(unsortedList, i, kindIndex);
                    i = kindIndex;
                }
                else 
                { 
                    break; 
                }
            }
        }

        public void Vertausche(List<int> unsortedList, int parentIndex, int childIndex)
        {
            int temp = unsortedList[parentIndex];
            unsortedList[parentIndex] = unsortedList[childIndex];
            unsortedList[childIndex] = temp;
        }


        public void Output(List<int> sortedList)
        {
            foreach (int item in sortedList)
            {
                Console.Write(item.ToString() + " ");
            }

            Console.WriteLine();
        }
    }
}
