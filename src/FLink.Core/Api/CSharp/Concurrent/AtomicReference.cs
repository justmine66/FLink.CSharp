using System.Threading;

namespace FLink.Core.Api.CSharp.Concurrent
{
    /// <summary>
    /// Implementation of the java.concurrent.util AtomicReference type.
    /// </summary>
    public sealed class AtomicReference<T> where T : class
    {
        private T _atomicValue;

        /// <summary>
        /// Sets the initial value.
        /// </summary>
        public AtomicReference(T originalValue) => _atomicValue = originalValue;

        /// <summary>Default constructor</summary>
        public AtomicReference() => _atomicValue = default(T);

        /// <summary>
        /// The current value.
        /// </summary>
        public T Value
        {
            get => Volatile.Read<T>(ref _atomicValue);
            set => Volatile.Write<T>(ref _atomicValue, value);
        }

        /// <summary>
        /// If <see cref="Value"/> equals <paramref name="expected" />, then set the Value to <paramref name="newValue" />.
        /// </summary>
        /// <param name="expected"></param>
        /// <param name="newValue"></param>
        /// <returns>Returns true if  <paramref name="newValue" /> was set, false otherwise.</returns>
        public bool CompareAndSet(T expected, T newValue) => Interlocked.CompareExchange<T>(ref _atomicValue, newValue, expected) == expected;

        /// <summary>
        /// Implicit conversion operator = automatically casts the <see cref="AtomicReference{T}" /> to an instance of <typeparam name="T"></typeparam>.
        /// </summary>
        public static implicit operator T(AtomicReference<T> aRef) => aRef.Value;

        /// <summary>
        /// Implicit conversion operator = allows us to cast any type directly into a <see cref="AtomicReference{T}"/> instance.
        /// </summary>
        /// <param name="newValue"></param>
        /// <returns></returns>
        public static implicit operator AtomicReference<T>(T newValue) => new AtomicReference<T>(newValue);
    }
}
