using System;

namespace TypedTree.Generator.Core.Builder.Exceptions
{
    /// <summary>
    /// Exception for when an alias identifier conflicts with another alias or enum identifier.
    /// </summary>
    public sealed class DuplicateAliasIdentifierException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DuplicateAliasIdentifierException"/> class
        /// </summary>
        /// <param name="identifier">Alias identifier that is duplicated</param>
        public DuplicateAliasIdentifierException(string identifier)
            : base(message: $"Duplicate alias / enum identifier: '{identifier}'")
        {
            this.Identifier = identifier;
        }

        /// <summary>
        /// Identifier that was duplicated as either a Alias or Enum.
        /// </summary>
        public string Identifier { get; }
    }
}
