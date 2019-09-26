using System;

namespace FLink.Extensions.DependencyInjection
{
    public class ObjectContainer : IObjectContainer
    {
        public static IObjectContainer Current { get; private set; }

        public static void SetContainer(IObjectContainer container)
        {
            Current = container;
        }

        public object GetService(Type serviceType)
        {
            return Current.GetService(serviceType);
        }

        public void Add(Type serviceType, Type implementationType, ObjectLifetime lifetime)
        {
            Current.Add(serviceType, implementationType, lifetime);
        }

        public void Build()
        {
            Current.Build();
        }
    }
}
