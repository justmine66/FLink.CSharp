using System;
using System.IO;
using FLink.Core.Memory;

namespace FLink.Core.Api.Common.TypeUtils
{
    /// <summary>
    /// A simple base class for TypeSerializerSnapshots, for serializers that have no parameters. The serializer is defined solely by its class name.
    /// Serializers that produce these snapshots must be public, have public a zero-argument constructor and cannot be a non-static inner classes.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class SimpleTypeSerializerSnapshot<T> : ITypeSerializerSnapshot<T>
    {
        private readonly Func<TypeSerializer<T>> _serializerSupplier;

        protected SimpleTypeSerializerSnapshot(Func<TypeSerializer<T>> serializerSupplier)
        {
            _serializerSupplier = serializerSupplier;
        }

        public int CurrentVersion => 3;

        public void WriteSnapshot(IDataOutputView output) { }

        public void ReadSnapshot(int readVersion, IDataInputView input)
        {
            switch (readVersion)
            {
                case 3:
                    break;
                default:
                    throw new IOException("Unrecognized version: " + readVersion);
            }
        }

        public TypeSerializer<T> RestoreSerializer() => _serializerSupplier();
    }
}
