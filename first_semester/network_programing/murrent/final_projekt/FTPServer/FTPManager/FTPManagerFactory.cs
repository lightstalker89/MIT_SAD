// *******************************************************
// * <copyright file="FTPManagerFactory.cs" company="MDMCoWorks">
// * Copyright (c) 2013 Mario Murrent. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Mario Murrent</author>
// *******************************************************/
namespace FTPManager
{
    /// <summary>
    /// </summary>
    public class FTPManagerFactory
    {
        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public static IFTPManager CreateFTPManager()
        {
            return new FTPManager();
        }
    }
}