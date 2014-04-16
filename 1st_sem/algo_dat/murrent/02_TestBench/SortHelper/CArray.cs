// *******************************************************
// * <copyright file="CArray.cs" company="MDMCoWorks">
// * Copyright (c) 2014 Mario Murrent. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Mario Murrent</author>
// *******************************************************/
namespace SortHelper
{
    #region Usings

    using System;

    #endregion

    /// <summary>
    /// </summary>
    public class CArray
    {
        #region Fields

        /// <summary>
        /// </summary>
        private readonly Random random = new Random();

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// </summary>
        /// <param name="maxCount">
        /// </param>
        /// <param name="maxRandomValue">
        /// </param>
        public CArray(int maxCount, int maxRandomValue)
        {
            this.CompareCount = 0;
            this.NumberArray = new int[maxCount];

            for (int i = 0; i < this.NumberArray.Length; ++i)
            {
                this.NumberArray[i] = this.random.Next(maxRandomValue);
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// </summary>
        public int[] ArraySorted
        {
            get
            {
                Array.Sort(this.NumberArray);
                return this.NumberArray;
            }
        }

        /// <summary>
        /// </summary>
        public int[] ArraySortedDescending
        {
            get
            {
                int[] sArray = this.ArraySorted;
                Array.Reverse(sArray);
                return sArray;
            }
        }

        /// <summary>
        /// </summary>
        public int CompareCount { get; set; }

        /// <summary>
        /// </summary>
        public int[] NumberArray { get; set; }

        #endregion
    }
}