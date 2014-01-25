using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algodat_UE3.SearchAlgos
{
    public class LinearSearch
    {
        public int CompareCount {get; private set; }

        public LinearSearch()
        {
            this.CompareCount = 0;
        }

        public int Search(int number, int[] compareArray)
        {
            foreach (int item in compareArray)
            {
                this.CompareCount++;
                if (item == number)
                {
                    return item;
                }
            }

            return -1;
        }
    }
}
