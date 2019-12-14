using System.Collections.Generic;

namespace FLink.Runtime.Topologies
{
    /// <summary>
    /// Base topology for all logical and execution topologies.
    /// A topology consists of <see cref="IVertex{TVertexId,TResultId,TVertex,TResult}"/> and <see cref="IResult{TVertexId,TResultId,TVertex,TResult}"/>.
    /// </summary>
    public interface ITopology<TVertexId, out TResultId, out TVertex, TResult>
        where TVertexId : IVertexId
        where TResultId : IResultId
        where TVertex : IVertex<TVertexId, TResultId, TVertex, TResult>
        where TResult : IResult<TVertexId, TResultId, TVertex, TResult>
    {
        /// <summary>
        /// Returns an iterable over all vertices, topologically sorted.
        /// </summary>
        IEnumerable<TVertex> Vertices { get; }

        /// <summary>
        /// Returns whether the topology contains co-location constraints.
        /// Co-location constraints are currently used for iterations.
        /// </summary>
        bool ContainsCoLocationConstraints { get; }
    }
}
