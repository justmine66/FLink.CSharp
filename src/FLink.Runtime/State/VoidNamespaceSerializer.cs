using FLink.Core.Api.Common.TypeUtils;
using FLink.Core.Api.Common.TypeUtils.Base;
using FLink.Core.Memory;

namespace FLink.Runtime.State
{
    public class VoidNamespaceSerializer : TypeSerializerSingleton<VoidNamespace>
    {
        public static readonly VoidNamespaceSerializer Instance = new VoidNamespaceSerializer();

        public override bool IsImmutableType => true;
        public override VoidNamespace CreateInstance()
        {
            throw new System.NotImplementedException();
        }

        public override VoidNamespace Copy(VoidNamespace @from)
        {
            throw new System.NotImplementedException();
        }

        public override VoidNamespace Copy(VoidNamespace @from, VoidNamespace reuse)
        {
            throw new System.NotImplementedException();
        }

        public override void Copy(IDataInputView source, IDataOutputView target)
        {
            throw new System.NotImplementedException();
        }

        public override int Length => 0;
        public override void Serialize(VoidNamespace record, IDataOutputView target)
        {
            throw new System.NotImplementedException();
        }

        public override VoidNamespace Deserialize(IDataInputView source)
        {
            throw new System.NotImplementedException();
        }

        public override VoidNamespace Deserialize(VoidNamespace reuse, IDataInputView source)
        {
            throw new System.NotImplementedException();
        }

        public override ITypeSerializerSnapshot<VoidNamespace> SnapshotConfiguration()
        {
            throw new System.NotImplementedException();
        }
    }
}
