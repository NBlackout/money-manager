using AutoFixture.Xunit2;

namespace Shared.TestTooling.AutoFixture;

public class InlineRandomDataAttribute : InlineAutoDataAttribute
{
    public InlineRandomDataAttribute(params object?[] values)
        : base(new RandomDataAttribute(), values)
    {
    }
}