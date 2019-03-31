using System;

namespace TypedTree.Generator.Core.Builder.Exceptions
{
    /// <summary>
    /// Exception for when an node type conflicts with another node type.
    /// </summary>
    public sealed class DuplicateNodeTypeException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DuplicateNodeTypeException"/> class
        /// </summary>
        /// <param name="typeIdentifier">Node-type that is duplicated</param>
        public DuplicateNodeTypeException(string typeIdentifier)
            : base(message: $"Duplicate node-type: '{typeIdentifier}'")
        {
            this.Identifier = typeIdentifier;
        }

        /// <summary>
        /// Node-type that is duplicated.
        /// </summary>
        public string Identifier { get; }
    }
}
