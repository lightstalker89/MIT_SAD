using System;
using System.Collections.Generic;

namespace PassSecure.Service
{
    public class SimpleContainer
    {
        static readonly Dictionary<Type, object> RegisteredTypes = new Dictionary<Type, object>();

        public static void Register<T>(T toRegister)
        {
            RegisteredTypes.Add(typeof(T), toRegister);
        }

        public static T Resolve<T>()
        {
            return (T)RegisteredTypes[typeof(T)];
        }
    }
}
