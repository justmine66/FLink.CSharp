namespace FLink.Core.Api.Common.TypeUtils.Base
{
    public abstract class TypeSerializerSingleton<T> : TypeSerializer<T>
    {
        public override TypeSerializer<T> Duplicate() => this;
    }
}
