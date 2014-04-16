// *******************************************************
// * <copyright file="DataConnectionOperation.cs" company="MDMCoWorks">
// * Copyright (c) 2013 Mario Murrent. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Mario Murrent</author>
// *******************************************************/

namespace FTPManager
{
    using System;
    using System.Net.Sockets;

    /// <summary>
    /// </summary>
    public sealed class DataConnectionOperation
    {
        /// <summary>
        /// </summary>
        public Func<NetworkStream, string, string> Operation { get; set; }

        /// <summary>
        /// </summary>
        public string Arguments { get; set; }
    }
}