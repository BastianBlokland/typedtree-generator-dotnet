using System;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;

using TypedTree.Generator.Core.Utilities;

namespace TypedTree.Generator.Core.Scheme
{
    /// <summary>
    /// Immutable representation of a single node in this tree.
    /// </summary>
    public sealed class NodeDefinition
    {
        internal NodeDefinition(string type, ImmutableArray<INodeField> fields)
        {
            if (string.IsNullOrEmpty(type))
                throw new ArgumentException($"Invalid type: '{type}'", nameof(type));
            if (fields == null)
                throw new ArgumentNullException(nameof(fields));

            // Verify that all fields have unique names
            Debug.Assert(fields.Select(f => f.Name).IsUnique(), "Field names must be unique");

            this.Type = type;
            this.Fields = fields;
        }

        /// <summary>
        /// Identifier for this node-type.
        /// </summary>
        public string Type { get; }

        /// <summary>
        /// Fields that this node can have.
        /// </summary>
        public ImmutableArray<INodeField> Fields { get; }

        /// <summary>
        /// Check if this node has a field with given identifier.
        /// </summary>
        /// <param name="identifier">Identifier to find</param>
        /// <returns>True if a field was found, otherwise false</returns>
        public bool HasField(string identifier) => this.Fields.Any(f => f.Name == identifier);
    }
}
