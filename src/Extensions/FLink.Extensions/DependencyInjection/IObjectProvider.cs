using System;

namespace FLink.Extensions.DependencyInjection
{
    public interface IObjectProvider
    {
        object GetService(Type serviceType);
    }
}
