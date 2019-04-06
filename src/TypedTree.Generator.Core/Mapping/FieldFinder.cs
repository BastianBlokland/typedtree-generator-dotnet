using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;

namespace TypedTree.Generator.Core.Mapping
{
    /// <summary>
    /// Utility for finding fields on a class / struct.
    /// </summary>
    public static class FieldFinder
    {
        /// <summary>
        /// Find fields on a given type.
        /// </summary>
        /// <param name="type">Type to find fields on (class or struct)</param>
        /// <param name="source">Source where to look for fields on the given type</param>
        /// <returns>Collection of fields (identifier and type)</returns>
        public static IEnumerable<(string identifier, Type type)> FindFields(
            this Type type,
            FieldSource source)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));
            if (type.IsEnum || type.IsPrimitive)
                throw new ArgumentException("Type has to be a class or a struct", nameof(type));

            switch (source)
            {
                case FieldSource.PublicProperties:
                    return FindProperties(includeNonPublic: false);
                case FieldSource.Properties:
                    return FindProperties(includeNonPublic: true);
                case FieldSource.PublicConstructorParameters:
                    return FindConstructorParameters(includeNonPublic: false);
                case FieldSource.ConstructorParameters:
                    return FindConstructorParameters(includeNonPublic: true);
                default:
                    throw new InvalidOperationException($"Unknown field-source: '{source}'");
            }

            IEnumerable<(string, Type)> FindConstructorParameters(bool includeNonPublic)
            {
                return FindConstructor(includeNonPublic)?.
                    GetParameters().Select(p => (p.Name, p.ParameterType)) ??
                    Array.Empty<(string, Type)>();
            }

            ConstructorInfo FindConstructor(bool includeNonPublic)
            {
                var bindingFlags = GetBindingFlags(includeNonPublic);

                // Return the constructor with the most parameters.
                return type.GetConstructors(bindingFlags).
                    OrderByDescending(ci => ci.GetParameters().Length).
                    FirstOrDefault();
            }

            IEnumerable<(string, Type)> FindProperties(bool includeNonPublic)
            {
                var bindingFlags = GetBindingFlags(includeNonPublic);

                return type.GetProperties(bindingFlags).
                    Where(pi => pi.CanRead && pi.CanWrite).
                    Select(pi => (pi.Name, pi.PropertyType));
            }

            BindingFlags GetBindingFlags(bool includeNonPublic) =>
                includeNonPublic ?
                    BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance :
                    BindingFlags.Public | BindingFlags.Instance;
        }
    }
}
