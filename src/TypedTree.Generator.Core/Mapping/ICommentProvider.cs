using System;

namespace TypedTree.Generator.Core.Mapping
{
    /// <summary>
    /// Interface for a comment provider.
    /// </summary>
    public interface ICommentProvider
    {
        /// <summary>
        /// Get a comment for given type.
        /// </summary>
        /// <remarks>Return null if no comment is availble</remarks>
        /// <param name="type">Type to get the comment for</param>
        /// <returns>Comment string if available, otherwise null</returns>
        string GetComment(Type type);
    }
}
