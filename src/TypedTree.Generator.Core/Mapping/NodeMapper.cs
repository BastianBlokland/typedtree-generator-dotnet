using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;

using TypedTree.Generator.Core.Builder;
using TypedTree.Generator.Core.Scheme;
using TypedTree.Generator.Core.Utilities;

using Classification = TypedTree.Generator.Core.Mapping.Classifier.Classification;

namespace TypedTree.Generator.Core.Mapping
{
    /// <summary>
    /// Utility for mapping a class / struct type to tree-definition builder.
    /// </summary>
    public static class NodeMapper
    {
        /// <summary>
        /// Map the implementations of an alias to a tree-defintion builder.
        /// </summary>
        /// <remarks>
        /// If nodes do not yet exist in the builder they are pushed, otherwise this will verify that
        /// the existing nodes match the fields of the new types.
        /// </remarks>
        /// <param name="builder">Builder to push the node definitions to</param>
        /// <param name="context">Context object with dependencies for the mapping</param>
        /// <param name="alias">Alias whose nodes to map</param>
        /// <exception cref="Exceptions.MissingTypeException">
        /// Thrown when a type for a node of the alias cannot be found in the context.
        /// </exception>
        /// <exception cref="Exceptions.DuplicateNodeException">
        /// Thrown when a node with the same identifier is already pushed but fields do not match.
        /// </exception>
        /// <returns>Definitions that the alias types are mapped to</returns>
        public static NodeDefinition[] MapNodes(
            this TreeDefinitionBuilder builder,
            Context context,
            AliasDefinition alias)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            if (alias == null)
                throw new ArgumentNullException(nameof(alias));

            var nodes = new NodeDefinition[alias.Values.Length];
            for (int i = 0; i < nodes.Length; i++)
            {
                var identifier = alias.Values[i];
                if (!context.Types.TryGetType(identifier, out var nodeType))
                    throw new Exceptions.MissingTypeException(identifier);

                nodes[i] = MapNode(builder, context, nodeType);
            }

            return nodes;
        }

        /// <summary>
        /// Map a struct / class type to a tree-defintion builder.
        /// </summary>
        /// <remarks>
        /// If node does not yet exist in the builder it is pushed, otherwise this will verify that
        /// the existing node matches the fields of the new type.
        /// </remarks>
        /// <param name="builder">Builder to push the node definition to</param>
        /// <param name="context">Context object with dependencies for the mapping</param>
        /// <param name="nodeType">Struct / class to map</param>
        /// <exception cref="Exceptions.DuplicateNodeException">
        /// Thrown when a node with the same identifier is already pushed but fields do not match.
        /// </exception>
        /// <returns>Definition that the type was mapped to</returns>
        public static NodeDefinition MapNode(
            this TreeDefinitionBuilder builder,
            Context context,
            Type nodeType)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            if (nodeType == null)
                throw new ArgumentNullException(nameof(nodeType));
            if (nodeType.IsPrimitive || nodeType.IsEnum || nodeType.IsInterface || nodeType.IsAbstract)
                throw new ArgumentException($"Provided type: '{nodeType}' is not a class / struct", nameof(nodeType));

            var identifier = nodeType.FullName;
            if (builder.TryGetNode(identifier, out var existingNode))
            {
                // Verify if this matches with the existing node with the same identifier.
                var fields = GetFields().Where(f => f.@class > Classification.Invalid);

                // Verify that it contains the same amount of fields.
                if (fields.Count() != existingNode.Fields.Length)
                    throw new Exceptions.DuplicateNodeException(identifier);

                // Verify that it contains all the same field identifiers.
                if (fields.Any(f => !existingNode.TryGetField(f.id, out var _)))
                    throw new Exceptions.DuplicateNodeException(identifier);

                return existingNode;
            }
            else
            {
                // Push new node
                var result = builder.PushNode(identifier, b =>
                {
                    // Add optional comment.
                    b.Comment = context.CommentProvider?.GetComment(nodeType);

                    // Add nodes.
                    foreach (var field in GetFields())
                    {
                        switch (field.@class)
                        {
                            case Classification.Number:
                                b.PushNumberField(field.id, field.isArray);
                                break;
                            case Classification.String:
                                b.PushStringField(field.id, field.isArray);
                                break;
                            case Classification.Boolean:
                                b.PushBooleanField(field.id, field.isArray);
                                break;
                            case Classification.Alias:
                                var aliasDefinition = builder.MapAlias(context, field.type);
                                b.PushAliasField(field.id, aliasDefinition, field.isArray);
                                break;
                            case Classification.Enum:
                                var enumDefinition = builder.MapEnum(context, field.type);
                                b.PushEnumField(field.id, enumDefinition, field.isArray);
                                break;
                        }
                    }
                });

                // Diagnostic logging.
                context.Logger?.LogDebug($"Mapped node '{identifier}'");
                if (result.Fields.Length == 0)
                {
                    context.Logger?.LogTrace("no fields");
                }
                else
                {
                    context.Logger?.LogTrace(
                        $"fields:\n{string.Join("\n", result.Fields.Select(f => $"* '{f}'"))}");
                }

                // Push all the 'inner' nodes of this node.
                foreach (var field in result.Fields)
                {
                    if (field is AliasNodeField aliasField)
                        MapNodes(builder, context, aliasField.Alias);
                }

                return result;
            }

            IEnumerable<(string id, Classification @class, Type type, bool isArray)> GetFields()
            {
                foreach (var field in nodeType.FindFields(context.FieldSource))
                {
                    var isArray = field.type.TryGetElementType(out var elementType);
                    var type = isArray ? elementType : field.type;
                    var classification = Classifier.Classify(context.Types, type, context.TypeIgnorePattern);

                    yield return (field.identifier, classification, type, isArray);
                }
            }
        }
    }
}
