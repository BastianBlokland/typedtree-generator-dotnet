using System;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;

using TypedTree.Generator.Core.Utilities;

namespace TypedTree.Generator.Core.Scheme
{
    /// <summary>
    /// Immutable representation of a TreeScheme.
    /// </summary>
    public sealed class TreeDefinition : IEquatable<TreeDefinition>
    {
        internal TreeDefinition(
            AliasDefinition rootAlias,
            ImmutableArray<AliasDefinition> aliases,
            ImmutableArray<EnumDefinition> enums,
            ImmutableArray<NodeDefinition> nodes)
        {
            if (rootAlias == null)
                throw new ArgumentNullException(nameof(rootAlias));
            if (aliases == null)
                throw new ArgumentNullException(nameof(aliases));
            if (enums == null)
                throw new ArgumentNullException(nameof(enums));
            if (nodes == null)
                throw new ArgumentNullException(nameof(nodes));

            // Verify that root-alias exists in the aliases array.
            Debug.Assert(aliases.Contains(rootAlias), "Root-alias must exist in aliases array");

            // Verify that there are no duplicate alias/enum identifiers.
            Debug.Assert(
                aliases.Select(a => a.Identifier).Concat(enums.Select(e => e.Identifier)).IsUnique(),
                "Alias/Enum identifiers must be unique");

            // Verify that there are no duplicate nodes.
            Debug.Assert(nodes.Select(n => n.Type).IsUnique(), "Nodetype must be unique");

            // Verify that aliases only reference nodes that actually exist.
            Debug.Assert(
                aliases.SelectMany(a => a.Values).All(aliasVal => nodes.Any(n => n.Type == aliasVal)),
                "Alias defines a value that is not a type in the types array");

            this.RootAlias = rootAlias;
            this.Aliases = aliases;
            this.Enums = enums;
            this.Nodes = nodes;
        }

        /// <summary>
        /// Alias that will be used for the root-node of the tree.
        /// </summary>
        public AliasDefinition RootAlias { get; }

        /// <summary>
        /// Set of aliasses that this tree can contain.
        /// </summary>
        public ImmutableArray<AliasDefinition> Aliases { get; }

        /// <summary>
        /// Set of enums that this tree can contain.
        /// </summary>
        public ImmutableArray<EnumDefinition> Enums { get; }

        /// <summary>
        /// Set of nodes that this tree can contain.
        /// </summary>
        public ImmutableArray<NodeDefinition> Nodes { get; }

        /// <summary>Check if two instances are equal.</summary>
        /// <param name="a">Item to compare to B</param>
        /// <param name="b">Item to compare to A</param>
        /// <returns>True if equal, otherwise false</returns>
        public static bool operator ==(TreeDefinition a, TreeDefinition b)
        {
            if (object.ReferenceEquals(a, null))
                return object.ReferenceEquals(b, null);
            return a.Equals(b);
        }

        /// <summary>Check if two instances are not equal.</summary>
        /// <param name="a">Item to compare to B</param>
        /// <param name="b">Item to compare to A</param>
        /// <returns>False if equal, otherwise true</returns>
        public static bool operator !=(TreeDefinition a, TreeDefinition b) => !(a == b);

        /// <summary>
        /// Attempt to get an alias by identifier.
        /// </summary>
        /// <param name="identifier">Identifier of the alias to get</param>
        /// <param name="alias">Found alias</param>
        /// <returns>True if found, otherwise false</returns>
        public bool TryGetAlias(string identifier, out AliasDefinition alias)
        {
            alias = this.Aliases.FirstOrDefault(a => a.Identifier == identifier);
            return alias != null;
        }

        /// <summary>
        /// Check if an alias with given identifier exists on this scheme.
        /// </summary>
        /// <param name="identifier">Identifier of the alias</param>
        /// <returns>True if found, otherwise false</returns>
        public bool ContainsAlias(string identifier) => this.TryGetAlias(identifier, out _);

        /// <summary>
        /// Attempt to get an enum by identifier.
        /// </summary>
        /// <param name="identifier">Identifier of the enum to get</param>
        /// <param name="enum">Found enum</param>
        /// <returns>True if found, otherwise false</returns>
        public bool TryGetEnum(string identifier, out EnumDefinition @enum)
        {
            @enum = this.Enums.FirstOrDefault(e => e.Identifier == identifier);
            return @enum != null;
        }

        /// <summary>
        /// Check if an enum with given identifier exists on this scheme.
        /// </summary>
        /// <param name="identifier">Identifier of the enum</param>
        /// <returns>True if found, otherwise false</returns>
        public bool ContainsEnum(string identifier) => this.TryGetEnum(identifier, out _);

        /// <summary>
        /// Attempt to get an node by type.
        /// </summary>
        /// <param name="type">Type of the node to get</param>
        /// <param name="node">Found node</param>
        /// <returns>True if found, otherwise false</returns>
        public bool TryGetNode(string type, out NodeDefinition node)
        {
            node = this.Nodes.FirstOrDefault(a => a.Type == type);
            return node != null;
        }

        /// <summary>
        /// Check if an node with given type exists on this scheme.
        /// </summary>
        /// <param name="type">Type of the node</param>
        /// <returns>True if found, otherwise false</returns>
        public bool ContainsNode(string type) => this.TryGetNode(type, out _);

        /// <summary>
        /// Check if this is structurally equal to given object.
        /// </summary>
        /// <param name="obj">Object to compare to</param>
        /// <returns>True if equal, otherwise false</returns>
        public override bool Equals(object obj) =>
            obj != null &&
            obj is TreeDefinition &&
            this.Equals((TreeDefinition)obj);

        /// <summary>
        /// Check if this is structurally equal to given other tree.
        /// </summary>
        /// <param name="other">Tree to compare to</param>
        /// <returns>True if equal, otherwise false</returns>
        public bool Equals(TreeDefinition other) =>
            other != null &&
            other.RootAlias == this.RootAlias &&
            other.Aliases.SequenceEqual(this.Aliases) &&
            other.Enums.SequenceEqual(this.Enums) &&
            other.Nodes.SequenceEqual(this.Nodes);

        /// <summary>
        /// Get a hashcode representing this tree.
        /// </summary>
        public override int GetHashCode() => HashCode.Combine(
            this.RootAlias,
            this.Aliases.GetSequenceHashCode(),
            this.Enums.GetSequenceHashCode(),
            this.Nodes.GetSequenceHashCode());
    }
}
