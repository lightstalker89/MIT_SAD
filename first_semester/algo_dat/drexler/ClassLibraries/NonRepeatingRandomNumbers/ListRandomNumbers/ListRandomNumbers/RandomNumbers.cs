//-----------------------------------------------------------------------
// <copyright file="RandomNumbers.cs" company="MD Development">
//     Copyright (c) MD Development. All rights reserved.
// </copyright>
// <author>Michael Drexler</author>
//-----------------------------------------------------------------------
namespace ListRandomNumbers
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Non repeating random numbers
    /// </summary>
    public class RandomNumbers
    {
        /// <summary>
        /// Generated no repeating random numbers;
        /// </summary>
        private ArrayList randomNumbers;

        /// <summary>
        /// Initializes a new instance of the <see cref="RandomNumbers"/> class
        /// </summary>
        public RandomNumbers()
        {
        }

        /// <summary>
        /// Generates a non repeating list of random numbers
        /// </summary>
        /// <param name="max">Maximum of the range for random numbers</param>
        /// <returns>List of non repeating random numbers</returns>
        public ArrayList GetNonRepeatingRandomNumbers(int max)
        {
            // Create an ArrayList object that will hold the numbers
            ArrayList lstNumbers = new ArrayList();
            // The Random class will be used to generate numbers
            Random rndNumber = new Random();

            // Generate a random number between 1 and the Max
            int number = rndNumber.Next(1, max + 1);
            // Add this first random number to the list
            lstNumbers.Add(number);
            // Set a count of numbers to 0 to start
            int count = 0;

            do // Repeatedly...
            {
                // ... generate a random number between 1 and the Max
                number = rndNumber.Next(1, max + 1);

                // If the newly generated number in not yet in the list...
                if (!lstNumbers.Contains(number))
                {
                    // ... add it
                    lstNumbers.Add(number);
                }

                // Increase the count
                count++;
            } while (count <= 10 * max); // Do that again

            this.randomNumbers = lstNumbers;
            // Once the list is built, return it
            return lstNumbers;
        }

        /// <summary>
        /// Representing the items as a string
        /// </summary>
        /// <returns>Items as a string</returns>
        public string MyToString()
        {
            string values = string.Empty;

            if (this.randomNumbers != null && this.randomNumbers.Count > 0)
            {
                foreach (int item in this.randomNumbers)
                {
                    values += item.ToString() + " ";
                }
            }

            return values;
        }
    }
}
