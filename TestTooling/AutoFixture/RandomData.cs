using Tooling;
using static TestTooling.Randomizer;

namespace TestTooling.AutoFixture;

public static class RandomData
{
    private static readonly Random Random = new();

    internal static Fixture Fixture { get; } = GetFixture();

    public static Fixture GetFixture()
    {
        Fixture fixture = new();
        fixture.Customize<DateOnly>(composer => composer.FromFactory<DateTime>(DateOnly.FromDateTime));
        fixture.Customize<bool>(composer => composer.FromFactory(() => Random.Next(0, 2) == 0));
        fixture.Register(() => Any<Guid>().ToString().ToUtf8Stream());

        return fixture;
    }
}