using System;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;

using TypedTree.Generator.Core.Utilities;

namespace TypedTree.Generator.Core.Scheme
{
    /// <summary>
    /// Immutable representation of an enum (a named value) in the Tree.
    /// </summary>
    public sealed class EnumDefinition : IEquatable<EnumDefinition>
    {
        internal EnumDefinition(string identifier, ImmutableArray<EnumEntry> values)
        {
            if (string.IsNullOrEmpty(identifier))
                throw new ArgumentException($"Invalid identifier: '{identifier}'", nameof(identifier));
            if (values == null)
                throw new ArgumentNullException(nameof(values));

            // Verify that the values at least contain 1 entry and no duplicates
            Debug.Assert(values.Length > 0, "Enum must have at least one value");
            Debug.Assert(values.Select(v => v.Value).IsUnique(), "Enum values must be unique");

            this.Identifier = identifier;
            this.Values = values;
        }

        /// <summary>
        /// Identifier for this Enum.
        /// </summary>
        public string Identifier { get; }

        /// <summary>
        /// Set of values that this Enum can have.
        /// </summary>
        public ImmutableArray<EnumEntry> Values { get; }

        /// <summary>Check if two instances are equal.</summary>
        /// <param name="a">Item to compare to B</param>
        /// <param name="b">Item to compare to A</param>
        /// <returns>True if equal, otherwise false</returns>
        public static bool operator ==(EnumDefinition a, EnumDefinition b)
        {
            if (object.ReferenceEquals(a, null))
                return object.ReferenceEquals(b, null);
            return a.Equals(b);
        }

        /// <summary>Check if two instances are not equal.</summary>
        /// <param name="a">Item to compare to B</param>
        /// <param name="b">Item to compare to A</param>
        /// <returns>False if equal, otherwise true</returns>
        public static bool operator !=(EnumDefinition a, EnumDefinition b) => !(a == b);

        /// <summary>
        /// Check if this is structurally equal to given object.
        /// </summary>
        /// <param name="obj">Object to compare to</param>
        /// <returns>True if equal, otherwise false</returns>
        public override bool Equals(object obj) =>
            obj != null &&
            obj is EnumDefinition &&
            this.Equals((EnumDefinition)obj);

        /// <summary>
        /// Check if this is structurally equal to given other enum.
        /// </summary>
        /// <param name="other">Enum to compare to</param>
        /// <returns>True if equal, otherwise false</returns>
        public bool Equals(EnumDefinition other) =>
            other != null &&
            other.Identifier == this.Identifier &&
            other.Values.SequenceEqual(this.Values);

        /// <summary>
        /// Get a hashcode representing this enum.
        /// </summary>
        public override int GetHashCode() => HashCode.Combine(
            this.Identifier,
            this.Values.GetSequenceHashCode());
    }
}
