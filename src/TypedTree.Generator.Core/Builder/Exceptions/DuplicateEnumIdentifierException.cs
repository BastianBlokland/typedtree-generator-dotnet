using System;

namespace TypedTree.Generator.Core.Builder.Exceptions
{
    /// <summary>
    /// Exception for when an enum identifier conflicts with another enum or alias identifier.
    /// </summary>
    public sealed class DuplicateEnumIdentifierException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DuplicateEnumIdentifierException"/> class
        /// </summary>
        /// <param name="identifier">Enum identifier that is duplicated</param>
        public DuplicateEnumIdentifierException(string identifier)
            : base(message: $"Duplicate alias / enum identifier: '{identifier}'")
        {
            this.Identifier = identifier;
        }

        /// <summary>
        /// Identifier that was duplicated as either a Enum or Alias.
        /// </summary>
        public string Identifier { get; }
    }
}
