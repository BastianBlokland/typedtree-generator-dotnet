using Xunit;

using TypedTree.Generator.Core.Mapping.NodeComments;

namespace TypedTree.Generator.Tests.Mapping.NodeComments
{
    public sealed class XmlNodeCommentProviderTests
    {
        [Fact]
        public void CommentsCanBeRetrievedFromXml()
        {
            var xml =
$@"<?xml version=""1.0""?>
<doc>
    <assembly>
        <name>Test</name>
    </assembly>
    <members>
        <member name=""T:{typeof(XmlNodeCommentProviderTests).FullName}"">
            <summary>
            Test line 1.
            Test line 2.
            </summary>
        </member>
    </members>
</doc>";
            Assert.True(XmlNodeCommentProvider.TryParse(xml, out var provider));
            Assert.Equal("Test line 1.\nTest line 2.\n", provider.GetComment(this.GetType()));
        }

        [Fact]
        public void NoCommentIsReturnedInCaseOfMissingSummary()
        {
            var xml =
$@"<?xml version=""1.0""?>
<doc>
    <assembly>
        <name>Test</name>
    </assembly>
    <members>
        <member name=""T:{typeof(XmlNodeCommentProviderTests).FullName}"">
        </member>
    </members>
</doc>";
            Assert.True(XmlNodeCommentProvider.TryParse(xml, out var provider));
            Assert.Null(provider.GetComment(this.GetType()));
        }

        [Fact]
        public void NoCommentIsReturnedInCaseOfMissingType()
        {
            var xml =
$@"<?xml version=""1.0""?>
<doc>
    <assembly>
        <name>Test</name>
    </assembly>
    <members>
    </members>
</doc>";
            Assert.True(XmlNodeCommentProvider.TryParse(xml, out var provider));
            Assert.Null(provider.GetComment(this.GetType()));
        }
    }
}
