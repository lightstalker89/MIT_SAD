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
    /// 
    /// </summary>
    public interface IContainer
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TContract"></typeparam>
        /// <typeparam name="TImplementation"></typeparam>
        void Register<TContract, TImplementation>();

        /// <summary>
        /// </summary>
        /// <typeparam name="TContract">
        /// </typeparam>
        /// <typeparam name="TImplementation">
        /// </typeparam>
        /// <param name="instance">
        /// </param>
        void Register<TContract, TImplementation>(TImplementation instance);

        /// <summary>
        /// </summary>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        /// </returns>
        T Resolve<T>();
    }
}