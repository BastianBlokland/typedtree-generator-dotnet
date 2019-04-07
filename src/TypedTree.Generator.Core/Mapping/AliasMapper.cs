using System;
using System.Linq;

using TypedTree.Generator.Core.Builder;
using TypedTree.Generator.Core.Scheme;
using TypedTree.Generator.Core.Utilities;

namespace TypedTree.Generator.Core.Mapping
{
    /// <summary>
    /// Utility for mapping a alias from a interface / class / struct to tree-definition builder.
    /// </summary>
    public static class AliasMapper
    {
        /// <summary>
        /// Map a interface / class / struct to an alias on a tree-defintion builder
        /// </summary>
        /// <remarks>
        /// If alias does not yet exist in the builder it is pushed, otherwise this will verify that
        /// the existing alias matches the implementations of the new type.
        /// </remarks>
        /// <param name="builder">Builder to push the alias definition to</param>
        /// <param name="context">Context object with dependencies for the mapping</param>
        /// <param name="aliasType">Interface / class / struct to map</param>
        /// <exception cref="Exceptions.DuplicateAliasException">
        /// Thrown when a alias with the same identifier is already pushed but implementations do
        /// not match.
        /// </exception>
        /// <returns>Definition that the type was mapped to</returns>
        public static AliasDefinition MapAlias(
            this TreeDefinitionBuilder builder,
            Context context,
            Type aliasType)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            if (aliasType == null)
                throw new ArgumentNullException(nameof(aliasType));

            var identifier = aliasType.FullName;
            var implementations = context.Types.GetImplementations(aliasType, context.TypeIgnorePattern);

            if (builder.TryGetAlias(identifier, out var existingAlias))
            {
                // Verify that the existing alias defines the same implementations.
                if (!implementations.Select(i => i.FullName).SequenceEqual(existingAlias.Values))
                    throw new Exceptions.DuplicateAliasException(identifier);

                return existingAlias;
            }
            else
            {
                // Push new alias
                return builder.PushAlias(identifier, implementations.Select(i => i.FullName));
            }
        }
    }
}
