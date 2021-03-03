namespace DeepEquality.Tests.Models
{
    public enum CustomEnum
    {
        DefaultConstant,
        ConstantWithCustomValue = 128,
        CombinedConstant = DefaultConstant | ConstantWithCustomValue
    }
}
