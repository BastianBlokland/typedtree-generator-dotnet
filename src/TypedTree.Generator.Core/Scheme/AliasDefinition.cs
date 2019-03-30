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
    public sealed class AliasDefinition
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
    }
}
