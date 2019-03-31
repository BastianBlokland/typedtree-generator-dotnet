using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

using TypedTree.Generator.Core.Scheme;
using TypedTree.Generator.Core.Utilities;

namespace TypedTree.Generator.Core.Builder
{
    /// <summary>
    /// Builder for creating tree-definitions.
    /// </summary>
    public sealed class TreeDefinitionBuilder
    {
        private readonly ImmutableArray<AliasDefinition>.Builder aliases = ImmutableArray.CreateBuilder<AliasDefinition>();
        private readonly ImmutableArray<Scheme.EnumDefinition>.Builder enums = ImmutableArray.CreateBuilder<Scheme.EnumDefinition>();
        private readonly ImmutableArray<NodeDefinition>.Builder nodes = ImmutableArray.CreateBuilder<NodeDefinition>();

        /// <summary>
        /// Construct a new tree-definition.
        /// </summary>
        /// <exception cref="Exceptions.AliasNotFoundException">
        /// Thrown when an alias is referenced that is not pushed to the builder.
        /// </exception>
        /// <exception cref="Exceptions.EnumNotFoundException">
        /// Thrown when an enum is referenced that is not pushed to the builder.
        /// </exception>
        /// <exception cref="Exceptions.NodeNotFoundException">
        /// Thrown when an alias references a node that is not pushed to the builder.
        /// </exception>
        /// <param name="rootAliasIdentifier">Identifier for the root-alias of the tree</param>
        /// <param name="build">Callback for adding content to the tree-definition</param>
        /// <returns>Newly constructed tree-definition</returns>
        public static TreeDefinition Create(string rootAliasIdentifier, Action<TreeDefinitionBuilder> build)
        {
            if (string.IsNullOrEmpty(rootAliasIdentifier))
                throw new ArgumentException($"Invalid identifier: '{rootAliasIdentifier}'", nameof(rootAliasIdentifier));
            if (build == null)
                throw new ArgumentNullException(nameof(build));

            var builder = new TreeDefinitionBuilder();
            build(builder);
            return builder.Build(rootAliasIdentifier);
        }

        /// <summary>
        /// Attempt to get an alias that was pushed to this builder.
        /// </summary>
        /// <param name="identifier">Identifier of the alias to get</param>
        /// <param name="alias">Found alias</param>
        /// <returns>True if found, otherwise false</returns>
        public bool TryGetAlias(string identifier, out AliasDefinition alias)
        {
            alias = this.aliases.FirstOrDefault(a => a.Identifier == identifier);
            return alias != null;
        }

        /// <summary>
        /// Check if an alias with given identifier is added to this builder.
        /// </summary>
        /// <param name="identifier">Identifier of the alias</param>
        /// <returns>True if found, otherwise false</returns>
        public bool ContainsAlias(string identifier) => this.TryGetAlias(identifier, out _);

        /// <summary>
        /// Attempt to get an enum that was pushed to this builder.
        /// </summary>
        /// <param name="identifier">Identifier of the enum to get</param>
        /// <param name="enum">Found enum</param>
        /// <returns>True if found, otherwise false</returns>
        public bool TryGetEnum(string identifier, out EnumDefinition @enum)
        {
            @enum = this.enums.FirstOrDefault(e => e.Identifier == identifier);
            return @enum != null;
        }

        /// <summary>
        /// Check if an enum with given identifier is added to this builder.
        /// </summary>
        /// <param name="identifier">Identifier of the enum</param>
        /// <returns>True if found, otherwise false</returns>
        public bool ContainsEnum(string identifier) => this.TryGetEnum(identifier, out _);

        /// <summary>
        /// Attempt to get an node that was pushed to this builder.
        /// </summary>
        /// <param name="type">Type of the node to get</param>
        /// <param name="node">Found node</param>
        /// <returns>True if found, otherwise false</returns>
        public bool TryGetNode(string type, out NodeDefinition node)
        {
            node = this.nodes.FirstOrDefault(a => a.Type == type);
            return node != null;
        }

        /// <summary>
        /// Check if an node with given type is added to this builder.
        /// </summary>
        /// <param name="type">Type of the node</param>
        /// <returns>True if found, otherwise false</returns>
        public bool ContainsNode(string type) => this.TryGetNode(type, out _);

        /// <summary>
        /// Add an alias to the scheme.
        /// </summary>
        /// <remarks>Identifier has to be unique across all aliases and enums.</remarks>
        /// <exception cref="Exceptions.EmptyAliasException">
        /// Thrown when alias has no values.
        /// </exception>
        /// <exception cref="Exceptions.DuplicateAliasIdentifierException">
        /// Thrown when alias identifier is not unique.
        /// </exception>
        /// <param name="identifier">Identifier of the alias</param>
        /// <param name="values">Value of the alias</param>
        /// <returns>Newly created definition</returns>
        public AliasDefinition PushAlias(string identifier, IEnumerable<string> values)
        {
            if (string.IsNullOrEmpty(identifier))
                throw new ArgumentException($"Invalid identifier: '{identifier}'", nameof(identifier));
            if (values == null)
                throw new ArgumentNullException(nameof(values));
            if (!values.Any())
                throw new Exceptions.EmptyAliasException(identifier);

            // Verify that alias / enum identifiers are unique.
            if (this.aliases.Any(a => a.Identifier == identifier) || this.enums.Any(e => e.Identifier == identifier))
                throw new Exceptions.DuplicateAliasIdentifierException(identifier);

            var definition = new AliasDefinition(identifier, ImmutableArray<string>.Empty.AddRange(values));
            this.aliases.Add(definition);
            return definition;
        }

        /// <summary>
        /// Add an enum to the scheme.
        /// </summary>
        /// <remarks>Identifier has to be unique across all enums and aliases.</remarks>
        /// <exception cref="Exceptions.EmptyEnumException">
        /// Thrown when enum has no entries.
        /// </exception>
        /// <exception cref="Exceptions.EnumDuplicateValueException">
        /// Thrown when enum contains duplicate values.
        /// </exception>
        /// <exception cref="Exceptions.DuplicateEnumIdentifierException">
        /// Thrown when enum identifier is not unique.
        /// </exception>
        /// <param name="identifier">Identifier of the enum</param>
        /// <param name="entries">Entries of the enum</param>
        /// <returns>Newly created definition</returns>
        public EnumDefinition PushEnum(string identifier, params EnumEntry[] entries) =>
            this.PushEnum(identifier, entries as IEnumerable<EnumEntry>);

        /// <summary>
        /// Add an enum to the scheme.
        /// </summary>
        /// <remarks>Identifier has to be unique across all enums and aliases.</remarks>
        /// <exception cref="Exceptions.EmptyEnumException">
        /// Thrown when enum has no entries.
        /// </exception>
        /// <exception cref="Exceptions.EnumDuplicateValueException">
        /// Thrown when enum contains duplicate values.
        /// </exception>
        /// <exception cref="Exceptions.DuplicateEnumIdentifierException">
        /// Thrown when enum identifier is not unique.
        /// </exception>
        /// <param name="identifier">Identifier of the enum</param>
        /// <param name="entries">Entries of the enum</param>
        /// <returns>Newly created definition</returns>
        public EnumDefinition PushEnum(string identifier, IEnumerable<EnumEntry> entries)
        {
            if (string.IsNullOrEmpty(identifier))
                throw new ArgumentException($"Invalid identifier: '{identifier}'", nameof(identifier));
            if (entries == null)
                throw new ArgumentNullException(nameof(entries));
            if (!entries.Any())
                throw new Exceptions.EmptyEnumException(identifier);
            if (!entries.Select(v => v.Value).IsUnique())
                throw new Exceptions.EnumDuplicateValueException(identifier);

            // Verify that alias / enum identifiers are unique.
            if (this.aliases.Any(a => a.Identifier == identifier) || this.enums.Any(e => e.Identifier == identifier))
                throw new Exceptions.DuplicateEnumIdentifierException(identifier);

            var definition = new EnumDefinition(identifier, ImmutableArray<EnumEntry>.Empty.AddRange(entries));
            this.enums.Add(definition);
            return definition;
        }

        /// <summary>
        /// Add an node to the scheme.
        /// </summary>
        /// <remarks>Identifier has to be unique across all nodes.</remarks>
        /// <exception cref="Exceptions.DuplicateNodeTypeException">
        /// Thrown when node-type is not unique.
        /// </exception>
        /// <param name="typeIdentifier">Identifier of the node-type</param>
        /// <param name="build">Callback to add content to the node</param>
        /// <returns>Newly created definition</returns>
        public NodeDefinition PushNode(string typeIdentifier, Action<NodeDefinitionBuilder> build = null)
        {
            if (string.IsNullOrEmpty(typeIdentifier))
                throw new ArgumentException($"Invalid type-identifier: '{typeIdentifier}'", nameof(typeIdentifier));

            // Verify that node-types are unique
            if (this.nodes.Any(n => n.Type == typeIdentifier))
                throw new Exceptions.DuplicateNodeTypeException(typeIdentifier);

            NodeDefinition definition;
            if (build == null)
            {
                definition = new NodeDefinition(typeIdentifier, ImmutableArray<INodeField>.Empty);
            }
            else
            {
                var builder = new NodeDefinitionBuilder(typeIdentifier);
                build(builder);
                definition = builder.Build();
            }

            this.nodes.Add(definition);
            return definition;
        }

        internal TreeDefinition Build(string rootAliasIdentifier)
        {
            // Verify that root-alias exists.
            if (!this.TryGetAlias(rootAliasIdentifier, out var rootAlias))
                throw new Exceptions.AliasNotFoundException(rootAliasIdentifier);

            // Verify that aliases only reference nodes that actually exist.
            foreach (var aliasVal in this.aliases.SelectMany(a => a.Values))
            {
                if (!this.ContainsNode(aliasVal))
                    throw new Exceptions.NodeNotFoundException(aliasVal);
            }

            // Verify that all fields on the nodes only reference valid aliases / enums.
            foreach (var nodeFieldPair in this.nodes.SelectMany(n => n.Fields.Select(f => (n, f))))
            {
                // If the field is an alias then verify that it exists on this builder.
                if (nodeFieldPair.f is AliasNodeField a && !this.aliases.Contains(a.Alias))
                    throw new Exceptions.AliasNotFoundException(a.Alias.Identifier);

                // If the field is an enum then verify that it exists on this builder.
                if (nodeFieldPair.f is EnumNodeField e && !this.enums.Contains(e.Enum))
                    throw new Exceptions.EnumNotFoundException(e.Enum.Identifier);
            }

            return new Scheme.TreeDefinition(
                rootAlias,
                this.aliases.ToImmutableArray(),
                this.enums.ToImmutableArray(),
                this.nodes.ToImmutableArray());
        }
    }
}
