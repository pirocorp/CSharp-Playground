namespace DeepEquality
{
    using System;
    using System.Collections;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    using Microsoft.AspNetCore.Routing;

    public static class DeepEquality
    {
        public static bool AreDeeplyEqual(object expected, object actual)
            => AreDeeplyEqual(expected, actual, new ConditionalWeakTable<object, object>());

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
        /// <returns></returns>
        private static bool AreDeeplyEqual(
            object expected, 
            object actual,
            ConditionalWeakTable<object, object> processedElements)
        {
            if (expected == null && actual == null)
            {
                return true;
            }

            if (expected == null || actual == null)
            {
                return false;
            }

            var expectedType = expected.GetType();

            if (expectedType != typeof(string) && !expectedType.GetTypeInfo().IsValueType)
            {
                if (processedElements.TryGetValue(expected, out _))
                {
                    return true;
                }

                processedElements.Add(expected, expected);
            }

            var actualType = actual.GetType();
            var objectType = typeof(object);

            if ((expectedType == objectType && actualType != objectType)
                || (actualType == objectType && expectedType != objectType))
            {
                return false;
            }

            if (expected is IEnumerable && expectedType != typeof(string))
            {
                return CollectionsAreDeeplyEqual(expected, actual, processedElements);
            }

            var expectedTypeIsAnonymous = IsAnonymousType(expectedType);
            if (expectedTypeIsAnonymous)
            {
                var actualIsAnonymous = IsAnonymousType(actualType);
                if (!actualIsAnonymous)
                {
                    return false;
                }
            }

            if (!expectedTypeIsAnonymous
                && expectedType != actualType
                && !expectedType.IsAssignableFrom(actualType)
                && !actualType.IsAssignableFrom(expectedType))
            {
                return false;
            }

            if (expectedType.GetTypeInfo().IsPrimitive || expectedType.GetTypeInfo().IsEnum)
            {
                return expected.ToString() == actual.ToString();
            }

            var equalsOperator = expectedType.GetMethods().FirstOrDefault(m => m.Name == "op_Equality");
            if (equalsOperator != null)
            {
                return (bool) equalsOperator.Invoke(null, new[] { expected, actual });
            }

            if (expectedType != objectType && !expectedTypeIsAnonymous)
            {
                var equalsMethod = expectedType.GetMethods()
                    .FirstOrDefault(m => m.Name == "Equals" && m.DeclaringType == expectedType);

                if (equalsMethod != null)
                {
                    return (bool) equalsMethod.Invoke(expected, new[] {actual});
                }
            }

            if (ComparablesAreDeeplyEqual(expected, actual))
            {
                return true;
            }

            if (!ObjectPropertiesAreDeeplyEqual(expected, actual, processedElements))
            {
                return false;
            }

            return true;
        }

        private static bool AreNotDeeplyEqual(object expected, object actual, ConditionalWeakTable<object, object> processedElements)
            => !AreDeeplyEqual(expected, actual, processedElements);

        private static bool CollectionsAreDeeplyEqual(
            object expected, 
            object actual, 
            ConditionalWeakTable<object, object> processedElements)
        {
            var expectedAsEnumerable = (IEnumerable) expected;
            if (!(actual is IEnumerable actualAsEnumerable))
            {
                return false;
            }

            var listOfExpectedValues = expectedAsEnumerable.Cast<object>().ToList();
            var listOfActualValues = actualAsEnumerable.Cast<object>().ToList();

            if (listOfExpectedValues.Count != listOfActualValues.Count)
            {
                return false;
            }

            var collectionIsNotEqual = listOfExpectedValues
                .Where((t, i) => AreNotDeeplyEqual(t, listOfActualValues[i], processedElements))
                .Any();

            if (collectionIsNotEqual)
            {
                return false;
            }

            return true;
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

        private static bool ComparablesAreDeeplyEqual(object expected, object actual)
        {
            if (expected is IComparable expectedAsComparable)
            {
                if (expectedAsComparable.CompareTo(actual) == 0)
                {
                    return true;
                }
            }

            if (ObjectImplementsIComparable(expected) && ObjectImplementsIComparable(actual))
            {
                var method = expected.GetType().GetMethod("CompareTo");

                if (method != null)
                {
                    return (int)method.Invoke(expected, new[] { actual }) == 0;
                }
            }

            return false;
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
            ConditionalWeakTable<object, object> processedElements)
        {
            var expectedProperties = new RouteValueDictionary(expected);
            var actualProperties = new RouteValueDictionary(actual);

            foreach (var key in expectedProperties.Keys)
            {
                var expectedPropertyValue = expectedProperties[key];
                var actualPropertyValue = actualProperties[key];

                if (expectedPropertyValue is IEnumerable && expectedPropertyValue.GetType() != typeof(string))
                {
                    if (!CollectionsAreDeeplyEqual(expectedPropertyValue, actualPropertyValue, processedElements))
                    {
                        return false;
                    }
                }

                var propertiesAreDifferent = AreNotDeeplyEqual(expectedPropertyValue, actualPropertyValue, processedElements);
                if (propertiesAreDifferent)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
