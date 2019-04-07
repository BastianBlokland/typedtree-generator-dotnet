using System;

using TypedTree.Generator.Core.Builder;
using TypedTree.Generator.Core.Scheme;

namespace TypedTree.Generator.Core.Mapping
{
    /// <summary>
    /// Utility for mapping a class structure to tree-definition.
    /// </summary>
    public static class TreeMapper
    {
        /// <summary>
        /// Map a class structure to a tree-definition.
        /// </summary>
        /// <param name="context">Context to use during mapping</param>
        /// <param name="rootAliasType">Type for the root-alias of the tree</param>
        /// <exception cref="Exceptions.MissingTypeException">
        /// Thrown when the type for the root-alias cannot be found in the context.
        /// </exception>
        /// <exception cref="Exceptions.MappingFailureException">
        /// Thrown when an error occurs during mapping.
        /// </exception>
        /// <returns>Newly created immutable tree-definition</returns>
        public static TreeDefinition MapTree(this Context context, Type rootAliasType)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            if (rootAliasType == null)
                throw new ArgumentNullException(nameof(rootAliasType));
            return MapTree(context, rootAliasType.FullName);
        }

        /// <summary>
        /// Map a class structure to a tree-definition.
        /// </summary>
        /// <param name="context">Context to use during mapping</param>
        /// <param name="rootAliasIdentifier">Identifier for the root-alias of the tree</param>
        /// <exception cref="Exceptions.MissingTypeException">
        /// Thrown when the type for the root-alias cannot be found in the context.
        /// </exception>
        /// <exception cref="Exceptions.MappingFailureException">
        /// Thrown when an error occurs during mapping.
        /// </exception>
        /// <returns>Newly created immutable tree-definition</returns>
        public static TreeDefinition MapTree(this Context context, string rootAliasIdentifier)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            if (string.IsNullOrEmpty(rootAliasIdentifier))
                throw new ArgumentException($"Alias-type '{rootAliasIdentifier}' is not valid", nameof(rootAliasIdentifier));

            if (!context.Types.TryGetType(rootAliasIdentifier, out var rootAliasType))
                throw new Exceptions.MissingTypeException(rootAliasIdentifier);

            try
            {
                return TreeDefinitionBuilder.Create(rootAliasIdentifier, b =>
                {
                    // Map the root alias
                    var rootAlias = b.MapAlias(context, rootAliasType);

                    // Map the nodes in the root-alias
                    b.MapNodes(context, rootAlias);
                });
            }
            catch (Exception e)
            {
                throw new Exceptions.MappingFailureException(e);
            }
        }
    }
}
