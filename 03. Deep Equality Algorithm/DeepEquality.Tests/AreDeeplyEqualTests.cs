namespace DeepEquality.Tests
{
    using System;
    using System.Collections.Generic;

    using Models;
    using Xunit;

    public class AreDeeplyEqualTests
    {
        [Fact]
        public void AreDeeplyEqualShouldWorkCorrectlyWithPrimitiveAndStructTypes()
        {
            Assert.True(DeepEquality.AreDeeplyEqual(1, 1));
            Assert.True(DeepEquality.AreDeeplyEqual(null, null));
            Assert.True(DeepEquality.AreDeeplyEqual("test", "test"));
            Assert.True(DeepEquality.AreDeeplyEqual('a', 'a'));
            Assert.True(DeepEquality.AreDeeplyEqual(1.1, 1.1));
            Assert.True(DeepEquality.AreDeeplyEqual(1.0m, (decimal)1));
            Assert.True(DeepEquality.AreDeeplyEqual(1L, (long)1));
            Assert.True(DeepEquality.AreDeeplyEqual(1.1m, 1.1m));
            Assert.True(DeepEquality.AreDeeplyEqual(true, true));
            Assert.True(DeepEquality.AreDeeplyEqual(new DateTime(2015, 10, 19), new DateTime(2015, 10, 19)));
            Assert.False(DeepEquality.AreDeeplyEqual(1, 0));
            Assert.False(DeepEquality.AreDeeplyEqual(1, null));
            Assert.False(DeepEquality.AreDeeplyEqual("test1", "test2"));
            Assert.False(DeepEquality.AreDeeplyEqual("Test", "test"));
            Assert.False(DeepEquality.AreDeeplyEqual('a', 'b'));
            Assert.False(DeepEquality.AreDeeplyEqual(1.1, 1.2));
            Assert.False(DeepEquality.AreDeeplyEqual(1.1m, 1.2m));
            Assert.False(DeepEquality.AreDeeplyEqual(true, false));
            Assert.False(DeepEquality.AreDeeplyEqual(1, "1"));
            Assert.False(DeepEquality.AreDeeplyEqual(new DateTime(2015, 10, 19), new DateTime(2015, 10, 20)));
        }

        [Fact]
        public void AreDeeplyEqualShouldWorkCorrectlyWithEnumerations()
        {
            // Enum with default values.
            Assert.True(DeepEquality.AreDeeplyEqual(DateTimeKind.Unspecified, DateTimeKind.Unspecified));
            Assert.False(DeepEquality.AreDeeplyEqual(DateTimeKind.Local, DateTimeKind.Utc));

            // Enum with overridden values.
            Assert.True(DeepEquality.AreDeeplyEqual(AttributeTargets.Delegate, AttributeTargets.Delegate));
            Assert.False(DeepEquality.AreDeeplyEqual(AttributeTargets.Assembly, AttributeTargets.All));
            Assert.False(DeepEquality.AreDeeplyEqual(AttributeTargets.Assembly, AttributeTargets.Module));

            // Enum with default and overriden values.
            Assert.True(DeepEquality.AreDeeplyEqual(CustomEnum.DefaultConstant, CustomEnum.DefaultConstant));
            Assert.False(DeepEquality.AreDeeplyEqual(CustomEnum.DefaultConstant, CustomEnum.ConstantWithCustomValue));
            Assert.False(DeepEquality.AreDeeplyEqual(CustomEnum.DefaultConstant, CustomEnum.CombinedConstant));
        }

        [Fact]
        public void AreDeeplyEqualsShouldWorkCorrectlyWithNormalObjects()
        {
            Assert.True(DeepEquality.AreDeeplyEqual(new object(), new object()));
            Assert.True(DeepEquality.AreDeeplyEqual((object)5, (object)5));
            Assert.True(DeepEquality.AreDeeplyEqual((object)5, 5));
            Assert.True(DeepEquality.AreDeeplyEqual(new { Integer = 1, String = "Test", Nested = new byte[] { 1, 2, 3 } }, new { Integer = 1, String = "Test", Nested = new byte[] { 1, 2, 3 } }));
            Assert.True(DeepEquality.AreDeeplyEqual(new RequestModel { Integer = 1 }, new RequestModel { Integer = 1 }));
            Assert.True(DeepEquality.AreDeeplyEqual(new RequestModel { Integer = 1, NonRequiredString = "test" }, new RequestModel { Integer = 1, NonRequiredString = "test" }));
            Assert.True(DeepEquality.AreDeeplyEqual(new GenericComparableModel { Integer = 1, String = "test" }, new GenericComparableModel { Integer = 1, String = "another" }));
            Assert.True(DeepEquality.AreDeeplyEqual(new ComparableModel { Integer = 1, String = "test" }, new ComparableModel { Integer = 1, String = "another" }));
            Assert.True(DeepEquality.AreDeeplyEqual(new EqualsModel { Integer = 1, String = "test" }, new EqualsModel { Integer = 1, String = "another" }));
            Assert.True(DeepEquality.AreDeeplyEqual(new EqualityOperatorModel { Integer = 1, String = "test" }, new EqualityOperatorModel { Integer = 1, String = "another" }));
            Assert.False(DeepEquality.AreDeeplyEqual(new object(), "test"));
            Assert.False(DeepEquality.AreDeeplyEqual(new object(), AttributeTargets.All));
            Assert.False(DeepEquality.AreDeeplyEqual(AttributeTargets.All, new object()));
            Assert.True(DeepEquality.AreDeeplyEqual(AttributeTargets.All, (object)AttributeTargets.All));
            Assert.True(DeepEquality.AreDeeplyEqual((object)AttributeTargets.All, AttributeTargets.All));
            Assert.False(DeepEquality.AreDeeplyEqual(DateTime.Now, "test"));
            Assert.False(DeepEquality.AreDeeplyEqual("test", DateTime.Now));
            Assert.False(DeepEquality.AreDeeplyEqual(true, new object()));
            Assert.False(DeepEquality.AreDeeplyEqual("test", new object()));
            Assert.False(DeepEquality.AreDeeplyEqual(new object(), true));
            Assert.False(DeepEquality.AreDeeplyEqual(new { Integer = 1, String = "Test", Nested = new byte[] { 1, 2, 3 } }, new { Integer = 1, String = "Test", Nested = new byte[] { 1, 2, 4 } }));
            Assert.False(DeepEquality.AreDeeplyEqual(new RequestModel { Integer = 2 }, new RequestModel { Integer = 1 }));
            Assert.False(DeepEquality.AreDeeplyEqual(new object(), new RequestModel { Integer = 1 }));
            Assert.False(DeepEquality.AreDeeplyEqual(new RequestModel { Integer = 2 }, new object()));
            Assert.False(DeepEquality.AreDeeplyEqual(new RequestModel { Integer = 2, NonRequiredString = "test" }, new RequestModel { Integer = 1 }));
            Assert.False(DeepEquality.AreDeeplyEqual(new GenericComparableModel { Integer = 1, String = "test" }, new GenericComparableModel { Integer = 2, String = "test" }));
            Assert.False(DeepEquality.AreDeeplyEqual(new ComparableModel { Integer = 1, String = "test" }, new ComparableModel { Integer = 2, String = "test" }));
            Assert.False(DeepEquality.AreDeeplyEqual(new EqualsModel { Integer = 1, String = "test" }, new EqualsModel { Integer = 2, String = "test" }));
            Assert.False(DeepEquality.AreDeeplyEqual(new EqualityOperatorModel { Integer = 1, String = "test" }, new EqualityOperatorModel { Integer = 2, String = "test" }));
            Assert.False(DeepEquality.AreDeeplyEqual(new ComparableModel { Integer = 1, String = "test" }, new RequestModel()));
        }

        [Fact]
        public void AreDeeplyEqualsShouldWorkCorrectlyWithNestedObjects()
        {
            Assert.True(DeepEquality.AreDeeplyEqual(
                new NestedModel
                {
                    Integer = 1,
                    String = "test1",
                    Enum = CustomEnum.ConstantWithCustomValue,
                    Nested = new NestedModel { Integer = 2, String = "test2", Nested = new NestedModel { Integer = 3, String = "test3" } }
                },
                new NestedModel
                {
                    Integer = 1,
                    String = "test1",
                    Enum = CustomEnum.ConstantWithCustomValue,
                    Nested = new NestedModel { Integer = 2, String = "test2", Nested = new NestedModel { Integer = 3, String = "test3" } }
                }));

            Assert.False(DeepEquality.AreDeeplyEqual(
                new NestedModel
                {
                    Integer = 1,
                    String = "test",
                    Enum = CustomEnum.ConstantWithCustomValue,
                    Nested = new NestedModel { Integer = 2, String = "test2", Nested = new NestedModel { Integer = 3, String = "test3" } }
                },
                new NestedModel
                {
                    Integer = 1,
                    String = "test",
                    Enum = CustomEnum.ConstantWithCustomValue,
                    Nested = new NestedModel { Integer = 2, String = "test1", Nested = new NestedModel { Integer = 3, String = "test3" } }
                }));

            Assert.False(DeepEquality.AreDeeplyEqual(
                new NestedModel
                {
                    Integer = 1,
                    String = "test1",
                    Enum = CustomEnum.ConstantWithCustomValue,
                    Nested = new NestedModel { Integer = 2, String = "test2", Nested = new NestedModel { Integer = 3, String = "test2" } }
                },
                new NestedModel
                {
                    Integer = 1,
                    String = "test1",
                    Enum = CustomEnum.ConstantWithCustomValue,
                    Nested = new NestedModel { Integer = 2, String = "test2", Nested = new NestedModel { Integer = 3, String = "test3" } }
                }));
        }

        [Fact]
        public void AreDeeplyEqualShouldWorkCorrectlyWithCollections()
        {
            Assert.True(DeepEquality.AreDeeplyEqual(
                new List<NestedModel>
                {
                    new NestedModel
                    {
                        Integer = 1, String = "test1",
                        Nested = new NestedModel { Integer = 2, String = "test2", Enum = CustomEnum.CombinedConstant, Nested = new NestedModel { Integer = 3, String = "test3" } }
                    },
                    new NestedModel
                    {
                        Integer = 1,
                        String = "test1",
                        Nested = new NestedModel { Integer = 2, String = "test2", Enum = CustomEnum.CombinedConstant, Nested = new NestedModel { Integer = 3, String = "test3" } }
                    }
                },
                new List<NestedModel>
                {
                    new NestedModel
                    {
                        Integer = 1, String = "test1",
                        Nested = new NestedModel { Integer = 2, String = "test2", Enum = CustomEnum.CombinedConstant, Nested = new NestedModel { Integer = 3, String = "test3" } }
                    },
                    new NestedModel
                    {
                        Integer = 1,
                        String = "test1",
                        Nested = new NestedModel { Integer = 2, String = "test2", Enum = CustomEnum.CombinedConstant, Nested = new NestedModel { Integer = 3, String = "test3" } }
                    }
                }));

            var listOfNestedModels = new List<NestedModel>
            {
                new NestedModel
                {
                    Integer = 1,
                    String = "test1",
                    Enum = CustomEnum.ConstantWithCustomValue,
                    Nested =
                        new NestedModel
                        {
                            Integer = 2,
                            String = "test2",
                            Enum = CustomEnum.ConstantWithCustomValue,
                            Nested = new NestedModel { Integer = 3, String = "test3" }
                        }
                },
                new NestedModel
                {
                    Integer = 1,
                    String = "test1",
                    Enum = CustomEnum.ConstantWithCustomValue,
                    Nested =
                        new NestedModel
                        {
                            Integer = 2,
                            String = "test2",
                            Enum = CustomEnum.ConstantWithCustomValue,
                            Nested = new NestedModel { Integer = 3, String = "test3" }
                        }
                }
            };

            var arrayOfNestedModels = new[]
            {
                new NestedModel
                {
                    Integer = 1,
                    String = "test1",
                    Enum = CustomEnum.ConstantWithCustomValue,
                    Nested =
                        new NestedModel
                        {
                            Integer = 2,
                            String = "test2",
                            Enum = CustomEnum.ConstantWithCustomValue,
                            Nested = new NestedModel { Integer = 3, String = "test3" }
                        }
                },
                new NestedModel
                {
                    Integer = 1,
                    String = "test1",
                    Enum = CustomEnum.ConstantWithCustomValue,
                    Nested =
                        new NestedModel
                        {
                            Integer = 2,
                            String = "test2",
                            Enum = CustomEnum.ConstantWithCustomValue,
                            Nested = new NestedModel { Integer = 3, String = "test3" }
                        }
                }
            };

            Assert.True(DeepEquality.AreDeeplyEqual(listOfNestedModels, arrayOfNestedModels));

            Assert.True(DeepEquality.AreDeeplyEqual(
                new NestedCollection
                {
                    Integer = 1,
                    String = "test",
                    Nested = new List<NestedModel>
                    {
                        new NestedModel
                        {
                            Integer = 1, String = "test1",
                            Nested = new NestedModel { Integer = 2, String = "test2", Nested = new NestedModel { Integer = 3, String = "test3" } }
                        }, new NestedModel
                        {
                            Integer = 1,
                            String = "test1",
                            Nested = new NestedModel { Integer = 2, String = "test2", Nested = new NestedModel { Integer = 3, String = "test3" } }
                        }
                    }
                },
                new NestedCollection
                {
                    Integer = 1,
                    String = "test",
                    Nested = new List<NestedModel>
                    {
                        new NestedModel
                        {
                            Integer = 1, String = "test1",
                            Nested = new NestedModel { Integer = 2, String = "test2", Nested = new NestedModel { Integer = 3, String = "test3" } }
                        }, new NestedModel
                        {
                            Integer = 1,
                            String = "test1",
                            Nested = new NestedModel { Integer = 2, String = "test2", Nested = new NestedModel { Integer = 3, String = "test3" } }
                        }
                    }
                }));

            Assert.True(DeepEquality.AreDeeplyEqual(
                new NestedCollection
                {
                    Integer = 1,
                    String = "test",
                    Nested = new List<NestedModel>
                    {
                        new NestedModel
                        {
                            Integer = 1, String = "test1",
                            Nested = new NestedModel { Integer = 2, String = "test2", Nested = new NestedModel { Integer = 3, String = "test3" } }
                        }, new NestedModel
                        {
                            Integer = 1,
                            String = "test1",
                            Nested = new NestedModel { Integer = 2, String = "test2", Nested = new NestedModel { Integer = 3, String = "test3" } }
                        }
                    }
                },
                new NestedCollection
                {
                    Integer = 1,
                    String = "test",
                    Nested = new[]
                    {
                        new NestedModel
                        {
                            Integer = 1, String = "test1",
                            Nested = new NestedModel { Integer = 2, String = "test2", Nested = new NestedModel { Integer = 3, String = "test3" } }
                        }, new NestedModel
                        {
                            Integer = 1,
                            String = "test1",
                            Nested = new NestedModel { Integer = 2, String = "test2", Nested = new NestedModel { Integer = 3, String = "test3" } }
                        }
                    }
                }));

            Assert.False(DeepEquality.AreDeeplyEqual(
                new List<NestedModel>
                {
                    new NestedModel
                    {
                        Integer = 1, String = "test1",
                        Nested = new NestedModel { Integer = 2, String = "test2", Nested = new NestedModel { Integer = 3, String = "test3" } }
                    }, new NestedModel
                    {
                        Integer = 1,
                        String = "test1",
                        Nested = new NestedModel { Integer = 2, String = "test2", Nested = new NestedModel { Integer = 3, String = "test3" } }
                    }
                },
                new List<NestedModel>
                {
                    new NestedModel
                    {
                        Integer = 1, String = "test2",
                        Nested = new NestedModel { Integer = 2, String = "test2", Nested = new NestedModel { Integer = 3, String = "test3" } }
                    }, new NestedModel
                    {
                        Integer = 1,
                        String = "test1",
                        Nested = new NestedModel { Integer = 2, String = "test2", Nested = new NestedModel { Integer = 3, String = "test3" } }
                    }
                }));

            listOfNestedModels = new List<NestedModel>
            {
                new NestedModel
                {
                    Integer = 1,
                    String = "test1",
                    Nested =
                        new NestedModel
                        {
                            Integer = 2,
                            String = "test2",
                            Nested = new NestedModel { Integer = 3, String = "test3" }
                        }
                },
                new NestedModel
                {
                    Integer = 1,
                    String = "test1",
                    Nested =
                        new NestedModel
                        {
                            Integer = 2,
                            String = "test2",
                            Nested = new NestedModel { Integer = 3, String = "test3" }
                        }
                }
            };

            arrayOfNestedModels = new[]
            {
                new NestedModel
                {
                    Integer = 1,
                    String = "test1",
                    Nested =
                        new NestedModel
                        {
                            Integer = 2,
                            String = "test2",
                            Nested = new NestedModel { Integer = 3, String = "test3" }
                        }
                },
                new NestedModel
                {
                    Integer = 1,
                    String = "test1",
                    Nested =
                        new NestedModel
                        {
                            Integer = 4,
                            String = "test2",
                            Nested = new NestedModel { Integer = 3, String = "test3" }
                        }
                }
            };

            Assert.False(DeepEquality.AreDeeplyEqual(listOfNestedModels, arrayOfNestedModels));

            Assert.False(DeepEquality.AreDeeplyEqual(
                new NestedCollection
                {
                    Integer = 1,
                    String = "test",
                    Nested = new List<NestedModel>
                    {
                        new NestedModel
                        {
                            Integer = 1, String = "test1",
                            Nested = new NestedModel { Integer = 2, String = "test2", Nested = new NestedModel { Integer = 3, String = "test3" } }
                        }, new NestedModel
                        {
                            Integer = 1,
                            String = "test1",
                            Nested = new NestedModel { Integer = 2, String = "test2", Nested = new NestedModel { Integer = 3, String = "test3" } }
                        }
                    }
                },
                new NestedCollection
                {
                    Integer = 1,
                    String = "test",
                    Nested = new List<NestedModel>
                    {
                        new NestedModel
                        {
                            Integer = 1, String = "test1",
                            Nested = new NestedModel { Integer = 2, String = "test2", Nested = new NestedModel { Integer = 3, String = "test3" } }
                        }, new NestedModel
                        {
                            Integer = 1,
                            String = "test1",
                            Nested = new NestedModel { Integer = 2, String = "test2", Nested = new NestedModel { Integer = 5, String = "test3" } }
                        }
                    }
                }));

            Assert.False(DeepEquality.AreDeeplyEqual(
                new NestedCollection
                {
                    Integer = 1,
                    String = "test",
                    Nested = new List<NestedModel>
                    {
                        new NestedModel
                        {
                            Integer = 1, String = "test1",
                            Nested = new NestedModel { Integer = 2, String = "test2", Nested = new NestedModel { Integer = 3, String = "test3" } }
                        }, new NestedModel
                        {
                            Integer = 1,
                            String = "test1",
                            Nested = new NestedModel { Integer = 2, String = "test2", Nested = new NestedModel { Integer = 3, String = "test3" } }
                        }
                    }
                },
                new NestedCollection
                {
                    Integer = 1,
                    String = "test",
                    Nested = new[]
                    {
                        new NestedModel
                        {
                            Integer = 1, String = "test1",
                            Nested = new NestedModel { Integer = 2, String = "test2", Nested = new NestedModel { Integer = 3, String = "test3" } }
                        }, new NestedModel
                        {
                            Integer = 1,
                            String = "test1",
                            Nested = new NestedModel { Integer = 2, String = "test3", Nested = new NestedModel { Integer = 3, String = "test3" } }
                        }
                    }
                }));

            Assert.True(DeepEquality.AreDeeplyEqual(new List<int> { 1, 2, 3 }, new[] { 1, 2, 3 }));
            Assert.False(DeepEquality.AreDeeplyEqual(new List<int> { 1, 2, 3, 4 }, new[] { 1, 2, 3 }));
            Assert.False(DeepEquality.AreDeeplyEqual(new List<int>(), new object()));
            Assert.False(DeepEquality.AreDeeplyEqual(new object(), new List<int>()));
        }

        [Fact]
        public void AreDeeplyEqualShouldWorkCorrectlyWithDictionaries()
        {
            var firstDictionary = new Dictionary<string, string>
            {
                { "Key", "Value" },
                { "AnotherKey", "AnotherValue" },
            };

            var secondDictionary = new Dictionary<string, string>
            {
                { "Key", "Value" },
                { "AnotherKey", "AnotherValue" },
            };

            Assert.True(DeepEquality.AreDeeplyEqual(firstDictionary, secondDictionary));

            firstDictionary = new Dictionary<string, string>
            {
                { "Key", "Value" },
                { "AnotherKey", "Value" },
            };

            secondDictionary = new Dictionary<string, string>
            {
                { "Key", "Value" },
                { "AnotherKey", "AnotherValue" },
            };

            Assert.False(DeepEquality.AreDeeplyEqual(firstDictionary, secondDictionary));

            var firstDictionaryWithObject = new Dictionary<string, NestedModel>
            {
                { "Key", new NestedModel { Integer = 1, String = "Text", Enum = CustomEnum.ConstantWithCustomValue} },
                { "AnotherKey", new NestedModel { Integer = 2, String = "AnotherText" } }
            };

            var secondDictionaryWithObject = new Dictionary<string, NestedModel>
            {
                { "Key", new NestedModel { Integer = 1, String = "Text", Enum = CustomEnum.ConstantWithCustomValue } },
                { "AnotherKey", new NestedModel { Integer = 2, String = "AnotherText" } }
            };

            Assert.True(DeepEquality.AreDeeplyEqual(firstDictionaryWithObject, secondDictionaryWithObject));

            firstDictionaryWithObject = new Dictionary<string, NestedModel>
            {
                { "Key", new NestedModel { Integer = 1, String = "Text", Enum = CustomEnum.ConstantWithCustomValue } },
                { "AnotherKey", new NestedModel { Integer = 2, String = "Text" } }
            };

            secondDictionaryWithObject = new Dictionary<string, NestedModel>
            {
                { "Key", new NestedModel { Integer = 1, String = "Text",  } },
                { "AnotherKey", new NestedModel { Integer = 2, String = "AnotherText" } }
            };

            Assert.False(DeepEquality.AreDeeplyEqual(firstDictionaryWithObject, secondDictionaryWithObject));
        }

        [Fact]
        public void AreDeeplyEqualShouldWorkCorrectlyWithSameReferences()
        {
            var firstObject = new NestedModel { Integer = 1, String = "Text" };
            var secondObject = new NestedModel { Integer = 1, String = "Text", Nested = firstObject };
            firstObject.Nested = secondObject;

            Assert.True(DeepEquality.AreDeeplyEqual(firstObject, secondObject));
        }

        [Fact]
        public void AreDeeplyEqualShouldReportCorrectly()
        {
            var firstObject = new NestedModel() {Integer = 1, String = "Text"};
            var secondObject = new NestedModel() {Integer = 2, String = "Text"};

            Assert.False(DeepEquality.AreDeeplyEqual(firstObject, secondObject, out var result));
            Assert.Equal("Difference occurs at 'NestedModel.Integer'. Expected a value of '1', but in fact it was '2'", result.ToString());
        }

        [Fact]
        public void AreDeeplyEqualShouldReportCorrectlyWithPrimitiveAndStructTypes()
        {
            Assert.False(DeepEquality.AreDeeplyEqual(1, 0, out var result));
            Assert.Equal("Expected a value of '1', but in fact it was '0'", result.ToString());
            Assert.False(DeepEquality.AreDeeplyEqual(1, null, out result));
            Assert.Equal("Expected a value of '1', but in fact it was null", result.ToString());
            Assert.False(DeepEquality.AreDeeplyEqual(null, 1, out result));
            Assert.Equal("Expected a value of null, but in fact it was '1'", result.ToString());
            Assert.False(DeepEquality.AreDeeplyEqual("test1", "test2", out result));
            Assert.Equal("Expected a value of 'test1', but in fact it was 'test2'", result.ToString());
            Assert.False(DeepEquality.AreDeeplyEqual(1, "1", out result));
            Assert.Equal("Expected a value of Int32 type, but in fact it was String", result.ToString());
            Assert.False(DeepEquality.AreDeeplyEqual(new DateTime(2015, 10, 19), new DateTime(2015, 10, 20), out result));
            Assert.Equal("Difference occurs at 'DateTime.== (Equality Operator)'. Expected a value of '10/19/2015 00:00:00', but in fact it was '10/20/2015 00:00:00'", result.ToString());
        }

        [Fact]
        public void AreDeeplyEqualShouldReportCorrectlyWithEnumerations()
        {
            Assert.False(DeepEquality.AreDeeplyEqual(DateTimeKind.Local, DateTimeKind.Utc, out var result));
            Assert.Equal("Expected a value of 'Local', but in fact it was 'Utc'", result.ToString());
        }

        [Fact]
        public void AreDeeplyEqualShouldReportCorrectlyWithNormalObjects()
        {
            Assert.False(DeepEquality.AreDeeplyEqual(new object(), "test", out var result));
            Assert.Equal("Expected a value of Object type, but in fact it was String", result.ToString());
            Assert.False(DeepEquality.AreDeeplyEqual(new object(), AttributeTargets.All, out result));
            Assert.Equal("Expected a value of Object type, but in fact it was AttributeTargets", result.ToString());
            Assert.False(DeepEquality.AreDeeplyEqual(AttributeTargets.All, new object(), out result));
            Assert.Equal("Expected a value of AttributeTargets type, but in fact it was Object", result.ToString());
            Assert.False(DeepEquality.AreDeeplyEqual(DateTime.Now, "test", out result));
            Assert.Equal("Expected a value of DateTime type, but in fact it was String", result.ToString());
            Assert.False(DeepEquality.AreDeeplyEqual("test", DateTime.Now, out result));
            Assert.Equal("Expected a value of String type, but in fact it was DateTime", result.ToString());
            Assert.False(DeepEquality.AreDeeplyEqual(true, new object(), out result));
            Assert.Equal("Expected a value of Boolean type, but in fact it was Object", result.ToString());
            Assert.False(DeepEquality.AreDeeplyEqual("test", new object(), out result));
            Assert.Equal("Expected a value of String type, but in fact it was Object", result.ToString());
            Assert.False(DeepEquality.AreDeeplyEqual(new object(), true, out result));
            Assert.Equal("Expected a value of Object type, but in fact it was Boolean", result.ToString());
            Assert.False(DeepEquality.AreDeeplyEqual(new { Integer = 1, String = "Test", Nested = new byte[] { 1, 2, 3 } }, new { Integer = 1, String = "Test", Nested = new byte[] { 1, 2, 4 } }, out result));
            Assert.Equal("Difference occurs at 'AnonymousType<Int32, String, Byte[]>.Nested[2]'. Expected a value of '3', but in fact it was '4'", result.ToString());
            Assert.False(DeepEquality.AreDeeplyEqual(new RequestModel { Integer = 2 }, new RequestModel { Integer = 1 }, out result));
            Assert.Equal("Difference occurs at 'RequestModel.Integer'. Expected a value of '2', but in fact it was '1'", result.ToString());
            Assert.False(DeepEquality.AreDeeplyEqual(new object(), new RequestModel { Integer = 1 }, out result));
            Assert.Equal("Expected a value of Object type, but in fact it was RequestModel", result.ToString());
            Assert.False(DeepEquality.AreDeeplyEqual(new RequestModel { Integer = 2 }, new object(), out result));
            Assert.Equal("Expected a value of RequestModel type, but in fact it was Object", result.ToString());
            Assert.False(DeepEquality.AreDeeplyEqual(new RequestModel { Integer = 2, NonRequiredString = "test" }, new RequestModel { Integer = 1 }, out result));
            Assert.Equal("Difference occurs at 'RequestModel.Integer'. Expected a value of '2', but in fact it was '1'", result.ToString());
            Assert.False(DeepEquality.AreDeeplyEqual(new GenericComparableModel { Integer = 1, String = "test" }, new GenericComparableModel { Integer = 2, String = "test" }, out result));
            Assert.Equal("Difference occurs at 'GenericComparableModel.CompareTo().Integer'. Expected a value of '1', but in fact it was '2'", result.ToString());
            Assert.False(DeepEquality.AreDeeplyEqual(new ComparableModel { Integer = 1, String = "test" }, new ComparableModel { Integer = 2, String = "test" }, out result));
            Assert.Equal("Difference occurs at 'ComparableModel.CompareTo().Integer'. Expected a value of '1', but in fact it was '2'", result.ToString());
            Assert.False(DeepEquality.AreDeeplyEqual(new EqualsModel { Integer = 1, String = "test" }, new EqualsModel { Integer = 2, String = "test" }, out result));
            Assert.Equal("Difference occurs at 'EqualsModel.Equals()'", result.ToString());
            Assert.False(DeepEquality.AreDeeplyEqual(new EqualityOperatorModel { Integer = 1, String = "test" }, new EqualityOperatorModel { Integer = 2, String = "test" }, out result));
            Assert.Equal("Difference occurs at 'EqualityOperatorModel.== (Equality Operator)'", result.ToString());
            Assert.False(DeepEquality.AreDeeplyEqual(new ComparableModel { Integer = 1, String = "test" }, new RequestModel(), out result));
            Assert.Equal("Expected a value of ComparableModel type, but in fact it was RequestModel", result.ToString());
        }

        [Fact]
        public void AreDeeplyEqualShouldReportCorrectlyWithNestedObjects()
        {
            var firstObject = new NestedModel { Integer = 1, String = "Text" };
            var secondObject = new NestedModel { Integer = 2, String = "Text" };

            Assert.False(DeepEquality.AreDeeplyEqual(firstObject, secondObject, out var result));
            Assert.Equal("Difference occurs at 'NestedModel.Integer'. Expected a value of '1', but in fact it was '2'", result.ToString());

            Assert.False(DeepEquality.AreDeeplyEqual(
                new NestedModel
                {
                    Integer = 1,
                    String = "test",
                    Enum = CustomEnum.ConstantWithCustomValue,
                    Nested = new NestedModel { Integer = 2, String = "test2", Nested = new NestedModel { Integer = 3, String = "test3" } }
                },
                new NestedModel
                {
                    Integer = 1,
                    String = "test",
                    Enum = CustomEnum.ConstantWithCustomValue,
                    Nested = new NestedModel { Integer = 2, String = "test1", Nested = new NestedModel { Integer = 3, String = "test3" } }
                }, out result));

            Assert.Equal("Difference occurs at 'NestedModel.Nested.String'. Expected a value of 'test2', but in fact it was 'test1'", result.ToString());
        }

        [Fact]
        public void AreDeeplyEqualShouldReportCorrectlyWithCollections()
        {
            Assert.False(DeepEquality.AreDeeplyEqual(new List<int> { 1, 2, 3, 4 }, new[] { 1, 2, 3 }, out var result));
            Assert.Equal("Difference occurs at 'List<Int32>.Count'. Expected a value of '4', but in fact it was '3'", result.ToString());

            Assert.False(DeepEquality.AreDeeplyEqual(new List<int> { 1, 2, 3, 4 }, new[] { 1, 2, 3, 5 }, out result));
            Assert.Equal("Difference occurs at 'List<Int32>[3]'. Expected a value of '4', but in fact it was '5'", result.ToString());

            Assert.False(DeepEquality.AreDeeplyEqual(
                new NestedCollection
                {
                    Integer = 1,
                    String = "test",
                    Nested = new List<NestedModel>
                    {
                        new NestedModel
                        {
                            Integer = 1, String = "test1",
                            Nested = new NestedModel { Integer = 2, String = "test2", Nested = new NestedModel { Integer = 3, String = "test3" } }
                        }, new NestedModel
                        {
                            Integer = 1,
                            String = "test1",
                            Nested = new NestedModel { Integer = 2, String = "test2", Nested = new NestedModel { Integer = 3, String = "test3" } }
                        }
                    }
                },
                new NestedCollection
                {
                    Integer = 1,
                    String = "test",
                    Nested = new List<NestedModel>
                    {
                        new NestedModel
                        {
                            Integer = 1, String = "test1",
                            Nested = new NestedModel { Integer = 2, String = "test2", Nested = new NestedModel { Integer = 3, String = "test3" } }
                        }, new NestedModel
                        {
                            Integer = 1,
                            String = "test1",
                            Nested = new NestedModel { Integer = 2, String = "test2", Nested = new NestedModel { Integer = 5, String = "test3" } }
                        }
                    }
                }, out result));

            Assert.Equal("Difference occurs at 'NestedCollection.Nested[1].Nested.Nested.Integer'. Expected a value of '3', but in fact it was '5'", result.ToString());

            Assert.False(DeepEquality.AreDeeplyEqual(
                new List<NestedModel>
                {
                    new NestedModel
                    {
                        Integer = 1, String = "test1",
                        Nested = new NestedModel { Integer = 2, String = "test2", Nested = new NestedModel { Integer = 3, String = "test4" } }
                    }, new NestedModel
                    {
                        Integer = 1,
                        String = "test1",
                        Nested = new NestedModel { Integer = 2, String = "test2", Nested = new NestedModel { Integer = 3, String = "test3" } }
                    }
                },
                new List<NestedModel>
                {
                    new NestedModel
                    {
                        Integer = 1, String = "test1",
                        Nested = new NestedModel { Integer = 2, String = "test2", Nested = new NestedModel { Integer = 3, String = "test3" } }
                    }, new NestedModel
                    {
                        Integer = 1,
                        String = "test1",
                        Nested = new NestedModel { Integer = 2, String = "test2", Nested = new NestedModel { Integer = 3, String = "test3" } }
                    }
                }, out result));

            Assert.Equal("Difference occurs at 'List<NestedModel>[0].Nested.Nested.String'. Expected a value of 'test4', but in fact it was 'test3'", result.ToString());
        }

        [Fact]
        public void AreDeeplyEqualShouldReportCorrectlyWithDictionaries()
        {
            var firstDictionary = new Dictionary<string, string>
            {
                { "Key", "Value" },
                { "AnotherKey", "Value" },
            };

            var secondDictionary = new Dictionary<string, string>
            {
                { "Key", "Value" },
                { "AnotherKey", "AnotherValue" },
            };

            Assert.False(DeepEquality.AreDeeplyEqual(firstDictionary, secondDictionary, out var result));
            Assert.Equal("Difference occurs at 'Dictionary<String, String>[AnotherKey].Value'. Expected a value of 'Value', but in fact it was 'AnotherValue'", result.ToString());

            var firstDictionaryWithObject = new Dictionary<string, NestedModel>
            {
                { "Key", new NestedModel { Integer = 1, String = "Text", Enum = CustomEnum.ConstantWithCustomValue } },
                { "AnotherKey", new NestedModel { Integer = 2, String = "Text" } }
            };

            var secondDictionaryWithObject = new Dictionary<string, NestedModel>
            {
                { "Key", new NestedModel { Integer = 1, String = "Text",  } },
                { "AnotherKey", new NestedModel { Integer = 2, String = "AnotherText" } }
            };

            Assert.False(DeepEquality.AreDeeplyEqual(firstDictionaryWithObject, secondDictionaryWithObject, out result));
            Assert.Equal("Difference occurs at 'Dictionary<String, NestedModel>[Key].Value.Enum'. Expected a value of 'ConstantWithCustomValue', but in fact it was 'DefaultConstant'", result.ToString());
        }
    }
}
