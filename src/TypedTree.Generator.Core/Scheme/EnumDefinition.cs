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
    public sealed class EnumDefinition
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
    }
}
