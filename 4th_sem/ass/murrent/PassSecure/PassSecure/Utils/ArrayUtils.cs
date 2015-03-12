#region File Header
// <copyright file="ArrayUtils.cs" company="">
// Copyright (c) 2015 Mario Murrent. All rights reserved.
// </copyright>
// <summary>
// </summary>
// <author>Mario Murrent</author>
#endregion

namespace PassSecure.Utils
{
    #region Usings

    using System.Linq;

    #endregion

    /// <summary>
    /// </summary>
    public static class ArrayUtils
    {
        /// <summary>
        /// </summary>
        /// <param name="list">
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        /// </returns>
        public static T[] ConcatArrays<T>(params T[][] list)
        {
            var result = new T[list.Sum(a => a.Length)];
            int offset = 0;
            for (int x = 0; x < list.Length; x++)
            {
                list[x].CopyTo(result, offset);
                offset += list[x].Length;
            }

            return result;
        }
    }
}
