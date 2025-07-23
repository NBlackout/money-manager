namespace TestTooling.Assertions;

public record FunctionAssertions(Func<Task> Actual)
{
    public async Task ThrowAsync<TException>() where TException : Exception =>
        await Assert.ThrowsAsync<TException>(this.Actual);
}