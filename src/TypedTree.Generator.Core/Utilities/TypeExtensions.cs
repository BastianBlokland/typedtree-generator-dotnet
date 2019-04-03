using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace TypedTree.Generator.Core.Utilities
{
    /// <summary>
    /// Utility extensions for Types
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// Get all implementations that can be assigned to given target type.
        /// </summary>
        /// <remarks>
        /// Implementations are considered types that can be constructed (both class or struct).
        /// So no interfaces, abstract classes, etc.
        /// </remarks>
        /// <param name="types">Collection to look for types</param>
        /// <param name="targetType">Type that the implementation needs to be assignable to.</param>
        /// <param name="ignoreRegex">If provided this regex will be used to filter the output</param>
        /// <param name="includeGenericTypes">
        /// Should generic-classes be considered as implementations
        /// </param>
        /// <returns>Implementations that are assignable to given target</returns>
        public static IEnumerable<Type> GetImplementations(
            this IEnumerable<Type> types,
            Type targetType,
            Regex ignoreRegex = null,
            bool includeGenericTypes = false)
        {
            return types.Where(IsAssignableToTarget).Where(IsPlainImplementation).Where(IsNotIgnored);

            bool IsAssignableToTarget(Type type) => targetType.IsAssignableFrom(type);

            bool IsPlainImplementation(Type type) =>
                !type.IsAbstract &&
                !type.IsInterface &&
                !type.IsArray &&
                !type.IsEnum &&
                (includeGenericTypes || !type.IsGenericType) &&
                !type.IsPrimitive;

            bool IsNotIgnored(Type type)
            {
                if (ignoreRegex == null)
                    return true;
                return !ignoreRegex.IsMatch(type.FullName);
            }
        }
    }
}
