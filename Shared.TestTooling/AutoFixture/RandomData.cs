using static Shared.TestTooling.Randomizer;

namespace Shared.TestTooling.AutoFixture;

public static class RandomData
{
    private static readonly Random Random = new();

    internal static Fixture Fixture { get; } = GetFixture();

    public static Fixture GetFixture()
    {
        Fixture fixture = new();
        fixture.Customize<DateOnly>(composer => composer.FromFactory<DateTime>(DateOnly.FromDateTime));
        fixture.Customize<bool>(composer => composer.FromFactory(() => Random.Next(0, 2) == 0));
        fixture.Register<Stream>(() => new MemoryStream(Any<Guid>().ToByteArray()));

        return fixture;
    }
}