using System;
using FLink.Core.Memory;

namespace FLink.Core.Api.Common.TypeUtils.Base
{
    /// <summary>
    /// Type serializer for <see cref="string"/>.
    /// </summary>
    public class StringSerializer : TypeSerializerSingleton<string>
    {
        /// <summary>
        /// Sharable instance of the IntSerializer.
        /// </summary>
        public static readonly StringSerializer Instance = new StringSerializer();

        public override bool IsImmutableType => true;

        public override string CreateInstance() => string.Empty;

        public override string Copy(string @from) => @from;

        public override string Copy(string @from, string reuse) => @from;

        public override void Copy(IDataInputView source, IDataOutputView target)
        {
            throw new NotImplementedException();
        }

        public override int Length => -1;

        public override void Serialize(string record, IDataOutputView target)
        {
            throw new NotImplementedException();
        }

        public override string Deserialize(IDataInputView source)
        {
            throw new NotImplementedException();
        }

        public override string Deserialize(string reuse, IDataInputView source)
        {
            throw new NotImplementedException();
        }

        public override ITypeSerializerSnapshot<string> SnapshotConfiguration() => new StringSerializerSnapshot();

        /// <summary>
        /// Serializer configuration snapshot for compatibility and format evolution.
        /// </summary>
        public sealed class StringSerializerSnapshot : SimpleTypeSerializerSnapshot<string>
        {
            public StringSerializerSnapshot()
                : base(() => StringSerializer.Instance)
            {
            }
        }
    }
}
