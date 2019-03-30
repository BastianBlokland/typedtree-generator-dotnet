using System;

namespace TypedTree.Generator.Core.Scheme
{
    /// <summary>
    /// Immutable representation of an enum field on a node.
    /// </summary>
    public sealed class EnumNodeField : INodeField
    {
        internal EnumNodeField(string name, EnumDefinition @enum, bool isArray)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException($"Invalid name: '{name}'", nameof(name));
            if (@enum == null)
                throw new ArgumentNullException(nameof(@enum));

            this.Name = name;
            this.Enum = @enum;
            this.IsArray = isArray;
        }

        /// <summary>
        /// Identifier for this field.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the Enum value that this field can have.
        /// </summary>
        public EnumDefinition Enum { get; }

        /// <summary>
        /// Is this field an array.
        /// </summary>
        public bool IsArray { get; }
    }
}
