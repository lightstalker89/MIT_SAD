// *******************************************************
// * <copyright file="Container.cs" company="FGrill">
// * Copyright (c) 2013 Florian Grill. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Florian Grill</author>
// *******************************************************/
namespace ConsoleBox
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Class representing the <see cref="Container"/>
    /// </summary>
    public class Container : IContainer
    {
        /// <summary>
        /// Dictionary containing <see cref="Type"/> instances
        /// </summary>
        private readonly Dictionary<Type, object> instances;

        /// <summary>
        /// Initializes a new instance of the <see cref="Container"/> class.
        /// </summary>
        public Container()
        {
            this.instances = new Dictionary<Type, object>();
        }

        /// <inheritdoc/>
        public void Register<TIn, TOut>()
        {
            if (!this.instances.ContainsKey(typeof(TIn)))
            {
                object instance = Activator.CreateInstance(typeof(TOut));
                this.instances[typeof(TIn)] = instance;
            }
        }

        /// <inheritdoc/>
        public void Register<TIn, TOut>(TOut instance)
        {
            if (!this.instances.ContainsKey(typeof(TIn)))
            {
                this.instances[typeof(TIn)] = instance;
            }
        }

        /// <inheritdoc/>
        public T GetService<T>() where T : class
        {
            if (this.instances.ContainsKey(typeof(T)))
            {
                return this.instances[typeof(T)] as T;
            }
            else
            {
                throw new ApplicationException("The type " + typeof(T).FullName + " is not registered in the container");
            } 
        }
    }
}
