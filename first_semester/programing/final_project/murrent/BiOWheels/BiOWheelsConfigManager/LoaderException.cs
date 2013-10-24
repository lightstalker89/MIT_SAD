// *******************************************************
// * <copyright file="LoaderException.cs" company="MDMCoWorks">
// * Copyright (c) Mario Murrent. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Mario Murrent</author>
// *******************************************************/
namespace BiOWheelsConfigManager
{
    using System;

    /// <summary>
    /// </summary>
    public class LoaderException
    {
        /// <summary>
        /// </summary>
        /// <param name="message">
        /// </param>
        /// <param name="exceptionType">
        /// </param>
        public LoaderException(string message, Type exceptionType)
        {
            this.Message = message;
            this.ExceptionType = exceptionType;
        }

        /// <summary>
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// </summary>
        public Type ExceptionType { get; set; }
    }
}