namespace DeepEquality
{
    using System;
    using System.Collections;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    using Microsoft.AspNetCore.Routing;
    using Models;

    public static class DeepEquality
    {
        /// <summary>
        /// Checks whether two objects are deeply equal by reflecting all their public properties recursively. Resolves successfully value and reference types, overridden Equals method, custom == operator, IComparable, nested objects and collection properties.
        /// </summary>
        /// <param name="expected">Expected object.</param>
        /// <param name="actual">Actual object.</param>
        /// <returns>True or false.</returns>
        public static bool AreDeeplyEqual(
            object expected, 
            object actual)
            => AreDeeplyEqual(expected, actual, new ConditionalWeakTable<object, object>(), new DeepEqualityResult(null, null));

        /// <summary>
        /// Checks whether two objects are deeply equal by reflecting all their public properties recursively. Resolves successfully value and reference types, overridden Equals method, custom == operator, IComparable, nested objects and collection properties.
        /// </summary>
        /// <param name="expected">Expected object.</param>
        /// <param name="actual">Actual object.</param>
        /// <param name="result">Result object containing differences between the two objects.</param>
        /// <returns>True or false.</returns>
        public static bool AreDeeplyEqual(object expected, object actual, out DeepEqualityResult result)
        {
            result = new DeepEqualityResult(expected?.GetType(), actual?.GetType());

            return AreDeeplyEqual(expected, actual, new ConditionalWeakTable<object, object>(), result);
        }

        /// <summary>
        /// Checks whether two objects are not deeply equal by reflecting all their public properties recursively. Resolves successfully value and reference types, overridden Equals method, custom == operator, IComparable, nested objects and collection properties.
        /// </summary>
        /// <param name="expected">Expected object.</param>
        /// <param name="actual">Actual object.</param>
        /// <returns>True or false.</returns>
        /// <remarks>This method is used for the route testing. Since the ASP.NET Core MVC model binder creates new instances, circular references are not checked.</remarks>
        public static bool AreNotDeeplyEqual(object expected, object actual)
            => !AreDeeplyEqual(expected, actual);

        /// <summary>
        /// Test if the all values of objects properties are equals recursively
        /// </summary>
        /// <param name="expected">The expected object</param>
        /// <param name="actual">Actual object</param>
        /// <param name="processedElements">
        ///     Table that binds a managed object, which is represented by a key, to its attached property, which is represented by a value.
        ///     Automatically removes the key/value entry as soon as no other references to a key exist outside the table.
        /// </param>
        /// <param name="result">For which property equality was broken</param>
        /// <returns></returns>
        private static bool AreDeeplyEqual(
            object expected, 
            object actual,
            ConditionalWeakTable<object, object> processedElements,
            DeepEqualityResult result)
        {
            result.ApplyValues(expected, actual);

            if (expected == null && actual == null)
            {
                return result.Success;
            }

            if (expected == null || actual == null)
            {
                return result.Failure;
            }

            var expectedType = expected.GetType();

            if (expectedType != typeof(string) && !expectedType.GetTypeInfo().IsValueType)
            {
                if (processedElements.TryGetValue(expected, out _))
                {
                    return result.Success;
                }

                processedElements.Add(expected, expected);
            }

            var actualType = actual.GetType();
            var objectType = typeof(object);

            if ((expectedType == objectType && actualType != objectType)
                || (actualType == objectType && expectedType != objectType))
            {
                return result.Failure;
            }

            var stringType = typeof(string);

            if (expected is IEnumerable && expectedType != stringType)
            {
                return CollectionsAreDeeplyEqual(expected, actual, processedElements, result);
            }

            var expectedTypeIsAnonymous = IsAnonymousType(expectedType);
            if (expectedTypeIsAnonymous)
            {
                var actualIsAnonymous = IsAnonymousType(actualType);
                if (!actualIsAnonymous)
                {
                    return result.Failure;
                }
            }

            if (!expectedTypeIsAnonymous
                && expectedType != actualType
                && !expectedType.IsAssignableFrom(actualType)
                && !actualType.IsAssignableFrom(expectedType))
            {
                return result.Failure;
            }

            if (expectedType.GetTypeInfo().IsPrimitive || expectedType.GetTypeInfo().IsEnum)
            {
                return expected.ToString() == actual.ToString()
                    ? result.Success
                    : result.Failure;
            }

            var equalsOperator = expectedType.GetMethods().FirstOrDefault(m => m.Name == "op_Equality");
            if (equalsOperator != null)
            {
                var equalsOperatorResult = (bool)equalsOperator.Invoke(null, new[] { expected, actual });

                if (!equalsOperatorResult && expectedType != stringType)
                {
                    result.PushPath("== (Equality Operator)");

                    if (!expectedType.IsDateTimeRelated())
                    {
                        result.ClearValues();
                    }
                }

                return equalsOperatorResult
                    ? result.Success
                    : result.Failure;
            }

            if (expectedType != objectType && !expectedTypeIsAnonymous)
            {
                var equalsMethod = expectedType.GetMethods()
                    .FirstOrDefault(m => m.Name == "Equals" && m.DeclaringType == expectedType);

                if (equalsMethod != null)
                {
                    var equalsMethodResult = (bool) equalsMethod.Invoke(expected, new[] {actual});

                    if (!equalsMethodResult)
                    {
                        result
                            .PushPath("Equals()")
                            .ClearValues();
                    }

                    return equalsMethodResult
                        ? result.Success
                        : result.Failure;
                }
            }

            if (ComparablesAreDeeplyEqual(expected, actual, result))
            {
                return result.Success;
            }

            if (!ObjectPropertiesAreDeeplyEqual(expected, actual, processedElements, result))
            {
                return false;
            }

            return true;
        }

        private static bool AreNotDeeplyEqual(
            object expected, 
            object actual, 
            ConditionalWeakTable<object, object> processedElements,
            DeepEqualityResult result)
            => !AreDeeplyEqual(expected, actual, processedElements, result);

        private static bool CollectionsAreDeeplyEqual(
            object expected, 
            object actual, 
            ConditionalWeakTable<object, object> processedElements,
            DeepEqualityResult result)
        {
            var expectedAsEnumerable = (IEnumerable) expected;
            if (!(actual is IEnumerable actualAsEnumerable))
            {
                return result.Failure;
            }

            var listOfExpectedValues = expectedAsEnumerable.Cast<object>().ToList();
            var listOfActualValues = actualAsEnumerable.Cast<object>().ToList();

            var listOfExpectedValuesCount = listOfExpectedValues.Count;
            var listOfActualValuesCount = listOfActualValues.Count;

            if (listOfExpectedValuesCount != listOfActualValuesCount)
            {
                return result
                    .PushPath(nameof(listOfExpectedValues.Count))
                    .ApplyValues(listOfExpectedValuesCount, listOfActualValuesCount)
                    .Failure;
            }

            for (var i = 0; i < listOfExpectedValuesCount; i++)
            {
                var expectedValue = listOfExpectedValues[i];
                var actualValue = listOfActualValues[i];

                var collectionIsDictionary = expected is IDictionary;

                var indexPath = collectionIsDictionary
                    ? $"[{expectedValue.AsDynamic().Key}]"
                    : $"[{i}]";

                result.PushPath(indexPath);

                if (AreNotDeeplyEqual(expectedValue, actualValue, processedElements, result))
                {
                    return result.Failure;
                }

                result.PopPath();
            }

            return result.Success;
        }

        private static bool IsAnonymousType(Type type)
        {
            if (!(type.Name.StartsWith("<>") || type.Name.StartsWith("VB$")))
            {
                return false;
            }

            var typeInfo = type.GetTypeInfo();
            return typeInfo.IsDefined(typeof(CompilerGeneratedAttribute), false)
                   && typeInfo.IsGenericType
                   && (type.Name.Contains("AnonymousType") || type.Name.Contains("AnonType"))
                   && (typeInfo.Attributes & TypeAttributes.NotPublic) == TypeAttributes.NotPublic;
        }

        private static bool ComparablesAreDeeplyEqual(object expected, object actual, DeepEqualityResult result)
        {
            if (expected is IComparable expectedAsComparable)
            {
                if (expectedAsComparable.CompareTo(actual) == 0)
                {
                    return result.Success;
                }
            }

            if (ObjectImplementsIComparable(expected) && ObjectImplementsIComparable(actual))
            {
                var methodName = "CompareTo";

                var method = expected.GetType().GetMethod(methodName);

                if (method != null)
                {
                    var compareToResult = (int)method.Invoke(expected, new[] { actual }) == 0;

                    if (!compareToResult)
                    {
                        result.PushPath($"{methodName}()");
                    }

                    return compareToResult;
                }
            }

            return result.Failure;
        }

        private static bool ObjectImplementsIComparable(object obj)
            => obj.GetType()
                .GetInterfaces()
                .FirstOrDefault(i => i.Name.StartsWith("IComparable")) != null;

        /// <summary>
        /// Using RouteValueDictionary because it caches internally property getters as delegates.
        /// It is better not to implement own cache, because these object types may be used
        /// during the action call thus they will be evaluated and cached twice.
        /// </summary>
        private static bool ObjectPropertiesAreDeeplyEqual(
            object expected,
            object actual,
            ConditionalWeakTable<object, object> processedElements,
            DeepEqualityResult result)
        {
            var expectedProperties = new RouteValueDictionary(expected);
            var actualProperties = new RouteValueDictionary(actual);

            foreach (var key in expectedProperties.Keys)
            {
                var expectedPropertyValue = expectedProperties[key];
                var actualPropertyValue = actualProperties[key];

                result.PushPath(key);

                if (expectedPropertyValue is IEnumerable && expectedPropertyValue.GetType() != typeof(string))
                {
                    if (!CollectionsAreDeeplyEqual(
                        expectedPropertyValue, 
                        actualPropertyValue, 
                        processedElements,
                        result))
                    {
                        return result.Failure;
                    }
                }

                var propertiesAreDifferent = AreNotDeeplyEqual(
                    expectedPropertyValue, 
                    actualPropertyValue, 
                    processedElements,
                    result);

                if (propertiesAreDifferent)
                {
                    return result.Failure;
                }

                result.PopPath();
            }

            return result.Success;
        }
    }
}
