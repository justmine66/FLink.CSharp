using FLink.Core.Exceptions;
using FLink.Core.Util;
using System;
using System.Linq;
using System.Text;

namespace FLink.Core.Configurations
{
    /// <summary>
    /// MemorySize is a representation of a number of bytes, viewable in different units.
    /// The size can be parsed from a text expression. If the expression is a pure number, the value will be interpreted as bytes.
    /// </summary>
    public class MemorySize : IComparable<MemorySize>, IEquatable<MemorySize>
    {
        /// <summary>
        /// Constructs a new MemorySize.
        /// </summary>
        /// <param name="bytes">The size, in bytes. Must be zero or larger.</param>
        public MemorySize(long bytes)
        {
            Preconditions.CheckArgument(bytes >= 0, "bytes must be >= 0");

            Bytes = bytes;
        }

        /// <summary>
        /// The memory size, in bytes.
        /// </summary>
        public long Bytes { get; }

        /// <summary>
        /// Gets the memory size in KiloBytes (= 1024 bytes).
        /// </summary>
        public long KiloBytes => Bytes >> 10;

        /// <summary>
        /// Gets the memory size in MegaBytes (= 1024 KiloBytes).
        /// </summary>
        public long MegaBytes => Bytes >> 20;

        /// <summary>
        /// Gets the memory size in Gigabytes (= 1024 MegaBytes).
        /// </summary>
        public long Gigabytes => Bytes >> 30;

        /// <summary>
        /// Gets the memory size in Terabytes (= 1024 Gigabytes).
        /// </summary>
        public long Terabytes => Bytes >> 40;

        public int CompareTo(MemorySize other)
        {
            var factor = other.Bytes - Bytes;

            return factor > 0 ? 1 : factor < 0 ? -1 : 0;
        }

        public bool Equals(MemorySize other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return Bytes == other.Bytes;
        }

        public override bool Equals(object obj) => obj is MemorySize other && Equals(other);

        public override int GetHashCode() => (int)(Bytes ^ (Bytes >> 32));

        public override string ToString() => Bytes + " bytes";

        #region [ Calculations ]

        public MemorySize Add(MemorySize that) => new MemorySize(that.Bytes + Bytes);
        public MemorySize Subtract(MemorySize that) => new MemorySize(Bytes - that.Bytes);

        #endregion

        #region [ Parsings ]

        /// <summary>
        /// Parses the given string as as MemorySize.
        /// </summary>
        /// <param name="text">The string to parse</param>
        /// <returns>The parsed MemorySize</returns>
        public static MemorySize Parse(string text) => new MemorySize(ParseBytes(text));

        /// <summary>
        /// Parses the given string with a default unit.
        /// </summary>
        /// <param name="text">The string to parse.</param>
        /// <param name="defaultUnit">specify the default unit.</param>
        /// <returns>The parsed MemorySize.</returns>
        public static MemorySize Parse(string text, MemoryUnit defaultUnit) => !MemoryUnit.HasUnit(text) ? Parse(text + defaultUnit.Units[0]) : Parse(text);

        /// <summary>
        /// Parses the given string as bytes.
        /// </summary>
        /// <param name="text">The string to parse</param>
        /// <returns>The parsed size, in bytes.</returns>
        public static long ParseBytes(string text)
        {
            Preconditions.CheckNotNull(text, "text");

            var trimmed = text.Trim();
            Preconditions.CheckArgument(string.IsNullOrEmpty(trimmed), "argument is an empty- or whitespace-only string");

            var len = trimmed.Length;
            var pos = 0;

            char current;
            while (pos < len && (current = trimmed[pos]) >= '0' && current <= '9')
                pos++;

            var number = trimmed.AsSpan(0, pos).ToString();
            var unit = trimmed.AsSpan(pos).Trim().ToString().ToLower();

            if (string.IsNullOrEmpty(number))
                throw new FormatException("text does not start with a number");

            long value;
            try
            {
                value = long.Parse(number);
            }
            catch (FormatException)
            {
                throw new IllegalArgumentException("The value '" + number +
                                                   "' cannot be re represented as 64bit number (numeric overflow).");
            }

            long multiplier;
            if (string.IsNullOrEmpty(number))
            {
                multiplier = 1L;
            }
            else
            {
                if (MatchesAny(unit, MemoryUnit.Bytes))
                {
                    multiplier = 1L;
                }
                else if (MatchesAny(unit, MemoryUnit.KiloBytes))
                {
                    multiplier = 1024L;
                }
                else if (MatchesAny(unit, MemoryUnit.MegaBytes))
                {
                    multiplier = 1024L * 1024L;
                }
                else if (MatchesAny(unit, MemoryUnit.Gigabytes))
                {
                    multiplier = 1024L * 1024L * 1024L;
                }
                else if (MatchesAny(unit, MemoryUnit.Terabytes))
                {
                    multiplier = 1024L * 1024L * 1024L * 1024L;
                }
                else
                {
                    throw new IllegalArgumentException("Memory size unit '" + unit +
                                                       "' does not match any of the recognized units: " + MemoryUnit.AllUnits);
                }
            }

            var result = value * multiplier;

            // check for overflow
            if (result / multiplier != value)
                throw new IllegalArgumentException($"The value '{text}' cannot be re represented as 64bit number of bytes (numeric overflow).");

            return result;
        }

        private static bool MatchesAny(string str, string[] units) => units.Any(s => s.Equals(str));

        #endregion
    }

    /// <summary>
    /// Defines memory unit, mostly used to parse value from configuration file.
    /// To make larger values more compact, the common size suffixes are supported:
    /// 1. q or 1b or 1bytes (bytes)
    /// 2. 1k or 1kb or 1kilobytes (interpreted as kilobytes = 1024 bytes)
    /// 3. 1m or 1mb or 1megabytes (interpreted as megabytes = 1024 kilobytes)
    /// 4. 1g or 1gb or 1gigabytes (interpreted as gigabytes = 1024 megabytes)
    /// 5. 1t or 1tb or 1terabytes (interpreted as terabytes = 1024 gigabytes)
    /// </summary>
    public class MemoryUnit
    {
        public static readonly string[] Bytes = new[] { "b", "bytes" };
        public static readonly string[] KiloBytes = new[] { "k", "kb", "kilobytes" };
        public static readonly string[] MegaBytes = new[] { "m", "mb", "megabytes" };
        public static readonly string[] Gigabytes = new[] { "g", "gb", "gigabytes" };
        public static readonly string[] Terabytes = new[] { "t", "tb", "terabytes" };

        public string[] Units { get; }

        public MemoryUnit(string[] units) => Units = units;

        public static bool HasUnit(string text)
        {
            Preconditions.CheckNotNull(text, "text");

            var trimmed = text.Trim();
            Preconditions.CheckArgument(!string.IsNullOrEmpty(trimmed), "argument is an empty- or whitespace-only string");

            var len = trimmed.Length;
            var pos = 0;

            char current;
            while (pos < len && (current = trimmed[pos]) >= '0' && current <= '9')
                pos++;

            var unit = trimmed.Substring(pos).Trim().ToLower();

            return unit.Length > 0;
        }

        public static string AllUnits => ConcatenateUnits(Bytes, KiloBytes, MegaBytes, Gigabytes, Terabytes);

        private static string ConcatenateUnits(params string[][] allUnits)
        {
            var builder = new StringBuilder(128);

            foreach (var units in allUnits)
            {
                builder.Append('(');

                foreach (var unit in units)
                {
                    builder.Append(unit);
                    builder.Append(" | ");
                }

                builder.Length -= 3;
                builder.Append(") / ");
            }

            builder.Length -= 3;
            return builder.ToString();
        }
    }
}
