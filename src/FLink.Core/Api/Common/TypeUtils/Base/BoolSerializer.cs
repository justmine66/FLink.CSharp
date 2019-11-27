using FLink.Core.Memory;

namespace FLink.Core.Api.Common.TypeUtils.Base
{
    /// <summary>
    /// Type serializer for <see cref="bool"/>.
    /// </summary>
    public class BoolSerializer : TypeSerializerSingleton<bool>
    {
        public static readonly BoolSerializer Instance = new BoolSerializer();

        public override bool IsImmutableType => true;

        public override bool CreateInstance() => false;

        public override bool Copy(bool @from) => @from;

        public override bool Copy(bool @from, bool reuse) => @from;
        public override void Copy(IDataInputView source, IDataOutputView target)
        {
            throw new System.NotImplementedException();
        }

        public override int Length => 1;

        public override void Serialize(bool record, IDataOutputView target) => target.WriteBool(record);

        public override bool Deserialize(IDataInputView source) => source.ReadBool();

        public override bool Deserialize(bool reuse, IDataInputView source) => source.ReadBool();
        public override ITypeSerializerSnapshot<bool> SnapshotConfiguration() => new BoolSerializerSnapshot();

        /// <summary>
        /// Serializer configuration snapshot for compatibility and format evolution.
        /// </summary>
        public sealed class BoolSerializerSnapshot : SimpleTypeSerializerSnapshot<bool>
        {
            public BoolSerializerSnapshot()
                : base(() => Instance)
            {
            }
        }
    }
}
