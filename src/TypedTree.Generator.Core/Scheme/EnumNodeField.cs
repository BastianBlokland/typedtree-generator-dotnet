using System;

using TypedTree.Generator.Core.Utilities;

namespace TypedTree.Generator.Core.Scheme
{
    /// <summary>
    /// Immutable representation of an enum field on a node.
    /// </summary>
    public sealed class EnumNodeField : INodeField, IEquatable<EnumNodeField>
    {
        internal EnumNodeField(string name, EnumDefinition @enum, bool isArray)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException($"Invalid name: '{name}'", nameof(name));
            if (@enum == null)
                throw new ArgumentNullException(nameof(@enum));

            this.Name = name;
            this.Enum = @enum;
            this.IsArray = isArray;
        }

        /// <summary>
        /// Identifier for this field.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the Enum value that this field can have.
        /// </summary>
        public EnumDefinition Enum { get; }

        /// <summary>
        /// Is this field an array.
        /// </summary>
        public bool IsArray { get; }

        /// <summary>Check if two instances are equal.</summary>
        /// <param name="a">Item to compare to B</param>
        /// <param name="b">Item to compare to A</param>
        /// <returns>True if equal, otherwise false</returns>
        public static bool operator ==(EnumNodeField a, EnumNodeField b)
        {
            if (object.ReferenceEquals(a, null))
                return object.ReferenceEquals(b, null);
            return a.Equals(b);
        }

        /// <summary>Check if two instances are not equal.</summary>
        /// <param name="a">Item to compare to B</param>
        /// <param name="b">Item to compare to A</param>
        /// <returns>False if equal, otherwise true</returns>
        public static bool operator !=(EnumNodeField a, EnumNodeField b) => !(a == b);

        /// <summary>
        /// Check if this is structurally equal to given object.
        /// </summary>
        /// <param name="obj">Object to compare to</param>
        /// <returns>True if equal, otherwise false</returns>
        public override bool Equals(object obj) =>
            obj != null &&
            obj is EnumNodeField &&
            this.Equals((EnumNodeField)obj);

        /// <summary>
        /// Check if this is structurally equal to given other enum-field.
        /// </summary>
        /// <param name="other">Enum-field to compare to</param>
        /// <returns>True if equal, otherwise false</returns>
        public bool Equals(EnumNodeField other) =>
            other != null &&
            other.Name == this.Name &&
            other.Enum == this.Enum &&
            other.IsArray == this.IsArray;

        /// <summary>
        /// Get a hashcode representing this enum-field.
        /// </summary>
        public override int GetHashCode() => HashCode.Combine(
            this.Name,
            this.Enum,
            this.IsArray);

        /// <summary>
        /// Get a string representation for this field.
        /// </summary>
        public override string ToString() =>
            $"{this.Name}:{this.Enum.Identifier}{(this.IsArray ? "[]" : string.Empty)}";
    }
}
