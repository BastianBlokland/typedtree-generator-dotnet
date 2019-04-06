using System;
using System.Collections.Generic;
using Xunit;

using TypedTree.Generator.Core.Mapping;
using TypedTree.Generator.Core.Mapping.Exceptions;
using TypedTree.Generator.Core.Scheme;
using TypedTree.Generator.Core.Builder;

namespace TypedTree.Generator.Tests.Mapping
{
    public sealed class EnumMapperTests
    {
        public delegate EnumDefinition BuildEnum(TreeDefinitionBuilder builder);

        private enum IntEnum : int
        {
            A = 0,
            B = int.MaxValue,
            C = int.MinValue,
        }

        private enum ShortEnum : short
        {
            A = 0,
            B = short.MaxValue,
            C = short.MinValue,
        }

        private enum SmallLongEnum : long
        {
            A = 0,
            B = int.MaxValue,
            C = int.MinValue,
        }

        private enum BigLongEnum : long
        {
            A = 0,
            B = long.MinValue,
            C = long.MaxValue,
        }

        private enum ByteEnum : byte
        {
            A = 0,
            B = 128,
            C = 255,
        }

        public static IEnumerable<object[]> GetEnumsData()
        {
            foreach (var tuple in GetEnumAndBuilderPairs())
                yield return new object[] { tuple.Item1 };
        }

        public static IEnumerable<object[]> GetEnumAndBuilderPairsData()
        {
            foreach (var tuple in GetEnumAndBuilderPairs())
                yield return new object[] { tuple.Item1, tuple.Item2 };
        }

        [Theory]
        [MemberData(nameof(GetEnumAndBuilderPairsData))]
        public void EnumTypesAreMappedAccordingToSchemeRules(Type enumType, BuildEnum referenceBuilder)
        {
            var tree = TreeDefinitionBuilder.Create("AliasA", b =>
            {
                b.PushAlias("AliasA", "NodeA");
                b.PushNode("NodeA");

                EnumMapper.MapEnum(b, enumType);
            });
            var expectedTree = TreeDefinitionBuilder.Create("AliasA", b =>
            {
                b.PushAlias("AliasA", "NodeA");
                b.PushNode("NodeA");

                referenceBuilder(b);
            });

            Assert.Equal(expectedTree, tree);
        }

        [Theory]
        [MemberData(nameof(GetEnumsData))]
        public void EnumsCanBeMappedMultipleTimes(Type enumType)
        {
            TreeDefinitionBuilder.Create("AliasA", b =>
            {
                b.PushAlias("AliasA", "NodeA");
                b.PushNode("NodeA");

                var definitionA = EnumMapper.MapEnum(b, enumType);
                var definitionB = EnumMapper.MapEnum(b, enumType);
                Assert.Equal(definitionA, definitionB);
            });
        }

        [Fact]
        public void ThrowsIfTooBigEnumValueIsPushed() => Assert.Throws<InvalidEnumValueException>(() =>
           TreeDefinitionBuilder.Create("AliasA", b =>
           {
               b.PushAlias("AliasA", "NodeA");
               b.PushNode("NodeA");

               EnumMapper.MapEnum(b, typeof(BigLongEnum));
           }));

        private static IEnumerable<(Type, BuildEnum)> GetEnumAndBuilderPairs()
        {
            yield return (typeof(IntEnum), b => b.PushEnum(
                $"{typeof(EnumMapperTests).FullName}+IntEnum",
                ("A", 0),
                ("B", int.MaxValue),
                ("C", int.MinValue)));

            yield return (typeof(ByteEnum), b => b.PushEnum(
                $"{typeof(EnumMapperTests).FullName}+ByteEnum",
                ("A", 0),
                ("B", 128),
                ("C", 255)));

            yield return (typeof(ShortEnum), b => b.PushEnum(
                $"{typeof(EnumMapperTests).FullName}+ShortEnum",
                ("A", 0),
                ("B", short.MaxValue),
                ("C", short.MinValue)));

            yield return (typeof(SmallLongEnum), b => b.PushEnum(
                $"{typeof(EnumMapperTests).FullName}+SmallLongEnum",
                ("A", 0),
                ("B", int.MaxValue),
                ("C", int.MinValue)));
        }
    }
}
