using FLink.Runtime.State.MetaInfo;

namespace FLink.Runtime.State
{
    /// <summary>
    /// Base class for all registered state in state backends.
    /// </summary>
    public abstract class RegisteredStateMetaInfoBase
    {
        /// <summary>
        /// The name of the state
        /// </summary>
        protected string Name;

        protected RegisteredStateMetaInfoBase(string name) => Name = name;

        public abstract StateMetaInfoSnapshot Snapshot();
    }
}
