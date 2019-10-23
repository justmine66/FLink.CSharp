using FLink.Core.Api.Common.TypeUtils;
using FLink.Core.Exceptions;
using FLink.Core.IO;
using FLink.Core.Memory;

namespace FLink.Runtime.Plugable
{
    /// <summary>
    /// The serialization delegate exposes an arbitrary element as a <see cref="IIOReadableWritable"/> for serialization, with the help of a type serializer.
    /// </summary>
    /// <typeparam name="T">The type to be represented as an IOReadableWritable.</typeparam>
    public class SerializationDelegate<T> : IIOReadableWritable
    {
        private readonly TypeSerializer<T> _serializer;

        public T Instance;

        public SerializationDelegate(TypeSerializer<T> serializer) => _serializer = serializer;

        public void SetInstance(T instance) => Instance = instance;

        public void Write(IDataOutputView output) => _serializer.Serialize(Instance, output);

        public void Read(IDataInputView input) => throw new IllegalStateException("Deserialization method called on SerializationDelegate.");
    }
}
