using System;

using TypedTree.Generator.Core.Utilities;

namespace TypedTree.Generator.Core.Scheme
{
    /// <summary>
    /// Immutable single entry in an enum. Basically a named number.
    /// </summary>
    public struct EnumEntry : IEquatable<EnumEntry>
    {
        internal EnumEntry(string name, int value)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException($"Invalid name: '{name}'", nameof(name));

            this.Name = name;
            this.Value = value;
        }

        /// <summary>
        /// Name for this value.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Value that this entry maps to.
        /// </summary>
        public int Value { get; }

        /// <summary>Check if two entries are equal.</summary>
        /// <remarks>Does not check if entries belong to the same 'Enum' or not.</remarks>
        /// <param name="a">Item to compare to B</param>
        /// <param name="b">Item to compare to A</param>
        /// <returns>True if equal, otherwise false</returns>
        public static bool operator ==(EnumEntry a, EnumEntry b) => a.Equals(b);

        /// <summary>Check if two entries are not equal.</summary>
        /// <remarks>Does not check if entries belong to the same 'Enum' or not.</remarks>
        /// <param name="a">Item to compare to B</param>
        /// <param name="b">Item to compare to A</param>
        /// <returns>False if equal, otherwise true</returns>
        public static bool operator !=(EnumEntry a, EnumEntry b) => !a.Equals(b);

        /// <summary>
        /// Check if this entry is equal to the given object.
        /// </summary>
        /// <remarks>Does not check if entries belong to the same 'Enum' or not.</remarks>
        /// <param name="obj">Object to compare to</param>
        /// <returns>True if equal, otherwise false</returns>
        public override bool Equals(object obj) =>
            obj is EnumEntry &&
            this.Equals((EnumEntry)obj);

        /// <summary>
        /// Check if this entry is equal to the given other entry.
        /// </summary>
        /// <remarks>Does not check if entries belong to the same 'Enum' or not.</remarks>
        /// <param name="other">Entry to compare to</param>
        /// <returns>True if equal, otherwise false</returns>
        public bool Equals(EnumEntry other) =>
            other.Name == this.Name &&
            other.Value == this.Value;

        /// <summary>
        /// Get a hashcode representing this entry.
        /// </summary>
        public override int GetHashCode() => HashCode.Combine(this.Name, this.Value);
    }
}
