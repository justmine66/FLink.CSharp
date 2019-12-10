using System;
using FLink.Core.Api.Common.TypeInfo;
using FLink.Core.Api.Common.TypeUtils;
using FLink.Core.Api.CSharp.Concurrent;
using FLink.Core.Api.CSharp.TypeUtils;
using FLink.Core.Exceptions;
using FLink.Core.Util;
using FLink.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FLink.Core.Api.Common.State
{
    using static Preconditions;

    /// <summary>
    /// Base class for state descriptors.
    /// A <see cref="StateDescriptor{TState,TValue}"/> is used for creating partitioned <see cref="IState"/> in stateful operations.
    /// </summary>
    /// <typeparam name="TState">The type of the State objects created from this state descriptor.</typeparam>
    /// <typeparam name="TValue">The type of the value of the state object described by this state descriptor.</typeparam>
    public abstract class StateDescriptor<TState, TValue> : IEquatable<StateDescriptor<TState, TValue>> where TState : IState
    {
        private static readonly ILogger Logger = ServiceLocator.GetService<ILogger<StateDescriptor<TState, TValue>>>();
        private readonly AtomicReference<TypeSerializer<TValue>> _serializerAtomicReference = new AtomicReference<TypeSerializer<TValue>>();
        private readonly TValue _defaultValue;

        /// <summary>
        /// Gets activation of state time-to-live (TTL).
        /// </summary>
        public StateTtlConfig TtlConfig = StateTtlConfig.Disabled;

        /// <summary>
        /// An enumeration of the types of supported states.
        /// Used to identify the state type when writing and restoring checkpoints and savepoints.
        /// </summary>
        public enum StateType
        {
            Value,
            List,
            Reducing,
            Folding,
            Aggregating,
            Map
        }

        /// <summary>
        /// Name that uniquely identifies state created from this StateDescriptor.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The default value returned by the state when no other value is bound to a key.
        /// </summary>
        public TValue DefaultValue
        {
            get
            {
                if (_defaultValue == null) return default;

                var serializer = _serializerAtomicReference.Value;

                return serializer != null
                    ? serializer.Copy(_defaultValue)
                    : throw new IllegalStateException("Serializer not yet initialized.");
            }
        }

        /// <summary>
        /// The serializer for the type. May be eagerly initialized in the constructor or lazily.
        /// </summary>
        public TypeSerializer<TValue> Serializer => _serializerAtomicReference.Value ?? throw new IllegalStateException("Serializer not yet initialized.");

        /// <summary>
        /// The type information describing the value type. Only used to if the serializer is created lazily.
        /// </summary>
        public TypeInformation<TValue> TypeInfo { get; }

        /// <summary>
        /// Create a new <see cref="StateDescriptor{TState,T}"/> with the given name and the given type information.
        /// </summary>
        /// <param name="name">The name of the <see cref="StateDescriptor{TState,TValue}"/>.</param>
        /// <param name="type">The class of the type of values in the state.</param>
        /// <param name="defaultValue">The default value that will be set when requesting state without setting a value before.</param>
        protected StateDescriptor(string name, System.Type type, TValue defaultValue = default)
        {
            Name = CheckNotNull(name, "name must not be null");
            CheckNotNull(type, "type class must not be null");

            try
            {
                TypeInfo = TypeExtractor.CreateTypeInfo<TValue>(type);
            }
            catch (Exception e)
            {
                throw new RuntimeException(
                    "Could not create the type information for '" + type.Name + "'. " +
                    "The most common reason is failure to infer the generic type information, due to Java's type erasure. " +
                    "In that case, please pass a 'TypeHint' instead of a class to describe the type. " +
                    "For example, to describe 'Tuple2<String, String>' as a generic type, use " +
                    "'new PravegaDeserializationSchema<>(new TypeHint<Tuple2<String, String>>(){}, serializer);'", e);
            }

            _defaultValue = defaultValue;
        }

        /// <summary>
        /// Create a new <see cref="StateDescriptor{TState,T}"/> with the given name and the given type serializer.
        /// </summary>
        /// <param name="name">The name of the <see cref="StateDescriptor{TState,T}"/>.</param>
        /// <param name="serializer">The type serializer for the values in the state.</param>
        /// <param name="defaultValue">The default value that will be set when requesting state without setting a value before.</param>
        protected StateDescriptor(string name, TypeSerializer<TValue> serializer, TValue defaultValue = default)
        {
            Name = CheckNotNull(name);
            _defaultValue = defaultValue;
            _serializerAtomicReference.Value = CheckNotNull(serializer, "serializer must not be null");
        }

        /// <summary>
        /// Create a new <see cref="StateDescriptor{TState,T}"/> with the given name and the given type information.
        /// </summary>
        /// <param name="name">The name of the <see cref="StateDescriptor{TState, TValue}"/>.</param>
        /// <param name="typeInfo">The type information for the values in the state.</param>
        /// <param name="defaultValue">The default value that will be set when requesting state without setting a value before.</param>
        protected StateDescriptor(string name, TypeInformation<TValue> typeInfo, TValue defaultValue = default)
        {
            Name = CheckNotNull(name, "name must not be null");
            TypeInfo = CheckNotNull(typeInfo, "type information must not be null");
            _defaultValue = defaultValue;
        }

        /// <summary>
        /// Returns the queryable state name.
        /// </summary>
        public string QueryableStateName { get; private set; }

        /// <summary>
        /// Returns whether the state created from this descriptor is queryable.
        /// </summary>
        public bool IsQueryable => !string.IsNullOrEmpty(QueryableStateName);

        /// <summary>
        /// Sets the name for queries of state created from this descriptor.
        /// </summary>
        /// <param name="queryableStateName"></param>
        public void SetQueryable(string queryableStateName)
        {
            CheckArgument(TtlConfig.UpdateType == TtlStateUpdateType.Disabled, "Queryable state is currently not supported with TTL");

            QueryableStateName = QueryableStateName == null
                ? CheckNotNull(queryableStateName, "Registration name")
                : throw new IllegalStateException("Queryable state name already set");
        }

        public void EnableTimeToLive(StateTtlConfig ttlConfig)
        {
            CheckNotNull(ttlConfig);
            CheckArgument(ttlConfig.UpdateType != TtlStateUpdateType.Disabled && QueryableStateName == null, "Queryable state is currently not supported with TTL");

            TtlConfig = ttlConfig;
        }

        /// <summary>
        /// Checks whether the serializer has been initialized. Serializer initialization is lazy, to allow parametrization of serializers with an <see cref="ExecutionConfig"/>.
        /// </summary>
        public bool IsSerializerInitialized => _serializerAtomicReference.Value != null;

        /// <summary>
        /// Initializes the serializer, unless it has been initialized before.
        /// </summary>
        /// <param name="executionConfig">The execution config to use when creating the serializer.</param>
        public void InitializeSerializerUnlessSet(ExecutionConfig executionConfig)
        {
            if (_serializerAtomicReference.Value != null) return;

            // try to instantiate and set the serializer
            var serializer = TypeInfo == null
                ? throw new IllegalStateException("no serializer and no type info")
                : TypeInfo.CreateSerializer(executionConfig);

            // use cas to assure the singleton
            if (!_serializerAtomicReference.CompareAndSet(null, serializer))
            {
                Logger.LogDebug("Someone else beat us at initializing the serializer.");
            }
        }

        public bool Equals(StateDescriptor<TState, TValue> other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return string.Equals(Name, other.Name);
        }

        public override bool Equals(object obj) => obj is StateDescriptor<TState, TValue> other && Equals(other);

        public override int GetHashCode() => Name.GetHashCode() + 31 * GetType().GetHashCode();

        public override string ToString() => $"{GetType().Name}{{name={Name}, defaultValue={_defaultValue}, serializer={_serializerAtomicReference.Value}{(IsQueryable ? ", queryableStateName=" + QueryableStateName + "" : "")}{'}'}";

        public abstract StateType Type { get; }
    }
}
