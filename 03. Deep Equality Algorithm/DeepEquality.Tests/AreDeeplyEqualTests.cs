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

        
    }
}
