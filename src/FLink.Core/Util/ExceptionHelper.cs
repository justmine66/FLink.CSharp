using System;
using FLink.Extensions.DependencyInjection;
using FLink.Extensions.Logging;

namespace FLink.Core.Util
{
    public class ExceptionHelper
    {
        private static readonly ILogger Logger = ObjectContainer.Current.GetService<ILogger<ExceptionHelper>>();

        public static void Eat(string name, Action action)
        {
            try
            {
                action();
            }
            catch (Exception e)
            {
                Logger.LogError($"The {name} has a unknown exception: {e}.");
            }
        }

        public static void Eat<T>(string name, Action<T> action, T state)
        {
            try
            {
                action(state);
            }
            catch (Exception e)
            {
                Logger.LogError($"The {name} has a unknown exception: {e}.");
            }
        }

        public static TResult Eat<TState, TResult>(string name, Func<TState, TResult> functor, TState state)
            where TResult : class
        {
            try
            {
                return functor(state);
            }
            catch (Exception e)
            {
                Logger.LogError($"The {name} has a unknown exception: {e}.");
                return null;
            }
        }
    }

}
