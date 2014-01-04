// *******************************************************
// * <copyright file="IContainer.cs" company="FGrill">
// * Copyright (c) 2013 Florian Grill. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Florian Grill</author>
// *******************************************************/
namespace ConsoleBox
{
    /// <summary>
    /// Interface representing methods of the container
    /// </summary>
    public interface IContainer
    {
        /// <summary>
        /// Gets an instance of a class.
        /// The type argument must be a reference type; this applies 
        /// also to any class, interface, delegate, or array type.
        /// </summary>
        /// <typeparam name="T">
        /// Type of the instance
        /// </typeparam>
        /// <returns>
        /// The instance
        /// </returns>
        T GetService<T>() where T : class;

        /// <summary>
        /// Register an instance of a class with the given instance
        /// </summary>
        /// <typeparam name="TIn">
        /// Interface for the instance
        /// </typeparam>
        /// <typeparam name="TOut">
        /// Class for the instance
        /// </typeparam>
        /// <param name="instance">
        /// Instance itself
        /// </param>
        void Register<TIn, TOut>(TOut instance);

        /// <summary>
        /// Register an instance of a class
        /// </summary>
        /// <typeparam name="TIn">Interface for the instance</typeparam>
        /// <typeparam name="TOut">Class for the instance</typeparam>
        void Register<TIn, TOut>();
    }
}
