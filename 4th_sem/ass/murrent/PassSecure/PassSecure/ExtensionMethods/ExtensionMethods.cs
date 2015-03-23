#region File Header
// <copyright file="TimeSpanExtensionMethods.cs" company="">
// Copyright (c) 2015 Mario Murrent. All rights reserved.
// </copyright>
// <summary>
// </summary>
// <author>Mario Murrent</author>
#endregion

namespace PassSecure.ExtensionMethods
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Documents;

    using PassSecure.Utils;

    #endregion

    /// <summary>
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// </summary>
        /// <param name="span">
        /// </param>
        /// <returns>
        /// </returns>
        public static byte[] ToByteArray(this TimeSpan span)
        {
            return BitConverter.GetBytes(span.Ticks);
        }

        /// <summary>
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] ToByteArray(this double value)
        {
            return BitConverter.GetBytes(value);
        }

        /// <summary>
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static byte[] ToByteArray(this List<int> list)
        {
            byte[] byteArray = new byte[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                byteArray = ArrayUtils.ConcatArrays(
                    byteArray,
                    BitConverter.GetBytes(list[i]));
            }
            return byteArray;
        }
    }
}
