using System;

namespace TypedTree.Generator.Core.Builder.Exceptions
{
    /// <summary>
    /// Exception for when an alias cannot be found.
    /// </summary>
    public sealed class AliasNotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AliasNotFoundException"/> class.
        /// </summary>
        /// <param name="identifier">Identifier of missing alias</param>
        public AliasNotFoundException(string identifier)
            : base(message: $"Alias '{identifier}' not found")
        {
            this.Identifier = identifier;
        }

        /// <summary>
        /// Identifier of the alias that was not found.
        /// </summary>
        public string Identifier { get; }
    }
}
