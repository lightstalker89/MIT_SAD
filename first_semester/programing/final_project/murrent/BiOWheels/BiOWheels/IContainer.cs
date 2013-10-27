// *******************************************************
// * <copyright file="IContainer.cs" company="MDMCoWorks">
// * Copyright (c) Mario Murrent. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Mario Murrent</author>
// *******************************************************/
namespace BiOWheels
{
    /// <summary>
    /// Interface representing the must implement methods
    /// </summary>
    public interface IContainer
    {
        /// <summary>
        /// Register an instance of a class
        /// </summary>
        /// <typeparam name="TContract">Interface for the instance</typeparam>
        /// <typeparam name="TImplementation">Class for the instance</typeparam>
        void Register<TContract, TImplementation>();

        /// <summary>
        /// Register an instance of a class with the given instance
        /// </summary>
        /// <typeparam name="TContract">
        /// Interface for the instance
        /// </typeparam>
        /// <typeparam name="TImplementation">
        /// Class for the instance
        /// </typeparam>
        /// <param name="instance">
        /// Instance itself
        /// </param>
        void Register<TContract, TImplementation>(TImplementation instance);

        /// <summary>
        /// Resolve an instance of a class
        /// </summary>
        /// <typeparam name="T">
        /// Type of the instance
        /// </typeparam>
        /// <returns>
        /// The instance
        /// </returns>
        T Resolve<T>();
    }
}