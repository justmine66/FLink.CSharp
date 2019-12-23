using FLink.Core.Api.Common.Functions;
using FLink.Core.Api.CSharp.TypeUtils;
using FLink.Core.Exceptions;

namespace FLink.Core.Api.Common.TypeInfos
{
    /// <summary>
    /// A utility class for describing generic types.
    /// </summary>
    /// <typeparam name="T">The type information to hint.</typeparam>
    public abstract class TypeHint<T>
    {
        protected TypeHint()
        {
            try
            {
                TypeInfo = TypeExtractor.CreateTypeInfo<T>();
            }
            catch (InvalidTypesException e)
            {
                throw new FLinkRuntimeException("The TypeHint is using a generic variable." + "This is not supported, generic types must be fully specified for the TypeHint.");
            }
        }

        /// <summary>
        /// The type information described by the hint.
        /// </summary>
        public TypeInformation<T> TypeInfo { get; }
    }
}
