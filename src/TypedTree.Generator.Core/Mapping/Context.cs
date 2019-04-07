using System;
using System.Reflection;
using System.Text.RegularExpressions;

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
            Regex typeIgnorePattern = null)
        {
            if (types == null)
                throw new ArgumentNullException(nameof(types));

            this.Types = types;
            this.FieldSource = fieldSource;
            this.TypeIgnorePattern = typeIgnorePattern;
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
        /// Create a mapping context.
        /// </summary>
        /// <param name="assembly">Assembly to use as the source of types</param>
        /// <param name="fieldSource">Where to look for fields on a node</param>
        /// <param name="typeIgnorePattern">Optional regex pattern for types to ignore</param>
        /// <returns>Newly created context</returns>
        public static Context Create(
            Assembly assembly,
            FieldSource fieldSource,
            Regex typeIgnorePattern = null)
        {
            if (assembly == null)
                throw new ArgumentNullException(nameof(assembly));

            var typeCollection = TypeCollection.Create(assembly);
            return Create(typeCollection, fieldSource, typeIgnorePattern);
        }

        /// <summary>
        /// Create a mapping context.
        /// </summary>
        /// <param name="types">Set of types</param>
        /// <param name="fieldSource">Where to look for fields on a node</param>
        /// <param name="typeIgnorePattern">Optional regex pattern for types to ignore</param>
        /// <returns>Newly created context</returns>
        public static Context Create(
            ITypeCollection types,
            FieldSource fieldSource,
            Regex typeIgnorePattern = null)
        {
            if (types == null)
                throw new ArgumentNullException(nameof(types));

            return new Context(types, fieldSource, typeIgnorePattern);
        }
    }
}
