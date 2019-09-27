using System;

namespace FLink.Extensions.DependencyInjection
{
    public interface IObjectRegister
    {
        void Add(Type serviceType, Type implementationType, ObjectLifetime lifetime);
    }
}
