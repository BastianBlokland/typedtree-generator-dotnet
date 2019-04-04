#pragma warning disable CA1812 // Avoid uninstantiated internal classes

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using Xunit;

using TypedTree.Generator.Core.Mapping;
using TypedTree.Generator.Core.Utilities;

namespace TypedTree.Generator.Tests.Mapping
{
    public sealed class ClassifierTests
    {
        private enum TestEmptyEnum
        {
        }

        private enum TestEnum
        {
            A,
            B
        }

        private interface ITestInterface
        {
        }

        public static IEnumerable<object[]> GetTypeClassificationPairsData()
        {
            foreach (var tuple in GetTypeClassificationPairs())
                yield return new object[] { tuple.Item1, tuple.Item2 };
        }

        [Theory]
        [MemberData(nameof(GetTypeClassificationPairsData))]
        public void TypesAreClassifiedAccordingToSchemeRules(
            Type inputType,
            Classifier.Classification expectedClassification)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var typeCollection = TypeCollection.Create(assembly);

            var classification = typeCollection.Classify(inputType, typeIgnoreRegex: new Regex(".*Ignored$"));

            Assert.Equal(expectedClassification, classification);
        }

        private static IEnumerable<(Type, Classifier.Classification)> GetTypeClassificationPairs()
        {
            // Well known types
            yield return (typeof(byte), Classifier.Classification.Number);
            yield return (typeof(sbyte), Classifier.Classification.Number);
            yield return (typeof(short), Classifier.Classification.Number);
            yield return (typeof(ushort), Classifier.Classification.Number);
            yield return (typeof(int), Classifier.Classification.Number);
            yield return (typeof(uint), Classifier.Classification.Number);
            yield return (typeof(long), Classifier.Classification.Number);
            yield return (typeof(ulong), Classifier.Classification.Number);
            yield return (typeof(string), Classifier.Classification.String);
            yield return (typeof(bool), Classifier.Classification.Boolean);

            // Enum
            yield return (typeof(TestEnum), Classifier.Classification.Enum);
            yield return (typeof(TestEmptyEnum), Classifier.Classification.Invalid);

            // Alias
            yield return (typeof(TestStruct), Classifier.Classification.Alias);
            yield return (typeof(TestClass), Classifier.Classification.Alias);
            yield return (typeof(ITestInterface), Classifier.Classification.Alias);

            // Ignored
            yield return (typeof(TestStructIgnored), Classifier.Classification.Ignored);

            // Invalid types
            yield return (typeof(TestAbstractClass), Classifier.Classification.Invalid);
            yield return (typeof(bool[]), Classifier.Classification.Invalid);
            yield return (typeof(List<bool>), Classifier.Classification.Invalid);
            yield return (typeof(Dictionary<int, bool>), Classifier.Classification.Invalid);
        }

        private struct TestStruct
        {
        }

        private struct TestStructIgnored
        {
        }

        private class TestClass : ITestInterface
        {
        }

        private abstract class TestAbstractClass
        {
        }
    }
}
