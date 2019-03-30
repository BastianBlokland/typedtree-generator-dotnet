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
    public sealed class TreeDefinition
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
    }
}
