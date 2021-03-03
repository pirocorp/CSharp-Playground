namespace DeepEquality
{
    using System;
    using System.Collections.Concurrent;
    using System.Linq;
    using System.Linq.Expressions;
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

        /// <summary>
        /// Returns the provided object cast as dynamic type.
        /// </summary>
        /// <returns>Object of dynamic type.</returns>
        public static dynamic AsDynamic(this object obj) => obj?.GetType().CastTo<dynamic>(obj);

        /// <summary>
        /// Performs dynamic casting from type to generic result.
        /// </summary>
        /// <typeparam name="TResult">Result type from casting.</typeparam>
        /// <param name="type">Type from which the casting should be done.</param>
        /// <param name="data">Object from which the casting should be done.</param>
        /// <returns>Cast object of type TResult.</returns>
        public static TResult CastTo<TResult>(this Type type, object data)
        {
            var dataParam = Expression.Parameter(typeof(object), "data");
            var firstConvert = Expression.Convert(dataParam, data.GetType());
            var secondConvert = Expression.Convert(firstConvert, type);
            var body = Expression.Block(secondConvert);

            var run = Expression.Lambda(body, dataParam).Compile();
            var ret = run.DynamicInvoke(data);
            return (TResult)ret;
        }

        public static bool IsDateTimeRelated(this Type type)
            => type == typeof(DateTime)
               || type == typeof(DateTime?)
               || type == typeof(TimeSpan)
               || type == typeof(TimeSpan?)
               || type == typeof(DateTimeOffset)
               || type == typeof(DateTimeOffset?);
    }
}
