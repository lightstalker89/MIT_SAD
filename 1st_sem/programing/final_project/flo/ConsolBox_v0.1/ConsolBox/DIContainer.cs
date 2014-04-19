using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsolBox
{
    class DIContainer : IDIContainer
    {
        public Dictionary<Type, object> Instances;

        public DIContainer()
        {
            this.Instances = new Dictionary<Type, object>();
        }

        /// <summary>
        /// Maps an interface type to an implementation of that interface, with optional arguments.
        /// </summary>
        /// <typeparam name="TIn">The interface type</typeparam>
        /// <typeparam name="TOut">The implementation type</typeparam>
        /// <param name="args">Optional arguments for the creation of the implementation type.</param>
        public void Map<TIn, TOut>()
        {
            if (!Instances.ContainsKey(typeof(TIn)))
            {
                object instance = Activator.CreateInstance(typeof(TOut));
                Instances[typeof(TIn)] = instance;
            }
        }

        /// <summary>
        /// Maps an interface type to an implementation of that interface, with optional arguments.
        /// </summary>
        /// <typeparam name="TIn">The interface type</typeparam>
        /// <typeparam name="TOut">The implementation type</typeparam>
        /// <param name="args">Optional arguments for the creation of the implementation type.</param>
        public void Map<TIn, TOut>(TOut instance)
        {
            if (!Instances.ContainsKey(typeof(TIn)))
            {
                Instances[typeof(TIn)] = instance;
            }
        }

        /// <summary>
        /// Gets a service which implements T
        /// </summary>
        /// <typeparam name="T">The interface type</typeparam>
        public T GetService<T>() where T : class
        {
            if (Instances.ContainsKey(typeof(T)))
                return Instances[typeof(T)] as T;
            else
                throw new ApplicationException("The type " + typeof(T).FullName + " is not registered in the container"); //Andere Lösung
        }
    }
}
