using System;

namespace SortHelper
{
    public class CArray
    {
        private readonly Random random = new Random();

        public CArray(int maxCount, int maxRandomValue)
        {
            this.Array = new int[maxCount];

            for (int i = 0; i < this.Array.Length; ++i)
            {
                this.Array[i] = random.Next(maxRandomValue);
            }
        }

        public int[] Array { get; set; }
    }
}
