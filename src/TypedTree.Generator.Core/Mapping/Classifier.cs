#pragma warning disable CA1720 // Identifiers should not contain type names

using System;
using System.Linq;
using System.Text.RegularExpressions;

using TypedTree.Generator.Core.Utilities;

namespace TypedTree.Generator.Core.Mapping
{
    /// <summary>
    /// Utility for classifying a type according to scheme-rules.
    /// </summary>
    public static class Classifier
    {
        /// <summary>
        /// Scheme classification for a type
        /// </summary>
        public enum Classification
        {
            /// <summary>
            /// Type is ignored by user
            /// </summary>
            Ignored = -1,

            /// <summary>
            /// Type is invalid / not-supported
            /// </summary>
            Invalid = 0,

            /// <summary>
            /// Scheme number type
            /// </summary>
            Number = 1,

            /// <summary>
            /// Scheme string type
            /// </summary>
            String = 2,

            /// <summary>
            /// Scheme boolean type
            /// </summary>
            Boolean = 3,

            /// <summary>
            /// Scheme alias type
            /// </summary>
            Alias = 4,

            /// <summary>
            /// Scheme enum type
            /// </summary>
            Enum = 5
        }

        /// <summary>
        /// Classify to what scheme-type a given type maps.
        /// </summary>
        /// <param name="typeCollection">Source of user-types</param>
        /// <param name="type">Type to classify</param>
        /// <param name="typeIgnoreRegex">Optional regex for ignoring types</param>
        /// <returns>Classification for the type</returns>
        public static Classification Classify(
            this ITypeCollection typeCollection,
            Type type,
            Regex typeIgnoreRegex = null)
        {
            if (typeCollection == null)
                throw new ArgumentNullException(nameof(typeCollection));
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            // Well known types
            if (IsNumber())
                return Classification.Number;
            if (IsString())
                return Classification.String;
            if (IsBoolean())
                return Classification.Boolean;

            // Check if type exists in the given collection
            if (!typeCollection.HasType(type))
                return Classification.Invalid;

            // Check if type was ignored by the user.
            if (IsIgnored())
                return Classification.Ignored;

            // Enum
            if (IsEnum())
                return Classification.Enum;

            // Alias
            if (!HasImplementations())
                return Classification.Invalid;
            return Classification.Alias;

            bool IsNumber() =>
                type == typeof(byte) ||
                type == typeof(sbyte) ||
                type == typeof(short) ||
                type == typeof(ushort) ||
                type == typeof(int) ||
                type == typeof(uint) ||
                type == typeof(long) ||
                type == typeof(ulong);

            bool IsString() =>
                type == typeof(string);

            bool IsBoolean() =>
                type == typeof(bool);

            bool IsEnum() =>
                type.IsEnum;

            bool IsIgnored()
            {
                if (typeIgnoreRegex == null)
                    return false;
                return typeIgnoreRegex.IsMatch(type.FullName);
            }

            bool HasImplementations() =>
                typeCollection.GetImplementations(type, typeIgnoreRegex).Any();
        }
    }
}
