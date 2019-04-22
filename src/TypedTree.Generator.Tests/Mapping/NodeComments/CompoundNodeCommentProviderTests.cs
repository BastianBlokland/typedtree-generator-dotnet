using System.Collections.Generic;
using Xunit;

using TypedTree.Generator.Core.Mapping.NodeComments;

namespace TypedTree.Generator.Tests.Mapping.NodeComments
{
    public sealed class CompoundNodeCommentProviderTests
    {
        [Fact]
        public void EnteredCommentsCanBeRetrieved()
        {
            var provider = new CompoundNodeCommentProvider(
                new ManualNodeCommentProvider(new Dictionary<string, string>
                {
                    { typeof(int).FullName, "This is a integer" }
                }),
                new ManualNodeCommentProvider(new Dictionary<string, string>
                {
                    { typeof(string).FullName, "This is a string" }
                }));
            Assert.Equal("This is a integer", provider.GetComment(typeof(int)));
            Assert.Equal("This is a string", provider.GetComment(typeof(string)));
        }

        [Fact]
        public void NoCommentIsReturnedInCaseOfMissingEntry()
        {
            var provider = new CompoundNodeCommentProvider();
            Assert.Null(provider.GetComment(this.GetType()));
        }
    }
}
