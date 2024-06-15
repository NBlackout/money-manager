using Shared.TestTooling.AutoFixture;
using AutoFixture;

namespace Shared.TestTooling;

public static class Randomizer
{
    public static T Any<T>() => RandomData.Fixture.Create<T>();

    public static bool Another(bool? value) => AnythingBut(value);
    public static int Another(int? value) => AnythingBut(value);
    public static decimal Another(decimal? value) => AnythingBut(value);
    public static string Another(string? value) => AnythingBut(value);
    public static Guid Another(Guid? value) => AnythingBut(value);
    public static DateTime Another(DateTime? value) => AnythingBut(value);
    public static DateOnly Another(DateOnly? value) => AnythingBut(value);
    public static string RandomCase(string value) => string.Join("", value.Select(x => new Random().Next() % 2 > 0 ? char.ToLower(x) : char.ToUpper(x)).ToArray());

    private static T AnythingBut<T>(T? _) where T : struct =>
        Any<T>();

    private static T AnythingBut<T>(T? _) where T : class =>
        Any<T>();
}