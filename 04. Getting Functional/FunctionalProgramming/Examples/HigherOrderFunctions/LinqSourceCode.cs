﻿namespace Concepts.HigherOrderFunctions
{
    using System;
    using System.Collections.Generic;

    public static class LinqSourceCode
    {
        public static int Count<TSource>(
            this IEnumerable<TSource> source, 
            Func<TSource, bool> predicate)
        {
            var count = 0;

            // If you have imperative code,
            // you should hide it behind a function
            foreach (var element in source)
            {
                checked
                {
                    if (predicate(element))
                    {
                        count++;
                    }
                }
            }

            return count;
        }
    }
}
