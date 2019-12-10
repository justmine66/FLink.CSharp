using FLink.Core.Api.Common.State;

namespace FLink.Runtime.State.MetaInfo
{
    /// <summary>
    /// Generalized snapshot for meta information about one state in a state backend <see cref="RegisteredKeyValueStateBackendMetaInfo{N,S}"/>
    /// </summary>
    public class StateMetaInfoSnapshot
    {
        /// <summary>
        /// Enum that defines the different types of state that live in Flink backends.
        /// </summary>
        public enum BackendStateType
        {
            KeyValue,
            Operator,
            Broadcast,
            PriorityQueue
        }

        /// <summary>
        /// Predefined keys for the most common options in the meta info.
        /// </summary>
        public enum CommonOptionsKeys
        {
            /// <summary>
            /// Key to define the <see cref="StateDescriptor{TState,T}.StateType"/> of a key/value keyed-state.
            /// </summary>
            KeyedStateType,
            /// <summary>
            /// Key to define <see cref="OperatorStateHandleMode"/>, about how operator state is distributed on restore
            /// </summary>
            OperatorStateDistributionMode,
        }

        /// <summary>
        /// Predefined keys for the most common serializer types in the meta info.
        /// </summary>
        public enum CommonSerializerKeys
        {
            KeySerializer,
            NamespaceSerializer,
            ValueSerializer
        }
    }
}
