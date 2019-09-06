﻿using FLink.Core.Api.Dag;
using FLink.Streaming.Api.Environment;

namespace FLink.Streaming.Api.DataStream
{
    /// <summary>
    /// <see cref="SingleOutputStreamOperator{T}"/> represents a user defined transformation applied on a <see cref="DataStream{T}"/> with one predefined output type.
    /// </summary>
    /// <typeparam name="T">Type of the elements in this Stream</typeparam>
    public class SingleOutputStreamOperator<T> : DataStream<T>
    {
        protected SingleOutputStreamOperator(StreamExecutionEnvironment environment, Transformation<T> transformation) 
            : base(environment, transformation)
        {
        }
    }
}
