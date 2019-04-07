using System;

namespace TypedTree.Generator.Core.Mapping.Exceptions
{
    /// <summary>
    /// Exception for when the same enum identifier is used twice but with different entries.
    /// </summary>
    public sealed class DuplicateEnumException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DuplicateEnumException"/> class.
        /// </summary>
        /// <param name="identifier">Identifier of the duplicated enum</param>
        public DuplicateEnumException(string identifier)
            : base(message: $"Enum '{identifier}' was used twice but with different entries")
        {
            this.Identifier = identifier;
        }

        /// <summary>
        /// Enum identifier that was used twice.
        /// </summary>
        public string Identifier { get; }
    }
}
