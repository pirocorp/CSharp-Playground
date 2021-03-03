﻿namespace DeepEquality.Tests.Models
{
    using System.Collections.Generic;

    public class NestedCollection
    {
        public int Integer { get; set; }

        public string String { get; set; }

        public ICollection<NestedModel> Nested { get; set; }
    }
}
