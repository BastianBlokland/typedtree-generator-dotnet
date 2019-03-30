using System;

namespace TypedTree.Generator.Core.Scheme
{
    /// <summary>
    /// Immutable representation of an alias field on a node.
    /// </summary>
    public sealed class AliasNodeField : INodeField
    {
        internal AliasNodeField(string name, AliasDefinition alias, bool isArray)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException($"Invalid name: '{name}'", nameof(name));
            if (alias == null)
                throw new ArgumentNullException(nameof(alias));

            this.Name = name;
            this.Alias = alias;
            this.IsArray = isArray;
        }

        /// <summary>
        /// Identifier for this field.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Get the Alias to a node that this field can contain.
        /// </summary>
        public AliasDefinition Alias { get; }

        /// <summary>
        /// Is this field an array.
        /// </summary>
        public bool IsArray { get; }
    }
}
