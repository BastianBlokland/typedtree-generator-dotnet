using System.Collections.Generic;
using Xunit;

using TypedTree.Generator.Core.Mapping;

namespace TypedTree.Generator.Tests.Mapping
{
    public sealed class ManualCommentProviderTests
    {
        [Fact]
        public void EnteredCommentsCanBeRetrieved()
        {
            var provider = new ManualNodeCommentProvider(new Dictionary<string, string>
            {
                { this.GetType().FullName, "Test comment" }
            });
            Assert.Equal("Test comment", provider.GetComment(this.GetType()));
        }

        [Fact]
        public void NoCommentIsReturnedInCaseOfMissingEntry()
        {
            var provider = new ManualNodeCommentProvider(new Dictionary<string, string>());
            Assert.Null(provider.GetComment(this.GetType()));
        }
    }
}
