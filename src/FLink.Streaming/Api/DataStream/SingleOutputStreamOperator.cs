namespace FLink.Streaming.Api.DataStream
{
    /// <summary>
    /// <see cref="SingleOutputStreamOperator{T}"/> represents a user defined transformation applied on a <see cref="DataStream{T}"/> with one predefined output type.
    /// </summary>
    /// <typeparam name="T">Type of the elements in this Stream</typeparam>
    public class SingleOutputStreamOperator<T> : DataStream<T>
    {

    }
}
