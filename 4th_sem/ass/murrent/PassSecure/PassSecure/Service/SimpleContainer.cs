#region File Header
// <copyright file="SimpleContainer.cs" company="">
// Copyright (c) 2015 Mario Murrent. All rights reserved.
// </copyright>
// <summary>
// </summary>
// <author>Mario Murrent</author>
#endregion
namespace PassSecure.Service
{
    #region Usings

    using System;
    using System.Collections.Generic;

    #endregion

    /// <summary>
    /// </summary>
    public class SimpleContainer
    {
        /// <summary>
        /// </summary>
        static readonly Dictionary<Type, object> RegisteredTypes = new Dictionary<Type, object>();

        /// <summary>
        /// </summary>
        /// <param name="toRegister">
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        public static void Register<T>(T toRegister)
        {
            RegisteredTypes.Add(typeof(T), toRegister);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        /// </returns>
        public static T Resolve<T>()
        {
            return (T)RegisteredTypes[typeof(T)];
        }
    }
}
