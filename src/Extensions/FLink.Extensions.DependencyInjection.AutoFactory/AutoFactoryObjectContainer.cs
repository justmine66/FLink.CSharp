using System;
using Autofac;

namespace FLink.Extensions.DependencyInjection.AutoFactory
{
    public class AutoFactoryObjectContainer : IObjectContainer
    {
        private readonly ContainerBuilder _containerBuilder;
        private IContainer _container;

        public AutoFactoryObjectContainer() : this(new ContainerBuilder())
        {
        }

        public AutoFactoryObjectContainer(ContainerBuilder containerBuilder)
        {
            _containerBuilder = containerBuilder;
        }

        public void Build()
        {
            _container = _containerBuilder.Build();
        }

        public object GetService(Type serviceType)
        {
            throw new NotImplementedException();
        }

        public void Add(Type serviceType, Type implementationType, ObjectLifetime lifetime)
        {
            var builder = _containerBuilder.RegisterType(implementationType).As(serviceType);
            switch (lifetime)
            {
                case ObjectLifetime.Singleton:
                    builder.SingleInstance();
                    break;
                case ObjectLifetime.Scoped:
                    break;
                case ObjectLifetime.Transient:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(lifetime), lifetime, null);
            }
        }
    }
}
