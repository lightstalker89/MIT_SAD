using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SortierAlgorithmen
{
    class BubbleSort : SortAlgorithm
    {
        public override List<int> Sort(List<int> elements)
        {
            for (int i = 0; i < elements.Count; ++i)
            {
                for (int a = i + 1; a < elements.Count; ++a)
                {
                    if (elements[i] > elements[a])
                    {
                        int temp = elements[a];
                        elements[a] = elements[i];
                        elements[i] = temp;
                    }
                }
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
