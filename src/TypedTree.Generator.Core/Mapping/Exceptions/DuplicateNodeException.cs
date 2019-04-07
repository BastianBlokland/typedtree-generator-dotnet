using System;

namespace TypedTree.Generator.Core.Mapping.Exceptions
{
    /// <summary>
    /// Exception for when the same node identifier is used twice but with different fields.
    /// </summary>
    public sealed class DuplicateNodeException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DuplicateNodeException"/> class.
        /// </summary>
        /// <param name="identifier">Identifier of the duplicated node</param>
        public DuplicateNodeException(string identifier)
            : base(message: $"Node '{identifier}' was used twice but with different fields")
        {
            this.Identifier = identifier;
        }

        /// <summary>
        /// Node identifier that was used twice.
        /// </summary>
        public string Identifier { get; }
    }
}
