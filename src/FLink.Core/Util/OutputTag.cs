using System;
using FLink.Core.Api.Common.TypeInfos;

namespace FLink.Core.Util
{
    /// <summary>
    /// An <see cref="OutputTag{TElement}"/> is a typed and named tag to use for tagging side outputs of an operator.
    /// An <see cref="OutputTag{TElement}"/> must always be an anonymous inner class so that Flink can derive a <see cref="TypeInformation{TType}"/> for the generic type parameter.
    /// </summary>
    /// <typeparam name="TElement">the type of elements in the side-output stream.</typeparam>
    public class OutputTag<TElement> : IEquatable<OutputTag<TElement>>
    {
        /// <summary>
        /// Creates a new named <see cref="OutputTag{T}"/> with the given id.
        /// </summary>
        /// <param name="id">The id of the created <see cref="OutputTag{T}"/>.</param>
        public OutputTag(string id)
        {
            Preconditions.CheckNotNull(id, "OutputTag id cannot be null.");
            Preconditions.CheckArgument(!string.IsNullOrEmpty(id), "OutputTag id must not be empty.");

            Id = id;
        }

        /// <summary>
        /// Creates a new named <see cref="OutputTag{T}"/> with the given id and output <see cref="TypeInformation{TType}"/>.
        /// </summary>
        /// <param name="id">The id of the created <see cref="OutputTag{T}"/>.</param>
        /// <param name="typeInfo">The <see cref="TypeInformation{TType}"/> for the side output.</param>
        public OutputTag(string id, TypeInformation<TElement> typeInfo)
        {
            Preconditions.CheckNotNull(id, "OutputTag id cannot be null.");
            Preconditions.CheckArgument(!string.IsNullOrEmpty(id), "OutputTag id must not be empty.");

            Id = id;
            TypeInfo = Preconditions.CheckNotNull(typeInfo, "TypeInformation cannot be null.");
        }

        public string Id { get; }
        public TypeInformation<TElement> TypeInfo { get; }

        public bool Equals(OutputTag<TElement> other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return string.Equals(Id, other.Id) && Equals(TypeInfo, other.TypeInfo);
        }

        public override bool Equals(object obj) => obj is OutputTag<TElement> other && Equals(other);

        public override int GetHashCode() => Id.GetHashCode();

        public override string ToString() => $"OutputTag({TypeInfo}, {Id})";
    }
}
