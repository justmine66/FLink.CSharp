using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using FLink.Core.Api.Common.States;
using FLink.Core.Api.Common.TypeUtils;
using FLink.Core.Memory;
using FLink.Core.Util;
using FLink.Runtime.State;
using FLink.Streaming.Api.Checkpoint;

namespace FLink.Streaming.Api.Functions.Source
{
    /// <summary>
    /// A stream source function that returns a sequence of elements.
    /// NOTE: This source has a parallelism of 1.
    /// </summary>
    /// <typeparam name="T">The type of elements returned by this function.</typeparam>
    public class FromElementsFunction<T> : ISourceFunction<T>, ICheckpointedFunction
    {
        // The (de)serializer to be used for the data elements.
        private readonly TypeSerializer<T> _serializer;

        // The actual data elements, in serialized form.
        private readonly byte[] _elementsSerialized;

        // The number of serialized elements.
        private volatile int _numElements;

        // The number of elements emitted already.
        private volatile int _numElementsEmitted;

        // The number of elements to skip initially.
        private volatile int _numElementsToSkip;

        // Flag to make the source cancelable.
        private volatile bool _isRunning = true;

        [IgnoreDataMember]
        private IListState<int> _checkpointedState;

        public FromElementsFunction(TypeSerializer<T> serializer, params T[] elements)
            : this(serializer, elements.ToList())
        {
        }

        public FromElementsFunction(TypeSerializer<T> serializer, IEnumerable<T> elements)
        {
            var stream = new MemoryStream();
            var output = new DataOutputViewStreamWrapper(stream);

            var count = 0;
            try
            {
                foreach (var element in elements)
                {
                    _serializer.Serialize(element, output);
                    count++;
                }
            }
            catch (Exception e)
            {
                throw new IOException("Serializing the source elements failed.", e);
            }

            _numElements = count;
            _serializer = serializer;
            _elementsSerialized = stream.ToArray();
        }

        /// <summary>
        /// Verifies that all elements in the collection are non-null, and are of the given class, or a subclass thereof.
        /// </summary>
        /// <typeparam name="TOnt">The generic type of the collection to be checked.</typeparam>
        /// <param name="elements">The collection to check.</param>
        /// <param name="parent">The class to which the elements must be assignable to.</param>
        public static void CheckCollection<TOnt>(IEnumerable<TOnt> elements, Type parent)
        {
            foreach (var element in elements)
            {
                if (element == null)
                    throw new ArgumentNullException(nameof(elements), "The collection contains a null element");

                if (!parent.IsInstanceOfType(element))
                    throw new ArgumentException(
                        $"The elements in the collection are not all subclasses of {nameof(parent)}");
            }
        }

        public void Run(ISourceFunctionContext<T> ctx)
        {
            var stream = new MemoryStream(_elementsSerialized);
            var input = new DataInputViewStreamWrapper(stream);

            // if we are restored from a checkpoint and need to skip elements, skip them now.
            var toSkip = _numElementsToSkip;
            if (toSkip <= 0) return;

            try
            {
                while (toSkip > 0)
                {
                    _serializer.Deserialize(input);
                    toSkip--;
                }
            }
            catch (Exception e)
            {
                throw new IOException("Failed to deserialize an element from the source. " +
                                      "If you are using user-defined serialization (Value and Writable types), check the " +
                                      "serialization functions.\nSerializer is " + _serializer, e);
            }

            _numElementsEmitted = _numElementsToSkip;

            var mutex = ctx.GetCheckpointLock();
            while (_isRunning && _numElementsEmitted < _numElements)
            {
                T next;
                try
                {
                    next = _serializer.Deserialize(input);
                }
                catch (Exception e)
                {
                    throw new IOException("Failed to deserialize an element from the source. " +
                                          "If you are using user-defined serialization (Value and Writable types), check the " +
                                          "serialization functions.\nSerializer is " + _serializer, e);
                }

                lock (mutex)
                {
                    ctx.Collect(next);
                    _numElementsEmitted++;
                }
            }
        }

        public void Cancel()
        {
            _isRunning = false;
        }

        public void SnapshotState(IFunctionSnapshotContext context)
        {
            Preconditions.CheckState(_checkpointedState != null, $"The {GetType()} has not been properly initialized.");

            _checkpointedState.Clear();
            _checkpointedState.Add(NumElementsEmitted);
        }

        public void InitializeState(IFunctionInitializationContext context)
        {
            Preconditions.CheckState(_checkpointedState != null, $"The {GetType()} has not been properly initialized.");

            _checkpointedState = context.OperatorStateStore
                .GetListState(new ListStateDescriptor<int>("from-elements-state", typeof(int), null));

            if (context.IsRestored)
            {
                var retrievedStates = new List<int>();
                foreach (var entry in _checkpointedState.Get())
                    retrievedStates.Add(entry);

                // given that the parallelism of the function is 1, we can only have 1 state.
                Preconditions.CheckArgument(retrievedStates.Count == 1, $"The {GetType()} retrieved invalid state.");

                _numElementsToSkip = retrievedStates[0];
            }
        }

        /// <summary>
        /// The number of elements produced in total by this function.
        /// </summary>
        public int NumElements => _numElements;

        /// <summary>
        /// The number of elements emitted so far.
        /// </summary>
        public int NumElementsEmitted => _numElementsEmitted;
    }
}
