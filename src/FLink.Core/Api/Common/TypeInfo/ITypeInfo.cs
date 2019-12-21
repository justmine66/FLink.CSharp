namespace FLink.Core.Api.Common.TypeInfo
{
    /// <summary>
    /// Annotation for specifying a corresponding <see cref="TypeInfoFactory{T}"/> that can produce <see cref="TypeInformation{TType}"/> for the annotated type. In a hierarchy of types the closest annotation that defines a factory will be chosen while traversing upwards, however, a globally registered factory has highest precedence.
    /// </summary>
    public interface ITypeInfo
    {
        TypeInfoFactory<T> Value<T>();
    }
}
