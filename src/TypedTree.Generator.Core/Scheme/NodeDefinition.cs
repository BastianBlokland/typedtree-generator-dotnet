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
    public sealed class NodeDefinition : IEquatable<NodeDefinition>
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

        /// <summary>Check if two instances are equal.</summary>
        /// <param name="a">Item to compare to B</param>
        /// <param name="b">Item to compare to A</param>
        /// <returns>True if equal, otherwise false</returns>
        public static bool operator ==(NodeDefinition a, NodeDefinition b)
        {
            if (object.ReferenceEquals(a, null))
                return object.ReferenceEquals(b, null);
            return a.Equals(b);
        }

        /// <summary>Check if two instances are not equal.</summary>
        /// <param name="a">Item to compare to B</param>
        /// <param name="b">Item to compare to A</param>
        /// <returns>False if equal, otherwise true</returns>
        public static bool operator !=(NodeDefinition a, NodeDefinition b) => !(a == b);

        /// <summary>
        /// Check if this node has a field with given identifier.
        /// </summary>
        /// <param name="identifier">Identifier to find</param>
        /// <returns>True if a field was found, otherwise false</returns>
        public bool HasField(string identifier) => this.Fields.Any(f => f.Name == identifier);

        /// <summary>
        /// Check if this is structurally equal to given object.
        /// </summary>
        /// <param name="obj">Object to compare to</param>
        /// <returns>True if equal, otherwise false</returns>
        public override bool Equals(object obj) =>
            obj != null &&
            obj is NodeDefinition &&
            this.Equals((NodeDefinition)obj);

        /// <summary>
        /// Check if this is structurally equal to given other node.
        /// </summary>
        /// <param name="other">Node to compare to</param>
        /// <returns>True if equal, otherwise false</returns>
        public bool Equals(NodeDefinition other) =>
            other != null &&
            other.Type == this.Type &&
            other.Fields.SequenceEqual(this.Fields);

        /// <summary>
        /// Get a hashcode representing this node.
        /// </summary>
        public override int GetHashCode() => HashCode.Combine(
            this.Type,
            this.Fields.GetSequenceHashCode());
    }
}
