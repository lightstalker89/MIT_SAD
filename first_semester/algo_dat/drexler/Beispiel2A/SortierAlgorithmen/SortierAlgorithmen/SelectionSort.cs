using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SortierAlgorithmen
{
    class SelectionSort : SortAlgorithm
    {
        public override List<int> Sort(List<int> elements)
        {
            // alle Zahlen durchgehen
            for (int i = 0; i < elements.Count; ++i)
            {
                // Position min der kleinsten Zahl ab Position i suchen
                int min = i;
                for (int j = i + 1 ; j < elements.Count; ++j)
                {
                    if (elements[min] > elements[j])
                    {
                        min = j;
                    }
                }

                // Zahl an Position i mit der kleinsten Zahl vertauschen
                int temp = elements[min];
                elements[min] = elements[i];
                elements[i] = temp;
            }

            return elements;
        }

        public override void Output(List<int> sortedList)
        {
            foreach (int item in sortedList)
            {
                Console.Write(item.ToString() + " ");
            }

            Console.WriteLine(Environment.NewLine);
        }
    }
}
