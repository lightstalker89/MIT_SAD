// *******************************************************
// * <copyright file="RecursionSample.cs" company="MDMCoWorks">
// * Copyright (c) 2014 Mario Murrent. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Mario Murrent</author>
// *******************************************************/
namespace RecursionSample
{
    #region Usings

    using System;
    using System.Numerics;

    #endregion

    /// <summary>
    /// The <see ref="RecursionSample"/> class and its interaction logic 
    /// </summary>
    public class RecursionSample
    {
        /// <summary>
        /// Calculates the factorial normal.
        /// </summary>
        /// <param name="number">
        /// The number.
        /// </param>
        /// <returns>
        /// The result of the calculation
        /// </returns>
        /// <exception cref="System.ArgumentException">
        /// Number must be positive
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// number;Numbers must be less than 20
        /// </exception>
        /// <exception cref="ArgumentException">
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// </exception>
        public BigInteger CalculateFactorialNormal(int number)
        {
            if (number < 0)
            {
                throw new ArgumentException("Number must be positive");
            }

            BigInteger x = 1;

            for (int i = 1; i <= number; ++i)
            {
                x *= i;
            }

            return x;
        }

        /// <summary>
        /// Calculates the factorial with recursion.
        /// </summary>
        /// <param name="number">
        /// The number
        /// </param>
        /// <returns>
        /// The result of the calculation
        /// </returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// number;Numbers must be less than 20
        /// </exception>
        public BigInteger CalculateFactorialWithRecursion(long number)
        {
            if (number < 0)
            {
                throw new ArgumentException("Number must be positive");
            }

            if (number > 1)
            {
                return this.CalculateFactorialWithRecursion(number - 1) * number;
            }

            return 1;
        }

        /// <summary>
        /// Calculates the factorial with tail recursion.
        /// </summary>
        /// <param name="number">
        /// The number.
        /// </param>
        /// <param name="product">
        /// The product.
        /// </param>
        /// <returns>
        /// The result
        /// </returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// number;Numbers must be less than 20
        /// </exception>
        public BigInteger CalculateFactorialWithTailRecursion(int number, BigInteger product)
        {
            if (number < 0)
            {
                throw new ArgumentException("Number must be positive");
            }

            return number < 2 ? product : this.CalculateFactorialWithTailRecursion(number - 1, number * product);
        }
    }
}