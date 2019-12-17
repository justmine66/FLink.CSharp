using FLink.Runtime.IO;
using FLink.Streaming.Runtime.Tasks;
using System;

namespace FLink.Streaming.Runtime.IO
{
    /// <summary>
    /// Interface for processing records by <see cref="StreamTask{TOutput,TOperator}"/>.
    /// </summary>
    public interface IStreamInputProcessor : IAvailabilityProvider, ICloneable
    {
        /// <summary>
        /// input status to estimate whether more records can be processed immediately or not.
        /// If there are no more records available at the moment and the caller should check finished state and/or <see cref="IAvailabilityProvider.AvailableFuture"/>.
        /// </summary>
        /// <returns></returns>
        InputStatus ProcessInput();
    }
}
