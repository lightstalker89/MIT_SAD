using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListRandomNumbers
{
    public class CArray
    {
        private int maxCount;
        private int[] unsortedArray;

        /// <summary>
        /// Initializes a new instance of the <see cref="CArray"/> class
        /// </summary>
        /// <param name="maxCount">Max size of the array</param>
        public CArray(int maxCount, int maxValue)
        {
            this.maxCount = maxCount;
            this.unsortedArray = new int[this.maxCount];
            this.Init(maxValue);
        }

        /// <summary>
        /// Gets an unsorted array of numbers
        /// </summary>
        public int[] UnsortedArray
        {
            get { return unsortedArray; }
        }

        /// <summary>
        /// Gets an sorted array of numbers
        /// </summary>
        public int[] SortedArray
        {
            get
            {
                Array.Sort(this.unsortedArray);
                return this.unsortedArray;
            }
        }

        /// <summary>
        /// Gets an sorted descending array of numbers
        /// </summary>
        public int[] SortedDescendingArray
        {
            get
            {
                int[] sortedDescendingArray = this.SortedArray;
                Array.Reverse(sortedDescendingArray);
                return sortedDescendingArray;
            }
        }

        /// <summary>
        /// Gets or sets a value for CompareCount
        /// </summary>
        public int CompareCount { get; set; }

        /// <summary>
        /// Initializes the array with random numbers
        /// </summary>
        /// <param name="maxValue">Maximum value of the random numbers</param>
        private void Init(int maxValue)
        {
            Random random = new Random();

            for (int i = 0; i < this.maxCount; ++i)
            {
                int nextNumber;

                do
                {
                    nextNumber = random.Next(1, maxValue);
                } 
                while (this.unsortedArray.Contains(nextNumber));

                this.unsortedArray[i] = nextNumber;
            }
        }
    }
}
