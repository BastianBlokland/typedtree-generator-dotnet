using System;
using System.Collections.Generic;

namespace TypedTree.Generator.Core.Mapping.NodeComments
{
    /// <summary>
    /// Implementation of <see cref="INodeCommentProvider"/> that combines other providers.
    /// </summary>
    public sealed class CompoundNodeCommentProvider : INodeCommentProvider
    {
        private readonly IEnumerable<INodeCommentProvider> providers;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompoundNodeCommentProvider"/> class.
        /// </summary>
        /// <param name="providers">Array of providers to wrap</param>
        public CompoundNodeCommentProvider(params INodeCommentProvider[] providers)
            : this((IEnumerable<INodeCommentProvider>)providers)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompoundNodeCommentProvider"/> class.
        /// </summary>
        /// <param name="providers">Enumerable of providers to wrap</param>
        public CompoundNodeCommentProvider(IEnumerable<INodeCommentProvider> providers)
        {
            this.providers = providers ?? throw new ArgumentNullException(nameof(providers));
        }

        /// <summary>
        /// Get a comment for given type.
        /// </summary>
        /// <remarks> Will return the comment from the first provider that has one.</remarks>
        /// <param name="type">Type to get the comment for</param>
        /// <returns>Comment string if available, otherwise null</returns>
        public string GetComment(Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            foreach (var provider in this.providers)
            {
                var comment = provider.GetComment(type);
                if (!string.IsNullOrEmpty(comment))
                    return comment;
            }

            return null;
        }
    }
}
