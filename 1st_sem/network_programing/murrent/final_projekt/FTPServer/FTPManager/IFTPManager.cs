// *******************************************************
// * <copyright file="IFTPManager.cs" company="MDMCoWorks">
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
    public interface IFTPManager
    {
        /// <summary>
        /// </summary>
        void Start();

        /// <summary> 
        /// </summary>
        event FTPManager.ServerStartedEventHandler ServerStarted;

        /// <summary>
        /// </summary>
        event FTPManager.ProgressUpdateHandler ProgressUpdate;
    }
}