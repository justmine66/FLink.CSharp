namespace FLink.Extensions.DependencyInjection
{
    public static class ObjectRegisterExtensions
    {
        public static void Add<TService, TImplementation>(this IObjectRegister register, ObjectLifetime lifetime)
            where TService : class
            where TImplementation : class, TService
        {
            register.Add(typeof(TService), typeof(TImplementation), lifetime);
        }

        public static void AddTransient<TService>(this IObjectRegister register)
            where TService : class
        {
            register.Add(typeof(TService), typeof(TService), ObjectLifetime.Transient);
        }

        public static void AddTransient<TService, TImplementation>(this IObjectRegister register)
            where TService : class
            where TImplementation : class, TService
        {
            register.Add(typeof(TService), typeof(TImplementation), ObjectLifetime.Transient);
        }

        public static void AddScoped<TService>(this IObjectRegister register)
            where TService : class
        {
            register.Add(typeof(TService), typeof(TService), ObjectLifetime.Scoped);
        }

        public static void AddScoped<TService, TImplementation>(this IObjectRegister register)
            where TService : class
            where TImplementation : class, TService
        {
            register.Add(typeof(TService), typeof(TImplementation), ObjectLifetime.Scoped);
        }

        public static void AddSingleton<TService>(this IObjectRegister register)
                where TService : class
        {
            register.Add(typeof(TService), typeof(TService), ObjectLifetime.Singleton);
        }

        public static void AddSingleton<TService, TImplementation>(this IObjectRegister register)
            where TService : class
            where TImplementation : class, TService
        {
            register.Add(typeof(TService), typeof(TImplementation), ObjectLifetime.Singleton);
        }
    }
}
