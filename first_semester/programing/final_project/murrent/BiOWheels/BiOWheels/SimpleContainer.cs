using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BiOWheels
{
    public class SimpleContainer : IContainer
    {
        public static readonly IContainer Instance = new SimpleContainer();

        private static readonly IDictionary<Type, Type> Types = new Dictionary<Type, Type>();
        private static readonly IDictionary<Type, object> TypeInstances = new Dictionary<Type, object>();

        public void Register<TContract, TImplementation>()
        {
            Types[typeof(TContract)] = typeof(TImplementation);
        }
        public void Register<TContract, TImplementation>(TImplementation instance)
        {
            TypeInstances[typeof(TContract)] = instance;
        }
        public T Resolve<T>()
        {
            return (T)Resolve(typeof(T));
        }
        public object Resolve(Type contract)
        {
            if (TypeInstances.ContainsKey(contract))
            {
                return TypeInstances[contract];
            }
            else
            {
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
}
