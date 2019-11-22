using System;
using System.Linq;

namespace FLink.Core.IO
{
    /// <summary>
    /// A locatable input split is an input split referring to input data which is located on one or more hosts.
    /// </summary>
    public class LocatableInputSplit : IInputSplit, IEquatable<LocatableInputSplit>
    {
        private static readonly string[] EmptyArr = new string[0];

        /// <summary>
        /// Creates a new locatable input split that refers to a multiple host as its data location.
        /// </summary>
        /// <param name="splitNumber">The number of the split</param>
        /// <param name="hostnames">The names of the hosts storing the data this input split refers to.</param>
        public LocatableInputSplit(int splitNumber, params string[] hostnames)
        {
            SplitNumber = splitNumber;
            Hostnames = hostnames ?? EmptyArr;
        }

        /// <summary>
        /// The number of the split.
        /// </summary>
        public int SplitNumber { get; }

        /// <summary>
        /// The names of the hosts storing the data this input split refers to.
        /// </summary>
        public string[] Hostnames { get; }

        public bool Equals(LocatableInputSplit other) => other != null && SplitNumber == other.SplitNumber && Hostnames.SequenceEqual(other.Hostnames);

        public override int GetHashCode() => SplitNumber;

        public override bool Equals(object obj) => obj is LocatableInputSplit other && Equals(other);

        public override string ToString() => "Locatable Split (" + SplitNumber + ") at " + Hostnames.Aggregate((x, y) => $"{x},{y}");
    }
}
