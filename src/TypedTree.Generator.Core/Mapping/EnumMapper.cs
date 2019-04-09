using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.Extensions.Logging;

using TypedTree.Generator.Core.Builder;
using TypedTree.Generator.Core.Scheme;

namespace TypedTree.Generator.Core.Mapping
{
    /// <summary>
    /// Utility for mapping a <see cref="System.Enum"/> type to tree-definition builder.
    /// </summary>
    public static class EnumMapper
    {
        /// <summary>
        /// Map a <see cref="System.Enum"/> type to a tree-defintion builder.
        /// </summary>
        /// <remarks>
        /// If enum does not yet exist in the builder it is pushed, otherwise this will verify that
        /// the existing enum matches the signature of the new enum.
        /// </remarks>
        /// <param name="builder">Builder to push the enum definition to</param>
        /// <param name="context">Context object with dependencies for the mapping</param>
        /// <param name="enumType">Enum-type to map</param>
        /// <exception cref="Exceptions.DuplicateEnumException">
        /// Thrown when two different enums have the same full-name.
        /// </exception>
        /// <exception cref="Exceptions.InvalidEnumValueException">
        /// Thrown when an enum value is not convertible to a 32 bit signed integer.
        /// </exception>
        /// <returns>Definition that the type was mapped to</returns>
        public static EnumDefinition MapEnum(
            this TreeDefinitionBuilder builder,
            Context context,
            Type enumType)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            if (enumType == null)
                throw new ArgumentNullException(nameof(enumType));
            if (!enumType.IsEnum)
                throw new ArgumentException($"Provided type: '{enumType}' is not an enum", nameof(enumType));

            var identifier = enumType.FullName;
            var entries = GetEntries();

            if (builder.TryGetEnum(identifier, out var existingEnum))
            {
                // Verify if this matches with the existing enum with the same identifier
                if (!existingEnum.Values.SequenceEqual(entries))
                    throw new Exceptions.DuplicateEnumException(identifier);
                return existingEnum;
            }
            else
            {
                // Diagnostic logging.
                context.Logger?.LogDebug($"Mapped enum '{identifier}'");
                context.Logger?.LogTrace(
                    $"entries:\n{string.Join("\n", entries.Select(e => $"* '{e}'"))}");

                // Push new enum
                return builder.PushEnum(identifier, entries);
            }

            IEnumerable<EnumEntry> GetEntries()
            {
                var names = Enum.GetNames(enumType);
                var values = Enum.GetValues(enumType);
                for (int i = 0; i < names.Length; i++)
                {
                    var rawVal = values.GetValue(i);
                    yield return new EnumEntry(names[i], ConvertEnumValue(rawVal));
                }
            }

            int ConvertEnumValue(object rawVal)
            {
                try
                {
                    return Convert.ToInt32(rawVal, CultureInfo.InvariantCulture);
                }
                catch (OverflowException)
                {
                    throw new Exceptions.InvalidEnumValueException(identifier, rawVal);
                }
            }
        }
    }
}
