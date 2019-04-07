#pragma warning disable CA1812 // Avoid uninstantiated internal classes

using System;
using System.Collections.Generic;
using Xunit;

using TypedTree.Generator.Core.Mapping;

using FieldCollection = System.Collections.Generic.IEnumerable<(string, System.Type)>;

namespace TypedTree.Generator.Tests.Mapping
{
    public sealed class FieldFinderTests
    {
        public static IEnumerable<object[]> GetSourceAndFieldsPairsData()
        {
            foreach (var tuple in GetSourceAndFieldsPairs())
                yield return new object[] { tuple.Item1, tuple.Item2, tuple.Item3 };
        }

        [Theory]
        [MemberData(nameof(GetSourceAndFieldsPairsData))]
        public void FieldsAreFound(Type type, FieldSource source, FieldCollection expectedFields)
        {
            var fields = type.FindFields(source);

            Assert.Equal(expectedFields, fields);
        }

        private static IEnumerable<(Type, FieldSource, FieldCollection)> GetSourceAndFieldsPairs()
        {
            // TestStruct
            yield return (typeof(TestStruct), FieldSource.PublicConstructorParameters, new[]
            {
                ( "field3", typeof(bool))
            });

            yield return (typeof(TestStruct), FieldSource.ConstructorParameters, new[]
            {
                ( "field1", typeof(string)),
                ( "field2", typeof(int)),
                ( "field3", typeof(bool))
            });

            yield return (typeof(TestStruct), FieldSource.PublicProperties, Array.Empty<(string, Type)>());

            yield return (typeof(TestStruct), FieldSource.Properties, Array.Empty<(string, Type)>());

            // TestClass
            yield return (typeof(TestClass), FieldSource.PublicConstructorParameters, Array.Empty<(string, Type)>());

            yield return (typeof(TestClass), FieldSource.ConstructorParameters, new[]
            {
                ( "field", typeof(string))
            });

            yield return (typeof(TestClass), FieldSource.PublicProperties, new[]
            {
                ( "Property1", typeof(string)),
                ( "Property2", typeof(double))
            });

            yield return (typeof(TestClass), FieldSource.Properties, new[]
            {
                ( "Property1", typeof(string)),
                ( "Property2", typeof(double)),
                ( "Property3", typeof(float))
            });
        }

        private struct TestStruct
        {
            private readonly string field1;
            private readonly int field2;
            private readonly bool field3;

            public TestStruct(bool field3)
            {
                this.field1 = "test";
                this.field2 = 1337;
                this.field3 = field3;
            }

            internal TestStruct(string field1, int field2, bool field3)
            {
                this.field1 = field1;
                this.field2 = field2;
                this.field3 = field3;
            }
        }

        private class TestClass
        {
            private readonly string field;

            public TestClass()
                : this("test")
            {
            }

            internal TestClass(string field)
            {
                this.field = field;
            }

            public string Property1 { get; set; }

            public double Property2 { get; set; }

            private float Property3 { get; set; }
        }
    }
}
