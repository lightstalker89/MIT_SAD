// *******************************************************
// * <copyright file="SimpleContainer.cs" company="MDMCoWorks">
// * Copyright (c) Mario Murrent. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Mario Murrent</author>
// *******************************************************/
namespace BiOWheels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Test
    /// </summary>
    public class SimpleContainer : IContainer
    {
        /// <summary>
        /// </summary>
        public static readonly IContainer Instance = new SimpleContainer();

        /// <summary>
        /// </summary>
        private static readonly IDictionary<Type, Type> Types = new Dictionary<Type, Type>();

        /// <summary>
        /// </summary>
        private static readonly IDictionary<Type, object> TypeInstances = new Dictionary<Type, object>();

        /// <inheritdoc/>
        public void Register<TContract, TImplementation>()
        {
            Types[typeof(TContract)] = typeof(TImplementation);
        }

        /// <inheritdoc/>
        public void Register<TContract, TImplementation>(TImplementation instance)
        {
            TypeInstances[typeof(TContract)] = instance;
        }

        /// <inheritdoc/>
        public T Resolve<T>()
        {
            return (T)Resolve(typeof(T));
        }

        /// <inheritdoc/>
        private object Resolve(Type contract)
        {
            if (TypeInstances.ContainsKey(contract))
            {
                return TypeInstances[contract];
            }

            Type implementation = Types[contract];
            ConstructorInfo constructor = implementation.GetConstructors()[0];
            ParameterInfo[] constructorParameters = constructor.GetParameters();
            if (constructorParameters.Length == 0)
            {
                return Activator.CreateInstance(implementation);
            }

            List<object> parameters = new List<object>(constructorParameters.Length);
            parameters.AddRange(constructorParameters.Select(parameterInfo => Resolve(parameterInfo.ParameterType)));
            return constructor.Invoke(parameters.ToArray());
        }
    }
}