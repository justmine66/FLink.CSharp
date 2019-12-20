using FLink.Core.Api.Common.Functions;
using FLink.Streaming.Api.Watermarks;

namespace FLink.Streaming.Api.Functions.Source
{
    /// <summary>
    /// Base interface for all stream data sources in FLink.
    /// The contract of a stream source is the following:
    /// 1. When the source should start emitting elements, the <see cref="Run"/> method is called with a <see cref="ISourceFunctionContext{T}"/> that can be used for emitting elements. The run method can run for as long as necessary.
    /// 2. The source must, however, react to an invocation of <see cref="Cancel"/> by breaking out of its main loop.
    /// </summary>
    /// <remarks>
    /// <example>CheckpointedFunction Sources:</example>
    /// Sources that also implement the <see cref="Checkpoint.ICheckpointedFunction"/> interface must ensure that state checkpointing, updating of internal state and emission of elements are not done concurrently. This is achieved by using the provided checkpointing lock object to protect update of state and emission of elements in a synchronized block.
    /// <code><see cref="FLink.Streaming.FunctionalTest.Source.CheckpointedExampleCountSource"/></code>
    ///
    /// <example>Timestamps and watermarks:</example>
    /// Sources may assign timestamps to elements and may manually emit watermarks. However, these are only interpreted if the streaming program runs on <see cref="TimeCharacteristic.EventTime"/>. On other time characteristics <see cref="TimeCharacteristic.IngestionTime"/> and <see cref="TimeCharacteristic.ProcessingTime"/>, the watermarks from the source function are ignored.
    ///
    /// <example>Gracefully Stopping Functions:</example>
    /// Functions may additionally implement the <see cref="IStoppableFunction"/> "Stopping" a function, in contrast to "canceling" means a graceful exit that leaves the
    /// state and the emitted elements in a consistent state.
    /// When a source is stopped, the executing thread is not interrupted, but expected to leave the
    /// <see cref="Run"/> method in reasonable time on its own, preserving the atomicity
    /// of state updates and element emission.
    /// interface. 
    /// </remarks>
    /// <typeparam name="TElement">The type of the elements produced by this source.</typeparam>
    public interface ISourceFunction<out TElement> : IFunction
    {
        /// <summary>
        /// Starts the source. Implementations can use the <see cref="ISourceFunctionContext{T}"/> emit elements.
        /// </summary>
        /// <param name="ctx">The context to emit elements to and for accessing locks.</param>
        void Run(ISourceFunctionContext<TElement> ctx);

        /// <summary>
        /// Cancels the source. Most sources will have a while loop inside the <see cref="ISourceFunctionContext{T}"/> method. The implementation needs to ensure that the source will break out of that loop after this method is called.
        /// </summary>
        void Cancel();
    }

    /// <summary>
    /// Interface that source functions use to emit elements, and possibly watermarks.
    /// </summary>
    /// <typeparam name="TElement">The type of the elements produced by the source.</typeparam>
    public interface ISourceFunctionContext<in TElement>
    {
        /// <summary>
        /// Emits one element from the source, without attaching a timestamp. In most cases, this is the default way of emitting elements.
        /// </summary>
        /// <remarks>
        /// The timestamp that the element will get assigned depends on the time characteristic of the streaming program:
        /// <list type="TimeCharacteristic">
        ///   <para>
        ///     <see cref="TimeCharacteristic.ProcessingTime"/>, the element has no timestamp.
        ///   </para>
        ///   <para>
        ///     <see cref="TimeCharacteristic.IngestionTime"/>, the element gets the system's current time as the timestamp.
        ///   </para>
        ///   <para>
        ///     <see cref="TimeCharacteristic.EventTime"/>, the element will have no timestamp initially. It needs to get a timestamp <see cref="ITimestampAssigner{T}"/> before any time-dependent operation(like time windows).
        ///   </para>
        /// </list>
        /// </remarks>
        /// <param name="element">The element to emit</param>
        void Collect(TElement element);

        /// <summary>
        /// Emits one element from the source, and attaches the given timestamp. This method is relevant for programs using <see cref="TimeCharacteristic.EventTime"/>, where the sources assign timestamps themselves, rather than relying on a <see cref="ITimestampAssigner{T}"/> on the stream.
        /// </summary>
        /// <remarks>
        /// On certain time characteristics, this timestamp may be ignored or overwritten. This allows programs to switch between the different time characteristics and behaviors without changing the code of the source functions.
        /// <list type="TimeCharacteristic">
        ///   <para>
        ///     <see cref="TimeCharacteristic.ProcessingTime"/>, the timestamp will be ignored, because processing time never works with element timestamps.
        ///   </para>
        ///   <para>
        ///     <see cref="TimeCharacteristic.IngestionTime"/>, the timestamp is overwritten with the system's current time, to realize proper ingestion time semantics.
        ///   </para>
        ///   <para>
        ///     <see cref="TimeCharacteristic.EventTime"/>, the timestamp will be used.
        ///   </para>
        /// </list>
        /// </remarks>
        /// <param name="element">The element to emit</param>
        /// <param name="timestamp">The timestamp in milliseconds since the Epoch</param>
        void CollectWithTimestamp(TElement element, long timestamp);

        /// <summary>
        /// Emits the given <see cref="Watermark"/>. 
        /// </summary>
        /// <param name="mark">The Watermark to emit</param>
        void EmitWatermark(Watermark mark);

        /// <summary>
        /// Marks the source to be temporarily idle. This tells the system that this source will temporarily stop emitting records and watermarks for an indefinite amount of time. This is only relevant when running on <see cref="TimeCharacteristic.IngestionTime"/> and <see cref="TimeCharacteristic.EventTime"/>, allowing downstream tasks to advance their watermarks without the need to wait for watermarks from this source while it is idle. 
        /// Source functions should make a best effort to call this method as soon as they acknowledge themselves to be idle. 
        /// </summary>
        void MarkAsTemporarilyIdle();

        /// <summary>
        /// Returns the checkpoint lock. Please refer to the class-level comment in <see cref="ISourceFunction{T}"/> for details about how to write a consistent checkpointed source.
        /// </summary>
        /// <returns>The object to use as the lock</returns>
        object GetCheckpointLock();

        /// <summary>
        /// This method is called by the system to shut down the context.
        /// </summary>
        void Close();
    }
}
