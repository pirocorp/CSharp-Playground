namespace DeepEquality
{
    using System;
    using System.Collections.Concurrent;
    using System.Linq;
    using System.Reflection;

    public static class Utilities
    {
        private static readonly ConcurrentDictionary<Type, string> FullFriendlyTypeNames = new ConcurrentDictionary<Type, string>();

        private static readonly ConcurrentDictionary<Type, string> FriendlyTypeNames = new ConcurrentDictionary<Type, string>();

        public static string ToFriendlyTypeName(this Type type, bool useFullName = false)
        {
            if (type == null)
            {
                return "null";
            }

            return useFullName
                ? FullFriendlyTypeNames
                    .GetOrAdd(type, _ => GetFriendlyTypeName(type, true))
                : FriendlyTypeNames
                    .GetOrAdd(type, _ => GetFriendlyTypeName(type, false));
        }

        private static string GetFriendlyTypeName(Type type, bool useFullName)
        {
            const string anonymousTypePrefix = "<>f__";

            var typeName = useFullName
                ? type?.FullName ?? type?.Name
                : type?.Name;

            if (typeName == null)
            {
                throw new InvalidOperationException("Type name cannot be null.");
            }

            if (!type.GetTypeInfo().IsGenericType)
            {
                return typeName.Replace(anonymousTypePrefix, string.Empty);
            }

            var genericArgumentNames = type.GetGenericArguments().Select(ga => ga.ToFriendlyTypeName(useFullName));
            var friendlyGenericName = typeName.Split('`')[0].Replace(anonymousTypePrefix, string.Empty);

            var anonymousName = "AnonymousType";

            if (friendlyGenericName.StartsWith(anonymousName))
            {
                friendlyGenericName = friendlyGenericName.Remove(anonymousName.Length);
            }

            var joinedGenericArgumentNames = string.Join(", ", genericArgumentNames);

            return $"{friendlyGenericName}<{joinedGenericArgumentNames}>";
        }

        public static (string, string) GetTypeComparisonNames(this (Type Expected, Type Actual) typeTuple)
        {
            var expected = typeTuple.Expected;
            var actual = typeTuple.Actual;

            var expectedName = expected.ToFriendlyTypeName();
            var actualName = actual.ToFriendlyTypeName();

            if (expectedName == actualName)
            {
                return (expected.ToFriendlyTypeName(true), actual.ToFriendlyTypeName(true));
            }

            return (expectedName, actualName);
        }

        /// <summary>
        /// Calls ToString on the provided object and returns the value. If the object is null, the provided optional name is returned.
        /// </summary>
        /// <param name="obj">Object to get error message name.</param>
        /// <param name="includeQuotes">Whether to include quotes around the error message name.</param>
        /// <param name="nullCaseName">Name to return in case of null object.</param>
        /// <returns>Error message name.</returns>
        public static string GetErrorMessageName(this object obj, bool includeQuotes = true, string nullCaseName = "null")
        {
            if (obj == null)
            {
                return nullCaseName;
            }

            var errorMessageName = obj.ToString();

            if (!includeQuotes)
            {
                return errorMessageName;
            }

            return $"'{errorMessageName}'";
        }
    }
}
