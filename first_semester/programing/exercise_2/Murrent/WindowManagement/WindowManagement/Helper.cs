// *******************************************************
// * <copyright file="Helper.cs" company="MDMCoWorks">
// * Copyright (c) Mario Murrent. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Mario Murrent</author>
// *******************************************************/
namespace WindowManagement
{
    /// <summary>
    /// Helper class holds extension methods
    /// </summary>
    public static class Helper
    {
        /// <summary>
        /// Extension method to convert an array to a string
        /// </summary>
        /// <param name="array">
        /// The char array
        /// </param>
        /// <param name="occurence">
        /// Specifies how often the array should be added to the string
        /// </param>
        /// <returns>
        /// The concatenated string
        /// </returns>
        public static string ToStringExtended(this char[] array, int occurence)
        {
            string result = string.Empty;

            for (int i = 0; i < occurence; i++)
            {
                result += string.Join(",", array);
            }

            return result;
        }
    }
}