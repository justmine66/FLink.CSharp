using System;
using System.Runtime.Serialization;

namespace FLink.Core.Util
{
    /// <summary>
    /// Wrapper around an object representing either a success (with a given value) or a failure cause.
    /// </summary>
    [Serializable]
    public class OptionalFailure<T>
    {
        [IgnoreDataMember]
        public T Value { get; }

        public Exception FailureCause { get; }

        public OptionalFailure(T value, Exception failureCause)
        {
            Value = value;
            FailureCause = failureCause;
        }

        public static OptionalFailure<T> Of(T value)
        {
            return new OptionalFailure<T>(value, null);
        }

        public static OptionalFailure<T> OfFailure(Exception failureCause)
        {
            return new OptionalFailure<T>(default, failureCause);
        }

        public bool IsFailure => FailureCause != null;
    }
}
