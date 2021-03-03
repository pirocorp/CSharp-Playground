namespace DeepEquality.Tests.Models
{
    public class NestedModel
    {
        public int Integer { get; set; }

        public string String { get; set; }

        public CustomEnum Enum { get; set; }

        public NestedModel Nested { get; set; }
    }
}
