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
                    return ++i;
                }
            }
            return -1;
        }

        public int BinarySearch(int[] array, int search)
        {
            int mid;
            int index = 0;
            int lowBound = 0;
            int highBound = array.Length;
            while (lowBound <= highBound)
            {
                ++index;
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
                    return index;
                }
            }
            return -1;
        }

        public int FibonacciSearch(int[] array, int n, double x)
        {
            int inf=0, pos, k;
            int kk = -1, nn = -1;
            int[] fib = new int[]{0, 1, 1, 2, 3, 5, 8, 13, 21, 34, 55, 89, 144, 233, 377, 610, 987, 1597, 2584, 4181, 6765, 10946, 17711, 28657, 46368, 75025, 121393, 196418, 317811, 514229, 832040, 1346269, 2178309, 3524578, 5702887, 9227465, 14930352, 24157817, 39088169, 63245986, 102334155, 165580141};

            if (nn != n)
            {
                k = 0;
                while (fib[k] < n) k++;
                kk = k;
                nn = n;
            }
            else
            {
                k = kk;
            }
            while(k>0)
            {
                pos=inf+fib[--k];
                if((pos>=n)||(x<array[pos]));
                else if (x>array[pos])
                {
                    inf=pos+1;
                    k--;
                }
 
                else {
                    return pos;
                }
            }
            return -1;
        }
    }
}
