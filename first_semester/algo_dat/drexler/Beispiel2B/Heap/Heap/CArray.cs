using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heap
{
    public class CArray
    {
        private int maxCount;
        private int[] array;

        /// <summary>
        /// Initializes a new instance of the <see cref="CArray"/> class
        /// </summary>
        /// <param name="maxCount">Max size of the array</param>
        public CArray(int maxCount)
        {
            this.maxCount = maxCount;
            this.array = new int[this.maxCount];
        }

        /// <summary>
        /// Gets an unsorted array
        /// </summary>
        public int[] Array
        {
            get { return array; }
        }

        /// <summary>
        /// Initializes the array with random numbers
        /// </summary>
        /// <param name="maxValue">Maximum value of the random numbers</param>
        public void Init(int maxValue)
        {
            Random random = new Random();

            for (int i = 0; i < this.maxCount; ++i)
            {
                int nextNumber;

                do
                {
                    nextNumber = random.Next(1, maxValue);
                }
                while (this.array.Contains(nextNumber));

                this.array[i] = nextNumber;
            }
        }
    }
}
