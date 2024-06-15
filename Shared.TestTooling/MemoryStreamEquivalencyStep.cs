using FluentAssertions;
using FluentAssertions.Equivalency;

namespace Shared.TestTooling;

public class MemoryStreamEquivalencyStep : IEquivalencyStep
{
    public EquivalencyResult Handle(
        Comparands comparands,
        IEquivalencyValidationContext context,
        IEquivalencyValidator nestedValidator)
    {
        if (comparands.Subject is not MemoryStream actual || comparands.Expectation is not MemoryStream expected)
            return EquivalencyResult.ContinueWithNext;

        actual.ToArray().Should().Equal(expected.ToArray());

        return EquivalencyResult.AssertionCompleted;
    }
}