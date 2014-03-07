//-----------------------------------------------------------------------
// <copyright file="Comparer.cs" company="MD Development">
//     Copyright (c) MD Development. All rights reserved.
// </copyright>
// <author>Michael Drexler</author>
//-----------------------------------------------------------------------
namespace Trees
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Abstract comparer class
    /// </summary>
    public class Comparer
    {
        /// <summary>
        /// Compare one key with another key
        /// </summary>
        /// <param name="inserted">First key</param>
        /// <param name="secondKey">Second key</param>
        /// <returns>Compare value</returns>
        public int Compare(int inserted, int secondKey)
        {
            if (inserted > secondKey)
            {
                return 1;
            }
            else if (inserted < secondKey)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }
    }
}
