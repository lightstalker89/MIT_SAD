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
    /// Class representing a <see cref="LoaderException"/>
    /// </summary>
    public class LoaderException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LoaderException"/> class
        /// </summary>
        /// <param name="message">
        /// The message for the <see cref="LoaderException"/>
        /// </param>
        /// <param name="exceptionType">
        /// The type for the <see cref="LoaderException"/>
        /// </param>
        public LoaderException(string message, Type exceptionType)
        {
            this.Message = message;
            this.ExceptionType = exceptionType;
        }

        /// <summary>
        /// Gets or sets the message of the <see cref="LoaderException"/>
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the type of the <see cref="LoaderException"/>
        /// </summary>
        public Type ExceptionType { get; set; }
    }
}