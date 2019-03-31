using System;

namespace TypedTree.Generator.Core.Builder.Exceptions
{
    /// <summary>
    /// Exception for when an node cannot be found.
    /// </summary>
    public sealed class NodeNotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NodeNotFoundException"/> class.
        /// </summary>
        /// <param name="typeIdentifier">Type of missing node</param>
        public NodeNotFoundException(string typeIdentifier)
            : base(message: $"Node '{typeIdentifier}' not found")
        {
            this.TypeIdentifier = typeIdentifier;
        }

        /// <summary>
        /// Type of the node that was not found.
        /// </summary>
        public string TypeIdentifier { get; }
    }
}
