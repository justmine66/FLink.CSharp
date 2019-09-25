using FLink.Core.Api.Common.Functions;
using FLink.Streaming.Api.Windowing.Assigners;
using FLink.Streaming.Api.Windowing.Windows;

namespace FLink.Streaming.Api.DataStream
{
    /// <summary>
    /// A <see cref="WindowedStream{T,TK,TW}"/> represents a data stream where elements are grouped by key, and for each key, the stream of elements is split into windows based on a <see cref="WindowAssigner{T,TW}"/>
    /// </summary>
    /// <typeparam name="T">The type of elements in the stream.</typeparam>
    /// <typeparam name="TK">The type of the key by which elements are grouped.</typeparam>
    /// <typeparam name="TW">The type of <see cref="Window"/> that the <see cref="WindowAssigner{T,TW}"/> assigns the elements to.</typeparam>
    public class WindowedStream<T, TK, TW>
        where TW : Window
    {
        public SingleOutputStreamOperator<T> Reduce(IReduceFunction<T> function)
        {
            return null;
        }
    }
}
