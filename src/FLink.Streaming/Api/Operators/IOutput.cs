using FLink.Core.Util;

namespace FLink.Streaming.Api.Operators
{
    /// <summary>
    /// A <see cref="IStreamOperator"/> is supplied with an object of this interface that can be used to emit elements and other messages, such as barriers and watermarks, from an operator.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IOutput<T> : ICollector<T>
    {

    }
}
