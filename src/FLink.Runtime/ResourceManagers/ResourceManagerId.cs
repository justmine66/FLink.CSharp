using FLink.Core.Util;

namespace FLink.Runtime.ResourceManagers
{
    public class ResourceManagerId : AbstractId
    {
        /// <summary>
        /// Generates a new random ResourceManagerId.
        /// </summary>
        private ResourceManagerId() { }

        /// <summary>
        /// Creates a ResourceManagerId that takes the bits from the given Guid.
        /// </summary>
        /// <param name="uuid"></param>
        private ResourceManagerId(Uuid uuid) : base(uuid.LeastSignificantBits, uuid.MostSignificantBits)
        {

        }

        /// <summary>
        /// Creates a UUID with the bits from this ResourceManagerId.
        /// </summary>
        /// <returns></returns>
        public Uuid ToUuid() => new Uuid(UpperPart, LowerPart);

        /// <summary>
        /// Generates a new random ResourceManagerId.
        /// </summary>
        /// <returns></returns>
        public static ResourceManagerId Generate() => new ResourceManagerId();

        /// <summary>
        /// Creates a ResourceManagerId that corresponds to the given UUID.
        /// </summary>
        /// <param name="uuid"></param>
        /// <returns></returns>
        public static ResourceManagerId FromUuid(Uuid uuid) => new ResourceManagerId(uuid);

        /// <summary>
        /// If the given uuid is null, this returns null, otherwise a ResourceManagerId that corresponds to the UUID, via <see cref="ResourceManagerId(Uuid)"/>.
        /// </summary>
        /// <param name="uuid"></param>
        /// <returns></returns>
        public static ResourceManagerId FromUuidOrNull(Uuid? uuid) => uuid == null ? null : new ResourceManagerId(uuid.Value);
    }
}
