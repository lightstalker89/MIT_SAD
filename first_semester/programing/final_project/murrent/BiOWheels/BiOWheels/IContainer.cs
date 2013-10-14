using System;

namespace BiOWheels
{
    public interface IContainer
    {
        void Register<TContract, TImplementation>();
        void Register<TContract, TImplementation>(TImplementation instance);
        T Resolve<T>();
        object Resolve(Type contract);
    }
}
