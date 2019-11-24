using System.Collections.Generic;
using FLink.Core.Api.Dag;
using FLink.Streaming.Runtime.Partitioners;

namespace FLink.Streaming.Api.Transformations
{
    /// <summary>
    /// This transformation represents a change of partitioning of the input elements.
    /// This does not create a physical operation, it only affects how upstream operations are connected to downstream operations.
    /// </summary>
    /// <typeparam name="TElement">The type of the elements that result from this <see cref="PartitionTransformation{TElement}"/></typeparam>
    public class PartitionTransformation<TElement> : Transformation<TElement>
    {
        /// <summary>
        /// Gets the input <see cref="Transformation{TElement}"/> of this <see cref="SinkTransformation{T}"/>.
        /// </summary>
        public Transformation<TElement> Input;

        /// <summary>
        /// Gets the <see cref="StreamPartitioner{T}"/> that must be used for partitioning the elements of the input <see cref="Transformation{TElement}"/>.
        /// </summary>
        public StreamPartitioner<TElement> Partitioner;

        /// <summary>
        /// Gets the <see cref="ShuffleMode"/> of this {@link PartitionTransformation}.
        /// </summary>
        public ShuffleMode ShuffleMode;

        /// <summary>
        /// Creates a new <see cref="PartitionTransformation{TElement}"/> from the given input and <see cref="StreamPartitioner{T}"/>.
        /// </summary>
        /// <param name="input">The input <see cref="Transformation{TElement}"/></param>
        /// <param name="partitioner">The <see cref="StreamPartitioner{T}"/></param>
        public PartitionTransformation(Transformation<TElement> input, StreamPartitioner<TElement> partitioner)
            : this(input, partitioner, ShuffleMode.Undefined)
        {
        }

        /// <summary>
        /// Creates a new <see cref="PartitionTransformation{TElement}"/> from the given input and <see cref="StreamPartitioner{T}"/>.
        /// </summary>
        /// <param name="input">The input <see cref="Transformation{TElement}"/></param>
        /// <param name="partitioner">The <see cref="StreamPartitioner{T}"/></param>
        /// <param name="shuffleMode">The <see cref="ShuffleMode"/></param>
        public PartitionTransformation(
            Transformation<TElement> input,
            StreamPartitioner<TElement> partitioner,
            ShuffleMode shuffleMode)
            : base("Partition", input.OutputType, input.Parallelism)
        {
            Input = input;
            Partitioner = partitioner;
            ShuffleMode = shuffleMode;
        }

        public override IList<Transformation<TElement>> TransitivePredecessors { get; }
    }
}
