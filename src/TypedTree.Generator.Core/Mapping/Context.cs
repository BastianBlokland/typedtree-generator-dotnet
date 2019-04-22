using System;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;

using TypedTree.Generator.Core.Utilities;

namespace TypedTree.Generator.Core.Mapping
{
    /// <summary>
    /// Class for settings required for mapping.
    /// </summary>
    public sealed class Context
    {
        internal Context(
            ITypeCollection types,
            FieldSource fieldSource,
            Regex typeIgnorePattern = null,
            INodeCommentProvider commentProvider = null,
            ILogger logger = null)
        {
            if (types == null)
                throw new ArgumentNullException(nameof(types));

            this.Types = types;
            this.FieldSource = fieldSource;
            this.TypeIgnorePattern = typeIgnorePattern;
            this.CommentProvider = commentProvider;
            this.Logger = logger;
        }

        /// <summary>
        /// Full set of custom types.
        /// </summary>
        public ITypeCollection Types { get; }

        /// <summary>
        /// Where to look for fields on a node.
        /// </summary>
        public FieldSource FieldSource { get; }

        /// <summary>
        /// Optional regex pattern for types to ignore.
        /// </summary>
        public Regex TypeIgnorePattern { get; }

        /// <summary>
        /// Optional provider for node-comments to be added to the scheme.
        /// </summary>
        public INodeCommentProvider CommentProvider { get; }

        /// <summary>
        /// Optional logger.
        /// </summary>
        public ILogger Logger { get; }

        /// <summary>
        /// Create a mapping context.
        /// </summary>
        /// <param name="types">Set of types</param>
        /// <param name="fieldSource">Where to look for fields on a node</param>
        /// <param name="typeIgnorePattern">Optional regex pattern for types to ignore</param>
        /// <param name="commentProvider">Optional provider for node comments</param>
        /// <param name="logger">Optional logger</param>
        /// <returns>Newly created context</returns>
        public static Context Create(
            ITypeCollection types,
            FieldSource fieldSource,
            Regex typeIgnorePattern = null,
            INodeCommentProvider commentProvider = null,
            ILogger logger = null)
        {
            if (types == null)
                throw new ArgumentNullException(nameof(types));

            return new Context(types, fieldSource, typeIgnorePattern, commentProvider, logger);
        }
    }
}
