using Shared.TestTooling.AutoFixture;

namespace Shared.TestTooling;

public static class Randomizer
{
    public static T Any<T>() =>
        RandomData.Fixture.Create<T>();

    public static T Another<T>(T value)
    {
        T newValue = Any<T>();
        while (newValue?.Equals(value) ?? false)
            newValue = Any<T>();

        return newValue;
    }
}