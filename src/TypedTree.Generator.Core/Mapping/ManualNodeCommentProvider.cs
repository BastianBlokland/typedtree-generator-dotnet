using System;
using System.Collections.Generic;

namespace TypedTree.Generator.Core.Mapping
{
    /// <summary>
    /// Implementation of <see cref="INodeCommentProvider"/> that uses manually entered comments.
    /// </summary>
    public sealed class ManualNodeCommentProvider : INodeCommentProvider
    {
        private readonly IReadOnlyDictionary<string, string> nodeComments;

        /// <summary>
        /// Initializes a new instance of the <see cref="ManualNodeCommentProvider"/> class.
        /// </summary>
        /// <param name="nodeComments">Dictionary of full type-name to comment</param>
        public ManualNodeCommentProvider(IReadOnlyDictionary<string, string> nodeComments)
        {
            this.nodeComments = nodeComments ?? throw new ArgumentNullException(nameof(nodeComments));
        }

        /// <summary>
        /// Get a comment for given type.
        /// </summary>
        /// <param name="type">Type to get the comment for</param>
        /// <returns>Comment string if available, otherwise null</returns>
        public string GetComment(Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            if (this.nodeComments.TryGetValue(type.FullName, out var comment))
                return comment;

            return null;
        }
    }
}
