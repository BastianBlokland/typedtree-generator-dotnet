using Xunit;

using TypedTree.Generator.Core.Mapping;

namespace TypedTree.Generator.Tests.Mapping
{
    public sealed class XmlCommentProviderTests
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
        <member name=""T:{typeof(XmlCommentProviderTests).FullName}"">
            <summary>
            Test line 1.
            Test line 2.
            </summary>
        </member>
    </members>
</doc>";
            Assert.True(XmlCommentProvider.TryParse(xml, out var provider));
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
        <member name=""T:{typeof(XmlCommentProviderTests).FullName}"">
        </member>
    </members>
</doc>";
            Assert.True(XmlCommentProvider.TryParse(xml, out var provider));
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
            Assert.True(XmlCommentProvider.TryParse(xml, out var provider));
            Assert.Null(provider.GetComment(this.GetType()));
        }
    }
}
