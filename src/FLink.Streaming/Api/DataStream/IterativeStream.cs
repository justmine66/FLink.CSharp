using System;
using System.Collections.Generic;
using System.Text;

namespace FLink.Streaming.Api.DataStream
{
    /// <summary>
    /// The iterative data stream represents the start of an iteration in a <see cref="DataStream{T}"/>.
    /// </summary>
    /// <typeparam name="T">Type of the elements in this Stream</typeparam>
    public class IterativeStream<T> : SingleOutputStreamOperator<T>
    {
    }
}
