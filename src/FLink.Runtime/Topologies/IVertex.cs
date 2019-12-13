using System.Collections.Generic;

namespace FLink.Runtime.Topologies
{
    public interface IVertex<out TVertexId, TResultId, TVertex, out TResult>
        where TVertexId : IVertexId
        where TResultId : IResultId
        where TVertex : IVertex<TVertexId, TResultId, TVertex, TResult>
        where TResult : IResult<TVertexId, TResultId, TVertex, TResult>
    {
        TVertexId Id { get; }

        IEnumerable<TResult> ConsumedResults { get; }

        IEnumerable<TResult> ProducedResults { get; }
    }
}
