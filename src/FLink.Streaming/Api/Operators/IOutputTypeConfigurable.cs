using System;
using FLink.Core.Api.Common;

namespace FLink.Streaming.Api.Operators
{
    /// <summary>
    /// Stream operators can implement this interface if they need access to the output type information
    /// at<see cref="FLink.Streaming.Api.graph.StreamGraph"/>generation.This can be useful for
    /// cases where the output type is specified by the returns method and, thus, after the stream
    /// operator has been created.
    /// </summary>
    public interface IOutputTypeConfigurable<T>
    {
        void SetOutputType(Type outTypeInfo, ExecutionConfig executionConfig);
    }
}
