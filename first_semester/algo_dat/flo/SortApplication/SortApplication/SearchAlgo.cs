using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SortApplication
{
    public class SearchAlgo
    {
        public int linearSearch(int[] array, int search)
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == search)
                {
                    return search;
                }
            }
            return -1;
        }

        public int BinarySearch(int[] array, int lowBound, int highBound, int search)
        {
            int mid;
            while (lowBound <= highBound)
            {
                mid = (lowBound + highBound) / 2;
                if (array[mid] < search)//the element we search is located to the right from the mid point
                {
                    lowBound = mid + 1;
                    continue;
                }
                else if (array[mid] > search)//the element we search is located to the left from the mid point
                {
                    highBound = mid - 1;
                    continue;
                }
                //at this point low and high bound are equal and we have found the element or
                //array[mid] is just equal to the search => we have found the searched element
                else
                {
                    return array[mid];
                }
            }
            return -1;
        }
    }
}
