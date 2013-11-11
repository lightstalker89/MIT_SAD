// *******************************************************
// * <copyright file="StringHelper.cs" company="MDMCoWorks">
// * Copyright (c) 2013 Mario Murrent. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Mario Murrent</author>
// *******************************************************/
namespace BiOWheelsFileWatcher
{
    using System;
    using System.IO;

    /// <summary>
    /// Class representing the string helper methods for the <see cref="FileWatcher"/>
    /// </summary>
    public static class StringHelper
    {
        /// <summary>
        /// Determines whether the specified input is directory.
        /// </summary>
        /// <param name="input">
        /// The input.
        /// </param>
        /// <returns>
        /// A value indicating whether the string is a directory or not
        /// </returns>
        public static bool IsDirectory(this string input)
        {
            bool retValue = false;

            try
            {
                if (Directory.Exists(input))
                {
                    retValue = true;
                }
            }
            catch (Exception)
            {
                retValue = false;
            }

            return retValue;
        }
    }
}