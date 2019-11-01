using System;
using System.Collections;
using System.Collections.Generic;
using FLink.Core.Util;

namespace FLink.Runtime.State
{
    /// <summary>
    /// This class defines a range of key-group indexes. Key-groups are the granularity into which the keyspace of a job is partitioned for keyed state-handling in state backends. The boundaries of the range are inclusive.
    /// </summary>
    public class KeyGroupRange : IKeyGroupsList, IEquatable<KeyGroupRange>
    {
        public static readonly KeyGroupRange EmptyKeyGroupRange = new KeyGroupRange();

        /// <summary>
        /// The first key-group in the range.
        /// </summary>
        public int StartKeyGroup;

        /// <summary>
        /// The last key-group in the range.
        /// </summary>
        public int EndKeyGroup;

        private KeyGroupRange()
        {
            StartKeyGroup = 0;
            EndKeyGroup = -1;
        }

        /// <summary>
        /// Defines the range [startKeyGroup, endKeyGroup]
        /// </summary>
        /// <param name="startKeyGroup">start of the range (inclusive)</param>
        /// <param name="endKeyGroup">end of the range (inclusive)</param>
        public KeyGroupRange(int startKeyGroup, int endKeyGroup)
        {
            Preconditions.CheckArgument(startKeyGroup >= 0);
            Preconditions.CheckArgument(startKeyGroup <= endKeyGroup);

            StartKeyGroup = startKeyGroup;
            EndKeyGroup = endKeyGroup;

            Preconditions.CheckArgument(NumberOfKeyGroups >= 0, "Potential overflow detected.");
        }

        public IEnumerator<int> GetEnumerator() => new KeyGroupIterator(StartKeyGroup, EndKeyGroup);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public int NumberOfKeyGroups => 1 + EndKeyGroup - StartKeyGroup;

        public int GetKeyGroupId(int idx)
        {
            if (idx < 0 || idx > NumberOfKeyGroups)
                throw new IndexOutOfRangeException("Key group index out of bounds: " + idx);

            return StartKeyGroup + idx;
        }

        public bool Contains(int keyGroupId) => keyGroupId >= StartKeyGroup && keyGroupId <= EndKeyGroup;

        public class KeyGroupIterator : IEnumerator<int>
        {
            private int _position;

            private readonly int _startKeyGroup;
            private readonly int _endKeyGroup;

            public KeyGroupIterator(int startKeyGroup, int endKeyGroup)
            {
                _startKeyGroup = startKeyGroup;
                _endKeyGroup = endKeyGroup;
                _position = 0;
            }

            public bool MoveNext() => _position < 1 + _endKeyGroup - _startKeyGroup;

            public void Reset() => _position = 0;

            public int Current
            {
                get
                {
                    var rv = _startKeyGroup + _position;
                    ++_position;
                    return rv;
                }
            }

            object IEnumerator.Current => Current;

            public void Dispose() => throw new InvalidOperationException("Unsupported by this iterator!");
        }

        #region [ Override Methods ]

        public bool Equals(KeyGroupRange other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return StartKeyGroup == other.StartKeyGroup && EndKeyGroup == other.EndKeyGroup;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((KeyGroupRange)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (StartKeyGroup * 397) ^ EndKeyGroup;
            }
        }

        public override string ToString() => "KeyGroupRange{" +
                                            "startKeyGroup=" + StartKeyGroup +
                                            ", endKeyGroup=" + EndKeyGroup +
                                            '}';

        #endregion

        #region [ Factory methods ]

        /// <summary>
        /// Factory method that also handles creation of empty key-groups.
        /// </summary>
        /// <param name="startKeyGroup">start of the range (inclusive)</param>
        /// <param name="endKeyGroup">end of the range (inclusive)</param>
        /// <returns>the key-group from start to end or an empty key-group range.</returns>
        public static KeyGroupRange Of(int startKeyGroup, int endKeyGroup) => startKeyGroup <= endKeyGroup
            ? new KeyGroupRange(startKeyGroup, endKeyGroup)
            : EmptyKeyGroupRange;

        #endregion
    }
}
