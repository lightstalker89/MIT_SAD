using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SortierAlgorithmen
{
    class InsertionSort : SortAlgorithm
    {
        public override List<int> Sort(List<int> elements)
        {
            for (int i = 1; i < elements.Count; i++)
            {
                int temp = elements[i];
                int j = i - 1;

                while (j >= 0 && elements[j] > temp)
                {
                    elements[j + 1] = elements[j];
                    j--;
                }

                elements[j + 1] = temp;
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
