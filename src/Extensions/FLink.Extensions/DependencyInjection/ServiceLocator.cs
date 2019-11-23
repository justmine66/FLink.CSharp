using Microsoft.Extensions.DependencyInjection;
using System;

namespace FLink.Extensions.DependencyInjection
{
    public class ServiceLocator
    {
        public static IServiceProvider Services { get; set; }

        public static T GetService<T>() => Services.GetService<T>();
    }
}
