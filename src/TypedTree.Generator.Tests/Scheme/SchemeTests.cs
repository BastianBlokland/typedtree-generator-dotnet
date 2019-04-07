using Xunit;

using TypedTree.Generator.Core.Scheme;
using TypedTree.Generator.Core.Builder;

namespace TypedTree.Generator.Tests.Scheme
{
    public sealed class SchemeTests
    {
        [Fact]
        public void SchemesAreEquatedStructurally()
        {
            var scheme1 = CreateTestScheme();
            var scheme2 = CreateTestScheme();
            Assert.Equal(scheme1, scheme2);
        }

        [Fact]
        public void AliasesCanBeFound()
        {
            var scheme = CreateTestScheme();

            Assert.True(scheme.ContainsAlias("AliasA"));
            Assert.True(scheme.TryGetAlias("AliasA", out var foundAlias) && foundAlias != null);

            Assert.False(scheme.ContainsAlias("AliasB"));
            Assert.False(scheme.TryGetAlias("AliasB", out _));
        }

        [Fact]
        public void EnumsCanBeFound()
        {
            var scheme = CreateTestScheme();

            Assert.True(scheme.ContainsEnum("EnumA"));
            Assert.True(scheme.TryGetEnum("EnumA", out var foundEnum) && foundEnum != null);

            Assert.False(scheme.ContainsEnum("EnumB"));
            Assert.False(scheme.TryGetEnum("EnumB", out _));
        }

        [Fact]
        public void NodesCanBeFound()
        {
            var scheme = CreateTestScheme();

            Assert.True(scheme.ContainsNode("NodeA"));
            Assert.True(scheme.TryGetNode("NodeA", out var foundNode) && foundNode != null);

            Assert.False(scheme.ContainsNode("NodeB"));
            Assert.False(scheme.TryGetNode("NodeB", out _));
        }

        [Fact]
        public void FieldsCanBeFound()
        {
            var scheme = CreateTestScheme();
            Assert.True(scheme.TryGetNode("NodeA", out var node));

            Assert.True(node.HasField("field1"));
            Assert.True(node.TryGetField<NumberNodeField>("field1", out _));

            Assert.True(node.HasField("field2"));
            Assert.True(node.TryGetField<StringNodeField>("field2", out _));

            Assert.True(node.HasField("field3"));
            Assert.True(node.TryGetField<BooleanNodeField>("field3", out _));

            Assert.True(node.HasField("field4"));
            Assert.True(node.TryGetField<AliasNodeField>("field4", out _));

            Assert.True(node.HasField("field5"));
            Assert.True(node.TryGetField<EnumNodeField>("field5", out _));
        }

        private static TreeDefinition CreateTestScheme() => TreeDefinitionBuilder.Create("AliasA", b =>
            {
                var aliasA = b.PushAlias("AliasA", "NodeA");
                var enumA = b.PushEnum("EnumA", ("Option1", 1), ("Option2", 2));
                b.PushNode("NodeA", nb =>
                {
                    nb.PushNumberField("field1");
                    nb.PushStringField("field2");
                    nb.PushBooleanField("field3");
                    nb.PushAliasField("field4", aliasA);
                    nb.PushEnumField("field5", enumA);
                    nb.PushNumberField("field6", isArray: true);
                    nb.PushStringField("field7", isArray: true);
                    nb.PushBooleanField("field8", isArray: true);
                    nb.PushAliasField("field9", aliasA, isArray: true);
                    nb.PushEnumField("field10", enumA, isArray: true);
                });
            });
    }
}
