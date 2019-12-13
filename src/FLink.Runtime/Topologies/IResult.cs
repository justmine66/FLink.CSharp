using System;
using FLink.Runtime.IO.Network.Partition;

namespace FLink.Runtime.Topologies
{
    /// <summary>
    /// Represents a data set produced by a <see cref="IVertex{TVertexId,TResultId,TVertex,TResult}"/>.
    /// Each result is produced by one <see cref="IVertex{TVertexId,TResultId,TVertex,TResult}"/>.
    /// Each result can be consumed by multiple <see cref="IVertex{TVertexId,TResultId,TVertex,TResult}"/>.
    /// </summary>
    /// <typeparam name="TVertexId"></typeparam>
    /// <typeparam name="TResultId"></typeparam>
    /// <typeparam name="TVertex"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    public interface IResult<TVertexId, out TResultId, TVertex, TResult>
        where TVertexId : IVertexId
        where TResultId : IResultId
        where TVertex : IVertex<TVertexId, TResultId, TVertex, TResult>
        where TResult : IResult<TVertexId, TResultId, TVertex, TResult>
    {
        TResultId Id { get; }

        ResultPartitionType ResultType { get; }

        TVertex Producer { get; }

        IEquatable<TVertex> Consumers { get; }
    }
}
