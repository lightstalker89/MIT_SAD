// *******************************************************
// * <copyright file="RecursionSample.cs" company="MDMCoWorks">
// * Copyright (c) 2013 Mario Murrent. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Mario Murrent</author>
// *******************************************************/
namespace RecursionSample
{
    /// <summary>
    /// </summary>
    public class RecursionSample
    {
        public long CalculateFactorialNormal(int number)
        {
            long x = 1;

            for (int i = 1; i <= number; i++)
            {
                x *= i;
            }

            return x;
        }

        public long CalculateFactorialWithRecursion(int number)
        {
            return 0;
        }

        public long CalculateFactorialWithTailRecursion(int number)
        {
            return 0;
        }
    }
}