using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heap
{
    class HeapSort
    {
        public ArrayList Sort(ArrayList unsortedList)
        {
            generateMaxHeap(unsortedList);

            //hier wird sortiert
            for (int i = unsortedList.Count - 1; i >= 0; --i)
            {
                Vertausche(unsortedList, i, 0);
                Versickere(unsortedList, 0, i);
            }

            return unsortedList;
        }

        private void generateMaxHeap(ArrayList unsortedList)
        {
            //starte von der Mitte rückwärts.
            for (int i = (int)(unsortedList.Count / 2 - 1); i >= 1; --i)
            {
                Versickere(unsortedList, i, unsortedList.Count);
            }
        }

        public void Versickere(ArrayList unsortedList, int i, int n)
        {
            while (i <= (n / 2 - 1))
            {
                int kindIndex = (i + 1) * 2 - 1;
                //berechnet den Index des linken kind

                //bestimme ob ein rechtes Kind existiert
                if (kindIndex + 1 <= n - 1)
                {
                    int leftChild = (int)unsortedList[kindIndex];
                    int rightChild = (int)unsortedList[kindIndex + 1];
                    //rechtes kind existiert
                    if (leftChild < rightChild)
                        kindIndex += 1;
                    //wenn rechtes kind größer ist nimm das 

                }

                //teste ob element sinken muss 
                int element = (int)unsortedList[i];
                int child = (int)unsortedList[kindIndex];
                if (element < child)
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

        public void Vertausche(ArrayList unsortedList, int parentIndex, int childIndex)
        {
            int temp = (int)unsortedList[parentIndex];
            unsortedList[parentIndex] = unsortedList[childIndex];
            unsortedList[childIndex] = temp;
        }


        public void Output(ArrayList sortedList)
        {
            foreach (int item in sortedList)
            {
                Console.Write(item.ToString() + " ");
            }

            Console.WriteLine();
        }
    }
}
