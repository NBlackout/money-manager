namespace Shared.TestTooling.Assertions;

public static class AssertionsExtensions
{
    public static BooleanAssertions Should(this bool actual) =>
        new(actual);

    public static ObjectAssertions Should(this object? actual) =>
        new(actual);

    public static CollectionAssertions<T> Should<T>(this T[] actual) =>
        new(actual);

    public static CollectionAssertions<T> Should<T>(this IEnumerable<T> actual) =>
        new(actual);

    public static FunctionAssertions Should(this Func<Task> action) =>
        new(action);

    public static Func<TResult> Invoking<T, TResult>(this T subject, Func<T, TResult> action) =>
        () => action(subject);
}