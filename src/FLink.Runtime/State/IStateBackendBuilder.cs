using System;

namespace FLink.Runtime.State
{
    /// <summary>
    /// A mutable builder to build a state backend instance.
    /// </summary>
    /// <typeparam name="T">The type of the state backend instance.</typeparam>
    /// <typeparam name="TE">The type of Exceptions thrown in build.</typeparam>
    public interface IStateBackendBuilder<out T, TE> where TE : Exception
    {
        T Build();
    }
}
