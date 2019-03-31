using System;
using System.Linq;
using Xunit;

using TypedTree.Generator.Core;
using TypedTree.Generator.Core.Scheme;
using TypedTree.Generator.Core.Builder;

namespace TypedTree.Generator.Tests
{
    public sealed class SchemeBuilderTests
    {
        [Fact]
        public void SchemesAreEquatedStructurally()
        {
            var scheme1 = CreateTestScheme();
            var scheme2 = CreateTestScheme();
            Assert.Equal(scheme1, scheme2);
        }

        private static TreeDefinition CreateTestScheme() => TreeDefinitionBuilder.Create("AliasA", b =>
            {
                var aliasA = b.PushAlias("AliasA", new[] { "NodeA" });
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
