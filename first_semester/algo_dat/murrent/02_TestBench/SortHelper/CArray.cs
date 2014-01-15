using System;

namespace SortHelper
{
    public class CArray
    {
        private readonly Random random = new Random();
        
        public CArray(int maxCount, int maxRandomValue)
        {
            this.CompareCount = 0;
            this.NumberArray = new int[maxCount];

            for (int i = 0; i < this.NumberArray.Length; ++i)
            {
                this.NumberArray[i] = random.Next(maxRandomValue);
            }
        }

        public int[] NumberArray { get; set; }

        public int CompareCount { get; set; }

        public int[] ArraySorted
        {
            get
            {
                Array.Sort(this.NumberArray);
                return this.NumberArray;
            }
        }
    }
}
