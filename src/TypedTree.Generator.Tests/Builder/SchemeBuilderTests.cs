using System;
using Xunit;

using TypedTree.Generator.Core.Builder;
using TypedTree.Generator.Core.Builder.Exceptions;
using TypedTree.Generator.Core.Scheme;

namespace TypedTree.Generator.Tests.Builder
{
    public sealed class SchemeBuilderTests
    {
        [Fact]
        public void ThrowsIfRootAliasDoesntExist() => Assert.Throws<AliasNotFoundException>(() =>
           TreeDefinitionBuilder.Create("AliasA", b =>
           {
               b.PushNode("NodeA");
           }));

        [Fact]
        public void ThrowsIfDuplicateAliasIsPushed() => Assert.Throws<DuplicateAliasIdentifierException>(() =>
           TreeDefinitionBuilder.Create("AliasA", b =>
           {
               b.PushAlias("AliasA", "NodeA");
               b.PushAlias("AliasA", "NodeA");
               b.PushNode("NodeA");
           }));

        [Fact]
        public void ThrowsIfAliasIsEmpty() => Assert.Throws<EmptyAliasException>(() =>
           TreeDefinitionBuilder.Create("AliasA", b =>
           {
               b.PushAlias("AliasA");
               b.PushNode("NodeA");
           }));

        [Fact]
        public void ThrowsIfAliasReferencesMissingNode() => Assert.Throws<NodeNotFoundException>(() =>
           TreeDefinitionBuilder.Create("AliasA", b =>
           {
               b.PushAlias("AliasA", "NodeB");
               b.PushNode("NodeA");
           }));

        [Fact]
        public void ThrowsIfDuplicateEnumIsPushed() => Assert.Throws<DuplicateEnumIdentifierException>(() =>
           TreeDefinitionBuilder.Create("AliasA", b =>
           {
               b.PushAlias("AliasA", "NodeA");
               b.PushEnum("EnumA", ("Option1", 1));
               b.PushEnum("EnumA", ("Option2", 2));
               b.PushNode("NodeA");
           }));

        [Fact]
        public void ThrowsIfEnumHasDuplicateValue() => Assert.Throws<EnumDuplicateValueException>(() =>
           TreeDefinitionBuilder.Create("AliasA", b =>
           {
               b.PushAlias("AliasA", "NodeA");
               b.PushEnum("EnumA", ("Option1", 1), ("Option1", 1));
               b.PushNode("NodeA");
           }));

        [Fact]
        public void ThrowsIfEnumIsEmpty() => Assert.Throws<EmptyEnumException>(() =>
           TreeDefinitionBuilder.Create("AliasA", b =>
           {
               b.PushAlias("AliasA", "NodeA");
               b.PushEnum("EnumA");
               b.PushNode("NodeA");
           }));

        [Fact]
        public void ThrowsIfAliasAndEnumIdentifiersConflict() => Assert.ThrowsAny<Exception>(() =>
           TreeDefinitionBuilder.Create("AliasOrEnumA", b =>
           {
               b.PushAlias("AliasOrEnumA", "NodeA");
               b.PushEnum("AliasOrEnumA", ("Option1", 1));
               b.PushNode("NodeA");
           }));

        [Fact]
        public void ThrowsIfDuplicateNodeIsPushed() => Assert.Throws<DuplicateNodeTypeException>(() =>
           TreeDefinitionBuilder.Create("AliasA", b =>
           {
               b.PushAlias("AliasA", "NodeA");
               b.PushNode("NodeA");
               b.PushNode("NodeA");
           }));

        [Fact]
        public void ThrowsIfDuplicateNodeFieldIsPushed() => Assert.Throws<DuplicateFieldNameException>(() =>
           TreeDefinitionBuilder.Create("AliasA", b =>
           {
               b.PushAlias("AliasA", "NodeA");
               b.PushNode("NodeA", nb =>
               {
                   nb.PushNumberField("field1");
                   nb.PushStringField("field1");
               });
           }));

        [Fact]
        public void PushedAliasesCanBeFound()
        {
            TreeDefinitionBuilder.Create("AliasA", b =>
            {
                var alias = b.PushAlias("AliasA", "NodeA");
                b.PushNode("NodeA");

                Assert.True(b.ContainsAlias("AliasA"));
                Assert.True(b.TryGetAlias("AliasA", out var foundAlias) && foundAlias == alias);

                Assert.False(b.ContainsAlias("AliasB"));
                Assert.False(b.TryGetAlias("AliasB", out _));
            });
        }

        [Fact]
        public void PushedEnumsCanBeFound()
        {
            TreeDefinitionBuilder.Create("AliasA", b =>
            {
                b.PushAlias("AliasA", "NodeA");
                var @enum = b.PushEnum("EnumA", ("Option1", 1));
                b.PushNode("NodeA");

                Assert.True(b.ContainsEnum("EnumA"));
                Assert.True(b.TryGetEnum("EnumA", out var foundEnum) && foundEnum == @enum);

                Assert.False(b.ContainsEnum("EnumB"));
                Assert.False(b.TryGetEnum("EnumB", out _));
            });
        }

        [Fact]
        public void PushedNodeCanBeFound()
        {
            TreeDefinitionBuilder.Create("AliasA", b =>
            {
                b.PushAlias("AliasA", "NodeA");
                var node = b.PushNode("NodeA");

                Assert.True(b.ContainsNode("NodeA"));
                Assert.True(b.TryGetNode("NodeA", out var foundNode) && foundNode == node);

                Assert.False(b.ContainsNode("NodeB"));
                Assert.False(b.TryGetNode("NodeB", out _));
            });
        }
    }
}
