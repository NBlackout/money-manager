using AutoFixture;

namespace Shared.TestTooling.AutoFixture;

public static class RandomData
{
    internal static Fixture Fixture { get; } = GetFixture();

    public static Fixture GetFixture()
    {
        Fixture fixture = new();
        fixture.Customize<DateOnly>(composer => composer.FromFactory<DateTime>(DateOnly.FromDateTime));
        fixture.Customize(new ImmutableCollectionsCustomization());
        return fixture;
    }
}