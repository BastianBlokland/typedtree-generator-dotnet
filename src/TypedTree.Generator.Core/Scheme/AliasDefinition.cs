using System;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;

using TypedTree.Generator.Core.Utilities;

namespace TypedTree.Generator.Core.Scheme
{
    /// <summary>
    /// Immutable representation of an indirection in the tree.
    /// For example a 'Alias' could be a 'ICondition' that can point to 'EnemyInRangeCondition'
    /// or 'OutOfHealthCondition'.
    /// </summary>
    public sealed class AliasDefinition : IEquatable<AliasDefinition>
    {
        internal AliasDefinition(string identifier, ImmutableArray<string> values)
        {
            if (string.IsNullOrEmpty(identifier))
                throw new ArgumentException($"Invalid identifier: '{identifier}'", nameof(identifier));
            if (values == null)
                throw new ArgumentNullException(nameof(values));

            // Verify that the values at least contain 1 entry and no duplicates
            Debug.Assert(values.Length > 0, "Enum must have at least one value");
            Debug.Assert(values.IsUnique(), "Enum values must be unique");

            this.Identifier = identifier;
            this.Values = values;
        }

        /// <summary>
        /// Identifier for this Alias.
        /// </summary>
        public string Identifier { get; }

        /// <summary>
        /// Set of node-types that this Alias can reference.
        /// </summary>
        public ImmutableArray<string> Values { get; }

        /// <summary>Check if two instances are equal.</summary>
        /// <param name="a">Item to compare to B</param>
        /// <param name="b">Item to compare to A</param>
        /// <returns>True if equal, otherwise false</returns>
        public static bool operator ==(AliasDefinition a, AliasDefinition b)
        {
            if (object.ReferenceEquals(a, null))
                return object.ReferenceEquals(b, null);
            return a.Equals(b);
        }

        /// <summary>Check if two instances are not equal.</summary>
        /// <param name="a">Item to compare to B</param>
        /// <param name="b">Item to compare to A</param>
        /// <returns>False if equal, otherwise true</returns>
        public static bool operator !=(AliasDefinition a, AliasDefinition b) => !(a == b);

        /// <summary>
        /// Check if this is structurally equal to given object.
        /// </summary>
        /// <param name="obj">Object to compare to</param>
        /// <returns>True if equal, otherwise false</returns>
        public override bool Equals(object obj) =>
            obj != null &&
            obj is AliasDefinition &&
            this.Equals((AliasDefinition)obj);

        /// <summary>
        /// Check if this is structurally equal to given other alias.
        /// </summary>
        /// <param name="other">Alias to compare to</param>
        /// <returns>True if equal, otherwise false</returns>
        public bool Equals(AliasDefinition other) =>
            other != null &&
            other.Identifier == this.Identifier &&
            other.Values.SequenceEqual(this.Values);

        /// <summary>
        /// Get a hashcode representing this alias.
        /// </summary>
        public override int GetHashCode() => HashCode.Combine(
            this.Identifier,
            this.Values.GetSequenceHashCode());
    }
}
