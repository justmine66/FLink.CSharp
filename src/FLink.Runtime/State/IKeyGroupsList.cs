using System.Collections.Generic;

namespace FLink.Runtime.State
{
    /// <summary>
    /// This interface offers ordered random read access to multiple key group ids.
    /// </summary>
    public interface IKeyGroupsList : IEnumerable<int>
    {
        /// <summary>
        /// Returns the number of key group ids in the list.
        /// </summary>
        int NumberOfKeyGroups { get; }

        /// <summary>
        /// Returns the id of the keygroup at the given index, where index in interval [0,  <see cref="NumberOfKeyGroups"/>}[.
        /// </summary>
        /// <param name="idx">the index into the list</param>
        /// <returns>key group id at the given index</returns>
        int GetKeyGroupId(int idx);

        /// <summary>
        /// Returns true, if the given key group id is contained in the list, otherwise false.
        /// </summary>
        /// <param name="keyGroupId">Key-group to check for inclusion.</param>
        /// <returns>True, only if the key-group is in the range.</returns>
        bool Contains(int keyGroupId);
    }
}
