using System;
using System.Collections.Immutable;
using System.Linq;

using TypedTree.Generator.Core.Scheme;

namespace TypedTree.Generator.Core.Builder
{
    /// <summary>
    /// Builder for creating node-definitions.
    /// </summary>
    public sealed class NodeDefinitionBuilder
    {
        private readonly ImmutableArray<INodeField>.Builder fields = ImmutableArray.CreateBuilder<INodeField>();
        private readonly string typeIdentifier;

        internal NodeDefinitionBuilder(string typeIdentifier)
        {
            if (string.IsNullOrEmpty(typeIdentifier))
                throw new ArgumentException($"Invalid type-identifier: '{typeIdentifier}'", nameof(typeIdentifier));

            this.typeIdentifier = typeIdentifier;
        }

        /// <summary>
        /// Optional comment about this node.
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// Add a string field to this node.
        /// </summary>
        /// <remarks>Name has to be unique across fields on the same node.</remarks>
        /// <exception cref="Exceptions.DuplicateFieldNameException">
        /// Thrown when field-name is not unique.
        /// </exception>
        /// <param name="name">Name of the field</param>
        /// <param name="isArray">Is this field an array</param>
        public void PushStringField(string name, bool isArray = false) =>
            this.PushField(new StringNodeField(name, isArray));

        /// <summary>
        /// Add a number field to this node.
        /// </summary>
        /// <remarks>Name has to be unique across fields on the same node.</remarks>
        /// <exception cref="Exceptions.DuplicateFieldNameException">
        /// Thrown when field-name is not unique.
        /// </exception>
        /// <param name="name">Name of the field</param>
        /// <param name="isArray">Is this field an array</param>
        public void PushNumberField(string name, bool isArray = false) =>
            this.PushField(new NumberNodeField(name, isArray));

        /// <summary>
        /// Add a boolean field to this node.
        /// </summary>
        /// <remarks>Name has to be unique across fields on the same node.</remarks>
        /// <exception cref="Exceptions.DuplicateFieldNameException">
        /// Thrown when field-name is not unique.
        /// </exception>
        /// <param name="name">Name of the field</param>
        /// <param name="isArray">Is this field an array</param>
        public void PushBooleanField(string name, bool isArray = false) =>
            this.PushField(new BooleanNodeField(name, isArray));

        /// <summary>
        /// Add a alias field to this node.
        /// </summary>
        /// <remarks>Name has to be unique across fields on the same node.</remarks>
        /// <exception cref="Exceptions.DuplicateFieldNameException">
        /// Thrown when field-name is not unique.
        /// </exception>
        /// <param name="name">Name of the field</param>
        /// <param name="alias">Alias for the value this field can have</param>
        /// <param name="isArray">Is this field an array</param>
        public void PushAliasField(string name, AliasDefinition alias, bool isArray = false) =>
            this.PushField(new AliasNodeField(name, alias, isArray));

        /// <summary>
        /// Add a enum field to this node.
        /// </summary>
        /// <remarks>Name has to be unique across fields on the same node.</remarks>
        /// <exception cref="Exceptions.DuplicateFieldNameException">
        /// Thrown when field-name is not unique.
        /// </exception>
        /// <param name="name">Name of the field</param>
        /// <param name="enum">Enum value this field can have</param>
        /// <param name="isArray">Is this field an array</param>
        public void PushEnumField(string name, EnumDefinition @enum, bool isArray = false) =>
            this.PushField(new EnumNodeField(name, @enum, isArray));

        internal NodeDefinition Build() =>
            new NodeDefinition(this.typeIdentifier, this.Comment, this.fields.ToImmutableArray());

        private void PushField(INodeField field)
        {
            if (field == null)
                throw new ArgumentNullException(nameof(field));

            if (this.fields.Any(f => f.Name == field.Name))
                throw new Exceptions.DuplicateFieldNameException(this.typeIdentifier, field.Name);

            this.fields.Add(field);
        }
    }
}
