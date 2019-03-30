using System;

namespace TypedTree.Generator.Core.Scheme
{
    /// <summary>
    /// Immutable representation of a string field on a node.
    /// </summary>
    public sealed class StringNodeField : INodeField
    {
        internal StringNodeField(string name, bool isArray)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException($"Invalid name: '{name}'", nameof(name));

            this.Name = name;
            this.IsArray = isArray;
        }

        /// <summary>
        /// Identifier for this field.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Is this field an array.
        /// </summary>
        public bool IsArray { get; }
    }
}
